using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearMath
{
    /// <summary>
    /// Add: ToString
    /// Add: Create3D
    /// </summary>
    public class Vector : Matrix, ICloneable
    {
        private enum VectorType { vertical, horizontal}
        private VectorType vectortype;

        /// <summary>
        /// Vector length
        /// </summary>
        public int Dimension
        {
            get
            {
                switch (vectortype)
                {
                    case VectorType.vertical:
                        {
                            return rows;
                        }
                        //break;
                    case VectorType.horizontal:
                        {
                            return cols;
                        }
                        //break;
                    default:
                        {
                            return 0;
                        }
                        //break;
                }
            }
        }

        /// <summary>
        /// Constructor. Initialization by 0.0
        /// </summary>
        /// <param name="Dimension">Vector dimension</param>
        public Vector(int Dimension)
            : base(Dimension, 1, 0.0)
        {
            vectortype = VectorType.vertical;
        }

        private Vector() 
            : base()
        {

        }

        public static Vector Create3D(double x, double y, double z)
        {
            Vector v = new Vector(3);
            v[0] = x;
            v[1] = y;
            v[2] = z;
            return v;
        }

        /// <summary>
        /// Accessor to element
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public double this[int n]
        {
            get
            {
                switch (vectortype)
                {
                    case VectorType.vertical:
                        {
                            return base[n, 0];
                        }
                        //break;
                    case VectorType.horizontal:
                        {
                            return base[0, n];
                        }
                        //break;
                    default:
                        {
                            return 0.0;
                        }
                        //break;
                }
            }

            set
            {
                switch (vectortype)
                {
                    case VectorType.vertical:
                        {
                            base[n, 0] = value;
                        }
                    break;
                    case VectorType.horizontal:
                        {
                            base[0, n] = value;
                        }
                    break;
                    default:
                        {
                          
                        }
                    break;
                }
            }
        }

        /// <summary>
        /// Vector transposing
        /// </summary>
        protected override void transpose()
        {
            base.transpose();

            switch (vectortype)
            {
                case VectorType.vertical:
                    {
                        vectortype = VectorType.horizontal;
                    }
                    break;
                case VectorType.horizontal:
                    {
                        vectortype = VectorType.vertical;
                    }
                    break;
                default:
                    {
                    }
                    break;
            }
        }

        /// <summary>
        /// Transpose to selected type
        /// </summary>
        /// <param name="vectortype">Type</param>
        private void transpose(VectorType vectortype)
        {
            if (this.vectortype == vectortype)
                return;
            this.transpose();
        }

        /// <summary>
        /// Sum of two vectors
        /// </summary>
        /// <param name="vector1">Vector 1</param>
        /// <param name="vector2">Vector 2</param>
        /// <returns>Orientation like first. Create new object</returns>
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            Vector vector = new Vector();
            
            Matrix.add(vector1, vector2, vector);

            return vector;
        }

        /// <summary>
        /// Subtract of two vectors
        /// </summary>
        /// <param name="vector1">Vector 1</param>
        /// <param name="vector2">Vector 2</param>
        /// <returns>Orientation like first. Create new object</returns>
        public static Vector operator -(Vector vector1, Vector vector2)
        {
            Vector vector = new Vector();
            
            Matrix.sub(vector1, vector2, vector);

            return vector;
        }

        /// <summary>
        /// Negative vector
        /// </summary>
        /// <param name="vector">Direct vector</param>
        /// <returns>Oposite vector. Create new object</returns>
        public static Vector operator -(Vector vector)
        {
            return -1 * vector;
        }

        /// <summary>
        /// Matrix to vector product
        /// </summary>
        /// <param name="matrix">Matrix</param>
        /// <param name="vector">Vector</param>
        /// <returns>Result vector</returns>
        public static Vector operator *(SquareMatrix matrix, Vector vector)
        {
            Vector V = new Vector();
            Matrix.mult(matrix, vector, V);

            return V;
        }

        /// <summary>
        /// Rectangle matrix to vector product
        /// </summary>
        /// <param name="matrix">Matrix</param>
        /// <param name="vector">Vector</param>
        /// <returns>Result vector</returns>
        public static Vector operator *(RectangleMatrix matrix, Vector vector)
        {
            Vector V = new Vector();
            Matrix.mult(matrix, vector, V);
            return V;
        }

        /// <summary>
        /// Multiplicate by scalar
        /// </summary>
        /// <param name="k">Coefficient</param>
        /// <param name="vector">Vector</param>
        /// <returns>Result</returns>
        public static Vector operator *(double k, Vector vector)
        {
            Vector V = new Vector();
            Matrix.mult(k, vector, V);
            return V;
        }

        /// <summary>
        /// Multiplicate by scalar
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="k">Coefficient</param>
        /// <returns>Result</returns>
        public static Vector operator *(Vector vector, double k)
        {
            Vector V = new Vector();
            Matrix.mult(k, vector, V);
            return V;
        }

        /// <summary>
        /// Inner product
        /// </summary>
        /// <param name="vector1">Vector 1</param>
        /// <param name="vector2">Vector 2</param>
        /// <returns>Inner product result</returns>
        public static double operator *(Vector vector1, Vector vector2)
        {
            if (vector1.Dimension != vector2.Dimension)
                return double.NaN;

            double S = 0.0;
            for (int n = 0; n < vector1.Dimension; n++)
                S += vector1[n] * vector2[n];
            return S;
        }

        /// <summary>
        /// Cross product
        /// </summary>
        /// <param name="vector1">Vector 1</param>
        /// <param name="vector2">Vector 2</param>
        /// <returns>Cross product result</returns>
        public static Vector operator %(Vector vector1, Vector vector2)
        {
            Vector vector = new Vector(3);
            cross(vector1, vector2, vector);
            return vector;
        }

        protected static void cross(Vector vector1, Vector vector2, Vector vector)
        {
            if (vector1.Dimension != 3)
                return;

            if (vector2.Dimension != 3)
                return;

            if (vector.Dimension != 3)
                return;

            double[] x = new double[3];

            x[0] = vector1[1] * vector2[2] - vector1[2] * vector2[1];
            x[1] = vector1[2] * vector2[0] - vector1[0] * vector2[2];
            x[2] = vector1[0] * vector2[1] - vector1[1] * vector2[0];

            for (int n = 0; n < 3; n++)
                vector[n] = x[n];
        }

        /// <summary>
        /// Vector absolute value
        /// </summary>
        /// <returns>Vector length</returns>
        public double Length()
        {
            return Math.Sqrt(this.LengthSqr());
        }

        /// <summary>
        /// Sqr of vector absolute value
        /// </summary>
        /// <returns>Sqr of vector length</returns>
        public double LengthSqr()
        {
            return this * this;
        }

        /// <summary>
        /// Weighted mean
        /// </summary>
        /// <param name="vector">Weights vector</param>
        /// <returns>Mean value</returns>
        public double average(Vector vector)
        {
            if (vector.Dimension != this.Dimension)
                return 0.0;

            VectorType vt = vector.vectortype;
            vector.transpose(this.vectortype);

            double val = base.average(vector.item);

            vector.transpose(vt);

            return val;
        }

        /// <summary>
        /// Rotate vector to quaternion
        /// </summary>
        /// <param name="quaternion">Quaternion</param>
        /// <returns>Rotated vector</returns>
        public Vector rotate(Quaternion quaternion)
        {
            if (this.Dimension != 3)
                return new Vector();

            Quaternion q = (Quaternion)quaternion.Clone();
            q.normilize();

            Quaternion vect2quat = new Quaternion(0.0, this);

            Quaternion quat2vect = q.invert() * vect2quat * q;

            quat2vect.VectorPart.Copy(this);

            return quat2vect.VectorPart;
        }

        /// <summary>
        /// Object clone
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            Vector vector = new Vector();

            Copy(vector);


            return vector;
        }

        /// <summary>
        /// Convertation to string
        /// </summary>
        /// <returns>Values from tab</returns>
        public override string ToString()
        {
            string str = String.Empty;
            for (int n = 0; n < this.Dimension; n++)
            {
                if (n != 0)
                    str += "   ";
                str += this[n].ToString();
            }
            return str;
        }
    }
}
