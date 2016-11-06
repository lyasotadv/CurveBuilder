using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MshToMatWPF.Geometry;
using MshToMatWPF.Drawing;

namespace MshToMatWPF.ModellingTree
{
    public class ModellingTreeNodeProject : ModellingTreeNode
    {
        public ModellingTreeNodeProject(string name, ModellingTreeController treeController)
        {
            this.Name = name;
            this.treeController = treeController;
        }

        public void ContextMenuNewGrid(object sender, EventArgs args)
        {
            string name = this.treeController.ExistedModellingTreeNodeName("Grid");
            this.treeController.AddNewGrid(this, name);
        }

        public event EventHandler DrawableObjectCreated;

        public event EventHandler DrawableObjectRemoved;

        public void DrawableObjectCreate(Drawable obj)
        {
            if (DrawableObjectCreated != null)
                DrawableObjectCreated(this, new EventArgsDrawableObject() { CurrentObj = obj });
        }

        public void DrawableObjectRemove(Drawable obj)
        {
            if ((DrawableObjectRemoved != null)&(obj != null))
                DrawableObjectRemoved(this, new EventArgsDrawableObject() { CurrentObj = obj });
        }
    }

    public class EventArgsDrawableObject : EventArgs
    {
        public Drawable CurrentObj { get; set; }
    }
}
