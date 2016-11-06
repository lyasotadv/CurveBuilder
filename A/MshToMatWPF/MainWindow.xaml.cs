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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;
using System.Windows.Media.Media3D;

using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

using MshToMatWPF.ModellingTree;
using MshToMatWPF.SubWindow;
using MshToMatWPF.Drawing.Primitives;
using MshToMatWPF.Geometry;
using MshToMatWPF.Preferences;
using MshToMatWPF.ModellingTree.ToolsNodeStrategy;

namespace MshToMatWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ModellingTreeController treeController;
        TreeMapping mapping;
        ColorManager colorManager;

        public MainWindow()
        {
            InitializeComponent();
            
            treeController = new ModellingTreeController();
            treeController.ActiveNodeChanged += ChangeWindowTitle;

            mapping = new TreeMapping(treeController, treeModellingTree);
            mapping.wind = this;

            colorManager = new ColorManager();
            treeController.colorManager = colorManager;
        }

        public void UpdatePanelTools(IToolsNodeStrategy formStrategy)
        {
            dockTools.Children.Clear();
            if (formStrategy != null)
            {
                foreach (var cont in formStrategy.controlList)
                {
                    dockTools.Children.Add(cont);
                }
            }
            
        }

        private void ChangeWindowTitle(object sender, EventArgs args)
        {
            if (args is EventArgsModellingTreeNode)
            {
                if ((args as EventArgsModellingTreeNode).nodeCurrent != null)
                    this.Title = (args as EventArgsModellingTreeNode).nodeCurrent.Name;
                else
                    this.Title = string.Empty;
            }
        }

        private void MainMenuFileNewProject(object sender, RoutedEventArgs args)
        {
            string name = treeController.ExistedModellingTreeNodeName("Project");
            ModellingTreeNode nodeProjectA = treeController.AddNewProject(name);
        }

        public void SubWindowRenameModellingTreeNode(ModellingTreeNode.ModellingTreeNodeDialogData data)
        {
            RenameModellingTreeNode wind = new RenameModellingTreeNode(data);
            wind.Owner = this;
            wind.ShowDialog();
        }

        public void SubWindowImportMesh(ModellingTreeNode.ModellingTreeNodeDialogData data)
        {
            if (data is ModellingTreeNodeGrid.ModellingTreeNodeDialogDataImport)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Mesh file|*.msh";
                if (ofd.ShowDialog() == true)
                    (data as ModellingTreeNodeGrid.ModellingTreeNodeDialogDataImport).filename = ofd.FileName;
            }
        }

        private void MainMenuViewModellingTree(object sender, RoutedEventArgs e)
        {
            layoutModellingTree.IsVisible = (e.Source as MenuItem).IsChecked;
        }

        private void MainMenuViewToolsPanel(object sender, RoutedEventArgs e)
        {
            layoutTools.IsVisible = (e.Source as MenuItem).IsChecked;
        }

        private void layoutModellingTreeIsVisibleChanged(object sender, EventArgs e)
        {
            menubtnModellingTree.IsChecked = layoutModellingTree.IsVisible;
        }

        private void layoutToolsPanelIsVisibleChanged(object sender, EventArgs e)
        {
            menubtnToolsPanel.IsChecked = layoutTools.IsVisible;
        }
    }

    class TreeMapping
    {
        ModellingTreeController treeController;

        TreeView treeView;

        List<ProjectDocument> docs;

        public MainWindow wind { get; set; }

        public TreeMapping(ModellingTreeController treeController, TreeView treeView)
        {
            this.treeController = treeController;
            this.treeView = treeView;
            this.wind = null;
            this.docs = new List<ProjectDocument>();

            treeController.NodeAdded += AddNode;
            treeController.ActiveNodeChanged += ActiveNodeChanged;
            treeController.RenameNode += NodeRename;
            treeController.RemoveNode += RemoveNode;

            treeView.SelectedItemChanged  += SelectItem;
        }

        private void AddNode(object sender, EventArgs args)
        {
            if (args is EventArgsModellingTreeNode)
            {
                TreeViewItem parent = null;
                foreach (var name in (args as EventArgsModellingTreeNode).path)
                {
                    TreeViewItem item = null;
                    if (parent == null)
                    {
                        item = FindNodeByName(name, treeView.Items);
                    }
                    else
                    {
                        item = FindNodeByName(name, parent.Items);
                    }

                    if (item == null)
                    {
                        break;
                    }
                    parent = item;
                }

                TreeViewItem newItem = new TreeViewItem();
                newItem.Header = (args as EventArgsModellingTreeNode).nodeCurrent.Name;
                newItem.ContextMenu = ConstructContextMenu((args as EventArgsModellingTreeNode).nodeCurrent);

                if (wind != null)
                {
                    (args as EventArgsModellingTreeNode).nodeCurrent.renameDialog += wind.SubWindowRenameModellingTreeNode;

                    if ((args as EventArgsModellingTreeNode).nodeCurrent is ModellingTreeNodeGrid)
                    {
                        ((args as EventArgsModellingTreeNode).nodeCurrent as ModellingTreeNodeGrid).importDialog += wind.SubWindowImportMesh;
                    }

                    if ((args as EventArgsModellingTreeNode).nodeCurrent is ModellingTreeNodeProject)
                    {
                        ProjectDocument doc = new ProjectDocument(wind.layPane, 
                            ((args as EventArgsModellingTreeNode).nodeCurrent as ModellingTreeNodeProject));
                        docs.Add(doc);
                        treeController.ActiveNodeChanged += doc.ActiveNodeChanged;
                        treeController.NodeAdded += doc.AddNode;
                        treeController.RemoveNode += doc.RemoveNode;
                    }
                }

                if (parent == null)
                {
                    treeView.Items.Add(newItem);
                }
                else
                { 
                    parent.Items.Add(newItem);
                }
            }
        }

        private ContextMenu ConstructContextMenu(ModellingTreeNode node)
        {
            System.Windows.Controls.ContextMenu menu = new ContextMenu();

            Action<string, EventHandler> AddItem = (str, handler) =>
            {
                MenuItem item = new MenuItem();
                item.Header = str;
                item.Click += new RoutedEventHandler(handler);
                menu.Items.Add(item);
            };

            AddItem("Rename", node.ContextMenuRename);
            AddItem("Remove", node.ContextMenuRemove);

            if (node is ModellingTreeNodeProject)
            {
                AddItem("New Grid", (node as ModellingTreeNodeProject).ContextMenuNewGrid);
            }

            if (node is ModellingTreeNodeGrid)
            {
                AddItem("New Fissure", (node as ModellingTreeNodeGrid).ContextMenuAddNewFissure);
                AddItem("Import", (node as ModellingTreeNodeGrid).ContextMenuImport);
            }

            return menu;
        }

        private void ActiveNodeChanged(object sender, EventArgs args)
        {
            if (args is EventArgsModellingTreeNode)
            {
                TreeViewItem item = null;
                foreach (var name in (args as EventArgsModellingTreeNode).path)
                {
                    if (item == null)
                    {
                        item = FindNodeByName(name, treeView.Items);
                    }
                    else
                    {
                        item = FindNodeByName(name, item.Items);
                    }

                    if (item != null)
                    {
                        item.IsExpanded = true;
                    }
                }

                if (item != null)
                {
                    if (!item.IsSelected)
                        item.IsSelected = true;
                }

                if (wind != null)
                    wind.UpdatePanelTools((args as EventArgsModellingTreeNode).nodeCurrent.toolsStrategy);
            }
        }

        private void SelectItem(object sender, EventArgs args)
        {
            if (args is RoutedPropertyChangedEventArgs<object>)
            {
                if ((args as RoutedPropertyChangedEventArgs<object>).NewValue != null)
                {
                    string name = (string)(((args as RoutedPropertyChangedEventArgs<object>).NewValue) as TreeViewItem).Header;
                    ModellingTreeNode node = treeController.FindModellingTreeNode(name);
                    if ((node != null) & (treeController.ActiveNode != node))
                    {
                        treeController.ActiveNode = node;
                    }
                }
            }
        }

        private void NodeRename(object sender, EventArgs args)
        {
            if (args is EventArgsModellingTreeNode)
            {
                TreeViewItem item = null;
                foreach (var name in (args as EventArgsModellingTreeNode).path)
                {
                    if (item == null)
                    {
                        item = FindNodeByName(name, treeView.Items);
                    }
                    else
                    {
                        item = FindNodeByName(name, item.Items);
                    }
                }

                if (item != null)
                {
                    item.Header = (args as EventArgsModellingTreeNode).nodeCurrent.Name;
                    item.IsManipulationEnabled = true;
                }
            }
        }

        private void RemoveNode(object sender, EventArgs args)
        {
            if (args is EventArgsModellingTreeNode)
            {
                TreeViewItem item = null;
                foreach (var name in (args as EventArgsModellingTreeNode).path)
                {
                    if (item == null)
                    {
                        item = FindNodeByName(name, treeView.Items);
                    }
                    else
                    {
                        item = FindNodeByName(name, item.Items);
                    }
                }

                if (item != null)
                {
                    TreeViewItem parent = item.Parent as TreeViewItem;
                    if (parent != null)
                        parent.Items.Remove(item);
                    else
                        treeView.Items.Remove(item);
                }

                if (((args as EventArgsModellingTreeNode).nodeCurrent is ModellingTreeNodeProject) & (wind != null))
                {
                    Predicate<ProjectDocument> predicat = (d) => 
                        d.projectNode == (ModellingTreeNodeProject)(args as EventArgsModellingTreeNode).nodeCurrent;
                    ProjectDocument doc = docs.Find(predicat);
                    if (doc != null)
                        doc.Remove(wind.layPane);
                }
            }
        }

        private TreeViewItem FindNodeByName(string name, ItemCollection items)
        {
            foreach (var node in items)
            {
                if (node is TreeViewItem)
                {
                    if ((string)(node as TreeViewItem).Header == name)
                        return node as TreeViewItem;
                }
            }
            return null;
        }
    }

    class ProjectDocument
    {
        private Viewport3D viewPort;
        
        private LayoutContent layDoc;
        
        private Grid grid;

        
        public ModellingTreeNodeProject projectNode { get; set; }

        
        public ProjectDocument(LayoutDocumentPane layPane, ModellingTreeNodeProject projectNode)
        {
            layDoc = new LayoutDocument();
            layDoc.Title = projectNode.Name;
            layPane.Children.Add(layDoc);
            this.projectNode = projectNode;

            layDoc.CanClose = false;
            layDoc.CanFloat = false;
            layDoc.IsActiveChanged += ActiveDocChanged;

            viewPort = new Viewport3D();
            grid = new Grid();
            grid.Children.Add(viewPort);
            layDoc.Content = grid;

            SolidColorBrush b = new SolidColorBrush(Colors.DarkSeaGreen);
            grid.Background = b;

            OrthographicCamera camera = new OrthographicCamera();
            camera.Position = new Point3D(0.0f, 0.0f, 100f);
            camera.LookDirection = new Vector3D(0.0f, 0.0f, -1.0f);
            camera.UpDirection = new Vector3D(0.0f, 1.0f, 0.0f);
            camera.Width = 300;
            viewPort.Camera = camera;

            DirectionalLight light = new DirectionalLight(Colors.White, new Vector3D(-1.0f, -1.0f, -1.0f));
            ModelVisual3D modelvisual = new ModelVisual3D();
            modelvisual.Content = light;
            viewPort.Children.Add(modelvisual);

            projectNode.DrawableObjectCreated += OnDrawableObjectCreated;
            projectNode.DrawableObjectRemoved += OnDrawableObjectRemoved;

            //ModelVisual3D mv = new ModelVisual3D();
            //Model3DGroup mg = new Model3DGroup();
            //mv.Content = mg;
            //viewPort.Children.Add(mv);

            //TubeIncline tube = new TubeIncline();

            //tube.StartVector = new LinearMath.Vector(3);
            //tube.EndVector = new LinearMath.Vector(3);

            //tube.StartNormal = new LinearMath.Vector(3);
            //tube.EndNormal = new LinearMath.Vector(3);

            //tube.StartVector[0] = -100.0;
            //tube.StartVector[1] = 0.0;

            //tube.EndVector[0] = 100.0;
            //tube.EndVector[1] = 0.0;

            //LinearMath.Vector v = new LinearMath.Vector(3);
            //v[0] = 1.0;
            //v[1] = 1.0;
            //v.normalize();

            //tube.StartNormal[0] = 1.0;
            //tube.StartNormal[1] = 0.0;

            //tube.EndNormal[0] = v[0];
            //tube.EndNormal[1] = v[1];

            //tube.r = 20;
            //tube.color = Colors.Blue;
            //mg.Children.Add(tube.model);

            grid.MouseWheel += OnMouseWheel;
            grid.MouseDown += OnMouseDown;
            grid.MouseMove += OnMouseMove;
            grid.MouseUp += OnMouseUp;
        }

        
        private LinearMath.Vector ScreenToWorld(Point point)
        {
            double dx = (viewPort.Camera as OrthographicCamera).Width / viewPort.ActualWidth;

            LinearMath.Vector v = new LinearMath.Vector(3);
            v[0] = (point.X - viewPort.ActualWidth / 2.0) * dx + (viewPort.Camera as OrthographicCamera).Position.X;
            v[1] = (viewPort.ActualHeight / 2.0 - point.Y) * dx + (viewPort.Camera as OrthographicCamera).Position.Y;
            return v;
        }
        
        private void ActiveDocChanged(object sender, EventArgs arg)
        {
            if (layDoc.IsActive)
            {
                bool parent;
                bool child;
                projectNode.IsActiveBranch(out parent, out child);

                if (child)
                    return;

                projectNode.IsActive = true;
            }
        }

        public void ActiveNodeChanged(object sender, EventArgs args)
        {
            if (args is EventArgsModellingTreeNode)
            {
                if ((args as EventArgsModellingTreeNode).nodeCurrent.ProjectNode == projectNode)
                {
                    layDoc.IsActive = true;

                    
                    if ((args as EventArgsModellingTreeNode).nodeCurrent.contextMenuStrategy != null)
                        grid.ContextMenu = (args as EventArgsModellingTreeNode).nodeCurrent.contextMenuStrategy.menu;
                    else
                        grid.ContextMenu = null;
                }
            }
        }

        public void Remove(LayoutDocumentPane layPane)
        {
            layPane.Children.Remove(layDoc);
        }

        public void AddNode(object sender, EventArgs arg)
        {
            if (arg is EventArgsModellingTreeNode)
            {
                if ((arg as EventArgsModellingTreeNode).nodeCurrent.ProjectNode != projectNode)
                    return;

                if ((arg as EventArgsModellingTreeNode).nodeCurrent is ModellingTreeNodeFissure)
                {
                    ((arg as EventArgsModellingTreeNode).nodeCurrent as ModellingTreeNodeFissure).ScreenToWorld = ScreenToWorld;
                    grid.MouseDown += ((arg as EventArgsModellingTreeNode).nodeCurrent as ModellingTreeNodeFissure).OnMouseDown;
                }
            }
        }

        public void RemoveNode(object sender, EventArgs arg)
        {
            if (arg is EventArgsModellingTreeNode)
            {
                if ((arg as EventArgsModellingTreeNode).nodeCurrent.ProjectNode != projectNode)
                    return;
            }
        }

        
        private void OnDrawableObjectCreated(object sender, EventArgs arg)
        {
            if (arg is EventArgsDrawableObject)
            {
                viewPort.Children.Add((arg as EventArgsDrawableObject).CurrentObj.modelvisual);
                
                viewPort.MouseDown += (arg as EventArgsDrawableObject).CurrentObj.OnMouseDown;
                viewPort.MouseUp += (arg as EventArgsDrawableObject).CurrentObj.OnMouseUp;
                viewPort.MouseLeave += (arg as EventArgsDrawableObject).CurrentObj.OnMouseLeave;
                grid.MouseMove += (arg as EventArgsDrawableObject).CurrentObj.OnMouseMove;

                (arg as EventArgsDrawableObject).CurrentObj.ScreenToWorld = ScreenToWorld;
            }
        }

        private void OnDrawableObjectRemoved(object sender, EventArgs arg)
        {
            if (arg is EventArgsDrawableObject)
            {
                viewPort.Children.Remove((arg as EventArgsDrawableObject).CurrentObj.modelvisual);

                viewPort.MouseDown -= (arg as EventArgsDrawableObject).CurrentObj.OnMouseDown;
                viewPort.MouseUp -= (arg as EventArgsDrawableObject).CurrentObj.OnMouseUp;
                viewPort.MouseLeave -= (arg as EventArgsDrawableObject).CurrentObj.OnMouseLeave;
                grid.MouseMove -= (arg as EventArgsDrawableObject).CurrentObj.OnMouseMove;
            }
        }


        LinearMath.Vector vect = null;

        private void OnMouseDown(object sender, EventArgs arg)
        {
            if (arg is MouseButtonEventArgs)
            {
                if ((arg as MouseButtonEventArgs).MiddleButton == MouseButtonState.Pressed)
                {
                    Point p = (arg as MouseEventArgs).GetPosition(sender as Grid);
                    vect = ScreenToWorld(p);
                }
            }
        }

        private void OnMouseMove(object sender, EventArgs arg)
        {
            if (arg is MouseEventArgs)
            {
                if (vect != null)
                {
                    Point p = (arg as MouseEventArgs).GetPosition(sender as Grid);
                    LinearMath.Vector dif = ScreenToWorld(p) - vect;
                    
                    double X = (viewPort.Camera as OrthographicCamera).Position.X - dif[0];
                    double Y = (viewPort.Camera as OrthographicCamera).Position.Y - dif[1];
                    double Z = (viewPort.Camera as OrthographicCamera).Position.Z;

                    (viewPort.Camera as OrthographicCamera).Position = new Point3D(X, Y, Z);

                }
            }
        }

        private void OnMouseUp(object sender, EventArgs arg)
        {
            if (arg is MouseButtonEventArgs)
            {
                if ((arg as MouseButtonEventArgs).MiddleButton == MouseButtonState.Released)
                {
                    vect = null;
                }
            }
        }

        private void OnMouseWheel(object sender, EventArgs arg)
        {
            if (arg is MouseWheelEventArgs)
            {
                (viewPort.Camera as OrthographicCamera).Width -= (arg as MouseWheelEventArgs).Delta / 5.0;
                if ((viewPort.Camera as OrthographicCamera).Width < 1.0)
                    (viewPort.Camera as OrthographicCamera).Width = 1.0;
            }
        }
    }
}
