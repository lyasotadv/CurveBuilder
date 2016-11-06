using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearMath
{
    public class Tree<T>
    {
        Dictionary<Guid, T> nodelist;

        Dictionary<Guid, Guid> parentlist;

        public Tree()
        {
            nodelist = new Dictionary<Guid, T>();
            parentlist = new Dictionary<Guid, Guid>();
        }

        private T getParent(T node)
        {
            if (IsRoot(node))
                return default(T);
            Guid pg;
            parentlist.TryGetValue(getGuid(node), out pg);
            T p;
            nodelist.TryGetValue(pg, out p);
            return p;
        }

        private T[] getChild(T node)
        {
            Guid pg = getGuid(node);
            List<Guid> lst = new List<Guid>();
            foreach (var pair in parentlist)
            {
                if (pair.Value == pg)
                {
                    lst.Add(pair.Key);
                }
            }

            int N = lst.Count;

            T[] t = new T[N];

            for (int n = 0; n < N; n++)
            {
                nodelist.TryGetValue(lst[n], out t[n]);
            }

            return t;
        }

        private List<T> getRoot()
        {
            List<T> root = new List<T>();
            foreach (var node in nodelist.Values)
            {
                if (IsRoot(node))
                    root.Add(node);
            }
            return root;
        }

        private Guid getGuid(T node)
        {
            return ((INode)node).guid;
        }

        private List<Guid> gain(T node)
        {
            List<Guid> lst = new List<Guid>();
            T nodeTmp = node;
            while (true)
            {
                lst.Add(getGuid(nodeTmp));
                if (IsRoot(nodeTmp))
                    break;
                nodeTmp = getParent(nodeTmp);
            }
            lst.Reverse();
            return lst;
        }

        private T CommonNode(T nodeA, T nodeB)
        {
            List<Guid> lstA = gain(nodeA);
            List<Guid> lstB = gain(nodeB);

            Guid g = Guid.Empty;
            for (int n = 0; n < Math.Min(lstA.Count, lstB.Count); n++)
            {
                if (lstA[n] != lstB[n])
                {
                    g = lstA[n - 1];
                    break;
                }
            }

            if (g == Guid.Empty)
                g = lstA.First<Guid>();

            T node;
            nodelist.TryGetValue(g, out node);
            return node;
        }

        private bool Find(T node, Predicate<T> condition, out T nodeFounded)
        {
            if (condition(node))
            {
                nodeFounded = node;
                return true;
            }

            T[] child = getChild(node);
            foreach (var t in child)
            {
                if (Find(t, condition, out nodeFounded))
                    return true;
            }

            nodeFounded = default(T);
            return false;
        }

        private void CreateTree(T parent, Action<T> creator)
        {
            creator(parent);
            foreach (var node in getChild(parent))
                CreateTree(node, creator);
        }

        protected void Path(T nodeA, T nodeB, out List<T> up, out List<T> down)
        {
            up = new List<T>();
            down = new List<T>();

            T node = CommonNode(nodeA, nodeB);
            T nodeTmp = nodeA;
            while (true)
            {
                up.Add(nodeTmp);
                if (getGuid(node) == getGuid(nodeTmp))
                    break;
                nodeTmp = getParent(nodeTmp);
            }

            nodeTmp = nodeB;
            while (true)
            {
                down.Add(nodeTmp);
                if (getGuid(node) == getGuid(nodeTmp))
                    break;
                nodeTmp = getParent(nodeTmp);
            }

            down.Reverse();
        }

        protected List<T> Path(T node)
        {
            List<T> list = new List<T>();
            if (node == null)
                return list;
            list.Add(node);
            T parent = node;
            while (true)
            {
                parent = getParent(parent);
                if (parent == null)
                    break;
                list.Add(parent);
            }
            list.Reverse();
            return list;
        }

        protected void AddNode(T node, T parent)
        {
            nodelist.Add(getGuid(node), node);
            if (parent != null)
                parentlist.Add(getGuid(node), getGuid(parent));
        }

        protected T Remove(T node)
        {
            T parent = getParent(node);
            T[] child = getChild(node);
            foreach (var n in child)
                Remove(n);
            parentlist.Remove(getGuid(node));
            nodelist.Remove(getGuid(node));
            return parent;
        }

        protected bool IsRoot(T node)
        {
            return !(parentlist.ContainsKey(getGuid(node)));
        }

        protected bool IsLeaf(T node)
        {
            return getChild(node).Length == 0;
        }

        protected T Find(Predicate<T> condition)
        {
            List<T> root = getRoot();
            foreach (var r in root)
            {
                T node;
                Find(r, condition, out node);
                if (node != null)
                    return node;
            }
            return default(T);
        }

        protected T[] FindAll(Predicate<T> condition)
        {
            List<T> lst = new List<T>();
            foreach (var item in nodelist.Values)
            {
                if (condition(item))
                    lst.Add(item);
            }
            return lst.ToArray();
        }

        protected T FindNearestParent(T node, Predicate<T> condition)
        {
            if (condition(node))
                return node;

            T parent = getParent(node);
            if (parent == null)
                return default(T);

            return FindNearestParent(parent, condition);
        }

        protected void CreateTree(Action<T> creator)
        {
            foreach (var node in getRoot())
            {
                CreateTree(node, creator);
            }
        }

        protected int Level(T node)
        {
            return Path(node).Count;
        }
    }

    public interface INode
    {
        Guid guid
        {
            get;
        }
    }

    
}
