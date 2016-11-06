using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MshToMatWPF.Geometry;
using LinearMath;
using MshToMatWPF.Geometry.Point;

namespace MshToMatWPF.Geometry.Spline
{
    abstract class SplineHermite : ISpline
    {
        public event EventHandler BeforeChanged;

        public event EventHandler Changed;

        public event EventHandler AfterChanged;


        private int dim;

        private Vertex _StartPoint;

        private Vertex _EndPoint;

        private Vector _StartDirection;

        private Vector _EndDirection;

        
        protected int order { get; private set; }
        
        protected Func<double, double>[] phi { get; private set; }

        protected Func<double, double>[] phis { get; private set; }

        protected Vertex[] a { get; private set; }

                
        public Vertex StartPoint
        {
            get
            {
                return _StartPoint;
            }
            set
            {
                if (value != _StartPoint)
                {
                    if (_StartPoint != null)
                    {
                        _StartPoint.q.Changed -= OnParametrsChanged;
                        _StartPoint.q.StateChanged -= OnParametersStateChanged;
                    }
                    _StartPoint = value;

                    if (_StartPoint != null)
                    {
                        _StartPoint.q.Changed += OnParametrsChanged;
                        _StartPoint.q.StateChanged += OnParametersStateChanged;
                        Update();
                    }
                }
            }
        }

        public Vertex EndPoint
        {
            get
            {
                return _EndPoint;
            }
            set
            {
                if (value != _EndPoint)
                {
                    if (_EndPoint != null)
                    {
                        _EndPoint.q.Changed -= OnParametrsChanged;
                        _EndPoint.q.StateChanged -= OnParametersStateChanged;
                    }
                    _EndPoint = value;

                    if (_EndPoint != null)
                    {
                        _EndPoint.q.Changed += OnParametrsChanged;
                        _EndPoint.q.StateChanged += OnParametersStateChanged;
                        Update();
                    }
                }
            }
        }

        public Vector StartDirection
        {
            get
            {
                return _StartDirection;
            }
            set
            {
                if (value != _StartDirection)
                {
                    if (_StartDirection != null)
                    {
                        _StartDirection.Changed -= OnParametrsChanged;
                        _StartDirection.StateChanged -= OnParametersStateChanged;
                    }
                    _StartDirection = value;

                    if (_StartDirection != null)
                    {
                        _StartDirection.Changed += OnParametrsChanged;
                        _StartDirection.StateChanged += OnParametersStateChanged;
                        Update();
                    }
                }
            }
        }

        public Vector EndDirection
        {
            get
            {
                return _EndDirection;
            }
            set
            {
                if (value != _EndDirection)
                {
                    if (_EndDirection != null)
                    {
                        _EndDirection.Changed -= OnParametrsChanged;
                        _EndDirection.StateChanged -= OnParametersStateChanged;
                    }
                    _EndDirection = value;

                    if (_EndDirection != null)
                    {
                        _EndDirection.Changed += OnParametrsChanged;
                        _EndDirection.StateChanged += OnParametersStateChanged;
                        Update();
                    }
                }
            }
        }


        public SplineHermite(int order)
        {
            this.order = order;
            phi = new Func<double, double>[order + 1];
            phis = new Func<double, double>[order + 1];
            a = new Vertex[order + 1];
        }


        private Vector Calc(Func<double, double>[] f, double t)
        {
            Vector v = new Vector(3);
            for (int n = 0; n <= order; n++)
            {
                v.CopyValue(v + f[n](t) * a[n].q);
            }
            return v;
        }

        private void OnParametrsChanged(object sender, EventArgs arg)
        {
            Update();
        }

        private void OnParametersStateChanged(object sender, EventArgs arg)
        {
            if (sender is Vector)
            {
                Vector vect = (Vector)sender;
                
                if ((vect.matrixState == MatrixState.stable) & (AfterChanged != null))
                    AfterChanged(this, EventArgs.Empty);
                
                if ((vect.matrixState == MatrixState.unstable) & (BeforeChanged != null))
                    BeforeChanged(this, EventArgs.Empty);
            }
        }
        
        private void Update()
        {
            if ((StartPoint == null) | (EndPoint == null))
                return;

            if ((StartDirection == null) | (EndDirection == null))
                return;

            SquareMatrix M = new SquareMatrix(order + 1);
            RectangleMatrix A = new RectangleMatrix(order + 1, dim);
            RectangleMatrix B = new RectangleMatrix(order + 1, dim);

            double t0 = 0.0;
            double t1 = 1.0;

            for (int n = 0; n <= order; n++)
            {
                M[0, n] = phi[n](t0);
                M[1, n] = phi[n](t1);
                M[2, n] = phis[n](t0);
                M[3, n] = phis[n](t1);
            }

            for (int n = 0; n < dim; n++)
            {
                B[0, n] = StartPoint.q[n];
                B[1, n] = EndPoint.q[n];
                B[2, n] = StartDirection[n];
                B[3, n] = EndDirection[n];
            }

            A = M.invert() * B;

            for (int m = 0; m < dim; m++)
            {
                for (int n = 0; n <= order; n++)
                {
                    a[n].q[m] = A[n, m];
                }
            }

            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        
        protected void Init()
        {
            dim = 0;
            if (a[0] is Vertex2D)
                dim = 2;
            if (a[0] is Vertex3D)
                dim = 3;
        }


        public void AddControllPoint(LinePointControl lcp)
        {

        }

        public void RemoveControllPoint(LinePointControl lcp)
        {

        }

        public void UpdatePoint(LinePointSegment lcp)
        {
            if (lcp is LinePointSegmentSpline)
            {
                lcp.vert.q.CopyValue(Calc(phi, (lcp as LinePointSegmentSpline).t));
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
                Vector b = Calc(phi, (seg as SegmentSpline).t) - (seg.startPoint as LinePointSegment).vert.q;
                a.normalize();
                (seg as SegmentSpline).distance = (b - (a * b) * a).Length();
            }
        }

        public LinePointSegmentSpline CreatePoint(double t)
        {
            LinePointSegmentSpline lcp = new LinePointSegmentSpline();
            lcp.spline = this;
            lcp.t = t;
            return lcp;
        }

        public double Length(double Ta, double Tb)
        {
            double dt = 1E-3;
            double t = Ta + dt / 2.0;
            double L = 0.0;
            while (t < Tb)
            {
                Vector dir = Calc(phis, t);
                L += dir.Length() * dt;
                t += dt;
            }
            return L;
        }

        public abstract List<LinePointSegmentSpline> Intersection(Vector pnt, Vector tau);
    }
}
