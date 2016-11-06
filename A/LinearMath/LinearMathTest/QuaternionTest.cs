using LinearMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LinearMathTest
{
    
    
    /// <summary>
    ///Это класс теста для QuaternionTest, в котором должны
    ///находиться все модульные тесты QuaternionTest
    ///</summary>
    [TestClass()]
    public class QuaternionTest
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
        ///Тест для Length
        ///</summary>
        [TestMethod()]
        public void LengthTest()
        {
            double scalar = 1.0; // TODO: инициализация подходящего значения
            Vector vector = new Vector(3); // TODO: инициализация подходящего значения
            vector[0] = 2.0;
            vector[1] = 3.0;
            vector[2] = 4.0;

            Quaternion target = new Quaternion(scalar, vector); // TODO: инициализация подходящего значения
            
            double expected = 5.47; // TODO: инициализация подходящего значения
            double actual;
            actual = target.Length();
            Assert.AreEqual(true, Math.Abs(actual - expected)<0.1);
        }

        /// <summary>
        ///Тест для invert
        ///</summary>
        [TestMethod()]
        public void invertTest()
        {
            double scalar = 1.0; // TODO: инициализация подходящего значения
            Vector vector = new Vector(3); // TODO: инициализация подходящего значения
            vector[0] = 0.0;
            vector[1] = 1.0;
            vector[2] = 0.0;

            Quaternion target = new Quaternion(scalar, (Vector)vector.Clone()); // TODO: инициализация подходящего значения

            vector[0] = 0.0;
            vector[1] = -0.5;
            vector[2] = 0.0;
            Quaternion expected = new Quaternion(0.5, (Vector)vector.Clone()); // TODO: инициализация подходящего значения
            
            Quaternion actual;
            actual = target.invert();
            Assert.AreEqual(true, actual.Equals(expected));
        }

        /// <summary>
        ///Тест для op_Multiply
        ///</summary>
        [TestMethod()]
        public void op_MultiplyTest()
        {
            Vector vector = new Vector(3);
            
            vector[0] = 0.0;
            vector[1] = 1.0;
            vector[2] = 0.0;
            Quaternion quat1 = new Quaternion(1.0, (Vector)vector.Clone()); // TODO: инициализация подходящего значения

            vector[0] = 0.5;
            vector[1] = 0.5;
            vector[2] = 0.75;
            Quaternion quat2 = new Quaternion(1.0, (Vector)vector.Clone()); // TODO: инициализация подходящего значения

            vector[0] = 1.25;
            vector[1] = 1.5;
            vector[2] = 0.25;
            Quaternion expected = new Quaternion(0.5, (Vector)vector.Clone()); // TODO: инициализация подходящего значения
            
            Quaternion actual;
            actual = (quat1 * quat2);
            Assert.AreEqual(true, actual.Equals(expected));
        }

        /// <summary>
        ///Тест для rotate
        ///</summary>
        [TestMethod()]
        public void rotateTest()
        {
            Quaternion target = new Quaternion(Vector.Create3D(0.0, 0.0, 1.0), -90.0); // TODO: инициализация подходящего значения
            Vector actual = Vector.Create3D(1.0, 0.0, 0.0); // TODO: инициализация подходящего значения
            target.rotate(actual);
            Vector expected = Vector.Create3D(0.0, 1.0, 0.0);
            Assert.AreEqual(true, actual.Equals(expected));
        }
    }
}
