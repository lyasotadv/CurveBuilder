using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MshToMatWPF.Geometry.Spline;

namespace MshToMatWPF.Geometry.Point
{
    public enum LinePointAdditionMethod { none, first, last, between, intoSpline };

    class AddPointComponent
    {
        private IAddPointStrategy _strategy;

        public LinePoint lcp { get; set; }

        public ILinePointCollection collection { get; set; }

        public IAddPointStrategy strategy
        {
            get
            {
                return _strategy;
            }
            set
            {
                _strategy = value;
                if (_strategy != null)
                    _strategy.component = this;
            }
        }

        public AddPointComponent()
        {
            lcp = null;
            strategy = null;
            collection = null;
        }

        public void Build()
        {
            if (lcp == null)
                return;

            if (strategy == null)
                return;

            if (collection == null)
                return;

            strategy.Update();

            if (strategy.next != null)
                lcp.next = strategy.next;

            if (strategy.prev != null)
                lcp.prev = strategy.prev;
        }

        public bool Complite()
        {
            if ((strategy.prev == null) & (strategy.next == null))
                return false;
            return true;
        }
    }

    
    interface ILinePointCollection
    {
        LinePoint this[int n] { get; }

        LinePoint Find(Predicate<LinePoint> pred);
    }

    class LinePointCollection<T> : ILinePointCollection
    {
        private List<T> pointList;

        public LinePoint this[int n]
        {
            get
            {
                return pointList[n] as LinePoint;
            }
        }

        public LinePointCollection(List<T> pointList)
        {
            this.pointList = pointList;
        }

        public LinePoint Find(Predicate<LinePoint> pred)
        {
            foreach (var lcp in pointList)
            {
                if (pred(lcp as LinePoint))
                    return lcp as LinePoint;
            }
            return null;
        }
    }

    interface IAddPointStrategy
    {
        LinePoint prev { get; }

        LinePoint next { get; }

        AddPointComponent component { get; set; }

        void Update();
    }

    
    class AddPointStrategyFirst : IAddPointStrategy
    {
        public LinePoint prev { get; private set; }

        public LinePoint next { get; private set; }

        public AddPointComponent component { get; set; }

        public void Update()
        {
            prev = null;
            next = component.collection.Find((p) => { return p.prev == null; });
        }
    }

    class AddPointStrategyLast : IAddPointStrategy
    {
        public LinePoint prev { get; private set; }

        public LinePoint next { get; private set; }

        public AddPointComponent component { get; set; }

        public void Update()
        {
            prev = component.collection.Find((p) => { return p.next == null; });
            next = null;
        }
    }

    class AddPointStrategyIntoSpline : IAddPointStrategy
    {
        public LinePoint prev { get; private set; }

        public LinePoint next { get; private set; }

        public AddPointComponent component { get; set; }

        public void Update()
        {
            if (component.lcp is LinePointSegmentSpline)
            {
                LinePointSegmentSpline lcp = component.lcp as LinePointSegmentSpline;

                Predicate<LinePoint> pred = (p) =>
                    {
                        if (p.next == null)
                            return false;

                        if (p is LinePointSegmentSpline)
                        {
                            LinePointSegmentSpline splA = p as LinePointSegmentSpline;
                            LinePointSegmentSpline splB = p.next as LinePointSegmentSpline;

                            if (splA.spline != splB.spline)
                                return ((lcp.spline == splA.spline) & (lcp.t > splA.t)) | ((lcp.spline == splB.spline) & (lcp.t < splB.t));
                            else
                                return (lcp.spline == splA.spline) & (lcp.t > splA.t) & (lcp.t < splB.t);
                        }
                        return false;
                    };

                LinePoint lcp0 = component.collection.Find(pred);
                if (lcp0 != null)
                {
                    next = lcp0.next;
                    prev = lcp0;
                    return;
                }

                pred = (p) =>
                    {
                        return (p.prev == null) & ((p as LinePointSegmentSpline).spline == lcp.spline)
                            & ((p as LinePointSegmentSpline).t > lcp.t);
                    };
                lcp0 = component.collection.Find(pred);
                if (lcp0 != null)
                {
                    next = lcp0;
                    return;
                }

                pred = (p) =>
                {
                    return (p.next == null) & ((p as LinePointSegmentSpline).spline == lcp.spline)
                        & ((p as LinePointSegmentSpline).t < lcp.t);
                };
                lcp0 = component.collection.Find(pred);
                if (lcp0 != null)
                {
                    prev = lcp0;
                    return;
                }
            }
        }
    }
    
    class LinePointIterator<T> where T : LinePoint
    {
        private enum State { ready, inprogress, finished}

        private State state;

        private List<T> pointList;

        private T _next;

        public T Current { get; private set; }

        public LinePointIterator(List<T> pointList)
        {
            this.pointList = pointList;
            Reset();
        }

        public void Reset()
        {
            state = State.ready;
            Current = null;
            _next = null;
        }

        public bool Next()
        {
            switch (state)
            {
                case State.ready:
                    {
                        Current = pointList.Find((p) => { return (p as LinePoint).prev == null; });
                        state = State.inprogress;
                        break;
                    }
                case State.inprogress:
                    {
                        Current = _next;
                        break;
                    }
                case State.finished:
                    {
                        Current = null;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (Current == null)
                state = State.finished;
            else
                _next = (Current as LinePoint).next as T;

            return state != State.finished;
        }
    }

    class ExIncorrectPointAddition : Exception
    {
        public override string Message
        {
            get
            {
                return "Incorrect addition of point";
            }
        }
    }
}
