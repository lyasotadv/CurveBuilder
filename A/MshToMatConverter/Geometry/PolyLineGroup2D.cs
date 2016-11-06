using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatConverter.Geometry
{
    class PolyLineGroup2D
    {
        List<Edge2D> edgeSet;

        List<int> colorSet;

        Dictionary<Edge2D, int> edgeColor;

        public PolyLineGroup2D()
        {
            edgeSet = new List<Edge2D>();
            colorSet = new List<int>();
            edgeColor = new Dictionary<Edge2D, int>();
        }

        private bool Contains(Edge2D e)
        {
            return edgeSet.Contains(e);
        }

        public void Add(Edge2D e)
        {
            if (!Contains(e))
                edgeSet.Add(e);
        }

        private Edge2D firstUnColored()
        {
            int i;
            foreach (var e in edgeSet)
            {
                if (!edgeColor.TryGetValue(e, out i))
                    return e;
            }
            return null;
        }

        private bool IsConnected(Edge2D e1, Edge2D e2, double prec)
        {
            if (e1 == e2)
                return false;

            Func<Vertex2D, Vertex2D, double> SqrDist = (V1, V2) =>
                {
                    double dx = V2.X - V1.X;
                    double dy = V2.Y - V1.Y;
                    return dx * dx + dy * dy;
                };

            double prec2 = prec * prec;

            if (SqrDist(e1.V1, e2.V1) < prec2)
                return true;

            if (SqrDist(e1.V2, e2.V2) < prec2)
                return true;

            if (SqrDist(e1.V1, e2.V2) < prec2)
                return true;

            if (SqrDist(e1.V2, e2.V1) < prec2)
                return true;

            return false;
        }

        private List<Edge2D> Connected(Edge2D e, double prec)
        {
            List<Edge2D> lst = new List<Edge2D>();
            foreach (var e0 in edgeSet)
                if (IsConnected(e, e0, prec))
                    lst.Add(e0);
            return lst;
        }

        private int getColor(Edge2D e)
        {
            int col;
            if (edgeColor.TryGetValue(e, out col))
                return col;
            return -1;
        }

        private void setColor(Edge2D e, int col)
        {
            int c;
            if (edgeColor.TryGetValue(e, out c))
                edgeColor.Remove(e);
            edgeColor.Add(e, col);
        }

        private void Coloring(Edge2D e, double prec)
        {
            int col = getColor(e);
            List<Edge2D> lst = Connected(e, prec);
            foreach (var ex in lst)
            {
                if (getColor(ex) != col)
                {
                    setColor(ex, col);
                    Coloring(ex, prec);
                }
            }
        }

        private int nextColor()
        {
            int col = colorSet.Count();
            col++;
            colorSet.Add(col);
            return col;
        }

        public void Coloring()
        {
            double prec = 0.0;
            foreach (var e in edgeSet)
                prec += e.Length();
            prec /= edgeSet.Count;
            prec /= 100.0;

            while (true)
            {
                Edge2D e = firstUnColored();
                if (e == null)
                    return;
                int col = nextColor();
                setColor(e, col);
                Coloring(e, prec);
            }
        }

        private double minDist(List<Edge2D> lst, double X, double Y)
        {
            Vertex2D v = new Vertex2D();
            v.X = X;
            v.Y = Y;

            double d_min = double.PositiveInfinity;
            foreach (var e in lst)
            {
                double d = e.Distance(v);
                if (d < d_min)
                    d_min = d;
            }
            return d_min;
        }

        public List<Edge2D> Closest(double[] X, double[] Y)
        {
            if (X.Length != Y.Length)
                return null;
            int N = X.Length;
            
            List<Edge2D>[] lst = new List<Edge2D>[colorSet.Count];
            int n = 0;
            foreach (var col in colorSet)
            {
                lst[n] = new List<Edge2D>();
                foreach (var e in edgeSet)
                    if (getColor(e) == col)
                        lst[n].Add(e);
                n++;
            }

            double[] minD = new double[colorSet.Count];
            n = 0;
            foreach (var l in lst)
            {
                double d = 0.0;
                for (int k = 0; k < N; k++)
                {
                    d += minDist(l, X[k], Y[k]);
                }
                minD[n] = d / N;
                n++;
            }

            double D = minD.Min();
            n = 0;
            foreach (var x in minD)
            {
                if (x == D)
                    break;
                n++;
            }

            return lst[n];
        }
    }
}
