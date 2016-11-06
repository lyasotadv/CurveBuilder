using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using MshToMatConverter.Geometry;

namespace MshToMatConverter
{
    class StepProcessing : Step
    {
        public StepProcessing()
            : base()
        {
            
        }

        public override void Start()
        {
            base.Start();
            form.geomManager = ((Mesh2DContainer)data).geomManager;
        }

        public override void Finish()
        {
            base.Finish();
            PointF[] p = form.mouseline;

            if (p.Length < 10)
                return;

            double[] X = new double[p.Length];
            double[] Y = new double[p.Length];

            for (int n = 0; n < p.Length; n++)
            {
                X[n] = p[n].X;
                Y[n] = p[n].Y;
            }


            CurveLine curve;
            switch (pref.Curve)
            {
                case Preferences.CurveType.SecondOrderCurve:
                    {
                        curve = new SecondOrderCurve();
                        break;
                    }
                case Preferences.CurveType.Circle:
                    {
                        curve = new Circle();
                        break;
                    }
                default:
                    {
                        curve = null;
                        break;
                    }
            }
            curve.LQR(X, Y);

            if ((pref.WeightFunctionCalculate) & (curve is Circle))
                ((Circle)curve).InitWeightFunc(pref.RadiusesDifference);

            ((Mesh2DContainer)data).CutLine(curve);
            PolyLineGroup2D polyline = ((Mesh2DContainer)data).GroupCutLine();
            polyline.Coloring();
            List<Edge2D> edgeSelected = polyline.Closest(X, Y);
            ((Mesh2DContainer)data).FilterCut(edgeSelected.ToArray());

            form.PlotCutLine();
        }

        public override string Name
        {
            get { return "Processing"; }
        }
    }
}
