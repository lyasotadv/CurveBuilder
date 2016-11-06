using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearMath
{
    /// <summary>
    /// Update: rotate
    /// </summary>
    public class Quaternion : ICloneable
    {
        private double _eps;

        /// <summary>
        /// Precision. Default is 1E-6
        /// </summary>
        public double eps
        {
            get
            {
                return _eps;
            }
            set
            {
                _eps = value;
                this.vector.eps = this._eps;
            }
        }

        double scalar;

        Vector vector;

        /// <summary>
        /// Scalar part
        /// </summary>
        public double ScalarPart
        {
            get
            {
                return this.scalar;
            }
        }

        /// <summary>
        /// Vector part (ref)
        /// </summary>
        public Vector VectorPart
        {
            get
            {
                return vector;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scalar">Scalar part</param>
        /// <param name="vector">Vector part</param>
        public Quaternion(double scalar, Vector vector)
        {
            if (vector.Dimension != 3)
            {
                this.scalar = 0.0;
                this.vector = new Vector(0);
                return;
            }

            this.scalar = scalar;
            this.vector = vector;

            this.eps = 1E-6;
        }

        /// <summary>
        /// Constructor. Normalized quaternion
        /// </summary>
        /// <param name="axis">Axis. Non-normilized</param>
        /// <param name="angle">Angle [deg]</param>
        public Quaternion(Vector axis, double angle)
        {
            angle = angle * Math.PI / 180.0;

            this.scalar = Math.Cos(angle / 2.0);
            this.vector = (Math.Sin(angle / 2.0) / axis.Length()) * axis;

            this.eps = 1E-6;
        }

        /// <summary>
        /// Constructor. Normilized quaternion
        /// </summary>
        /// <param name="vect">Vector length is rotation angle [rad]. Axis is vector</param>
        public Quaternion(Vector vect)
        {
            double eps = 1E-6;
            double angle = -vect.Length();

            if (Math.Abs(angle) < eps)
            {
                this.scalar = 0.0;
                this.vector = Vector.Create3D(0.0, 0.0, 0.0);
            }
            else
            {
                this.scalar = Math.Cos(angle / 2.0);
                this.vector = -(Math.Sin(angle / 2.0) / angle) * vect;
            }
            this.eps = eps;
        }

        /// <summary>
        /// Quaternion multiplication
        /// </summary>
        /// <param name="quat1">Quaternoin 1</param>
        /// <param name="quat2">Quaternion 2</param>
        /// <returns>Product</returns>
        public static Quaternion operator *(Quaternion quat1, Quaternion quat2)
        {
            double scalar = quat1.scalar * quat2.scalar - quat1.vector * quat2.vector;

            Vector vector = quat1.scalar * quat2.vector + quat2.scalar * quat1.vector + quat1.vector % quat2.vector;

            return new Quaternion(scalar, vector);
        }

        /// <summary>
        /// Quaternoin inversion
        /// </summary>
        /// <returns>Invered quaternion</returns>
        public Quaternion invert()
        {
            double leng = this.Length();
            double k = 1.0 / (leng * leng);

            return new Quaternion(k * this.scalar, -k * this.vector);
        }

        /// <summary>
        /// Quaternion length
        /// </summary>
        /// <returns></returns>
        public double Length()
        {
            return Math.Sqrt(this.LengthSqr());
        }

        private double LengthSqr()
        {
            return this.scalar * this.scalar + this.vector.LengthSqr();
        }

        /// <summary>
        /// Normialization
        /// </summary>
        public void normilize()
        {
            double leng = this.Length();

            this.scalar /= leng;

            this.vector = (1.0 / leng) * this.vector;
        }

        /// <summary>
        /// Rotate vector
        /// </summary>
        /// <param name="vector">Rotated vector reference</param>
        public void rotate(Vector vector)
        {
            if (this.LengthSqr() < eps)
                return;
            vector = vector.rotate(this);
        }

        /// <summary>
        /// Verificate quaternion to empty
        /// </summary>
        /// <returns>Empty flag</returns>
        public bool IsEmpty()
        {
            if (this.scalar != 0.0)
                return false;

            if (!this.vector.IsEmpty())
                return false;

            return true;
        }

        /// <summary>
        /// Equality verification
        /// </summary>
        /// <param name="obj">Input object</param>
        /// <returns>Equalityt flag</returns>
        public override bool Equals(object obj)
        {
            Quaternion quat = obj as Quaternion;
            if (Math.Abs(this.scalar - quat.scalar) > this._eps)
                return false;

            if (!this.vector.Equals(quat.vector))
                return false;

            return true;
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Object clone
        /// </summary>
        /// <returns>Cloned quaternion</returns>
        public object Clone()
        {
            return new Quaternion(this.scalar, this.vector);
        }
    }
}
