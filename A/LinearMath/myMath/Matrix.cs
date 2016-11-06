using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearMath
{
    public enum UpdateMode { active, frozen }

    public enum MatrixState { stable, unstable }

    public abstract class Matrix
    {
        public event EventHandler Changed;

        public event EventHandler StateChanged;

        private UpdateMode _updateMode;

        public UpdateMode updateMode
        {
            get
            {
                return _updateMode;
            }
            set
            {
                if (_updateMode != value)
                {
                    _updateMode = value;
                    if ((Changed != null) & (_updateMode == UpdateMode.active))
                        Changed(this, EventArgs.Empty);
                }
            }
        }

        private MatrixState _matrixState;

        public MatrixState matrixState
        {
            get
            {
                return _matrixState;
            }
            set
            {
                _matrixState = value;
                if (StateChanged != null)
                    StateChanged(this, EventArgs.Empty);
            }
        }

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
            }
        }

        double[,] _item;
        /// <summary>
        /// Items
        /// </summary>
        protected double[,] item
        {
            get
            {
                return _item;
            }
            private set
            {
                _item = value;
                Init();
            }
        }

        int _rows;
        int _cols;

        /// <summary>
        /// Rows
        /// </summary>
        protected int rows
        {
            get
            {
                return _rows;
            }
        }
        /// <summary>
        /// Columns
        /// </summary>
        protected int cols
        {
            get
            {
                return _cols;
            }
        }

        /// <summary>
        /// Matrix constructor
        /// </summary>
        /// <param name="Rows">Rows</param>
        /// <param name="Cols">Columns</param>
        /// <param name="val">Initial value of elements</param>
        protected Matrix(int Rows, int Cols, double val)
        {
            item = new double[Rows, Cols];

            for (int n = 0; n < rows; n++)
                for (int m = 0; m < cols; m++)
                    item[n, m] = val;

            this.eps = 1E-6;

            updateMode = UpdateMode.active;
            matrixState = MatrixState.stable;
        }

        /// <summary>
        /// Empty matrix
        /// </summary>
        protected Matrix()
        {
            item = null;

            this.eps = 1E-6;

            updateMode = UpdateMode.active;
            matrixState = MatrixState.stable;
        }

        /// <summary>
        /// Init non-empty matrix. Set rows and columns count
        /// </summary>
        private void Init()
        {
            if (item == null)
            {
                _rows = 0;
                _cols = 0;
                return;
            }

            _rows = item.GetLength(0);
            _cols = item.GetLength(1);
        }

        /// <summary>
        /// Access to element in matrix
        /// </summary>
        /// <param name="n">Dimenshion 0</param>
        /// <param name="m">Dimenshion 1</param>
        /// <returns></returns>
        protected double this[int n, int m]
        {
            get
            {
                
                if (n >= _rows)
                    return 0.0;
                if (m >= _cols)
                    return 0.0;
                if (n < 0)
                    return 0.0;
                if (m < 0)
                    return 0.0;

                
                return item[n, m];
            }
            set
            {
                
                if (n >= _rows)
                    return;
                if (m >= _cols)
                    return;
                if (n < 0)
                    return;
                if (m < 0)
                    return;


                double x = item[n, m];
                item[n, m] = value;

                if ((Changed != null) & (_updateMode == UpdateMode.active))
                    Changed(this, new EventArgsMatrixChanged() { row = n, col = m, dif = value - x });
            }
        }

        /// <summary>
        /// Sum by element
        /// </summary>
        /// <param name="matrix1">Matrix 1</param>
        /// <param name="matrix2">Matrix 2</param>
        /// <returns>Matrix</returns>
        static protected void add(Matrix matrix1, Matrix matrix2, Matrix matrix)
        {
            if (matrix1.rows != matrix2.rows)
                return;
            if (matrix1.cols != matrix2.cols)
                return;

            int N = matrix1.rows;
            int M = matrix2.cols;

            if (matrix.IsEmpty())
                matrix.item = new double[N,M];

            for (int n = 0; n < N; n++)
                for (int m = 0; m < M; m++)
                    matrix[n, m] = matrix1[n, m] + matrix2[n, m];
        }

        /// <summary>
        /// Subtract by elements
        /// </summary>
        /// <param name="matrix1">Matrix 1</param>
        /// <param name="matrix2">Matrix 2</param>
        /// <returns>Matrix</returns>
        static protected void sub(Matrix matrix1, Matrix matrix2, Matrix matrix)
        {
            if (matrix1.rows != matrix2.rows)
                return;
            if (matrix1.cols != matrix2.cols)
                return;

            int N = matrix1.rows;
            int M = matrix2.cols;

            if (matrix.IsEmpty())
                matrix.item = new double[N, M];

            for (int n = 0; n < N; n++)
                for (int m = 0; m < M; m++)
                    matrix[n, m] = matrix1[n, m] - matrix2[n, m];
        }

        /// <summary>
        /// Matrix multiplication
        /// </summary>
        /// <param name="matrix1">Matrix 1</param>
        /// <param name="matrix2">Matrix 2</param>
        /// <returns>Matrix</returns>
        static protected void mult(Matrix matrix1, Matrix matrix2, Matrix matrix)
        {
            int N1 = matrix1.rows;
            int M1 = matrix1.cols;

            int N2 = matrix2.rows;
            int M2 = matrix2.cols;

            if (M1 != N2)
                return;

            int K = M1;
            int N = N1;
            int M = M2;

            if (matrix.IsEmpty())
                matrix.item = new double[N, M];

            for (int n = 0; n < N; n++)
                for (int m = 0; m < M; m++)
                    for (int k = 0; k < K; k++)
                        matrix[n, m] += matrix1[n, k] * matrix2[k, m];
        }

        /// <summary>
        /// Multiplication by coefficient
        /// </summary>
        /// <param name="k">Coefficient</param>
        static protected void mult(double k, Matrix matrix0, Matrix matrix)
        {
            if (matrix.IsEmpty())
                matrix.item = new double[matrix0.rows, matrix0.cols];

            for (int n = 0; n < matrix0.rows; n++)
                for (int m = 0; m < matrix0.cols; m++)
                    matrix[n, m] = k*matrix0[n, m];
        }

        /// <summary>
        /// Matrix transpose
        /// </summary>
        protected virtual void transpose()
        {
            double[,] B = new double[cols, rows];
            for (int n = 0; n < rows; n++)
                for (int m = 0; m < cols; m++)
                    B[m, n] = this[n, m];
            item = B;
        }

        /// <summary>
        /// Mean value
        /// </summary>
        /// <returns>Value</returns>
        public double average()
        {
            double[,] weight = new double[rows, cols];

            for (int n = 0; n < rows; n++)
                for (int m = 0; m < cols; m++)
                    weight[n, m] = 1.0;

            return average(weight);
        }

        /// <summary>
        /// Weighted mean value
        /// </summary>
        /// <param name="weight">Weight matrix</param>
        /// <returns>Value</returns>
        protected double average(double[,] weight)
        {
            if (this.rows != weight.GetLength(0))
                return 0.0;
            if (this.cols != weight.GetLength(1))
                return 0.0;

            double S = 0.0;
            double Sw = 0.0;

            for (int n = 0; n < rows; n++)
                for (int m = 0; m < cols; m++)
                {
                    S += this[n, m] * weight[n, m];
                    Sw += weight[n, m];
                }

            return S / Sw;
        }

        /// <summary>
        /// Sum of elemets
        /// </summary>
        /// <returns></returns>
        public double sum()
        {
            double S = 0.0;
            for (int n = 0; n < rows; n++)
                for (int m = 0; m < cols; m++)
                {
                    S += item[n, m];
                }
            return S;
        }

        /// <summary>
        /// Эвклидова норма матрицы
        /// </summary>
        /// <returns>Значение нормы</returns>
        public double norm()
        {
            double S = 0.0;

            for (int n = 0; n < rows; n++)
                for (int m = 0; m < cols; m++)
                    S += Math.Pow(this[n, m], 2.0);
            
            return S;
        }

        /// <summary>
        /// Matrix normalization
        /// </summary>
        public void normalize()
        {
            double leng = Math.Sqrt(this.norm());

            for (int n = 0; n < this.rows; n++)
                for (int m = 0; m < this.cols; m++)
                    this[n, m] /= leng;
        }

        /// <summary>
        /// Objects equality
        /// </summary>
        /// <param name="obj">Matrix</param>
        /// <returns>Equality</returns>
        public override bool Equals(object obj)
        {
            Matrix matrix = obj as Matrix;

            if (this._rows != matrix._rows)
                return false;
            if (this._cols != matrix._cols)
                return false;

            for (int n = 0; n < this._rows; n++)
                for (int m = 0; m < this._cols; m++)
                    if (Math.Abs(this[n, m] - matrix[n, m]) > this._eps)
                        return false;
            return true;
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            double S = 0.0;
            for (int n = 0; n < _rows; n++)
            {
                for (int m = 0; m < _cols; m++)
                {
                    S += this[n, m] * (n + m);
                }
            }

            if (S < 0)
                S *= (_cols+_rows);

            if (S == 0)
                return 0;

            return (int)(S * 1E+3);
        }

        /// <summary>
        /// Copy currently matrix to another
        /// </summary>
        /// <param name="matrix"></param>
        protected void Copy(Matrix matrix)
        {
            matrix.item = new double[this.rows, this.cols];

            for (int n = 0; n < this.rows; n++)
                for (int m = 0; m < this.cols; m++)
                    matrix[n, m] = this[n, m];
        }

        /// <summary>
        /// Verification matrix to empty equal
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            if (this.rows != 0)
                return false;

            if (this.cols != 0)
                return false;
            
            return true;
        }

        public void CopyValue(Matrix matrix)
        {            
            if ((this.cols != matrix.cols)|(this.rows != matrix.rows))
                return;

            UpdateMode mode = this.updateMode;
            this.updateMode = LinearMath.UpdateMode.frozen;

            for (int n = 0; n < this.rows; n++)
                for (int m = 0; m < this.cols; m++)
                    this[n, m] = matrix[n, m];

            this.updateMode = mode;
        }

        public void Refine()
        {
            int p = -(int)Math.Log10(eps);
            for (int n = 0; n < rows; n++)
                for (int m = 0; m < cols; m++)
                    item[n, m] = Math.Round(item[n, m], p);
        }
    }

    public class EventArgsMatrixChanged : EventArgs
    {
        public int row { get; set; }

        public int col { get; set; }

        public double dif { get; set; }
    }
}
