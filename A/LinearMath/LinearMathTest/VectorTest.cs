using LinearMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LinearMathTest
{
    
    
    /// <summary>
    ///Это класс теста для VectorTest, в котором должны
    ///находиться все модульные тесты VectorTest
    ///</summary>
    [TestClass()]
    public class VectorTest
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
            Vector vector1 = new Vector(2); // TODO: инициализация подходящего значения
            vector1[0] = 1.0;
            vector1[1] = 2.0;

            Vector vector2 = new Vector(2); // TODO: инициализация подходящего значения
            vector2[0] = 10.0;
            vector2[1] = 20.0;

            Vector expected = new Vector(2); // TODO: инициализация подходящего значения
            expected[0] = 9.0;
            expected[1] = 18.0;

            Vector actual;
            actual = (vector2 - vector1);
            Assert.AreEqual(true, actual.Equals(expected));
        }

        /// <summary>
        ///Тест для op_Multiply
        ///</summary>
        [TestMethod()]
        public void op_MultiplyTest()
        {
            RectangleMatrix matrix = new RectangleMatrix(3, 2); // TODO: инициализация подходящего значения
            matrix[0, 0] = 1.0;
            matrix[1, 0] = 2.0;
            matrix[2, 0] = 3.0;
            matrix[0, 1] = 4.0;
            matrix[1, 1] = 5.0;
            matrix[2, 1] = 6.0;

            Vector vector = new Vector(2); // TODO: инициализация подходящего значения
            vector[0] = 1.0;
            vector[1] = 2.0;

            Vector expected = new Vector(3); // TODO: инициализация подходящего значения
            expected[0] = 9.0;
            expected[1] = 12.0;
            expected[2] = 15.0;

            Vector actual;
            actual = (matrix * vector);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для op_Multiply
        ///</summary>
        [TestMethod()]
        public void op_MultiplyTest1()
        {
            double k = 10.0; // TODO: инициализация подходящего значения
            Vector vector = new Vector(2); // TODO: инициализация подходящего значения
            vector[0] = 1.0;
            vector[1] = 2.0;

            Vector expected = new Vector(2); // TODO: инициализация подходящего значения
            expected[0] = 10.0;
            expected[1] = 20.0;

            Vector actual;
            actual = (k * vector);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для op_Multiply
        ///</summary>
        [TestMethod()]
        public void op_MultiplyTest2()
        {
            SquareMatrix matrix = new SquareMatrix(2); // TODO: инициализация подходящего значения
            matrix[0, 0] = 1.0;
            matrix[0, 1] = 2.0;
            matrix[1, 0] = 3.0;
            matrix[1, 1] = 4.0;

            Vector vector = new Vector(2); // TODO: инициализация подходящего значения
            vector[0] = 1.0;
            vector[1] = 2.0;

            Vector expected = new Vector(2); // TODO: инициализация подходящего значения
            expected[0] = 5.0;
            expected[1] = 11.0;

            Vector actual;
            actual = (matrix * vector);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для op_Multiply
        ///</summary>
        [TestMethod()]
        public void op_MultiplyTest3()
        {
            Vector vector1 = new Vector(2); // TODO: инициализация подходящего значения
            vector1[0] = 1.0;
            vector1[1] = 2.0;

            Vector vector2 = new Vector(2); // TODO: инициализация подходящего значения
            vector2[0] = 3.0;
            vector2[1] = 4.0;

            double expected = 11.0; // TODO: инициализация подходящего значения
            double actual;
            actual = vector1 * vector2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для op_Modulus
        ///</summary>
        [TestMethod()]
        public void op_ModulusTest()
        {
            Vector vector1 = new Vector(3); // TODO: инициализация подходящего значения
            vector1[0] = 1.0;
            vector1[1] = 2.0;
            vector1[2] = 3.0;

            Vector vector2 = new Vector(3); // TODO: инициализация подходящего значения
            vector2[0] = 4.0;
            vector2[1] = 5.0;
            vector2[2] = 6.0; 
            
            Vector expected = new Vector(3); // TODO: инициализация подходящего значения
            expected[0] = -3.0;
            expected[1] = 6.0;
            expected[2] = -3.0;
            
            Vector actual;
            actual = (vector1 % vector2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для op_Addition
        ///</summary>
        [TestMethod()]
        public void op_AdditionTest()
        {
            Vector vector1 = new Vector(2); // TODO: инициализация подходящего значения
            vector1[0] = 1.0;
            vector1[1] = 2.0;

            Vector vector2 = new Vector(2); // TODO: инициализация подходящего значения
            vector2[0] = 10.0;
            vector2[1] = 20.0;

            Vector expected = new Vector(2); // TODO: инициализация подходящего значения
            expected[0] = 11.0;
            expected[1] = 22.0;

            Vector actual;
            actual = (vector1 + vector2);
            Assert.AreEqual(true, actual.Equals(expected));
        }

        /// <summary>
        ///Тест для average
        ///</summary>
        [TestMethod()]
        public void averageTest()
        {
            int Length = 3; // TODO: инициализация подходящего значения
            Vector target = new Vector(Length); // TODO: инициализация подходящего значения
            target[0] = 1.0;
            target[1] = 2.0;
            target[2] = 3.0;

            Vector vector = (Vector)target.Clone(); // TODO: инициализация подходящего значения
            double expected = 2.33; // TODO: инициализация подходящего значения
            double actual;
            actual = target.average(vector);

            double x = Math.Abs(actual - expected);

            Assert.AreEqual(true, x < 0.1);
        }

        /// <summary>
        ///Тест для Length
        ///</summary>
        [TestMethod()]
        public void LengthTest()
        {
            int Length = 2; // TODO: инициализация подходящего значения
            Vector target = new Vector(Length); // TODO: инициализация подходящего значения
            target[0] = 3.0;
            target[1] = 4.0;

            double expected = 5.0; // TODO: инициализация подходящего значения
            double actual;
            actual = target.Length();
            Assert.AreEqual(true, Math.Abs(expected - actual) < 0.001);
        }

        /// <summary>
        ///Тест для rotate
        ///</summary>
        [TestMethod()]
        public void rotateTest()
        {
            Vector target = new Vector(3); // TODO: инициализация подходящего значения
            target[0] = 1.0;
            target[1] = 0.0;
            target[2] = 0.0;

            Vector vector = new Vector(3);
            vector[0] = 0.0;
            vector[1] = 0.0;
            vector[2] = 1.0;

            Quaternion quaternion = new Quaternion(vector, -90.0); // TODO: инициализация подходящего значения
            
            Vector expected = new Vector(3); // TODO: инициализация подходящего значения
            expected[0] = 0.0;
            expected[1] = 1.0;
            expected[2] = 0.0;
            
            Vector actual;
            actual = target.rotate(quaternion);
            Assert.AreEqual(true, actual.Equals(expected));
        }

        /// <summary>
        ///Тест для Changed event
        ///</summary>
        [TestMethod()]
        public void ChangedTest()
        {
            Vector a = Vector.Create3D(0.0, 0.0, 0.0);

            bool flag = false;
            EventHandler handler = (obj, arg) =>
                {
                    flag = true;
                };

            a.Changed += handler;

            a.CopyValue(Vector.Create3D(1.0, 1.0, 1.0));

            Assert.IsTrue(flag);
        }
    }
}
