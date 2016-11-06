using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MshToMatWPF.Geometry.Spline;

namespace MshToMatWPF.Geometry.Point
{
    abstract class Segment
    {
        LinePointSegment _startPoint;

        LinePointSegment _endPoint;

        public LinePointSegment startPoint
        {
            get
            {
                return _startPoint;
            }
            set
            {
                if (_startPoint != value)
                {
                    LinePointSegment oldPoint = _startPoint;
                    _startPoint = value;
                    if ((oldPoint != null) && (oldPoint.outSeg != null))
                    {
                        oldPoint.outSeg = null;
                        oldPoint.Updated -= OnPointUpdated;
                    }

                    _startPoint = value;

                    if ((_startPoint != null) && (_startPoint.outSeg != this))
                    {
                        _startPoint.outSeg = this;
                        _startPoint.Updated += OnPointUpdated;
                    }
                    Update();
                }
            }
        }

        public LinePointSegment endPoint
        {
            get
            {
                return _endPoint;
            }
            set
            {
                if (_endPoint != value)
                {
                    LinePointSegment oldPoint = _endPoint;
                    _endPoint = value;
                    if ((oldPoint != null) && (oldPoint.inSeg != null))
                    {
                        oldPoint.inSeg = null;
                        oldPoint.Updated -= OnPointUpdated;
                    }

                    _endPoint = value;

                    if ((_endPoint != null) && (_endPoint.inSeg != this))
                    {
                        _endPoint.inSeg = this;
                        _endPoint.Updated += OnPointUpdated;
                    }
                    Update();
                }
            }
        }

        
        private void OnPointUpdated(object sender, EventArgs arg)
        {
            Update();
        }


        protected abstract void Update();


        public virtual void Unsubscribe()
        {
            //startPoint = null;
            //endPoint = null;
        }

    }

    class SegmentSpline : Segment
    {
        private const double distCritical = 0.5;

        public event EventHandler DistanceIsCritical;


        private double _t;

        public double t
        {
            get
            {
                return _t;
            }
            set
            {
                if (value != _t)
                {
                    _t = value;
                }
            }
        }

        private double _distance;

        public double distance
        {
            get
            {
                return _distance;
            }
            set
            {
                if (_distance != value)
                {
                    _distance = value;
                    if ((_distance > distCritical) && (DistanceIsCritical != null))
                        DistanceIsCritical(this, EventArgs.Empty);
                }
            }
        }

        
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


        public SegmentSpline()
        {
            t = 0.0;
            distance = 0.0;
        }

        ~SegmentSpline()
        {
            Unsubscribe();
        }
        

        protected override void Update()
        {
            if ((spline != null) & (startPoint != null) & (endPoint != null))
            {
                spline.UpdateSegment(this);
            }
        }

        
        private void OnSplineChanged(object sender, EventArgs arg)
        {
            Update();
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            spline = null;
        }
    }


}
