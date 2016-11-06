using MshToMatWPF.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MshToMatWPF.Geometry.Point;

using LinearMath;
using MshToMatWPF.Geometry.Spline;

namespace MshToMatWPFTest
{
    
    
    /// <summary>
    ///Это класс теста для LineViewerTest, в котором должны
    ///находиться все модульные тесты LineViewerTest
    ///</summary>
    [TestClass()]
    public class LineViewerTest
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




        [TestMethod()]
        public void AddVertexTest()
        {
            LineViewerSpline line = new LineViewerSpline();
            line.colorManager = new MshToMatWPF.Preferences.ColorManager();

            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            lcpA.vert.q.CopyValue(Vector.Create3D(0.0, 0.0, 0.0));
            line.AddVertex(lcpA, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();
            lcpB.vert.q.CopyValue(Vector.Create3D(1.0, 0.0, 0.0));
            line.AddVertex(lcpB, LinePointAdditionMethod.last);

            Assert.AreEqual(lcpA.next, lcpB);
        }


        [TestMethod()]
        public void RemoveVertexTest()
        {
            LineViewerSpline line = new LineViewerSpline();
            line.colorManager = new MshToMatWPF.Preferences.ColorManager();

            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            lcpA.vert.q.CopyValue(Vector.Create3D(0.0, 0.0, 0.0));
            line.AddVertex(lcpA, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();
            lcpB.vert.q.CopyValue(Vector.Create3D(1.0, 0.0, 0.0));
            line.AddVertex(lcpB, LinePointAdditionMethod.last);

            line.RemoveVertex(lcpB);

            Assert.IsNull(lcpA.next);
        }

        [TestMethod()]
        public void RemoveVertexCentralTest()
        {
            LineViewerSpline line = new LineViewerSpline();
            line.colorManager = new MshToMatWPF.Preferences.ColorManager();

            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            lcpA.vert.q.CopyValue(Vector.Create3D(0.0, 0.0, 0.0));
            line.AddVertex(lcpA, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();
            lcpB.vert.q.CopyValue(Vector.Create3D(1.0, 0.0, 0.0));
            line.AddVertex(lcpB, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpC = new LinePointSegmentSpline();
            lcpC.vert.q.CopyValue(Vector.Create3D(2.0, 0.0, 0.0));
            line.AddVertex(lcpC, LinePointAdditionMethod.last);

            line.RemoveVertex(lcpB);

            Assert.AreEqual(lcpA.next, lcpC);
        }

        [TestMethod()]
        public void ConfirmCollectionTest()
        {
            LineViewerSpline line = new LineViewerSpline();
            line.colorManager = new MshToMatWPF.Preferences.ColorManager();

            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            lcpA.vert.q.CopyValue(Vector.Create3D(0.0, 0.0, 0.0));
            line.AddVertex(lcpA, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();
            lcpB.vert.q.CopyValue(Vector.Create3D(1.0, 0.0, 0.0));
            line.AddVertex(lcpB, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpC = new LinePointSegmentSpline();
            lcpC.vert.q.CopyValue(Vector.Create3D(2.0, 0.0, 0.0));
            line.AddVertex(lcpC, LinePointAdditionMethod.last);

            Assert.IsTrue(line.ConfirmCollection());
        }

        [TestMethod()]
        public void ConfirmCollectionAfterRemoveTest()
        {
            LineViewerSpline line = new LineViewerSpline();
            line.colorManager = new MshToMatWPF.Preferences.ColorManager();

            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            lcpA.vert.q.CopyValue(Vector.Create3D(0.0, 0.0, 0.0));
            line.AddVertex(lcpA, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();
            lcpB.vert.q.CopyValue(Vector.Create3D(1.0, 0.0, 0.0));
            line.AddVertex(lcpB, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpC = new LinePointSegmentSpline();
            lcpC.vert.q.CopyValue(Vector.Create3D(2.0, 0.0, 0.0));
            line.AddVertex(lcpC, LinePointAdditionMethod.last);

            line.RemoveVertex(lcpB);

            Assert.IsTrue(line.ConfirmCollection());
        }
        
        [TestMethod()]
        public void RefineTest()
        {
            LineViewerSpline line = new LineViewerSpline();
            line.colorManager = new MshToMatWPF.Preferences.ColorManager();

            LinePointSegmentSpline lcpA = new LinePointSegmentSpline();
            lcpA.vert.q.CopyValue(Vector.Create3D(0.0, 0.0, 0.0));
            line.AddVertex(lcpA, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpB = new LinePointSegmentSpline();
            lcpB.vert.q.CopyValue(Vector.Create3D(1.0, 0.0, 0.0));
            line.AddVertex(lcpB, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpC = new LinePointSegmentSpline();
            lcpC.vert.q.CopyValue(Vector.Create3D(2.0, 0.0, 0.0));
            line.AddVertex(lcpC, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpD = new LinePointSegmentSpline();
            lcpD.vert.q.CopyValue(Vector.Create3D(3.0, 0.0, 0.0));
            line.AddVertex(lcpD, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpE = new LinePointSegmentSpline();
            lcpE.vert.q.CopyValue(Vector.Create3D(4.0, 0.0, 0.0));
            line.AddVertex(lcpE, LinePointAdditionMethod.last);

            line.Refine();

            Assert.AreEqual(lcpA.next, lcpE);
        }


        /// <summary>
        /// x^2 + y^2 = 1.0
        /// </summary>
        class SplineTestHelper : ISpline
        {
            public event EventHandler Changed;

            public void UpdatePoint(LinePointSegment lcp)
            {
                if (lcp is LinePointSegmentSpline)
                {
                    double phi = Math.PI * (lcp as LinePointSegmentSpline).t;
                    double x = Math.Cos(phi);
                    double y = Math.Sin(phi);
                    double z = 0.0;
                    lcp.vert.q.CopyValue(Vector.Create3D(x, y, z));
                }
            }

            public void UpdateSegment(Segment seg)
            {
                if (seg is SegmentSpline)
                {
                    Vector p = 0.5 * (seg.startPoint.vert.q + seg.endPoint.vert.q);
                    (seg as SegmentSpline).distance = 1.0 - p.Length();
                }
            }

            public void AddControllPoint(LinePointControl lcp)
            {

            }

            public void RemoveControllPoint(LinePointControl lcp)
            {

            }

            public LinePointSegmentSpline CreatePoint(double t)
            {
                LinePointSegmentSpline lcp = new LinePointSegmentSpline();
                lcp.t = t;
                lcp.spline = this;
                return lcp;
            }
        }

        [TestMethod()]
        public void RefineAddTest()
        {
            LineViewerSpline line = new LineViewerSpline();
            line.colorManager = new MshToMatWPF.Preferences.ColorManager();

            SplineTestHelper spline = new SplineTestHelper();

            LinePointSegmentSpline lcpA = spline.CreatePoint(0.0);
            line.AddVertex(lcpA, LinePointAdditionMethod.last);

            LinePointSegmentSpline lcpB = spline.CreatePoint(1.0);
            line.AddVertex(lcpB, LinePointAdditionMethod.last);

            line.Refine();
            line.Refine();

            Assert.AreEqual(lcpA.next.next, lcpB);
        }
    }
}
