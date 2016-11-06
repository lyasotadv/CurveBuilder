using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MshToMatWPF.ModellingTree;

namespace MshToMatWPF.SubWindow
{
    /// <summary>
    /// Логика взаимодействия для RenameModellingTreeNode.xaml
    /// </summary>
    public partial class RenameModellingTreeNode : Window
    {
        ModellingTreeNode.ModellingTreeNodeDialogDataRename data;

        public RenameModellingTreeNode(ModellingTreeNode.ModellingTreeNodeDialogData data)
        {
            InitializeComponent();
            if (data is ModellingTreeNode.ModellingTreeNodeDialogDataRename)
                this.data = data as ModellingTreeNode.ModellingTreeNodeDialogDataRename;
            txtName.Text = this.data.node.Name;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text != string.Empty)
                data.newName = txtName.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
