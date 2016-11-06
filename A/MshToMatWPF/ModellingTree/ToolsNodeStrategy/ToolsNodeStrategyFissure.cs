using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;

namespace MshToMatWPF.ModellingTree.ToolsNodeStrategy
{
    class ToolsNodeStrategyFissure : IToolsNodeStrategy
    {
        public List<Control> controlList { get; private set; }

        private CheckBox chbxAddPoint;

        public  ToolsNodeStrategyFissure()
        {
            controlList = new List<Control>();

            chbxAddPoint = new CheckBox();
            chbxAddPoint.Content = "Add point";
            chbxAddPoint.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            chbxAddPoint.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            chbxAddPoint.Width = 75;
            chbxAddPoint.Height = 16;
            DockPanel.SetDock(chbxAddPoint, Dock.Top);
            controlList.Add(chbxAddPoint);
        }

        public bool IsAddNewPointEnabled
        {
            get
            {
                return chbxAddPoint.IsChecked == true;
            }
        }
    }
}
