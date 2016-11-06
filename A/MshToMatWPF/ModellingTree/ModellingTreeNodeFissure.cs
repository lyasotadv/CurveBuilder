using System;

using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

using MshToMatWPF.Geometry.Spline;
using MshToMatWPF.ModellingTree.ToolsNodeStrategy;
using MshToMatWPF.ModellingTree.ContextMenuNodeStrategy;

namespace MshToMatWPF.ModellingTree
{
    class ModellingTreeNodeFissure : ModellingTreeNode
    {
        private SplineController _spline;

        public SplineController spline
        {
            get
            {
                return _spline;
            }
            set
            {
                if (value != null)
                {
                    _spline = value;
                    _spline.SetColorManager(colorManager);
                    if (ProjectNode != null)
                    {
                        ProjectNode.DrawableObjectCreate(spline.lineViewer);
                        ProjectNode.DrawableObjectCreate(spline.controlPointCloud);

                        spline.lineViewer .Selected += OnDrawableObjectSelected;
                        spline.controlPointCloud.Selected += OnDrawableObjectSelected;
                    }
                }
                else
                {
                    if (ProjectNode != null)
                    {
                        ProjectNode.DrawableObjectRemove(spline.lineViewer);
                        ProjectNode.DrawableObjectRemove(spline.controlPointCloud);

                        spline.lineViewer.Selected -= OnDrawableObjectSelected;
                        spline.controlPointCloud.Selected -= OnDrawableObjectSelected;
                    }
                    _spline = value;
                }
            }
        }

        public Func<Point, LinearMath.Vector> ScreenToWorld { get; set; }

        public ModellingTreeNodeFissure(string name, ModellingTreeController treeController)
        {
            this.Name = name;
            this.treeController = treeController;
            this.toolsStrategy = new ToolsNodeStrategyFissure();
            this.contextMenuStrategy = new ContextMenuNodeStrategyFissure(this);

            this.treeController.RemoveNode += RemoveCurrentNode;
        }

        public override void Initialize()
        {
            spline = new SplineController();
        }

        private void RemoveCurrentNode(object sender, EventArgs args)
        {
            if (args is EventArgsModellingTreeNode)
            {
                MshToMatWPF.ModellingTree.ModellingTreeNode node = (args as EventArgsModellingTreeNode).nodeCurrent;
                if ((node != this) & (treeController.DetermParentFromPair(node, this) != node))
                    return;
                spline = null;
            }
        }

        public void OnMouseDown(object sender, EventArgs arg)
        {
            if ((IsActive) & (spline != null))
            {
                if (arg is MouseButtonEventArgs)
                {
                    if ((arg as MouseButtonEventArgs).LeftButton == MouseButtonState.Pressed)
                    {
                        Point p = (arg as MouseEventArgs).GetPosition(sender as Grid);
                        if ((this.toolsStrategy as ToolsNodeStrategyFissure).IsAddNewPointEnabled)
                            spline.AddPoint(ScreenToWorld(p));
                    }

                    if ((arg as MouseButtonEventArgs).RightButton == MouseButtonState.Pressed)
                    {
                        contextMenuStrategy.Reload();
                    }
                }
            }
        }

        public void Split(object sender, EventArgs args)
        {

        }

        public void DeletePoint(object sender, EventArgs arg)
        {

        }
    }
}
