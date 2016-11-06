using System;


using LinearMath;
using MshToMatWPF.Preferences;
using MshToMatWPF.ModellingTree.ToolsNodeStrategy;
using MshToMatWPF.ModellingTree.ContextMenuNodeStrategy;

namespace MshToMatWPF.ModellingTree
{
    abstract public class ModellingTreeNode : INode
    {
        public IToolsNodeStrategy toolsStrategy { get; protected set; }

        public IContextMenuNodeStrategy contextMenuStrategy { get; protected set; }

        public class ModellingTreeNodeDialogData
        {
            public ModellingTreeNode node { get; set; }
        }

        public class ModellingTreeNodeDialogDataRename : ModellingTreeNodeDialogData
        {
            public string newName { get; set; }

            public ModellingTreeNodeDialogDataRename()
            {
                newName = string.Empty;
            }
        }

        public ColorManager colorManager { get; set; }

        public delegate void ShowDialog(ModellingTreeNodeDialogData data);
        
        public ShowDialog renameDialog { get; set; }

        public Guid guid { get; private set; }

        public string Name { get; protected set; }

        protected ModellingTreeController treeController { get; set; }

        public ModellingTreeNode()
        {
            guid = Guid.NewGuid();
        }

        public virtual void Initialize()
        {

        }

        public void ContextMenuRename(object sender, EventArgs args)
        {            
            if (renameDialog != null)
            {
                string oldName = Name;

                ModellingTreeNodeDialogDataRename data = new ModellingTreeNodeDialogDataRename();
                data.node = this;
                renameDialog(data);

                if (data.newName == string.Empty)
                    return;

                Name = data.newName;
                this.treeController.Rename(this, oldName);
            }
        }

        public void ContextMenuRemove(object sender, EventArgs args)
        {
            treeController.RemoveModellingNode(this);
        }

        public ModellingTreeNodeProject ProjectNode
        {
            get
            {
                return (ModellingTreeNodeProject)treeController.FindParentModellingTreeNodeByType(this, typeof(ModellingTreeNodeProject));
            }
        }

        public bool IsActive
        {
            get
            {
                return this == treeController.ActiveNode;
            }
            set
            {
                if (value & !IsActive)
                    treeController.ActiveNode = this;
            }
        }

        public void IsActiveBranch(out bool parent, out bool child)
        {
            parent = false;
            child = false;

            ModellingTreeNode node = treeController.DetermParentFromPair(this, treeController.ActiveNode);

            if (node == this)
            {
                child = true;
                return;
            }

            if (node == treeController.ActiveNode)
            {
                parent = true;
                return;
            }
        }

        protected void OnDrawableObjectSelected(object sender, EventArgs arg)
        {
            this.IsActive = true;
        }
    }
}
