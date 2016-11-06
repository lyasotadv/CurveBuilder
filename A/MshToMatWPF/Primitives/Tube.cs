using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Media3D;

using LinearMath;

namespace MshToMatWPF.Primitives
{
    public class Tube : IPrimitive
    {
        int pointCount;
        double r;

        Vector _StartVector;
        Vector _EndVector;

        public Vector StartVector
        {
            get
            {
                return _StartVector;
            }
            set
            {
                _StartVector = value;
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
                Update();
            }
        }

        private MeshGeometry3D Mesh { get; set; }

        public GeometryModel3D model { get; private set; }

        public Tube()
        {
            Init();
        }

        public Tube(double Xstart, double Ystart, double Xend, double Yend)
        {
            Init();

            StartVector = Vector.Create3D(Xstart, Ystart, 0.0);
            EndVector = Vector.Create3D(Xend, Yend, 0.0);
        }

        private void Init()
        {
            pointCount = 10;
            r = 1.0;
            Mesh = new MeshGeometry3D();

            model = new GeometryModel3D();
            model.Geometry = Mesh;

            Brush brush = new SolidColorBrush(Colors.Green);
            DiffuseMaterial material = new DiffuseMaterial(brush);
            model.Material = material;
        }

        protected virtual void Update()
        {
            if ((StartVector == null) | (EndVector == null))
                return;

            Vector dir = EndVector - StartVector;
            dir.normalize();

            Point3D[] start = Circle(StartVector, dir);
            Point3D[] end = Circle(EndVector, dir);

            CreateMesh(start, end);
        }

        protected Point3D[] Circle(Vector center, Vector normal)
        {
            Vector z0 = Vector.Create3D(0.0, 0.0, 1.0);
            Vector p = normal % z0;

            Func<Vector, Point3D> converter = (v) =>
            {
                return new Point3D(v[0], v[1], v[2]);
            };

            Point3D[] pntList = new Point3D[pointCount];

            double dPhi = 2.0 * Math.PI / pointCount;
            for (int n = 0; n < pointCount; n++)
            {
                Vector v = p * Math.Cos(n * dPhi) + z0 * Math.Sin(n * dPhi);
                pntList[n] = converter(center + r * v);
            }

            return pntList;
        }

        protected void CreateMesh(Point3D[] start, Point3D[] end)
        {
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
                Mesh.TriangleIndices.Add(cycle(n));
                Mesh.TriangleIndices.Add(cycle(n + 1));
                Mesh.TriangleIndices.Add(cycle(n) + pointCount);

                Mesh.TriangleIndices.Add(cycle(n) + pointCount);
                Mesh.TriangleIndices.Add(cycle(n + 1));
                Mesh.TriangleIndices.Add(cycle(n + 1) + pointCount);
            }
        }
    }
}
