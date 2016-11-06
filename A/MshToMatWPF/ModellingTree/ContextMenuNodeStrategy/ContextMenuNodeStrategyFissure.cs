using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows;

namespace MshToMatWPF.ModellingTree.ContextMenuNodeStrategy
{
    class ContextMenuNodeStrategyFissure : IContextMenuNodeStrategy
    {
        private ModellingTreeNodeFissure node;
        
        public ContextMenu menu { get; private set; }

        public void Reload()
        {
            Action<string, EventHandler> AddItem = (str, handler) =>
            {
                MenuItem item = new MenuItem();
                item.Header = str;
                item.Click += new RoutedEventHandler(handler);
                menu.Items.Add(item);
            };

            menu.Items.Clear();

            //if (node.spline.line.IsSelected)
            //    AddItem("Split", node.Split);

            //if (node.spline.cloud.IsSelected)
            //    AddItem("Delete point", node.DeletePoint);
        }

        public ContextMenuNodeStrategyFissure(ModellingTreeNodeFissure node)
        {
            this.node = node;
            menu = new ContextMenu();
        }
    }
}
