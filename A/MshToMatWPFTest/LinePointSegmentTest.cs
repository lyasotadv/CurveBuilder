using MshToMatWPF.Geometry.Point;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LinearMath;

namespace MshToMatWPFTest
{
    
    
    /// <summary>
    ///Это класс теста для LinePointSegmentTest, в котором должны
    ///находиться все модульные тесты LinePointSegmentTest
    ///</summary>
    [TestClass()]
    public class LinePointSegmentTest
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


        internal virtual LinePointSegment_Accessor CreateLinePointSegment_Accessor()
        {
            // TODO: создайте экземпляр подходящего конкретного класса.
            LinePointSegment_Accessor target = null;
            return target;
        }

        /// <summary>
        ///Тест для tau
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MshToMatWPF.exe")]
        public void tauTest1()
        {
            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();

            lcpA.vert.q.CopyValue(LinearMath.Vector.Create3D(0.0, 0.0, 0.0));
            lcpB.vert.q.CopyValue(LinearMath.Vector.Create3D(1.0, 0.0, 0.0));

            lcpA.next = lcpB;

            Assert.AreEqual(lcpA.tau, Vector.Create3D(1.0, 0.0, 0.0));
        }

        /// <summary>
        ///Тест для tau
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MshToMatWPF.exe")]
        public void tauTest2()
        {
            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();

            lcpA.vert.q.CopyValue(LinearMath.Vector.Create3D(0.0, 0.0, 0.0));
            lcpB.vert.q.CopyValue(LinearMath.Vector.Create3D(1.0, 0.0, 0.0));

            lcpA.next = lcpB;

            Assert.AreEqual(lcpB.tau, Vector.Create3D(1.0, 0.0, 0.0));
        }

        /// <summary>
        ///Тест для tau
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MshToMatWPF.exe")]
        public void tauTest3()
        {
            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();

            lcpA.vert.q.CopyValue(LinearMath.Vector.Create3D(0.0, 0.0, 0.0));
            lcpB.vert.q.CopyValue(LinearMath.Vector.Create3D(1.0, 0.0, 0.0));

            lcpA.next = lcpB;

            lcpA.vert.q.CopyValue(LinearMath.Vector.Create3D(0.0, 0.0, 0.0));
            lcpB.vert.q.CopyValue(LinearMath.Vector.Create3D(0.0, 1.0, 0.0));

            Assert.AreEqual(lcpA.tau, Vector.Create3D(0.0, 1.0, 0.0));
        }

        /// <summary>
        ///Тест для tau
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MshToMatWPF.exe")]
        public void tauTest4()
        {
            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();

            lcpA.vert.q.CopyValue(LinearMath.Vector.Create3D(0.0, 0.0, 0.0));
            lcpB.vert.q.CopyValue(LinearMath.Vector.Create3D(1.0, 0.0, 0.0));

            lcpA.next = lcpB;

            lcpA.vert.q.CopyValue(LinearMath.Vector.Create3D(0.0, 0.0, 0.0));
            lcpB.vert.q.CopyValue(LinearMath.Vector.Create3D(0.0, 1.0, 0.0));

            Assert.AreEqual(lcpB.tau, Vector.Create3D(0.0, 1.0, 0.0));
        }

        internal virtual LinePointSegment CreateLinePointSegment()
        {
            // TODO: создайте экземпляр подходящего конкретного класса.
            LinePointSegment target = null;
            return target;
        }

        /// <summary>
        ///Тест для outSeg
        ///</summary>
        [TestMethod()]
        public void outSegTest()
        {
            LinePointSegmentSpline lcp = new LinePointSegmentSpline();
            SegmentSpline seg = new SegmentSpline();

            lcp.outSeg = seg;

            Assert.AreEqual(seg.startPoint, lcp);
        }

        /// <summary>
        ///Тест для inSeg
        ///</summary>
        [TestMethod()]
        public void inSegTest()
        {
            LinePointSegmentSpline lcp = new LinePointSegmentSpline();
            SegmentSpline seg = new SegmentSpline();

            lcp.inSeg = seg;

            Assert.AreEqual(seg.endPoint, lcp);
        }

        /// <summary>
        ///Тест для outSeg
        ///</summary>
        [TestMethod()]
        public void outSegNullTest()
        {
            LinePointSegmentSpline lcp = new LinePointSegmentSpline();
            SegmentSpline seg = new SegmentSpline();

            lcp.outSeg = seg;
            lcp.outSeg = null;

            Assert.IsNull(seg.startPoint);
        }

        /// <summary>
        ///Тест для inSeg
        ///</summary>
        [TestMethod()]
        public void inSegNullTest()
        {
            LinePointSegmentSpline lcp = new LinePointSegmentSpline();
            SegmentSpline seg = new SegmentSpline();

            lcp.inSeg = seg;
            lcp.inSeg = null;

            Assert.IsNull(seg.endPoint);
        }
    }
}
