using LinearMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LinearMathTest
{
    
    
    /// <summary>
    ///Это класс теста для SquareMatrixTest, в котором должны
    ///находиться все модульные тесты SquareMatrixTest
    ///</summary>
    [TestClass()]
    public class SquareMatrixTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Дополнительные атрибуты теста
        // 
        //При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        //ClassInitialize используется для выполнения кода до запуска первого теста в классе
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //TestInitialize используется для выполнения кода перед запуском каждого теста
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //TestCleanup используется для выполнения кода после завершения каждого теста
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Тест для determinant
        ///</summary>
        [TestMethod()]
        public void determinantTest()
        {
            int Size = 3; // TODO: инициализация подходящего значения
            SquareMatrix target = new SquareMatrix(Size); // TODO: инициализация подходящего значения

            target[0, 0] = 0.8147;
            target[0, 1] = 0.9134;
            target[0, 2] = 0.2785;

            target[1, 0] = 0.9058;
            target[1, 1] = 0.6324;
            target[1, 2] = 0.5469;

            target[2, 0] = 0.1270;
            target[2, 1] = 0.0975;
            target[2, 2] = 0.9575;

            double expected = -0.2767; // TODO: инициализация подходящего значения
            double actual;
            actual = target.determinant();
            Assert.AreEqual(true, Math.Abs(actual - expected) < 0.0001);
        }

        /// <summary>
        ///Тест для invert
        ///</summary>
        [TestMethod()]
        public void invertTest()
        {
            int Size = 3;

            SquareMatrix target = new SquareMatrix(Size);

            target[0, 0] = 0.8147;
            target[0, 1] = 0.9134;
            target[0, 2] = 0.2785;

            target[1, 0] = 0.9058;
            target[1, 1] = 0.6324;
            target[1, 2] = 0.5469;

            target[2, 0] = 0.1270;
            target[2, 1] = 0.0975;
            target[2, 2] = 0.9575;

            SquareMatrix actual = target.invert() * target;

            SquareMatrix expected = SquareMatrix.eye(Size);

            double x = (expected - actual).norm();

            Assert.AreEqual(true, x < 0.001);
        }

        /// <summary>
        ///Тест для linsolve
        ///</summary>
        [TestMethod()]
        public void linsolveTest()
        {
            int Size = 2; // TODO: инициализация подходящего значения
            SquareMatrix target = new SquareMatrix(Size); // TODO: инициализация подходящего значения
            target[0, 0] = 1.0;
            target[0, 1] = 2.0;
            target[1, 0] = 3.0;
            target[1, 1] = 4.0;

            Vector vector = new Vector(Size); // TODO: инициализация подходящего значения
            vector[0] = 1.0;
            vector[1] = 2.0;

            Vector expected = new Vector(Size); // TODO: инициализация подходящего значения
            expected[0] = 0.0;
            expected[1] = 0.5;

            Vector actual;
            actual = target.linsolve(vector);

            double x = (expected - actual).norm();

            Assert.AreEqual(true, x < 0.001);
        }

        /// <summary>
        ///Тест для op_Addition
        ///</summary>
        [TestMethod()]
        public void op_AdditionTest()
        {
            SquareMatrix matrix1 = new SquareMatrix(2); // TODO: инициализация подходящего значения
            matrix1[0, 0] = 1.0;
            matrix1[0, 1] = 2.0;
            matrix1[1, 0] = 3.0;
            matrix1[1, 1] = 4.0;

            SquareMatrix matrix2 = new SquareMatrix(2); // TODO: инициализация подходящего значения
            matrix2[0, 0] = 10.0;
            matrix2[0, 1] = 20.0;
            matrix2[1, 0] = 30.0;
            matrix2[1, 1] = 40.0;

            SquareMatrix expected = new SquareMatrix(2); // TODO: инициализация подходящего значения
            expected[0, 0] = 11.0;
            expected[0, 1] = 22.0;
            expected[1, 0] = 33.0;
            expected[1, 1] = 44.0;

            SquareMatrix actual;
            actual = (matrix1 + matrix2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для op_Multiply
        ///</summary>
        [TestMethod()]
        public void op_MultiplyTest()
        {
            double k = 10.0; // TODO: инициализация подходящего значения
            SquareMatrix matrix = new SquareMatrix(2); // TODO: инициализация подходящего значения
            matrix[0, 0] = 1.0;
            matrix[0, 1] = 2.0;
            matrix[1, 0] = 3.0;
            matrix[1, 1] = 4.0;
            
            SquareMatrix expected = new SquareMatrix(2); // TODO: инициализация подходящего значения
            expected[0, 0] = 10.0;
            expected[0, 1] = 20.0;
            expected[1, 0] = 30.0;
            expected[1, 1] = 40.0;

            SquareMatrix actual;
            actual = (k * matrix);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для op_Multiply
        ///</summary>
        [TestMethod()]
        public void op_MultiplyTest1()
        {
            SquareMatrix matrix1 = new SquareMatrix(2); // TODO: инициализация подходящего значения
            matrix1[0, 0] = 1.0;
            matrix1[0, 1] = 2.0;
            matrix1[1, 0] = 3.0;
            matrix1[1, 1] = 4.0;

            SquareMatrix matrix2 = new SquareMatrix(2); // TODO: инициализация подходящего значения
            matrix2[0, 0] = 10.0;
            matrix2[0, 1] = 20.0;
            matrix2[1, 0] = 30.0;
            matrix2[1, 1] = 40.0;

            SquareMatrix expected = new SquareMatrix(2); // TODO: инициализация подходящего значения
            expected[0, 0] = 70.0;
            expected[0, 1] = 100.0;
            expected[1, 0] = 150.0;
            expected[1, 1] = 220.0;

            SquareMatrix actual;
            actual = (matrix1 * matrix2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для op_Subtraction
        ///</summary>
        [TestMethod()]
        public void op_SubtractionTest()
        {
            SquareMatrix matrix1 = new SquareMatrix(2); // TODO: инициализация подходящего значения
            matrix1[0, 0] = 1.0;
            matrix1[0, 1] = 2.0;
            matrix1[1, 0] = 3.0;
            matrix1[1, 1] = 4.0;

            SquareMatrix matrix2 = new SquareMatrix(2); // TODO: инициализация подходящего значения
            matrix2[0, 0] = 10.0;
            matrix2[0, 1] = 20.0;
            matrix2[1, 0] = 30.0;
            matrix2[1, 1] = 40.0;

            SquareMatrix expected = new SquareMatrix(2); // TODO: инициализация подходящего значения
            expected[0, 0] = 9.0;
            expected[0, 1] = 18.0;
            expected[1, 0] = 27.0;
            expected[1, 1] = 36.0;

            SquareMatrix actual;
            actual = (matrix2 - matrix1);
            Assert.AreEqual(expected, actual);
        }
    }
}
