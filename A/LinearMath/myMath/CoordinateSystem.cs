using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearMath
{
    public class CoordinateSystem : ICloneable
    {
        private int dim;

        private CoordinateSystem bcs;

        /// <summary>
        /// Reference to base coordinate system
        /// </summary>
        public CoordinateSystem BaseCS
        {
            get
            {
                return bcs;
            }
            set
            {
                bcs = value;
            }
        }

        private Vector shift;

        /// <summary>
        /// Center in base coordinate system
        /// </summary>
        public Vector Shift
        {
            get
            {
                return shift;
            }
            protected set
            {
                shift = value;
            }
        }

        /// <summary>
        /// Orts of current coordinate system in base 
        /// </summary>
        private Vector[] ort;

        /// <summary>
        /// X0 coordinate ort. Cloned object
        /// </summary>
        protected Vector ortX
        {
            get
            {
                return ort[0];
            }
            set
            {
                ort[0] = value;
            }
        }

        /// <summary>
        /// Y0 coordinate ort. Cloned object
        /// </summary>
        protected Vector ortY
        {
            get
            {
                return ort[1];
            }
            set
            {
                ort[1] = value;
            }
        }

        /// <summary>
        /// Z0 coordinate ort. Cloned object
        /// </summary>
        protected Vector ortZ
        {
            get
            {
                return ort[2];
            }
            set
            {
                ort[2] = value;
            }
        }

        /// <summary>
        /// Set unit axis
        /// </summary>
        private void InitAxis()
        {
            this.ort = new Vector[dim];
            for (int n = 0; n < dim; n++)
            {
                ort[n] = new Vector(dim);
                ort[n][n] = 1.0;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bcs">Reference to base coordinate system. Null for global base system</param>
        /// <param name="quaternion">Rotation quaternion. Null for non-rotated system</param>
        public CoordinateSystem(CoordinateSystem bcs, Quaternion quaternion, Vector shift)
        {
            this.dim = 3;
            InitAxis();

            if (bcs == null)
            {
                shift = new Vector(this.dim);
                return;
            }

            this.bcs = bcs;
            this.shift = shift;

            if (quaternion != null)
                this.rotate(quaternion);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bcs">Base coordinate system</param>
        /// <param name="X0">X0 ort</param>
        /// <param name="Y0">Vector lie in x0y plain</param>
        /// <param name="shift">Start point coordinate in base coordinate system</param>
        public CoordinateSystem(CoordinateSystem bcs, Vector X0, Vector Y0, Vector shift)
        {
            this.dim = 3;
            InitAxis();

            this.bcs = bcs;
            this.shift = shift;

            ort[0] = (Vector)X0.Clone();
            ort[2] = X0 % Y0;
            ort[1] = ort[2] % ort[0];

            foreach(var v in ort)
                v.normalize();
        }

        /// <summary>
        /// Void constructor
        /// </summary>
        protected CoordinateSystem()
        {
            this.dim = 3;
            InitAxis();
            shift = new Vector(dim);
        }

        /// <summary>
        /// Rotate curent coordinate system from base
        /// </summary>
        /// <param name="quaternion">Rotation quaternion</param>
        public void rotate(Quaternion quaternion)
        {
            //InitAxis();
            foreach (var v in ort)
                quaternion.rotate(v);
        }

        /// <summary>
        /// Create convert matrix
        /// </summary>
        /// <returns></returns>
        private SquareMatrix ToBaseMatrix()
        {
            SquareMatrix matrix = new SquareMatrix(3);

            for (int n = 0; n < dim; n++)
            {
                for (int m = 0; m < dim; m++)
                {
                    matrix[n, m] = ort[m][n];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Convert vector to base system
        /// </summary>
        /// <param name="vector">Vector in curent system</param>
        /// <returns>Vector in base system</returns>
        public Vector ConvertPointToBase(Vector vector)
        {
            if (this.bcs == null)
                return null;

            return this.ConvertVectorToBase(vector) + this.shift;
        }

        public Vector ConvertPointFromBase(Vector vector)
        {
            if (this.bcs == null)
                return null;

            return this.ConvertVectorFromBase(vector - this.shift);
        }

        /// <summary>
        /// Convert vector to base coordinate system
        /// </summary>
        /// <param name="vector">Vector in current system</param>
        /// <returns>Vector in base system</returns>
        public Vector ConvertVectorToBase(Vector vector)
        {
            if (this.bcs == null)
                return null;

            return this.ToBaseMatrix() * vector;
        }

        /// <summary>
        /// Convert vector from base coordinate system to current
        /// </summary>
        /// <param name="vector">Vector in base coordinate system</param>
        /// <returns>Vector in current coordinate system</returns>
        public Vector ConvertVectorFromBase(Vector vector)
        {
            if (this.bcs == null)
                return null;

            return this.ToBaseMatrix().invert() * vector;
        }

        /// <summary>
        /// Return angles [deg] of current vector with orts
        /// </summary>
        /// <param name="vector">Vector in base coordinate system</param>
        /// <returns>Vector of angles values</returns>
        public double[] AnglesWithOrts(Vector vector)
        {
            double[] ang = new double[dim];
            double V_abs = vector.Length();
            for (int n = 0; n < dim; n++)
            {
                ang[n] = Math.Acos(ort[n] * vector / V_abs) * 180.0 / Math.PI;
            }
            return ang;
        }

        /// <summary>
        /// Angles vector projection with orts
        /// </summary>
        /// <param name="vector">Vector in base coordinate system</param>
        /// <param name="AxisIndex">Axis index: 0 = X, 1 = Y, 2 = Z</param>
        /// <returns>Angles [deg]. Element's index relates to axis number. 
        /// Element with the same number is angle to plane like 90.0 - AnglesWithOrts </returns>
        public double[] AnglesProjectionWithPlanes(Vector vector, int AxisIndex)
        {
            if (AxisIndex >= dim)
                return null;

            double[] ang = new double[dim];
            Vector v = (Vector)vector.Clone();
            v.normalize();

            double[] P = new double[3];
            int k = 0;
            foreach (var p in ort)
            {
                P[k] = v * p;
                k++;
            }

            Func<double, double> gate = (x) => 
                {
                    if (Math.Abs(x) > 1.0)
                        return Math.Sign(x);
                    else 
                        return x;
                };


            for (int n = 0; n < dim; n++)
            {
                if (n == AxisIndex)
                {
                    ang[n] = 90.0 - 180.0 / Math.PI * Math.Acos(P[AxisIndex]);
                    continue;
                }
                ang[n] = 180.0 / Math.PI * Math.Acos(gate(P[AxisIndex] / Math.Sqrt(1.0 - P[n] * P[n])));
            }

            return ang;
        }

        /// <summary>
        /// Clone object
        /// </summary>
        /// <returns>Cloned coordinate system</returns>
        public object Clone()
        {
            CoordinateSystem cs = new CoordinateSystem();
            cs.bcs = this.bcs;
            cs.dim = this.dim;
            cs.shift = this.shift;

            cs.ort = new Vector[this.dim];
            for (int n = 0; n < this.dim; n++)
            {
                cs.ort[n] = (Vector)this.ort[n].Clone();
            }

            return cs;
        }

        /// <summary>
        /// Representation as string
        /// </summary>
        /// <returns>Shift vector and orts</returns>
        public override string ToString()
        {
            string str = String.Empty;
            str += this.Shift.ToString() + "    \n";
            str += this.ortX.ToString() + "   \n";
            str += this.ortY.ToString() + "   \n";
            str += this.ortZ.ToString() + "\n";
            return str;
        }

        /// <summary>
        /// Contain point coordinates in spherical system
        /// </summary>
        public struct PointSpherical
        {
            double phi;
            double theta;
            double r;

            /// <summary>
            /// Angle in horizontal plane to x axis [deg]
            /// </summary>
            public double Phi
            {
                get
                {
                    return phi;
                }
                set
                {
                    phi = value;
                    if (Math.Abs(phi) > 180.0)
                        phi = 180.0 * Math.Sign(phi);
                }
            }

            /// <summary>
            /// Angle in vertical plane to z axis [deg]
            /// </summary>
            public double Theta
            {
                get
                {
                    return theta;
                }
                set
                {
                    theta = value;
                    if (Math.Abs(theta) > 90.0)
                        theta = 90.0 * Math.Sign(theta);
                }
            }

            /// <summary>
            /// Distance to center [m]
            /// </summary>
            public double R
            {
                get
                {
                    return r;
                }
                set
                {
                    r = value;
                    if (r < 0.0)
                        r = 0.0;
                }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="Phi">Angle in horizontal plane to x axis [deg]</param>
            /// <param name="Theta">Angle in vertical plane to z axis [deg]</param>
            /// <param name="R">Distance to center [m]</param>
            public PointSpherical(double Phi, double Theta, double R)
            {
                this.phi = 0;
                this.theta = 0;
                this.r = 0;

                this.Phi = Phi;
                this.Theta = Theta;
                this.R = R;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="vector">Vector in cartesian coordinate system</param>
            public PointSpherical(Vector vector)
            {
                this.phi = 0;
                this.theta = 0;
                this.r = 0;

                Vector v = (Vector)vector.Clone();
                this.R = v.Length();
                v.normalize();
                if (Math.Abs(v[2]) > 1.0)
                    v[2] = Math.Sign(v[2]);
                this.Theta = Math.Asin(v[2]) * 180.0 / Math.PI;
                double cos_theta = Math.Sqrt(1.0 - Math.Pow(v[2], 2.0));
                double x = v[0] / cos_theta;
                if (Math.Abs(x) > 1.0)
                    x = Math.Sign(x);
                this.Phi = Math.Sign(v[1]) * Math.Acos(x) * 180.0 / Math.PI;
            }

            /// <summary>
            /// Convert point to cartesian vector. Z is vertical
            /// </summary>
            /// <returns>Vector in cartesian system</returns>
            public Vector ToCartesian()
            {
                double phi = this.phi * Math.PI / 180.0;
                double theta = this.theta * Math.PI / 180.0;
                double r = this.r;

                double sin_phi = Math.Sin(phi);
                double cos_phi = Math.Cos(phi);
                double sin_theta = Math.Sin(theta);
                double cos_theta = Math.Cos(theta);

                double x = cos_theta * cos_phi;
                double y = cos_theta * sin_phi;
                double z = sin_theta;

                return Vector.Create3D(x, y, z) * r;
            }

            /// <summary>
            /// Convert to string
            /// </summary>
            /// <returns>Values</returns>
            public override string ToString()
            {
                string str = String.Empty;
                str += "Phi = " + this.phi + "\n";
                str += "Theta = " + this.theta + "\n";
                str += "R = " + this.r;
                return str;
            }
        }

        public void Refine()
        {
            foreach (var v in ort)
                v.Refine();
        }
    }
}
