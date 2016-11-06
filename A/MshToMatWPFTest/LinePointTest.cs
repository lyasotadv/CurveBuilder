using MshToMatWPF.Geometry.Point;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MshToMatWPFTest
{
    
    
    /// <summary>
    ///Это класс теста для LinePointTest, в котором должны
    ///находиться все модульные тесты LinePointTest
    ///</summary>
    [TestClass()]
    public class LinePointTest
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


        internal virtual LinePoint CreateLinePoint()
        {
            // TODO: создайте экземпляр подходящего конкретного класса.
            LinePoint target = null;
            return target;
        }

        /// <summary>
        ///Тест для next
        ///</summary>
        [TestMethod()]
        public void nextTest()
        {
            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();

            lcpA.vert.q.CopyValue(LinearMath.Vector.Create3D(0.0, 0.0, 0.0));
            lcpB.vert.q.CopyValue(LinearMath.Vector.Create3D(1.0, 1.0, 1.0));

            lcpA.next = lcpB;

            Assert.AreSame(lcpA, lcpB.prev);
        }

        /// <summary>
        ///Тест для next
        ///</summary>
        [TestMethod()]
        public void nextNullTest()
        {
            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();

            lcpA.next = lcpB;
            lcpA.next = null;

            Assert.IsNull(lcpB.prev);
        }

        [TestMethod()]
        public void nextChangedTest()
        {
            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();
            LinePointSegmentSpline lcpC = new LinePointSegmentSpline();

            lcpA.vert.q.CopyValue(LinearMath.Vector.Create3D(0.0, 0.0, 0.0));
            lcpB.vert.q.CopyValue(LinearMath.Vector.Create3D(1.0, 1.0, 1.0));
            lcpC.vert.q.CopyValue(LinearMath.Vector.Create3D(2.0, 2.0, 2.0));

            lcpA.next = lcpB;
            lcpA.next = lcpC;

            Assert.AreSame(lcpA, lcpC.prev);
        }

        [TestMethod()]
        public void nextChangedNullTest()
        {
            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();
            LinePointSegmentSpline lcpC = new LinePointSegmentSpline();

            lcpA.vert.q.CopyValue(LinearMath.Vector.Create3D(0.0, 0.0, 0.0));
            lcpB.vert.q.CopyValue(LinearMath.Vector.Create3D(1.0, 1.0, 1.0));
            lcpC.vert.q.CopyValue(LinearMath.Vector.Create3D(2.0, 2.0, 2.0));

            lcpA.next = lcpB;
            lcpA.next = lcpC;

            Assert.IsNull(lcpB.prev);
        }
    }
}
