using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Media3D;

using LinearMath;

namespace MshToMatWPF.Drawing.Primitives
{
    public class Tube : IPrimitive
    {
        int pointCount;
        
        double _r;

        Vector _StartVector;
        
        Vector _EndVector;

        Color _color;

        bool _IsSelected;

        public double r
        {
            get
            {
                return _r;
            }
            set
            {
                _r = value;
                Update();
            }
        }

        public Vector StartVector
        {
            get
            {
                return _StartVector;
            }
            set
            {
                _StartVector = value;
                _StartVector.Changed += OnVectorChanged;
                Update();
            }
        }

        public Vector EndVector
        {
            get
            {
                return _EndVector;
            }
            set
            {
                _EndVector = value;
                _EndVector.Changed += OnVectorChanged;
                Update();
            }
        }

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
                _IsSelected = value;
                if (Selected != null)
                    Selected(this, EventArgs.Empty);
            }
        }

        public event EventHandler Selected;

        public Tube()
        {
            Init();
        }

        private void Init()
        {
            pointCount = 5;
            _r = 1.0;
            _color = Colors.Green;
            _IsSelected = false;
            Mesh = new MeshGeometry3D();

            model = new GeometryModel3D();
            model.Geometry = Mesh;

            Brush brush = new SolidColorBrush(color);
            DiffuseMaterial material = new DiffuseMaterial(brush);
            model.Material = material;
        }

        protected void OnVectorChanged(object sender, EventArgs arg)
        {
            Update();
        }

        protected virtual void Update()
        {
            if ((StartVector == null) | (EndVector == null))
                return;

            Vector dir = EndVector - StartVector;
            dir.normalize();

            Point3D[] start = Circle(StartVector, dir, dir);
            Point3D[] end = Circle(EndVector, dir, dir);


            CreateMesh(start, end);
        }

        protected Point3D[] Circle(Vector center, Vector normal, Vector direction)
        {
            Vector z0 = Vector.Create3D(0.0, 0.0, 1.0);
            Vector p = direction % z0;

            Func<Vector, Point3D> converter = (v) =>
            {
                return new Point3D(v[0], v[1], v[2]);
            };

            Point3D[] pntList = new Point3D[pointCount];

            double A = p * normal;
            double B = z0 * normal;
            double C = direction * normal;

            Func<double, double> phi = (alp) =>
                {
                    if (C == 0)
                        return 0.0;
                    else
                        return -(A * Math.Cos(alp) + B * Math.Sin(alp)) / C;
                };

            double dPhi = 2.0 * Math.PI / pointCount;
            for (int n = 0; n < pointCount; n++)
            {
                Vector v = p * Math.Cos(n * dPhi) + z0 * Math.Sin(n * dPhi) + direction * phi(n * dPhi);
                pntList[n] = converter(center + _r * v);
            }

            return pntList;
        }

        protected void UpdateMesh(Point3D[] start, Point3D[] end)
        {
            Mesh.Positions.Clear();

            foreach (var pnt in start)
                Mesh.Positions.Add(pnt);

            foreach (var pnt in end)
                Mesh.Positions.Add(pnt);
        }

        protected void CreateMesh(Point3D[] start, Point3D[] end)
        {
            if (Mesh.Positions.Count != 0)
            {
                UpdateMesh(start, end);
                return;
            }

            Mesh.Positions.Clear();
            Mesh.TriangleIndices.Clear();

            Dictionary<int, Point3D> Vertex = new Dictionary<int, Point3D>();

            for (int n = 0; n < pointCount; n++)
            {
                Vertex.Add(n, start[n]);
                Vertex.Add(n + pointCount, end[n]);
            }

            for (int n = 0; n < 2 * pointCount; n++)
            {
                Point3D pnt;
                if (Vertex.TryGetValue(n, out pnt))
                    Mesh.Positions.Add(pnt);
            }

            Func<int, int> cycle = (n) =>
            {
                if (n >= pointCount)
                    return n - pointCount;
                return n;
            };

            for (int n = 0; n < pointCount; n++)
            {
                Mesh.TriangleIndices.Add(cycle(n) + pointCount);
                Mesh.TriangleIndices.Add(cycle(n + 1));
                Mesh.TriangleIndices.Add(cycle(n));

                Mesh.TriangleIndices.Add(cycle(n + 1) + pointCount);
                Mesh.TriangleIndices.Add(cycle(n + 1));
                Mesh.TriangleIndices.Add(cycle(n) + pointCount);
            }
        }
    }
}
