using LinearMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LinearMathTest
{
    
    
    /// <summary>
    ///Это класс теста для RectangleMatrixTest, в котором должны
    ///находиться все модульные тесты RectangleMatrixTest
    ///</summary>
    [TestClass()]
    public class RectangleMatrixTest
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
        ///Тест для op_Subtraction
        ///</summary>
        [TestMethod()]
        public void op_SubtractionTest()
        {
            RectangleMatrix matrix1 = new RectangleMatrix(3, 2); // TODO: инициализация подходящего значения
            matrix1[0, 0] = 1.0;
            matrix1[0, 1] = 2.0;
            matrix1[1, 0] = 3.0;
            matrix1[1, 1] = 4.0;
            matrix1[2, 0] = 5.0;
            matrix1[2, 1] = 6.0;

            RectangleMatrix matrix2 = new RectangleMatrix(3, 2); // TODO: инициализация подходящего значения
            matrix2[0, 0] = 1.0;
            matrix2[0, 1] = 2.0;
            matrix2[1, 0] = 3.0;
            matrix2[1, 1] = 4.0;
            matrix2[2, 0] = 5.0;
            matrix2[2, 1] = 6.0;

            RectangleMatrix expected = new RectangleMatrix(3, 2); // TODO: инициализация подходящего значения
            expected[0, 0] = 0.0;
            expected[0, 1] = 0.0;
            expected[1, 0] = 0.0;
            expected[1, 1] = 0.0;
            expected[2, 0] = 0.0;
            expected[2, 1] = 0.0;

            RectangleMatrix actual;
            actual = (matrix1 - matrix2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для op_Multiply
        ///</summary>
        [TestMethod()]
        public void op_MultiplyTest()
        {
            RectangleMatrix matrix1 = new RectangleMatrix(3, 2); // TODO: инициализация подходящего значения
            matrix1[0, 0] = 1.0;
            matrix1[0, 1] = 2.0;
            matrix1[1, 0] = 3.0;
            matrix1[1, 1] = 4.0;
            matrix1[2, 0] = 5.0;
            matrix1[2, 1] = 6.0;

            RectangleMatrix matrix2 = new RectangleMatrix(2, 3); // TODO: инициализация подходящего значения
            matrix2[0, 0] = 1.0;
            matrix2[1, 0] = 2.0;
            matrix2[0, 1] = 3.0;
            matrix2[1, 1] = 4.0;
            matrix2[0, 2] = 5.0;
            matrix2[1, 2] = 6.0;

            RectangleMatrix expected = new RectangleMatrix(2, 2); // TODO: инициализация подходящего значения
            expected[0, 0] = 35.0;
            expected[0, 1] = 44.0;
            expected[1, 0] = 44.0;
            expected[1, 1] = 56.0;

            RectangleMatrix actual;
            actual = (matrix2 * matrix1);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для op_Addition
        ///</summary>
        [TestMethod()]
        public void op_AdditionTest()
        {
            RectangleMatrix matrix1 = new RectangleMatrix(3, 2); // TODO: инициализация подходящего значения
            matrix1[0, 0] = 1.0;
            matrix1[0, 1] = 2.0;
            matrix1[1, 0] = 3.0;
            matrix1[1, 1] = 4.0;
            matrix1[2, 0] = 5.0;
            matrix1[2, 1] = 6.0;

            RectangleMatrix matrix2 = new RectangleMatrix(3, 2); // TODO: инициализация подходящего значения
            matrix2[0, 0] = 1.0;
            matrix2[0, 1] = 2.0;
            matrix2[1, 0] = 3.0;
            matrix2[1, 1] = 4.0;
            matrix2[2, 0] = 5.0;
            matrix2[2, 1] = 6.0;

            RectangleMatrix expected = new RectangleMatrix(3, 2); // TODO: инициализация подходящего значения
            expected[0, 0] = 2.0;
            expected[0, 1] = 4.0;
            expected[1, 0] = 6.0;
            expected[1, 1] = 8.0;
            expected[2, 0] = 10.0;
            expected[2, 1] = 12.0;

            RectangleMatrix actual;
            actual = (matrix1 + matrix2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для LQR
        ///</summary>
        [TestMethod()]
        public void LQRTest()
        {
            RectangleMatrix target = new RectangleMatrix(5,2); // TODO: инициализация подходящего значения
            Vector v = new Vector(5); // TODO: инициализация подходящего значения

            target[0, 0] = 0;
            target[1, 0] = 1;
            target[2, 0] = 2;
            target[3, 0] = 3;
            target[4, 0] = 4;

            target[0, 1] = 1;
            target[1, 1] = 1;
            target[2, 1] = 1;
            target[3, 1] = 1;
            target[4, 1] = 1;

            v[0] = 0;
            v[1] = 2;
            v[2] = 0;
            v[3] = 2;
            v[4] = 0;

            Vector actual;
            actual = target.LQR(v);
            Assert.AreEqual(Math.Round(actual[0], 1), 0.0);
            Assert.AreEqual(Math.Round(actual[1], 1), 0.8);
        }
    }
}
