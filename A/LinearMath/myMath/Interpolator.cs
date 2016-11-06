using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearMath
{
    public class Interpolator
    {
        double[] X; //argument
        double[] Y; //argument
        int Length; //number of points

        public enum ExtrapolatorType { None, Const }

        ExtrapolatorType _extrapolator;

        public ExtrapolatorType extrapolator
        {
            get
            {
                return _extrapolator;
            }
            set
            {
                _extrapolator = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="X">Argument array</param>
        /// <param name="Y">Value array</param>
        public Interpolator(double[] X, double[] Y)
        {
            extrapolator = ExtrapolatorType.None;

            Length = 0;
            if (X.Length != Y.Length)
                return;

            Length = X.Length;
            this.X = (double[])X.Clone();
            this.Y = (double[])Y.Clone();
        }

        /// <summary>
        /// Linaear interpolation without extrapolation
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value</returns>
        public double Linear(double x)
        {
            if (this.Length == 0)
                return double.NaN;

            for (int n = 0; n < Length; n++)
            {
                if (x == X[n])
                    return Y[n];

                if (n == 0)
                    continue;

                if ((x > X[n - 1]) & (x < X[n]))
                    return linear(X[n - 1], X[n], Y[n - 1], Y[n], x);
            }

            switch (extrapolator)
            {
                case ExtrapolatorType.None:
                    return double.NaN;

                case ExtrapolatorType.Const:
                    if (x < X[0])
                        return Y[0];
                    else if (x > X[Length - 1])
                        return Y[Length - 1];
                    else
                        return double.NaN;
            }

            return double.NaN;
        }

        public double Derivation(double x)
        {
            if (this.Length == 0)
                return double.NaN;

            for (int n = 1; n < Length; n++)
            {
                if ((x > X[n - 1]) & (x < X[n]))
                    return (Y[n] - Y[n - 1]) / (X[n] - X[n - 1]);
            }

            for (int n = 0; n < Length; n++)
            {
                if (x == X[n])
                {
                    if (n == 0)
                        return Derivation((X[n + 1] - X[n]) / 2.0);

                    if (n == Length - 1)
                        return Derivation((X[n] - X[n - 1]) / 2.0);

                    double ys1 = Derivation((X[n] - X[n - 1]) / 2.0);
                    double ys2 = Derivation((X[n + 1] - X[n]) / 2.0);
                    return (ys1 + ys2) / 2.0;
                }
            }

            switch (extrapolator)
            {
                case ExtrapolatorType.None:
                    return double.NaN;

                case ExtrapolatorType.Const:
                    if (x < X[0])
                        return Derivation((X[1] - X[0]) / 2.0);
                    else if (x > X[Length - 1])
                        return Derivation((X[Length - 1] - X[Length - 2]) / 2.0);
                    else
                        return double.NaN;
            }

            return double.NaN;
        }

        /// <summary>
        /// Linear interpolation foreach element in array
        /// </summary>
        /// <param name="x">Arguments array</param>
        /// <returns>Array of values</returns>
        public double[] Linear(double[] x)
        {
            int N = x.Length;
            double[] y = new double[N];
            for (int n = 0; n < N; n++)
                y[n] = Linear(x[n]);
            return y;
        }
        
        /// <summary>
        /// Linear interpolation by two points
        /// </summary>
        /// <param name="x1">Argument 1</param>
        /// <param name="x2">Argument 2</param>
        /// <param name="y1">Value 1</param>
        /// <param name="y2">Value 2</param>
        /// <param name="x">Argument</param>
        /// <returns>Value</returns>
        private double linear(double x1, double x2, double y1, double y2, double x)
        {
            if (x1 == x2)
                return double.NaN;
            return y1 + (x - x1) / (x2 - x1) * (y2 - y1);
        }
    }
}
