using LinearMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LinearMathTest
{
    
    
    /// <summary>
    ///Это класс теста для CoordinateSystemTest, в котором должны
    ///находиться все модульные тесты CoordinateSystemTest
    ///</summary>
    [TestClass()]
    public class CoordinateSystemTest
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
        ///Тест для ConvertToBase
        ///</summary>
        [TestMethod()]
        public void ConvertToBaseTest()
        {
            CoordinateSystem bcs = new CoordinateSystem(null, null, null);
            CoordinateSystem cs = new CoordinateSystem(bcs, new Quaternion(Vector.Create3D(0.0, 0.0, 1.0), -45.0), Vector.Create3D(4.0, 3.0, 0.0));
            Vector actual = cs.ConvertPointToBase(Vector.Create3D(1.0, 1.0, 0.0));
            Vector expected = Vector.Create3D(4.0, 3.0 + Math.Sqrt(2), 0.0);
            Assert.AreEqual(true, actual.Equals(expected));
        }
    }
}
