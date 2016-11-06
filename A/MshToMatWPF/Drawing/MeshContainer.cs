using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;

using MshToMatWPF.Drawing.Primitives;
using MshToMatWPF.Geometry;
using MshToMatWPF.Preferences;

namespace MshToMatWPF.Drawing
{
    public class MeshContainer : Drawable
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

        public MeshContainer(double[,] point, int[,] element)
        {
            _vertex = new List<Vertex2D>();
            _edge = new List<Edge2D>();
            _triangle = new List<Triangle2D>();
            
            for (int n = 0; n < point.GetLength(0); n++)
            {
                Vertex2D v = AddVertex(point[n, 0], point[n, 1]);
                v.extID = n + 1;
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

        private Edge2D AddEdgeIfNotExist(Vertex2D v1, Vertex2D v2)
        {
            Edge2D e1 = _edge.Find(item => (item.V1.ID == v1.ID) & (item.V2.ID == v2.ID));
            Edge2D e2 = _edge.Find(item => (item.V1.ID == v2.ID) & (item.V2.ID == v1.ID));

            if (e1 != null)
                return e1;

            if (e2 != null)
                return e2;

            Edge2D e = new Edge2D(v1, v2);
            AddEdge(e);
            return e;
        }

        private Vertex2D AddVertex(double x, double y)
        {
            Vertex2D v = new Vertex2D();
            v.X = x;
            v.Y = y;
            _vertex.Add(v);

            IPrimitive prim = new Sphere();
            (prim as Sphere).r = 2.5;
            (prim as Sphere).center = v.q;

            AddPrimitive(prim);

            return v;
        }

        private void AddEdge(Edge2D e)
        {
            if (e != null)
            {
                _edge.Add(e);

                IPrimitive prim = new Tube();
                (prim as Tube).StartVector = e.V1.q;
                (prim as Tube).EndVector = e.V2.q;

                AddPrimitive(prim);
            }
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

        protected override void OnPrimitiveSelected(object sender, EventArgs arg)
        {
            if ((sender is IPrimitive)&(colorManager != null))
            {
                if ((sender as IPrimitive).IsSelected)
                    (sender as IPrimitive).color = colorManager.gridSelected;
                else
                    (sender as IPrimitive).color = colorManager.grid;
            }

            base.OnPrimitiveSelected(sender, arg);
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
