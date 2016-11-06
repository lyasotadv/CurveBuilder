using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LinearMath;
using MshToMatWPF.Geometry.Spline;

namespace MshToMatWPF.Geometry.Point
{
    abstract class LinePoint
    {
        public event EventHandler Updated;

        private const double distCrit = 0.5;

        public event EventHandler DistanceIsCritical;


        private LinePoint _prev;

        private LinePoint _next;

        private Vertex _vert;

        private double _Dist;


        public Vertex vert
        {
            get
            {
                return _vert;
            }
            private set
            {
                if (_vert != value)
                {
                    if (_vert != null)
                    {
                        _vert.q.Changed -= OnUpdateVertex;
                    }
                    _vert = value;
                    if (_vert != null)
                    {
                        _vert.q.Changed += OnUpdateVertex;
                    }
                }
            }
        }

        public LinePoint prev
        {
            get
            {
                return _prev;
            }
            set
            {
                if (_prev != value)
                {
                    LinePoint prevOld = _prev;
                    _prev = value;
                    if (prevOld != null)
                    {
                        prevOld.next = null;
                        prevOld.vert.q.Changed -= OnUpdateSibling;
                    }
                    _prev = value;
                    if (_prev != null)
                    {
                        _prev.next = this;
                        _prev.vert.q.Changed += OnUpdateSibling;
                    }
                    Update();
                }
            }
        }

        public LinePoint next
        {
            get
            {
                return _next;
            }
            set
            {
                if (_next != value)
                {
                    LinePoint nextOld = _next;
                    _next = value;
                    if (nextOld != null)
                    {
                        nextOld.prev = null;
                        nextOld.vert.q.Changed -= OnUpdateSibling;
                    }
                    _next = value;
                    if (_next != null)
                    {
                        _next.prev = this;
                        _next.vert.q.Changed += OnUpdateSibling;
                    }
                    Update();
                }
            }
        }

        public double Dist
        {
            get
            {
                return _Dist;
            }
            private set
            {
                if (_Dist != value)
                {
                    if (value < 0.0)
                        throw new ArgumentOutOfRangeException("Distance must be positive");

                    _Dist = value;
                    if ((_Dist < distCrit) & (!double.IsNaN(_Dist)) && (DistanceIsCritical != null))
                        DistanceIsCritical(this, EventArgs.Empty);
                }
            }
        }


        public LinePoint()
        {
            prev = null;
            next = null;
            vert = new Vertex3D();
        }


        protected virtual void OnUpdateSibling(object sender, EventArgs arg)
        {

        }


        protected virtual void OnUpdateVertex(object sender, EventArgs arg)
        {
            Update();
        }
        
        protected virtual void Update()
        {
            if ((prev != null) & (next != null))
            {
                Vector a = next.vert.q - prev.vert.q;
                Vector b = this.vert.q - prev.vert.q;
                a.normalize();
                Dist = (a % b).Length();
            }
            else
            {
                Dist = double.NaN;
            }

            if (Updated != null)
                Updated(this, EventArgs.Empty);
        }

        
        public virtual void Unsubcribe()
        {
            prev = null;
            next = null;
        }

        public override string ToString()
        {
            return vert.q.ToString();
        }
    }

    abstract class LinePointSegment : LinePoint
    {
        private Segment _inSeg;

        private Segment _outSeg;

        public Segment inSeg
        {
            get
            {
                return _inSeg;
            }
            set
            {
                if (value != _inSeg)
                {
                    if ((_inSeg != null) && (_inSeg.endPoint != null))
                    {
                        _inSeg.endPoint = null;
                    }
                    _inSeg = value;
                    if ((_inSeg != null) && (_inSeg.endPoint != this))
                    {
                        _inSeg.endPoint = this;
                    }
                }
            }
        }

        public Segment outSeg
        {
            get
            {
                return _outSeg;
            }
            set
            {
                if (value != _outSeg)
                {
                    if ((_outSeg != null) && (_outSeg.startPoint != null))
                    {
                        _outSeg.startPoint = null;
                    }
                    _outSeg = value;
                    if ((_outSeg != null) && (_outSeg.startPoint != this))
                    {
                        _outSeg.startPoint = this;
                    }
                }
            }
        }


        private Vector _tau;

        public Vector tau
        {
            get
            {
                return _tau;
            }
            private set
            {
                _tau = value;
            }
        }


        public LinePointSegment()
        {
            tau = new Vector(3);
        }

        
        private void UpdateTau()
        {
            Vector v = null;
            if ((prev == null) & (next == null))
            {
                v = null;
            }

            if ((prev != null) & (next == null))
            {
                v = vert.q - prev.vert.q;
                v.normalize();
            }

            if ((prev == null) & (next != null))
            {
                v = next.vert.q - vert.q;
                v.normalize();
            }

            if ((prev != null) & (next != null))
            {
                Vector dirA = vert.q - prev.vert.q;
                dirA.normalize();

                Vector dirB = next.vert.q - vert.q;
                dirB.normalize();

                v = 0.5 * (dirA + dirB);
            }

            if ((v == null) | (tau == null))
                tau = v;
            else
                tau.CopyValue(v);
        }


        protected override void OnUpdateSibling(object sender, EventArgs arg)
        {
            UpdateTau();
        }

        protected override void OnUpdateVertex(object sender, EventArgs arg)
        {
            UpdateTau();
        }

        protected override void Update()
        {
            UpdateTau();
            base.Update();
        }


        public override void Unsubcribe()
        {
            base.Unsubcribe();

            inSeg = null;
            outSeg = null;
        }
    }

    class LinePointSegmentSpline : LinePointSegment
    {
        private ISpline _spline;

        public ISpline spline
        {
            get
            {
                return _spline;
            }
            set
            {
                if (_spline != value)
                {
                    if (_spline != null)
                    {
                        _spline.Changed -= OnSplineChanged;
                    }
                    _spline = value;

                    if (_spline != null)
                    {
                        _spline.Changed += OnSplineChanged;
                    }
                    Update();
                }
            }
        }

        private double _t;

        public double t
        {
            get
            {
                return _t;
            }
            set
            {
                if (_t != value)
                {
                    _t = value;
                    Update();
                }
            }
        }


        public LinePointSegmentSpline()
        {
            spline = null;
            t = double.NaN;
        }

        
        private void OnSplineChanged(object sender, EventArgs arg)
        {
            Update();
        }


        protected override void Update()
        {
            if (spline != null)
                spline.UpdatePoint(this);
            base.Update();
        }


        public override void Unsubcribe()
        {
            base.Unsubcribe();

            spline = null;
        }
    }

    class LinePointControl : LinePoint
    {
        
    }
}
