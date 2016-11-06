using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LinearMath;

namespace MshToMatWPF.Geometry.Curve
{
    public struct WeightFunc2D
    {
        private double _q;
        private double _qx;
        private double _qy;

        public double q { get { return _q; } private set { _q = value; } }
        public double qx { get { return _qx; } private set { _qx = value; } }
        public double qy { get { return _qy; } private set { _qy = value; } }

        public WeightFunc2D(double Q, double Qx, double Qy)
        {
            _q = 0.0;
            _qx = 0.0;
            _qy = 0.0;

            _q = Q;
            _qx = Qx;
            _qy = Qy;
        }
    }

    class Circle : CurveLine
    {
        private double x0;
        private double y0;
        private double R;

        private Func<double, double, double> q;
        private Func<double, double, double> qx;
        private Func<double, double, double> qy;

        public Circle()
            : base(4)
        {
            phi[0] = (x, y) => { return x * x + y * y; };
            phi[1] = (x, y) => { return -2.0 * x; };
            phi[2] = (x, y) => { return -2.0 * y; };
            phi[3] = (x, y) => { return 1.0; };
            C = 0.0;

            q = null;
            qx = null;
            qy = null;
        }

        public override void LQR(double[] X, double[] Y)
        {
            if (X.Length != Y.Length)
                return;

            int N = X.Length;
            int M = order - 1;

            RectangleMatrix A = new RectangleMatrix(N, M);
            Vector B = new Vector(N);

            for (int n = 0; n < N; n++)
            {
                B[n] = -phi[0](X[n], Y[n]);
                for (int m = 0; m < M; m++)
                {
                    A[n, m] = phi[m + 1](X[n], Y[n]);
                }
            }

            Vector V = A.LQR(B);
            for (int n = 0; n < M; n++)
            {
                a[n + 1] = V[n];
            }

            a[0] = 1.0;

            x0 = a[1];
            y0 = a[2];

            R = Math.Sqrt(x0 * x0 + y0 * y0 - a[3]);
        }

        public double ValueInCenter()
        {
            return base.Calc(x0, y0);
        }

        public void InitWeightFunc(double diff)
        {
            if (diff + R < 0.0)
                diff = -R;

            double R1 = R + diff;
            Func<double, double, double> ro = (x, y) => { return Math.Sqrt((x - x0) * (x - x0) + (y - y0) * (y - y0)); };
            Func<double, double> q_center = (r) => { return (1.0 + Math.Cos(Math.PI * (r - R) / diff)) / 2.0; };
            q = (x, y) =>
                {
                    double r = ro(x, y);
                    if (r < R)
                        return 1.0;
                    if (r > R1)
                        return 0.0;
                    return q_center(r);
                };

            Func<double, double> qs = (r) => { return -Math.Sin(Math.PI * (r - R) / diff) * Math.PI / (diff * 2.0); };

            qx = (x, y) =>
                {
                    double r = ro(x, y);
                    if ((r > R) & (r < R1))
                    {
                        double s = qs(r);
                        return s * (x - x0) / r;
                    }
                    return 0.0;
                };

            qy = (x, y) =>
            {
                double r = ro(x, y);
                if ((r > R) & (r < R1))
                {
                    double s = qs(r);
                    return s * (y - y0) / r;
                }
                return 0.0;
            };
        }

        public WeightFunc2D CalcWeightFunc(double x, double y)
        {
            return new WeightFunc2D(q(x, y), qx(x, y), qy(x, y));
        }
    }
}
