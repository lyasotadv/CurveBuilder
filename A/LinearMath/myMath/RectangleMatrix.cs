using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearMath
{
    public class RectangleMatrix : Matrix, ICloneable
    {
        /// <summary>
        /// Rows
        /// </summary>
        public int Rows
        {
            get
            {
                return rows;
            }
        }

        /// <summary>
        /// Collumns
        /// </summary>
        public int Cols
        {
            get
            {
                return cols;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Rows">Rows</param>
        /// <param name="Cols">Columns</param>
        public RectangleMatrix(int Rows, int Cols)
            : base(Rows, Cols, 0.0)
        {

        }

        /// <summary>
        /// Empty matrix constructor
        /// </summary>
        private RectangleMatrix()
            : base()
        {

        }

        /// <summary>
        /// Matrix element
        /// </summary>
        /// <param name="n">Rows index</param>
        /// <param name="m">Collumns index</param>
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
        /// Matrix sum
        /// </summary>
        /// <param name="matrix1">Matrix 1</param>
        /// <param name="matrix2">Matrix 2</param>
        /// <returns>Result</returns>
        public static RectangleMatrix operator +(RectangleMatrix matrix1, RectangleMatrix matrix2)
        {
            RectangleMatrix m1 = (RectangleMatrix)matrix1.Clone();
            RectangleMatrix m2 = (RectangleMatrix)matrix2.Clone();

            RectangleMatrix matrix = new RectangleMatrix();

            Matrix.add(m1, m2, matrix);

            return matrix;
        }

        /// <summary>
        /// Matrix subtract
        /// </summary>
        /// <param name="matrix1">Matrix 1</param>
        /// <param name="matrix2">Matrix 2</param>
        /// <returns>Result</returns>
        public static RectangleMatrix operator -(RectangleMatrix matrix1, RectangleMatrix matrix2)
        {
            RectangleMatrix m1 = (RectangleMatrix)matrix1.Clone();
            RectangleMatrix m2 = (RectangleMatrix)matrix2.Clone();

            RectangleMatrix matrix = new RectangleMatrix();

            Matrix.sub(m1, m2, matrix);

            return matrix;
        }

        /// <summary>
        /// Matrix multiplication
        /// </summary>
        /// <param name="matrix1">Matrix 1</param>
        /// <param name="matrix2">Matrix 2</param>
        /// <returns>Result</returns>
        public static RectangleMatrix operator *(RectangleMatrix matrix1, RectangleMatrix matrix2)
        {
            RectangleMatrix m1 = (RectangleMatrix)matrix1.Clone();
            RectangleMatrix m2 = (RectangleMatrix)matrix2.Clone();

            RectangleMatrix matrix = new RectangleMatrix();

            Matrix.mult(m1, m2, matrix);

            return matrix;
        }

        public static RectangleMatrix operator *(SquareMatrix matrix1, RectangleMatrix matrix2)
        {
            SquareMatrix m1 = (SquareMatrix)matrix1.Clone();
            RectangleMatrix m2 = (RectangleMatrix)matrix2.Clone();

            RectangleMatrix matrix = new RectangleMatrix();

            Matrix.mult(m1, m2, matrix);

            return matrix;
        }

        public Vector LQR(Vector v)
        {
            if (this.rows != v.Dimension)
                return null;

            RectangleMatrix M = (RectangleMatrix)(this.Clone());
            M.transpose();
            Vector V = M * v;
            M = M * this;
            SquareMatrix s = new SquareMatrix(M.rows);
            s.CopyValue(M);

            return s.linsolve(V);
        }

        /// <summary>
        /// Object clone
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            RectangleMatrix matrix = new RectangleMatrix();

            base.Copy(matrix);

            return matrix;
        }
    }
}
