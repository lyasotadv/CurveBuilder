using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MshToMatWPF.Geometry;
using LinearMath;
using MshToMatWPF.Geometry.Point;

namespace MshToMatWPF.Geometry.Spline
{
    class Spline3rd : SplineHermite
    {
        
        public Spline3rd()
            : base(3)
        {
            phi[0] = (t) => { return 1.0; };
            phi[1] = (t) => { return t; };
            phi[2] = (t) => { return t * t; };
            phi[3] = (t) => { return t * t * t; };

            phis[0] = (t) => { return 0.0; };
            phis[1] = (t) => { return 1.0; };
            phis[2] = (t) => { return 2.0 * t; };
            phis[3] = (t) => { return 3.0 * t * t; };

            for (int n = 0; n <= order; n++)
            {
                a[n] = new Vertex3D();
            }

            Init();
        }

        public override List<LinePointSegmentSpline> Intersection(Vector pnt, Vector tau)
        {
            Vector z0 = Vector.Create3D(0.0, 0.0, 1.0);

            double[] A = new double[order + 1];
            for (int n = 0; n <= order; n++)
            {
                A[n] = z0 * (base.a[n].q % tau);
            }

            double B = z0 * (pnt % tau);

            double a = A[3];
            double b = A[2];
            double c = A[1];
            double d = A[0] - B;

            double[] t = null;

            if (a != 0.0)
            {

                double Q = (3.0 * a * c - b * b) / (9.0 * a * a);
                double R = (9.0 * a * b * c - 27.0 * a * a * d - 2.0 * b * b * b) / (54.0 * a * a * a);
                double D = Q * Q * Q + R * R;

                
                if (D == 0)
                {
                    t = new double[2];

                    t[0] = 2.0 * Math.Pow(R, 1.0 / 3.0) - b / (3.0 * a);
                    t[1] = -Math.Pow(R, 1.0 / 3.0) - b / (3.0 * a);
                }

                if (D > 0)
                {
                    t = new double[1];

                    double S = Math.Pow(R + Math.Sqrt(D), 1.0 / 3.0);
                    double T = Math.Pow(R - Math.Sqrt(D), 1.0 / 3.0);

                    t[0] = S + T - b / (3.0 * a);
                }

                if (D < 0)
                {
                    t = new double[3];

                    double E = Math.Sqrt(-D);
                    double cos_phi = R / Math.Sqrt(R * R + E * E);
                    double phi = Math.Acos(cos_phi);

                    double C = Math.Pow(R * R + E * E, 1.0 / 6.0);
                    double p = C * Math.Cos(phi / 3.0);
                    double q = C * Math.Sin(phi / 3.0);

                    double sqrt3 = Math.Sqrt(3.0);

                    t[0] = 2.0 * p - b / (3.0 * a);
                    t[1] = -p - sqrt3 * q - b / (3.0 * a);
                    t[2] = -p + sqrt3 * q - b / (3.0 * a);
                }
            }
            else if (b != 0.0)
            {
                a = b;
                b = c;
                c = d;

                double D = b * b - 4 * a * c;
                if (D < 0.0)
                {
                    t = new double[0];
                }

                if (D == 0.0)
                {
                    t = new double[1];
                    t[0] = -b / (2.0 * a);
                }

                if (D > 0.0)
                {
                    t = new double[2];
                    double sqrtD = Math.Sqrt(D);
                    t[0] = (-b - sqrtD) / (2.0 * a);
                    t[1] = (-b - sqrtD) / (2.0 * a);
                }
            }
            else if (c != 0.0)
            {
                t = new double[1];
                t[0] = -d / c;
            }
            else
            {
                t = new double[0];
            }

            double eps = 1E-6;
            List<LinePointSegmentSpline> lst = new List<LinePointSegmentSpline>();
            foreach (var tp in t)
            {
                if ((tp >= 0.0 - eps) & (tp <= 1.0 + eps))
                {
                    LinePointSegmentSpline lcp = new LinePointSegmentSpline();
                    lcp.spline = this;
                    lcp.t = tp;
                    lst.Add(lcp);
                }
            }

            return lst;
        }
    }
}
