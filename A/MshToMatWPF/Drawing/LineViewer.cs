using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Windows.Input;

using MshToMatWPF.Drawing.Primitives;
using MshToMatWPF.Preferences;
using MshToMatWPF.Geometry.Point;

namespace MshToMatWPF.Drawing
{
    abstract class LineViewer<TPoint, TSegment> : Drawable
        where TPoint : LinePointSegment
        where TSegment : Segment
    {
        private double _r;

        public double r
        {
            get
            {
                return _r;
            }
            set
            {
                _r = value;
                foreach (var p in primitiveList)
                {
                    if (p is Tube)
                        (p as Tube).r = r;
                }
            }
        }

        protected List<TPoint> pointList { get; private set; }

        protected List<TSegment> segmentList { get; private set; }

        public LineViewer()
        {
            pointList = new List<TPoint>();
            segmentList = new List<TSegment>();
        }


        protected virtual void AddSegment(TSegment seg)
        {
            if (seg != null)
            {
                TubeIncline tube = new TubeIncline();

                tube.r = r;
                tube.color = colorManager.spline;

                tube.StartVector = seg.startPoint.vert.q;
                tube.EndVector = seg.endPoint.vert.q;

                tube.StartNormal = seg.startPoint.tau;
                tube.EndNormal = seg.endPoint.tau;

                AddPrimitive(tube);

                segmentList.Add(seg);
            }
        }

        protected virtual void RemoveSegment(TSegment seg)
        {
            if (seg != null)
            {
                IPrimitive prim = primitiveList.Find((p) => 
                                                    {
                                                        return (p is Tube) && ((p as Tube).StartVector == seg.startPoint.vert.q &
                                                                (p as Tube).EndVector == seg.endPoint.vert.q);
                                                    }
                                                    );
                if (prim != null)
                {
                    RemovePrimitive(prim);
                }

                segmentList.Remove(seg);
                seg.Unsubscribe();
            }
        }


        protected abstract TSegment CreateSegment(TPoint startPoint, TPoint endPoint);
        
        protected override void OnPrimitiveSelected(object sender, EventArgs arg)
        {
            if ((sender is IPrimitive) & (colorManager != null))
            {
                if ((sender as IPrimitive).IsSelected)
                    (sender as IPrimitive).color = colorManager.splineSelected;
                else
                    (sender as IPrimitive).color = colorManager.spline;
            }

            base.OnPrimitiveSelected(sender, arg);
        }

        protected virtual void AddVertexIntoCollection(TPoint lcp, LinePointAdditionMethod method)
        {
            AddPointComponent comp = new AddPointComponent();
            comp.collection = new LinePointCollection<TPoint>(pointList);
            comp.lcp = lcp;
            switch (method)
            {
                case LinePointAdditionMethod.last:
                    {
                        comp.strategy = new AddPointStrategyLast();
                        break;
                    }
                case LinePointAdditionMethod.first:
                    {
                        comp.strategy = new AddPointStrategyFirst();
                        break;
                    }
            }
            comp.Build();
        }


        public abstract void Refine();

        public virtual void AddVertex(TPoint lcp, LinePointAdditionMethod method)
        {
            AddVertexIntoCollection(lcp, method);

            pointList.Add(lcp);

            if ((lcp.prev != null) & (lcp.next != null))
            {
                TSegment seg = (lcp.prev as LinePointSegment).outSeg as TSegment;
                RemoveSegment(seg);
            }

            if (lcp.prev != null)
            {
                TSegment seg = CreateSegment(lcp.prev as TPoint, lcp as TPoint);
                AddSegment(seg);
            }

            if (lcp.next != null)
            {
                TSegment seg = CreateSegment(lcp as TPoint, lcp.next as TPoint);
                AddSegment(seg);
            }
        }

        public virtual void RemoveVertex(TPoint lcp)
        {
            pointList.Remove(lcp);

            TPoint lcpPrev = lcp.prev as TPoint;
            TPoint lcpNext = lcp.next as TPoint;

            TSegment inSeg = lcp.inSeg as TSegment;
            TSegment outSeg = lcp.outSeg as TSegment;

            RemoveSegment(inSeg);
            RemoveSegment(outSeg);

            lcp.Unsubcribe();

            if (lcpPrev != null & lcpNext != null)
            {
                lcpPrev.next = lcpNext;
                TSegment seg = CreateSegment(lcpPrev, lcpNext);
                AddSegment(seg);
            }
        }

        
        public void SetUpdateMode(LinearMath.UpdateMode mode)
        {
            foreach (var prim in primitiveList)
            {
                if (prim is TubeIncline)
                {
                    TubeIncline tube = prim as TubeIncline;

                    tube.StartVector.updateMode = mode;
                    tube.EndVector.updateMode = mode;
                    tube.StartVector.updateMode = mode;
                    tube.EndNormal.updateMode = mode;
                }
            }
        }

        public virtual bool ConfirmCollection()
        {
            return pointList.Count - segmentList.Count == 1;
        }
        
    }
}
