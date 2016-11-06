using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatConverter.Geometry
{
    public class GeometryManager : ICloneable
    {
        List<Vertex2D> _vertex;

        List<Edge2D> _edge;

        List<Triangle2D> _triangle;

        public List<Vertex2D> vertex
        {
            get
            {
                return _vertex;
            }
        }

        public List<Edge2D> edge
        {
            get
            {
                return _edge;
            }
        }

        public List<Triangle2D> triangle
        {
            get
            {
                return _triangle;
            }
        }

        public Dictionary<Triangle2D, ElementCutParameters> cutParam { get; set; }

        public Dictionary<Vertex2D, WeightFunc2D> weightfunc { get; set; }

        private Edge2D AddEdgeIfNotExist(Vertex2D v1, Vertex2D v2)
        {
            Edge2D e1 = _edge.Find(item => (item.V1.ID == v1.ID) & (item.V2.ID == v2.ID));
            Edge2D e2 = _edge.Find(item => (item.V1.ID == v2.ID) & (item.V2.ID == v1.ID));

            if (e1 != null)
                return e1;

            if (e2 != null)
                return e2;

            Edge2D e = new Edge2D(v1, v2);
            _edge.Add(e);
            return e;
        }

        public GeometryManager(double[,] point, int[,] element)
        {
            _vertex = new List<Vertex2D>();
            _edge = new List<Edge2D>();
            _triangle = new List<Triangle2D>();
            cutParam = new Dictionary<Triangle2D, ElementCutParameters>();
            weightfunc = new Dictionary<Vertex2D, WeightFunc2D>();

            for (int n = 0; n < point.GetLength(0); n++)
            {
                Vertex2D v = new Vertex2D();
                v.X = point[n, 0];
                v.Y = point[n, 1];
                v.extID = n + 1;
                _vertex.Add(v);
            }

            for (int n = 0; n < element.GetLength(0); n++)
            {
                Vertex2D v1 = _vertex.Find(item => item.extID == element[n, 0]);
                Vertex2D v2 = _vertex.Find(item => item.extID == element[n, 1]);
                Vertex2D v3 = _vertex.Find(item => item.extID == element[n, 2]);

                Triangle2D t = new Triangle2D(v1, v2, v3);
                t.extID = n + 1;

                _triangle.Add(t);

                t.E1 = AddEdgeIfNotExist(v1, v2);
                t.E2 = AddEdgeIfNotExist(v2, v3);
                t.E3 = AddEdgeIfNotExist(v3, v1);
            }
        }

        private GeometryManager()
        {
            _vertex = new List<Vertex2D>();
            _edge = new List<Edge2D>();
            _triangle = new List<Triangle2D>();
            cutParam = new Dictionary<Triangle2D, ElementCutParameters>();
            weightfunc = new Dictionary<Vertex2D, WeightFunc2D>();
        }

        public object Clone()
        {
            GeometryManager g = new GeometryManager();

            foreach (var x in _vertex)
            {
                Vertex2D v = (Vertex2D)x.Clone();
                g._vertex.Add(v);

                WeightFunc2D w;
                if (weightfunc.TryGetValue(x, out w))
                    g.weightfunc.Add(v, w);
            }

            foreach (var x in _edge)
                g._edge.Add((Edge2D)x.Clone());

            foreach (var x in triangle)
            {
                Triangle2D tr = (Triangle2D)x.Clone();
                g._triangle.Add(tr);

                ElementCutParameters p;
                if (cutParam.TryGetValue(x, out p))
                    g.cutParam.Add(tr, (ElementCutParameters)p.Clone());
            }

            return g;
        }

        private Vertex2D FarestInDirection(double Vx, double Vy)
        {
            double dist = double.NegativeInfinity;
            double d = double.NaN;
            Vertex2D V = null;
            foreach (var v in _vertex)
            {
                d = Vx * v.X + Vy * v.Y;
                if (d > dist)
                {
                    dist = d;
                    V = v;
                }
            }
            return V;
        }

        public void RectangleBox(out double Xmin, out double Xmax, out double Ymin, out double Ymax)
        {
            Vertex2D v;
            
            v = FarestInDirection(-1.0, 0.0);
            Xmin = v.X;

            v = FarestInDirection(1.0, 0.0);
            Xmax = v.X;

            v = FarestInDirection(0.0, -1.0);
            Ymin = v.Y;

            v = FarestInDirection(0.0, 1.0);
            Ymax = v.Y;
        }
    }
}
