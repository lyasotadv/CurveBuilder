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
using LinearMath;

namespace MshToMatWPF.Drawing
{
    class SegmentLineViewer : LineViewerBase
    {
        private List<SplineSegmentLinePoint> _point;

        private List<SegmentSpline> _segment;


        public SegmentLineViewer()
            : base()
        {
            _point = new List<SplineSegmentLinePoint>();
            _segment = new List<SegmentSpline>();
        }


        private void RemoveFromAllCollections(Segment seg)
        {
            if (seg == null)
                return;

            IPrimitive primFound = primitiveList.Find((prim) =>
            {
                return (prim is Tube)
                    & ((prim as Tube).StartVector == (seg.startPoint as LinePoint).vert.q)
                    & ((prim as Tube).EndVector == (seg.endPoint as LinePoint).vert.q);
            });

            if (primFound == null)
                return;

            modelgroup.Children.Remove(primFound.model);
            primitiveList.Remove(primFound);
            _segment.Remove(seg as SegmentSpline);
        }

        private void AddTube(SegmentSpline seg)
        {
            TubeIncline tube = new TubeIncline();
            tube.StartVector = ((SplineLinePoint)seg.startPoint).vert.q;
            tube.StartNormal = ((SplineLinePoint)seg.startPoint).tau;

            tube.EndVector = ((SplineLinePoint)seg.endPoint).vert.q;
            tube.EndNormal = ((SplineLinePoint)seg.endPoint).tau;

            primitiveList.Add(tube);
            modelgroup.Children.Add(tube.model);

            if (colorManager != null)
                tube.color = colorManager.spline;
            tube.Selected += OnPrimitiveSelected;
        }

        private void AddSegment(SplineSegmentLinePoint lcp)
        {
            if (lcp == null)
                return;

            if (lcp.next != null)
            {
                RemoveFromAllCollections(((SplineSegmentLinePoint)lcp.next).inSeg);

                SegmentSpline seg = new SegmentSpline();
                seg.startPoint = lcp as ISegmentPoint;
                seg.endPoint = lcp.next as ISegmentPoint;
                seg.spline = lcp.spline;
                _segment.Add(seg);

                AddTube(seg);
            }

            if (lcp.prev != null)
            {
                RemoveFromAllCollections(((SplineSegmentLinePoint)lcp.prev).outSeg);
                
                SegmentSpline seg = new SegmentSpline();
                seg.startPoint = lcp.prev as ISegmentPoint;
                seg.endPoint = lcp as ISegmentPoint;
                seg.spline = lcp.spline;
                _segment.Add(seg);

                AddTube(seg);
            }
        }

        
        protected override bool ConfirmCollection()
        {
            return true;
        }


        public override void Refine()
        {
            double eps = 1.0;

            int itertion = 0;
            while (true)
            {
                List<SplineSegmentLinePoint> pointDel = new List<SplineSegmentLinePoint>();

                foreach (var p in _point)
                {

                    if (p.distance < eps)
                    {
                        pointDel.Add(p);
                    }
                }

                foreach (var p in pointDel)
                    DeleteVertex(p);

                itertion++;
                if ((pointDel.Count == 0) | (itertion > 10))
                    break;
            }

            itertion = 0;
            while (true)
            {
                List<SplineSegmentLinePoint> pointAdd = new List<SplineSegmentLinePoint>();

                foreach (var s in _segment)
                {
                    if (s.distance > eps)
                    {
                        SplineSegmentLinePoint lcp = new SplineSegmentLinePoint();
                        lcp.spline = s.spline;
                        lcp.t = s.t;
                        pointAdd.Add(lcp);
                    }
                }

                foreach (var p in pointAdd)
                {
                    AddVertex(p, LinePointAdditionMethod.intoSpline);
                }

                itertion++;
                if ((pointAdd.Count == 0) | (itertion > 10))
                    break;
            }
        }

        public override void AddVertex(SplineLinePoint lcp, LinePointAdditionMethod method)
        {
            if (lcp == null)
                return;

            if (!ConfirmCollection())
                return;

            if (lcp is SplineSegmentLinePoint)
            {
                SplineSegmentLinePoint lcpNew = lcp as SplineSegmentLinePoint;

                AddPointComponent component = new AddPointComponent();
                component.lcp = lcpNew;
                component.collection = new LinePointCollection<SplineSegmentLinePoint>(_point);
                switch (method)
                {
                    case LinePointAdditionMethod.last:
                        {
                            component.strategy = new AddPointStrategyLast();
                            break;
                        }
                    case LinePointAdditionMethod.first:
                        {
                            component.strategy = new AddPointStrategyFirst();
                            break;
                        }
                    case LinePointAdditionMethod.intoSpline:
                        {
                            component.strategy = new AddPointStrategyIntoSpline();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                component.Build();

                if ((!component.Complite()) & (_point.Count > 1))
                    return;

                _point.Add(lcpNew);

                AddSegment(lcpNew);
            }
        }

        public override void DeleteVertex(SplineLinePoint lcp)
        {
            if (lcp is SplineSegmentLinePoint)
            {
                SplineSegmentLinePoint lcpNew = lcp as SplineSegmentLinePoint;
                
                RemoveFromAllCollections(lcpNew.inSeg);
                RemoveFromAllCollections(lcpNew.outSeg);

                if ((lcp.prev != null) & (lcp.next != null))
                {
                    SegmentSpline seg = new SegmentSpline();
                    seg.startPoint = lcp.prev as ISegmentPoint;
                    seg.endPoint = lcp.next as ISegmentPoint;
                    seg.spline = lcp.spline;
                    _segment.Add(seg);

                    AddTube(seg);

                    lcp.prev.next = lcp.next;
                }

                lcpNew.spline = null;
                _point.Remove(lcpNew);
            }
        }
    }
}
