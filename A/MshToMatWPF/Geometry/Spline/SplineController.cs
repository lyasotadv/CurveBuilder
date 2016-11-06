using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using MshToMatWPF.Drawing;
using MshToMatWPF.Preferences;
using LinearMath;
using MshToMatWPF.Geometry.Point;

namespace MshToMatWPF.Geometry.Spline
{
    class SplineController
    {
        public ControlLinePointCloud controlPointCloud { get; private set; }

        public LineViewerSpline lineViewer { get; private set; }

        protected ISpline spline { get; private set; }


        public SplineController()
        {
            controlPointCloud = new ControlLinePointCloud();
            lineViewer = new LineViewerSpline();

            controlPointCloud.r = 5.0;
            lineViewer.r = 1.0;

            //spline = new SplineBetta();
            //(spline as SplineBetta).Dim = 1;
            spline = new SplineLinear();
        }


        private void OnControlPointUpdated(object sender, EventArgs arg)
        {
            //lineViewer.Refine();
        }

        private void FillLineViewer()
        {
            int N = 15;
            double dt = 1.0 / (N - 1);

            for (int n = 0; n < N; n++)
            {
                LinePointSegmentSpline lcp = spline.CreatePoint(n * dt);
                lineViewer.AddVertex(lcp, LinePointAdditionMethod.last);
            }
        }


        public void AddPoint(Vector pnt)
        {
            LinePointControl lcp = new LinePointControl();
            lcp.vert.q.CopyValue(pnt);
            spline.AddControllPoint(lcp);
            controlPointCloud.AddPoint(lcp, LinePointAdditionMethod.last);

            lcp.Updated += OnControlPointUpdated;

            if (controlPointCloud.Count == 2)
                FillLineViewer();
        }

        public void RemovePoint(LinePointControl pnt)
        {

        }

        public void SetColorManager(ColorManager colorManager)
        {
            lineViewer.colorManager = colorManager;
            controlPointCloud.colorManager = colorManager;
        }
    }
}
