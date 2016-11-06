using LinearMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LinearMathTest
{
    
    
    /// <summary>
    ///Это класс теста для TreeTest, в котором должны
    ///находиться все модульные тесты TreeTest
    ///</summary>
    [TestClass()]
    public class TreeTest
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
        ///Тест для Конструктор Tree`1
        ///</summary>
        public void TreeConstructorTestHelper<T>()
        {
            TreeHelper tree = new TreeHelper();

            NodeHelper root = tree.CreateRoot();

            tree.CreateRandomBranch(root, 5, 2, 3);

            NodeHelper nodeA = tree.GetRandomNode();
            NodeHelper nodeB = tree.GetRandomNode();

            List<NodeHelper> path = tree.GetPath(nodeA, nodeB);

            List<int> common = new List<int>();

            for (int n = 0; n < Math.Min(nodeA.adr.Count, nodeB.adr.Count); n++)
            {
                if (nodeA.adr[n] == nodeB.adr[n])
                    common.Add(nodeA.adr[n]);
                else
                    break;
            }

            int n_up = nodeA.adr.Count - common.Count + 1;
            int n_down = nodeB.adr.Count - common.Count + 1;

            List<int>[] lst = new List<int>[n_up + n_down];

            for (int n = 0; n < n_up; n++)
            {
                lst[n] = new List<int>();
                for (int m = 0; m < nodeA.adr.Count - n; m++)
                    lst[n].Add(nodeA.adr[m]);
            }

            for (int n = 0; n < n_down; n++)
            {
                lst[n + n_up] = new List<int>();
                for (int m = 0; m < common.Count + n; m++)
                    lst[n + n_up].Add(nodeB.adr[m]);
            }

            bool result = true;

            if (lst.Length == path.Count)
            {
                for (int n = 0; n < lst.Length; n++)
                {
                    for (int m = 0; m < lst[n].Count; m++)
                    {
                        if (lst[n][m] != path[n].adr[m])
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                result = false;
            }

            Assert.IsTrue(result);
        }

        class TreeHelper : Tree<NodeHelper>
        {
            List<NodeHelper> _nodelist;

            Random rand;

            public List<NodeHelper> nodelist
            {
                get
                {
                    return _nodelist;
                }
            }

            public TreeHelper()
                : base()
            {
                _nodelist = new List<NodeHelper>();
                rand = new Random(DateTime.Now.Millisecond);
            }

            private NodeHelper CreateNode(NodeHelper parent, int cnt)
            {
                List<int> adr = null;
                if (parent != null)
                    adr = parent.adr;
                NodeHelper node = new NodeHelper(adr, cnt);
                AddNode(node, parent);
                nodelist.Add(node);
                return node;
            }

            public NodeHelper CreateRoot()
            {
                NodeHelper node = CreateNode(null, 0);
                return node;
            }

            public void CreateRandomBranch(NodeHelper root, int Generations, int MinBranchCnt, int MaxBranchCnt)
            {
                if (Generations == 0)
                    return;

                int cnt = rand.Next(MinBranchCnt, MaxBranchCnt + 1);

                for (int n = 0; n < cnt; n++)
                {
                    NodeHelper node = CreateNode(root, n);
                    CreateRandomBranch(node, Generations - 1, MinBranchCnt, MaxBranchCnt);
                }

            }

            public NodeHelper GetRandomNode()
            {
                int ind = rand.Next(nodelist.Count - 1);
                return nodelist[ind];
            }

            public List<NodeHelper> GetPath(NodeHelper nodeA, NodeHelper nodeB)
            {
                List<NodeHelper> up;
                List<NodeHelper> down;
                Path(nodeA, nodeB, out up, out down);
                up.AddRange(down);
                return up;
            }

            public NodeHelper Find(string adr)
            {
                Predicate<NodeHelper> condition = (node) => { return node.ToString() == adr; };
                return base.Find(condition);
            }
        }

        class NodeHelper : INode
        {
            Guid _guid;

            List<int> _adr;

            public Guid guid
            {
                get
                {
                    return _guid;
                }
            }

            public List<int> adr
            {
                get
                {
                    return _adr;
                }
            }

            public NodeHelper(List<int> adrRoot, int cnt)
            {
                _guid = Guid.NewGuid();
                _adr = new List<int>();
                if (adrRoot != null)
                {
                    foreach (var x in adrRoot)
                        adr.Add(x);
                }
                adr.Add(cnt);
            }

            public override string ToString()
            {
                string str = string.Empty;
                foreach (var s in adr)
                    str += s.ToString();
                return str;
            }
        }

        [TestMethod()]
        public void TreeConstructorTest()
        {
            TreeConstructorTestHelper<GenericParameterHelper>();
        }

        [TestMethod()]
        public void TreeFindTest()
        {
            TreeHelper tree = new TreeHelper();
            NodeHelper root = tree.CreateRoot();
            tree.CreateRandomBranch(root, 5, 2, 3);
            NodeHelper Node = tree.GetRandomNode();
            NodeHelper n = tree.Find(Node.ToString());
            Assert.IsTrue(n.ToString() == Node.ToString());
        }
    }
}
