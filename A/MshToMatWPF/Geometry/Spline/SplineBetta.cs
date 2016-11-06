using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MshToMatWPF.Geometry.Point;
using LinearMath;

namespace MshToMatWPF.Geometry.Spline
{
    class SplineBetta : ISpline
    {
        public event EventHandler Changed;


        private List<Vertex> p;

        private double[] t;

        private int _Dim;

        public int Dim
        {
            get
            {
                return _Dim;
            }
            set
            {
                if (value != _Dim)
                {
                    _Dim = value;
                    Update();
                }
            }
        }

        private int dim
        {
            get
            {
                if (Dim < p.Count)
                    return Dim;
                return p.Count - 1;
            }
        }


        public SplineBetta()
        {
            p = new List<Vertex>();
        }


        private void UpdateKnot()
        {
            int P = p.Count;
            if (P > dim)
            {
                //int T = P + dim + 1;
                //int I = T - 2 * dim - 2;
                //double dt = 1.0 / ((double)I + 1.0);

                //t = new double[T];

                //for (int n = 0; n < T; n++)
                //{
                //    if (n <= dim)
                //    {
                //        t[n] = 0.0;
                //        continue;
                //    }

                //    if (n > P)
                //    {
                //        t[n] = 1.0;
                //        continue;
                //    }

                //    t[n] = (n - dim) * dt;
                //}

                int T = P + dim + 1;
                double dt = 1.0 / (T - 1.0);
                t = new double[T];
                for (int n = 0; n < T; n++)
                {
                    t[n] = n * dt;
                }
            }
        }

        private double NBasic(double T, int n, int m)
        {
            if (m == 0)
            {
                if ((t[n] <= T & T < t[n + 1]) | (T == t.Last()))
                    return 1.0;
                else
                    return 0.0;
            }
            else
            {
                return (T - t[n]) / (t[n + m] - t[n]) * NBasic(T, n, m - 1) + (t[n + m] - T) / (t[n + m + 1] - t[n + 1]) * NBasic(T, n + 1, m - 1);
            }
        }

        private Vector Calc(double t)
        {
            Vector v = new Vector(3);
            if (p.Count > dim)
            {
                for (int n = 0; n < p.Count; n++)
                {
                    v = v + NBasic(t, n, dim) * p[n].q;
                }
            }
            return v;
        }

        private void OnControlPointChanged(object sender, EventArgs arg)
        {
            Update();
        }


        protected void Update()
        {
            UpdateKnot();

            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }


        public void AddControllPoint(LinePointControl lcp)
        {
            if (lcp != null)
            {
                p.Add(lcp.vert);
                lcp.vert.q.Changed += OnControlPointChanged;
                Update();
            }
        }

        public void RemoveControllPoint(LinePointControl lcp)
        {
            if (lcp != null)
            {
                p.Remove(lcp.vert);
                lcp.vert.q.Changed -= OnControlPointChanged;
                Update();
            }
        }

        public LinePointSegmentSpline CreatePoint(double t)
        {
            LinePointSegmentSpline lcp = new LinePointSegmentSpline();
            lcp.t = t;
            lcp.spline = this;
            return lcp;
        }

        public void UpdatePoint(LinePointSegment lcp)
        {
            if (lcp is LinePointSegmentSpline)
            {
                lcp.vert.q.CopyValue(Calc((lcp as LinePointSegmentSpline).t));
            }
        }

        public void UpdateSegment(Segment seg)
        {
            if (seg is SegmentSpline)
            {
                double tA = (seg.startPoint as LinePointSegmentSpline).t;
                double tB = (seg.endPoint as LinePointSegmentSpline).t;

                (seg as SegmentSpline).t = (tA + tB) / 2.0;
                Vector a = (seg.endPoint as LinePointSegment).vert.q - (seg.startPoint as LinePointSegment).vert.q;
                Vector b = Calc((seg as SegmentSpline).t) - (seg.startPoint as LinePointSegment).vert.q;
                a.normalize();
                (seg as SegmentSpline).distance = (b - (a * b) * a).Length();
            }
        }
    }
}
