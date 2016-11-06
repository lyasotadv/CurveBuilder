using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearMath
{
    public class SquareMatrix : Matrix, ICloneable
    {
        /// <summary>
        /// Matrix size
        /// </summary>
        public int Dimension
        {
            get
            {
                return rows;
            }
        }

        /// <summary>
        /// Constructor. Initialization by 0.0
        /// </summary>
        /// <param name="Size">Matrix size</param>
        public SquareMatrix(int Size)
            : base(Size, Size, 0.0)
        {

        }

        /// <summary>
        /// Empty matrix
        /// </summary>
        public SquareMatrix()
            : base()
        {

        }

        /// <summary>
        /// Eye matrix
        /// </summary>
        /// <param name="Size">Size</param>
        /// <returns>Matrix result</returns>
        public static SquareMatrix eye(int Size)
        {
            SquareMatrix matrix = new SquareMatrix(Size);

            for (int n = 0; n < Size; n++)
                matrix[n, n] = 1.0;

            return matrix;
        }

        /// <summary>
        /// Accessor to element
        /// </summary>
        /// <param name="n">Rows index</param>
        /// <param name="m">Columns index</param>
        /// <returns></returns>
        public new double this[int n, int m]
        {
            get
            {
                return base[n, m];
            }
            set
            {
                base[n, m] = value;
            }
        }

        /// <summary>
        /// Sum of matrix
        /// </summary>
        /// <param name="matrix1">Matrix 1</param>
        /// <param name="matrix2">Matrix 2</param>
        /// <returns>Result</returns>
        public static SquareMatrix operator +(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            SquareMatrix m1 = (SquareMatrix)matrix1.Clone();
            SquareMatrix m2 = (SquareMatrix)matrix2.Clone();

            SquareMatrix matrix = new SquareMatrix();

            Matrix.add(m1, m2, matrix);

            return matrix;
        }

        /// <summary>
        /// Substrate matrix
        /// </summary>
        /// <param name="matrix1">Matrix 1</param>
        /// <param name="matrix2">Matrix 2</param>
        /// <returns>Result</returns>
        public static SquareMatrix operator -(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            SquareMatrix m1 = (SquareMatrix)matrix1.Clone();
            SquareMatrix m2 = (SquareMatrix)matrix2.Clone();

            SquareMatrix matrix = new SquareMatrix();

            Matrix.sub(m1, m2, matrix);

            return matrix;
        }

        /// <summary>
        /// Matrix multiplication
        /// </summary>
        /// <param name="matrix1">Matrix 1</param>
        /// <param name="matrix2">Matrix 2</param>
        /// <returns>Result</returns>
        public static SquareMatrix operator *(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            SquareMatrix m1 = (SquareMatrix)matrix1.Clone();
            SquareMatrix m2 = (SquareMatrix)matrix2.Clone();

            SquareMatrix matrix = new SquareMatrix();

            Matrix.mult(m1, m2, matrix);

            return matrix;
        }

        /// <summary>
        /// Multiplicate by scalar
        /// </summary>
        /// <param name="k">Coefficient</param>
        /// <param name="matrix">Matrix</param>
        /// <returns>Result</returns>
        public static SquareMatrix operator *(double k, SquareMatrix matrix)
        {
            SquareMatrix m = (SquareMatrix)matrix.Clone();
            SquareMatrix M = new SquareMatrix();
            Matrix.mult(k, m, M);
            return M;
        }

        /// <summary>
        /// Determinant calculation
        /// </summary>
        /// <returns></returns>
        public double determinant()
        {
            if (Dimension == 1)
                return this[0, 0];

            double S = 0.0;

            for (int n = 0; n < Dimension; n++)
            {
                S += isEven(n) * this.ExcludingRowColumn(n, 0).determinant() * this[n, 0];
            }

            return S;
        }

        /// <summary>
        /// Is value even
        /// </summary>
        /// <param name="n">Input value</param>
        /// <returns></returns>
        private double isEven(int n)
        {
            return (n % 2 == 0 ? 1.0 : -1.0);
        }

        /// <summary>
        /// Create square matrix without row and collumn
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="col">Collumn index</param>
        /// <returns></returns>
        private SquareMatrix ExcludingRowColumn(int row, int col)
        {
            SquareMatrix matrix = new SquareMatrix(Dimension - 1);

            int n0 = 0;

            for (int n = 0; n < Dimension; n++)
            {
                int m0 = 0;

                if (n == row)
                    continue;

                for (int m = 0; m < Dimension; m++)
                {
                    
                    if (m == col)
                        continue;

                    matrix[n0, m0] = this[n, m];
                    
                    m0++;
                }

                n0++;
            }

            return matrix;
        }

        /// <summary>
        /// Inverse matrix
        /// </summary>
        /// <returns></returns>
        public SquareMatrix invert()
        {
            double det = this.determinant();
            if (det == 0.0)
                return null;

            SquareMatrix matrix = new SquareMatrix(Dimension);

            for (int n = 0; n < Dimension; n++)
                for (int m = 0; m < Dimension; m++)
                {
                    matrix[n, m] = isEven(n + m) * this.ExcludingRowColumn(n, m).determinant();
                }

            matrix.transpose();

            return (1.0 / det) * matrix;
        }

        /// <summary>
        /// System of linear equation solving
        /// </summary>
        /// <param name="vector">Right side vector</param>
        /// <returns>Result vector</returns>
        public Vector linsolve(Vector vector)
        {
            SquareMatrix matrix = (SquareMatrix)this.Clone();

            return matrix.invert() * vector;
        }
        /// <summary>
        /// Object clone
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            SquareMatrix matrix = new SquareMatrix();

            base.Copy(matrix);

            return matrix;
        }
    }
}
