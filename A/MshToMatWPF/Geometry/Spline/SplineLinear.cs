using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MshToMatWPF.Geometry.Point;
using LinearMath;

namespace MshToMatWPF.Geometry.Spline
{
    class SplineLinear : ISpline
    {
        public event EventHandler Changed;

        private List<Vertex> p;


        public SplineLinear()
        {
            p = new List<Vertex>();
        }


        private void OnControlPointChanged(object sender, EventArgs arg)
        {
            Update();
        }

        private double RelativeParameter(double t, out int n, out double t_mod)
        {
            double dt = 1.0 / (p.Count - 1.0);
            n = (int)(t / dt);
            t_mod = t - n * dt;
            return t_mod / dt;
        }

        private Vector Calc(double t, Vector a, Vector b)
        {
            if (0 <= t & t <= 1.0)
                return (1.0 - t) * a + t * b;

            throw new ArgumentOutOfRangeException();
        }

        private Vector Calc(double t)
        {
            if (p.Count < 2)
                throw new ArgumentOutOfRangeException();

            if (0 <= t & t <= 1.0)
            {
                int n = 0;
                double t_mod = 0.0;
                double ts = RelativeParameter(t, out n, out t_mod);

                if (t_mod == 0)
                    return (Vector)p[n].q.Clone();

                return Calc(ts, p[n].q, p[n + 1].q);
            }

            throw new ArgumentException();
        }


        protected void Update()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }


        public void UpdatePoint(LinePointSegment lcp)
        {
            if ((lcp is LinePointSegmentSpline) & (lcp.vert != null))
            {
                lcp.vert.q.CopyValue(Calc((lcp as LinePointSegmentSpline).t));
            }
        }

        public void UpdateSegment(Segment seg)
        {
            if (seg is SegmentSpline)
            {
                int nA = 0;
                double tA = 0;
                double tsA = RelativeParameter((seg.startPoint as LinePointSegmentSpline).t, out nA, out tA);

                int nB = 0;
                double tB = 0;
                double tsB = RelativeParameter((seg.endPoint as LinePointSegmentSpline).t, out nB, out tB);

                if (nB - nA > 0)
                {
                    Vector a = seg.endPoint.vert.q - seg.startPoint.vert.q;
                    Vector b = p[nB].q - seg.startPoint.vert.q;
                    a.normalize();
                    (seg as SegmentSpline).distance = (a % b).Length();
                }
                else
                    (seg as SegmentSpline).distance = 0.0;
            }
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
    }
}
