using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;


using MshToMatWPF.Drawing.Primitives;
using MshToMatWPF.Preferences;

namespace MshToMatWPF.Drawing
{
    public class Drawable
    {
        public event EventHandler Selected;


        private Model3DGroup modelgroup { get; set; }


        protected List<IPrimitive> primitiveList { get; private set; }


        public ModelVisual3D modelvisual { get; set; }

        public ColorManager colorManager { get; set; }

        public Func<System.Windows.Point, LinearMath.Vector> ScreenToWorld { get; set; }


        public Drawable()
        {
            modelgroup = new Model3DGroup();
            modelvisual = new ModelVisual3D();
            modelvisual.Content = modelgroup;
            primitiveList = new List<IPrimitive>();
        }

        ~Drawable()
        {
            foreach (var prim in primitiveList)
            {
                prim.Selected -= OnPrimitiveSelected;
            }
        }

        protected virtual void OnPrimitiveSelected(object sender, EventArgs arg)
        {
            if ((Selected != null) & (sender as IPrimitive).IsSelected)
                Selected(this, EventArgs.Empty);
        }

        protected void AddPrimitive(IPrimitive prim)
        {
            if (prim != null)
            {
                modelgroup.Children.Add(prim.model);
                primitiveList.Add(prim);

                if (colorManager != null)
                    prim.color = colorManager.grid;
                prim.Selected += OnPrimitiveSelected;
            }
        }

        protected void RemovePrimitive(IPrimitive prim)
        {
            if (prim != null)
            {
                modelgroup.Children.Remove(prim.model);
                primitiveList.Remove(prim);
                prim.Selected -= OnPrimitiveSelected;
            }
        }


        public bool IsSelected
        {
            get
            {
                foreach (var prim in primitiveList)
                {
                    if (prim.IsSelected)
                        return true;
                }
                return false;
            }
        }


        public void OnMouseDown(object sender, EventArgs arg)
        {
            if ((arg is MouseButtonEventArgs) & (sender is Viewport3D))
            {
                Point p = (arg as MouseButtonEventArgs).GetPosition(sender as Viewport3D);
                RayMeshGeometry3DHitTestResult ray = (RayMeshGeometry3DHitTestResult)VisualTreeHelper.HitTest(sender as Viewport3D, p);

                if (ray != null)
                {
                    foreach (var prim in primitiveList)
                    {
                        if (prim.model.Geometry == ray.MeshHit)
                        {
                            prim.IsSelected = (arg as MouseButtonEventArgs).ButtonState == MouseButtonState.Pressed;
                        }
                    }
                }
            }
        }

        public void OnMouseMove(object sender, EventArgs arg)
        {
            if ((arg is MouseEventArgs) & (sender is Grid))
            {
                Point p = (arg as MouseEventArgs).GetPosition(sender as Grid);
                foreach (var prim in primitiveList)
                {
                    if ((prim.IsSelected) & (prim is Sphere))
                    {
                        (prim as Sphere).center.CopyValue(ScreenToWorld(p));
                    }
                }
            }
        }

        public void OnMouseLeave(object sender, EventArgs arg)
        {
            if (arg is MouseEventArgs)
            {
                if ((arg as MouseEventArgs).LeftButton == MouseButtonState.Released)
                {
                    foreach (var prim in primitiveList)
                        prim.IsSelected = false;
                }
            }
        }

        public void OnMouseUp(object sender, EventArgs arg)
        {
            if (arg is MouseButtonEventArgs)
            {
                foreach (var prim in primitiveList)
                {
                    prim.IsSelected = (arg as MouseButtonEventArgs).ButtonState == MouseButtonState.Pressed;
                }
            }
        }
    }
}
