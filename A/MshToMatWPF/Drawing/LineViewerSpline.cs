using System;
using System.Collections.Generic;

using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Windows.Input;

using MshToMatWPF.Drawing.Primitives;
using MshToMatWPF.Preferences;
using MshToMatWPF.Geometry.Point;
using MshToMatWPF.Geometry.Spline;
using LinearMath;

namespace MshToMatWPF.Drawing
{
    class LineViewerSpline : LineViewer<LinePointSegmentSpline, SegmentSpline>
    {
        private void OnPointCriticalDistance(object sender, EventArgs arg)
        {
            if (sender is LinePointSegmentSpline)
            {
                RemoveVertex(sender as LinePointSegmentSpline);
            }
        }

        private void OnSegmentCriticalDistance(object sender, EventArgs arg)
        {
            if (sender is SegmentSpline)
            {
                SplitSegment(sender as SegmentSpline);
            }
        }


        protected override SegmentSpline CreateSegment(LinePointSegmentSpline startPoint, LinePointSegmentSpline endPoint)
        {
            if ((startPoint != null & endPoint != null) && (startPoint.next == endPoint))
            {
                SegmentSpline segTemp = segmentList.Find((s) => { return (s.startPoint == startPoint & s.endPoint == endPoint); });
                if (segTemp != null)
                    return null;

                SegmentSpline seg = new SegmentSpline();
                seg.startPoint = startPoint;
                seg.endPoint = endPoint;

                if (startPoint.spline != endPoint.spline)
                    throw new ArgumentException();

                seg.spline = startPoint.spline;

                return seg;
            }
            else
                throw new ArgumentException();
        }

        protected override void AddVertexIntoCollection(LinePointSegmentSpline lcp, LinePointAdditionMethod method)
        {
            if (lcp != null)
            {
                if (method == LinePointAdditionMethod.intoSpline)
                {
                    AddPointComponent comp = new AddPointComponent();
                    comp.lcp = lcp;
                    comp.collection = new LinePointCollection<LinePointSegmentSpline>(pointList);
                    comp.strategy = new AddPointStrategyIntoSpline();
                    comp.Build();
                }
                else
                    base.AddVertexIntoCollection(lcp, method);
            }
        }

        protected void SplitSegment(SegmentSpline seg)
        {
            double tA = (seg.startPoint as LinePointSegmentSpline).t;
            double tB = (seg.endPoint as LinePointSegmentSpline).t;

            double t = (tA + tB) / 2.0;

            LinePointSegmentSpline lcp = (seg.startPoint as LinePointSegmentSpline).spline.CreatePoint(t);
            AddVertex(lcp, LinePointAdditionMethod.intoSpline);
        }

        protected override void AddSegment(SegmentSpline seg)
        {
            base.AddSegment(seg);
            if (seg != null)
            {
                seg.DistanceIsCritical += OnSegmentCriticalDistance;
            }
        }

        protected override void RemoveSegment(SegmentSpline seg)
        {
            base.RemoveSegment(seg);
            if (seg != null)
            {
                seg.DistanceIsCritical -= OnSegmentCriticalDistance;
            }
        }


        public override void AddVertex(LinePointSegmentSpline lcp, LinePointAdditionMethod method)
        {
            base.AddVertex(lcp, method);
            lcp.DistanceIsCritical += OnPointCriticalDistance;
        }

        public override void RemoveVertex(LinePointSegmentSpline lcp)
        {
            base.RemoveVertex(lcp);
            lcp.DistanceIsCritical -= OnPointCriticalDistance;
        }

        public override void Refine()
        {
            double EPS = 0.5;

            LinePointIterator<LinePointSegmentSpline> iter = new LinePointIterator<LinePointSegmentSpline>(pointList);
            while (iter.Next())
            {
                if (iter.Current.Dist < EPS)
                    RemoveVertex(iter.Current);
            }

            List<SegmentSpline> marked = new List<SegmentSpline>();
            do
            {
                marked.Clear();

                foreach (var seg in segmentList)
                {
                    if (seg.distance > EPS)
                    {
                        marked.Add(seg);
                    }
                }

                foreach (var seg in marked)
                {
                    double tA = (seg.startPoint as LinePointSegmentSpline).t;
                    double tB = (seg.endPoint as LinePointSegmentSpline).t;

                    double t = (tA + tB) / 2.0;

                    LinePointSegmentSpline lcp = (seg.startPoint as LinePointSegmentSpline).spline.CreatePoint(t);
                    AddVertex(lcp, LinePointAdditionMethod.intoSpline);
                }
            }
            while (marked.Count != 0);
        }

        public override bool ConfirmCollection()
        {
            return base.ConfirmCollection();
        }
    }
}
