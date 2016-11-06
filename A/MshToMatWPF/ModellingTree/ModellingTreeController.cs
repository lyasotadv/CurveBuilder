using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LinearMath;
using MshToMatWPF.Preferences;

namespace MshToMatWPF.ModellingTree
{
    public class ModellingTreeController : Tree<ModellingTreeNode>
    {
        public event EventHandler NodeAdded;

        public event EventHandler ActiveNodeChanged;

        public event EventHandler RenameNode;

        public event EventHandler RemoveNode;

        private ModellingTreeNode _ActiveNode;

        public ModellingTreeNode ActiveNode
        {
            get
            {
                return _ActiveNode;
            }
            set
            {
                _ActiveNode = value;
                if (ActiveNodeChanged != null)
                    ActiveNodeChanged(this, new EventArgsModellingTreeNode() { path = ModellingTreeNodePath(ActiveNode), nodeCurrent = ActiveNode });
            }
        }

        public ColorManager colorManager { get; set; }

        public ModellingTreeController()
            : base()
        {
            ActiveNode = null;
        }

        private void AddModellingTreeNode(ModellingTreeNode node, ModellingTreeNode parent)
        {
            if (FindModellingTreeNode(node.Name) == null)
            {
                AddNode(node, parent);
                node.colorManager = colorManager;
                if (NodeAdded != null)
                {
                    NodeAdded(this, new EventArgsModellingTreeNode() { path = ModellingTreeNodePath(node), nodeCurrent = node });
                }
                node.Initialize();
                ActiveNode = node;
            }
        }

        private string[] ModellingTreeNodePath(ModellingTreeNode node)
        {
            List<ModellingTreeNode> list = Path(node);
            string[] pathName = new string[list.Count];
            for (int n = 0; n < pathName.Length; n++)
            {
                pathName[n] = list[n].Name;
            }
            return pathName;
        }

        public ModellingTreeNode AddNewProject(string projectname)
        {
            ModellingTreeNodeProject nodeProj = new ModellingTreeNodeProject(projectname, this);
            AddModellingTreeNode(nodeProj, null);
            return nodeProj;
        }

        public ModellingTreeNode AddNewGrid(ModellingTreeNode nodeProject, string gridname)
        {
            if (nodeProject is ModellingTreeNodeProject)
            {
                ModellingTreeNodeGrid nodeGrid = new ModellingTreeNodeGrid(gridname, this);
                AddModellingTreeNode(nodeGrid, nodeProject);
                return nodeGrid;
            }
            return null;
        }

        public ModellingTreeNode AddNewFissure(ModellingTreeNode nodeGrid, string fissurename)
        {
            if (nodeGrid is ModellingTreeNodeGrid)
            {
                ModellingTreeNodeFissure nodeFissure = new ModellingTreeNodeFissure(fissurename, this);
                AddModellingTreeNode(nodeFissure, nodeGrid);
                return nodeFissure;
            }
            return null;
        }

        public void RemoveModellingNode(ModellingTreeNode node)
        {
            EventArgsModellingTreeNode args = new EventArgsModellingTreeNode();
            args.nodeCurrent = node;
            args.path = ModellingTreeNodePath(node);

            if (RemoveNode != null)
                RemoveNode(this, args);

            ModellingTreeNode parent = Remove(node);
            if (parent != null)
                ActiveNode = parent;
        }

        public ModellingTreeNode FindModellingTreeNode(string name)
        {
            Predicate<ModellingTreeNode> condition = (t) => { return t.Name == name; };
            return Find(condition);
        }

        public string ExistedModellingTreeNodeName(string name)
        {
            int n = 1;
            while (true)
            {
                string str = name + " " + n.ToString();
                if (FindModellingTreeNode(str) == null)
                    return str;
                n++;
            }
        }

        public void Rename(ModellingTreeNode node, string oldName)
        {
            EventArgsModellingTreeNode args = new EventArgsModellingTreeNode();
            args.path = ModellingTreeNodePath(node);
            args.path[args.path.Length - 1] = oldName;
            args.nodeCurrent = node;

            if (RenameNode != null)
                RenameNode(this, args);
        }

        public ModellingTreeNode FindParentModellingTreeNodeByType(ModellingTreeNode node, Type T)
        {
            Predicate<ModellingTreeNode> condition = (t) =>
                {
                    return t.GetType() == T;
                };
            return FindNearestParent(node, condition);
        }

        public ModellingTreeNode DetermParentFromPair(ModellingTreeNode nodeA, ModellingTreeNode nodeB)
        {
            if (nodeA == nodeB)
                return null;

            Predicate<ModellingTreeNode> conditionA = (t) =>
                {
                    return t == nodeA;
                };

            Predicate<ModellingTreeNode> conditionB = (t) =>
                {
                    return t == nodeB;
                };

            if (FindNearestParent(nodeA, conditionB) != null)
                return nodeB;

            if (FindNearestParent(nodeB, conditionA) != null)
                return nodeA;

            return null;
        }
    }

    class EventArgsModellingTreeNode : EventArgs
    {
        public string[] path { get; set; }

        public ModellingTreeNode nodeCurrent { get; set; }

        public EventArgsModellingTreeNode()
        {

        }
    }
}
