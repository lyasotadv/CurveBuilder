using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MshToMatWPF.MeshImporter;
using MshToMatWPF.Drawing;
using MshToMatWPF.Drawing.Primitives;

namespace MshToMatWPF.ModellingTree
{
    public class ModellingTreeNodeGrid : ModellingTreeNode
    {
        public class ModellingTreeNodeDialogDataImport : ModellingTreeNodeDialogData
        {
            public string filename { get; set; }

            public ModellingTreeNodeDialogDataImport()
            {
                filename = string.Empty;
            }
        }

        public ShowDialog importDialog { get; set; }

        private MeshContainer _geomManager;

        public MeshContainer geomManager
        {
            get
            {
                return _geomManager;
            }
            set
            {
                if (value != null)
                {
                    _geomManager = value;
                    _geomManager.colorManager = colorManager;
                    if (ProjectNode != null)
                        ProjectNode.DrawableObjectCreate(geomManager);
                }
                else
                {
                    if (ProjectNode != null)
                        ProjectNode.DrawableObjectRemove(geomManager);
                    _geomManager = value;
                }
            }
        }

        public ModellingTreeNodeGrid(string name, ModellingTreeController treeController)
        {
            base.Name = name;
            this.treeController = treeController;
            geomManager = null;

            this.treeController.RemoveNode += RemoveCurrentNode;
        }

        private void RemoveCurrentNode(object sender, EventArgs args)
        {
            if (args is EventArgsModellingTreeNode)
            {
                MshToMatWPF.ModellingTree.ModellingTreeNode node = (args as EventArgsModellingTreeNode).nodeCurrent;
                if ((node != this) & (treeController.DetermParentFromPair(node, this) != node))
                    return;
                geomManager = null;
            }
        }

        public void ContextMenuAddNewFissure(object sender, EventArgs args)
        {
            string name = treeController.ExistedModellingTreeNodeName("Fissure");
            treeController.AddNewFissure(this, name);
        }

        public void ContextMenuImport(object sender, EventArgs args)
        {
            if (importDialog != null)
            {
                ModellingTreeNodeDialogDataImport data = new ModellingTreeNodeDialogDataImport();
                data.node = this;

                importDialog(data);

                if (data.filename != string.Empty)
                {
                    ImportMesh imp = new ImportMeshMsh(data.filename);
                    imp.Build();
                    this.geomManager = imp.geomManager;
                }
            }
        }
    }
}
