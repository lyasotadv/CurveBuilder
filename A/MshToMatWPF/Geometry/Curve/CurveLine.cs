using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LinearMath;

namespace MshToMatWPF.Geometry.Curve
{
    abstract public class CurveLine
    {
        protected Func<double, double, double>[] phi { get; private set; }

        protected double[] a { get; private set; }

        protected double C { get; set; }

        protected int order { get; private set; }

        public CurveLine(int order)
        {
            this.order = order;
            phi = new Func<double, double, double>[order];
            a = new double[order];
            C = 0.0;
        }

        public virtual double Calc(double x, double y)
        {
            double s = 0.0;
            for (int n = 0; n < order; n++)
            {
                s += a[n] * phi[n](x, y);
            }
            s -= C;
            return s;
        }

        public virtual void LQR(double[] X, double[] Y)
        {
            if (X.Length != Y.Length)
                return;

            int N = X.Length;
            int M = order;

            RectangleMatrix A = new RectangleMatrix(N, M);
            Vector B = new Vector(N);

            for (int n = 0; n < N; n++)
            {
                B[n] = C;
                for (int m = 0; m < M; m++)
                {
                    A[n, m] = phi[m](X[n], Y[n]);
                }
            }

            Vector V = A.LQR(B);
            for (int n = 0; n < order; n++)
            {
                a[n] = V[n];
            }
        }

        public double LinearCut(Edge2D e)
        {
            double x0 = e.V1.X;
            double y0 = e.V1.Y;

            double x1 = e.V2.X;
            double y1 = e.V2.Y;

            Func<double, double> Lin = (t) => 
            {
                Vertex2D v = e.Verteces2LinCut(t);
                return Calc(v.X, v.Y);
            };

            return Dihotomy(Lin, 0.0, 1.0, 1e-3);
        }

        private double Dihotomy(Func<double, double> f, double a, double b, double precision)
        {
            if (a >= b)
                return double.NaN;
            
            double A = f(a);
            double B = f(b);

            if (A * B >= 0.0)
                return double.NaN;

            while (true)
            {
                double c = (a + b) / 2.0;
                double C = f(c);

                if (C == 0.0)
                    return c;

                if (b - a < precision)
                    return c;

                if (A * C < 0.0)
                {
                    b = c;
                    B = C;
                    continue;
                }
                if (B * C < 0.0)
                {
                    a = c;
                    A = C;
                    continue;
                }
            }
        }
    }
}
