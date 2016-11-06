using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Media3D;

using MshToMatWPF.Geometry;
using LinearMath;

namespace MshToMatWPF.Drawing.Primitives
{
    class Sphere : IPrimitive
    {
        private class ICOSphereCreator : Tree<ICOSphereFacet>
        {
            public double R { get; set; }

            public Vector P0 { get; set; }

            public MeshGeometry3D Mesh { get; set; }

            List<Vertex3D> vertex;

            List<Edge3D> edge;

            Dictionary<Edge3D, Vertex3D> centerVertex;

            public ICOSphereCreator()
            {
                vertex = new List<Vertex3D>();
                edge = new List<Edge3D>();
                centerVertex = new Dictionary<Edge3D, Vertex3D>();

                P0 = null;
                R = double.NaN;
            }

            private void MoveToSurface(Vertex3D v)
            {
                Vector dir = v.q - P0;
                dir.normalize();
                v.q.CopyValue(P0 + R * dir);
            }

            private Vertex3D FindOrAddVertex(Edge3D e)
            {
                Vertex3D v;
                if (!centerVertex.TryGetValue(e, out v))
                {
                    v = e.Center;
                    v.extID = vertex.Count;
                    MoveToSurface(v);
                    vertex.Add(v);
                    centerVertex.Add(e, v);
                }
                return v;
            }

            private Edge3D FindOrAddEdge(Vertex3D v1, Vertex3D v2)
            {
                Predicate<Edge3D> cond = (e) =>
                    {
                        return ((e.V1 == v1) & (e.V2 == v2)) | ((e.V1 == v2) & (e.V2 == v1));
                    };
                Edge3D ed = edge.Find(cond);
                if (ed == null)
                {
                    ed = new Edge3D(v1, v2);
                    edge.Add(ed);
                }
                return ed;
            }

            private ICOSphereFacet AddFacet(Vertex3D v1, Vertex3D v2, Vertex3D v3, ICOSphereFacet parent)
            {
                ICOSphereFacet fc = new ICOSphereFacet(v1, v2, v3);

                fc.E1 = FindOrAddEdge(v2, v3);
                fc.E2 = FindOrAddEdge(v3, v1);
                fc.E3 = FindOrAddEdge(v1, v2);

                AddNode(fc, parent);
                return fc;
            }

            private void RefineFacet(ICOSphereFacet fc)
            {
                Vertex3D v1 = FindOrAddVertex(fc.E1);
                Vertex3D v2 = FindOrAddVertex(fc.E2);
                Vertex3D v3 = FindOrAddVertex(fc.E3);

                AddFacet(fc.V1, v3, v2, fc);
                AddFacet(v3, fc.V2, v1, fc);
                AddFacet(v2, v1, fc.V3, fc);
                AddFacet(v3, v1, v2, fc);
            }

            private void CreateMesh()
            {
                Mesh.Positions.Clear();
                Mesh.TriangleIndices.Clear();

                foreach (var v in vertex)
                {
                    Point3D p = new Point3D(v.X, v.Y, v.Z);
                    Mesh.Positions.Add(p);
                }

                ICOSphereFacet[] fcl = FindAll((fc) => { return IsLeaf(fc); });
                foreach (var fc in fcl)
                {
                    Mesh.TriangleIndices.Add(fc.V1.extID);
                    Mesh.TriangleIndices.Add(fc.V2.extID);
                    Mesh.TriangleIndices.Add(fc.V3.extID);
                }
            }

            public void AddVertex(double phi, double theta)
            {
                CoordinateSystem.PointSpherical pnt = new CoordinateSystem.PointSpherical(phi, theta, R);
                Vertex3D v = new Vertex3D();
                v.q.CopyValue(pnt.ToCartesian() + P0);
                v.extID = vertex.Count;
                vertex.Add(v);
            }

            public void AddFacet(int id1, int id2, int id3)
            {
                Vertex3D v1 = vertex.Find((v) => { return v.extID == id1; });
                Vertex3D v2 = vertex.Find((v) => { return v.extID == id2; });
                Vertex3D v3 = vertex.Find((v) => { return v.extID == id3; });
                AddFacet(v1, v2, v3, null);
            }

            public void Refine(int deep)
            {
                Action<ICOSphereFacet> creator = (fc) =>
                    {
                        if (Level(fc) < deep)
                            RefineFacet(fc);
                    };
                CreateTree(creator);
                CreateMesh();
            }
        }

        private class ICOSphereFacet : Triangle3D, INode
        {
            public Guid guid { get; private set; }

            public ICOSphereFacet(Vertex3D v1, Vertex3D v2, Vertex3D v3) 
                : base(v1, v2, v3)
            {
                guid = Guid.NewGuid();
            }
        }

        double _r;

        Vector _center;

        bool _IsSelected;

        Color _color;

        const double alp = 31.717;

        private MeshGeometry3D Mesh { get; set; }

        public GeometryModel3D model { get; private set; }

        public Color color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                ((model.Material as DiffuseMaterial).Brush as SolidColorBrush).Color = color;
            }
        }

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (value != IsSelected)
                {
                    _IsSelected = value;
                    if (Selected != null)
                        Selected(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler Selected;

        public double r
        {
            get
            {
                return _r;
            }
            set
            {
                if (r != value)
                {
                    _r = value;
                    Update();
                }
            }
        }

        public Vector center
        {
            get
            {
                return _center;
            }
            set
            {
                if (center != value)
                {
                    _center = value;
                    _center.Changed += OnVectorChanged;
                    Update();
                }
            }
        }

        public Sphere()
        {
            Init();
        }

        private void Init()
        {
            _r = 1.0;
            _center = null;
            _color = Colors.Green;
            _IsSelected = false;
            Mesh = new MeshGeometry3D();

            model = new GeometryModel3D();
            model.Geometry = Mesh;

            Brush brush = new SolidColorBrush(color);
            DiffuseMaterial material = new DiffuseMaterial(brush);
            model.Material = material;
        }

        private void OnVectorChanged(object sender, EventArgs arg)
        {
            Update();
        }

        private void Update()
        {
            if ((center == null)|(double.IsNaN(r)))
                return;

            ICOSphereCreator creator = new ICOSphereCreator();
            creator.R = r;
            creator.P0 = center;
            creator.Mesh = Mesh;

            creator.AddVertex(180.0, 90.0 - alp); //0
            creator.AddVertex(0.0, 90.0 - alp); //1
            creator.AddVertex(-alp, 0.0); //2
            creator.AddVertex(alp, 0.0); //3
            creator.AddVertex(0.0, -90.0 + alp); //4
            creator.AddVertex(180.0, -90.0 + alp);//5
            creator.AddVertex(180.0 - alp, 0.0);//6
            creator.AddVertex(-180.0 + alp, 0.0);//7
            creator.AddVertex(-90.0, alp);//8
            creator.AddVertex(-90.0, -alp);//9
            creator.AddVertex(90.0, -alp);//10
            creator.AddVertex(90, alp);//11

            creator.AddFacet(1, 2, 3);
            creator.AddFacet(1, 8, 2);
            creator.AddFacet(1, 3, 11);
            creator.AddFacet(0, 6, 7);
            creator.AddFacet(0, 7, 8);
            creator.AddFacet(0, 8, 1);
            creator.AddFacet(0, 11, 6);
            creator.AddFacet(0, 1, 11);
            creator.AddFacet(2, 4, 3);
            creator.AddFacet(2, 9, 4);
            creator.AddFacet(2, 8, 9);
            creator.AddFacet(4, 9, 5);
            creator.AddFacet(7, 9, 8);
            creator.AddFacet(5, 9, 7);
            creator.AddFacet(5, 7, 6);
            creator.AddFacet(6, 11, 10);
            creator.AddFacet(3, 10, 11);
            creator.AddFacet(3, 4, 10);
            creator.AddFacet(5, 6, 10);
            creator.AddFacet(4, 5, 10);

            creator.Refine(2);
        }
    }
}
