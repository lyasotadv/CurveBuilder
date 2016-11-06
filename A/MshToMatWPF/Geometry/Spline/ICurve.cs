using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MshToMatWPF.Geometry.Point;

namespace MshToMatWPF.Geometry.Spline
{
    interface ICurve
    {
        event EventHandler Changed;

        void UpdatePoint(LinePointSegment lcp);

        void UpdateSegment(Segment seg);
    }

    interface ISpline : ICurve
    {
        void AddControllPoint(LinePointControl lcp);

        void RemoveControllPoint(LinePointControl lcp);

        LinePointSegmentSpline CreatePoint(double t);
    }
}
