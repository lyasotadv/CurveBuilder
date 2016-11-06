using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearMath
{
    public class Plane
    {
        Vector n;
        Vector p;

        public Vector Normal
        {
            get
            {
                return n;
            }
            set
            {
                n = value;
                n.normalize();
            }
        }

        public Vector Point
        {
            get
            {
                return p;
            }
            set
            {
                p = value;
            }
        }

        public Plane(Vector p, Vector n)
        {
            this.p = p;
            this.n = n;
        }

        public Plane(Vector p, Vector a, Vector b)
        {
            this.p = p;
            this.n = a % b;
        }

        /// <summary>
        /// Angle between vector and normal [deg]
        /// </summary>
        /// <param name="v">Input vector</param>
        /// <returns>Angle [geg]</returns>
        public double Angle(Vector v)
        {
            double c = v * n / v.Length();
            double  a = Math.Acos(Math.Abs(c));
            return a * 180.0 / Math.PI;
        }

        public bool InOneSide(Vector p1, Vector p2)
        {
            Vector v1 = p1 - p;
            Vector v2 = p2 - p;

            if ((v1 * n) * (v2 * n) > 0.0)
                return true;
            return false;
        }

        public Vector Projection(Vector v)
        {
            return v - n * (v * n);
        }
    }
}
