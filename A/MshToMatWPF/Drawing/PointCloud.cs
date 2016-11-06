using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;

using MshToMatWPF.Geometry;
using MshToMatWPF.Drawing.Primitives;
using LinearMath;
using MshToMatWPF.Preferences;
using MshToMatWPF.Geometry.Point;

namespace MshToMatWPF.Drawing
{
    abstract class PointCloud<TPoint> : Drawable
        where TPoint : LinePoint
    {
        private double _r;

        public double r
        {
            get
            {
                return _r;
            }
            set
            {
                _r = value;
                foreach (var p in primitiveList)
                {
                    if (p is Sphere)
                        (p as Sphere).r = r;
                }
            }
        }

        protected List<TPoint> linePoint { get; private set; }


        public int Count
        {
            get
            {
                return linePoint.Count;
            }
        }


        public PointCloud()
        {
            linePoint = new List<TPoint>();
        }


        public void AddPoint(TPoint lcp, LinePointAdditionMethod method)
        {
            if (lcp != null)
            {
                AddPointComponent comp = new AddPointComponent();
                comp.lcp = lcp;
                comp.collection = new LinePointCollection<TPoint>(linePoint);
                switch (method)
                {
                    case LinePointAdditionMethod.last:
                        {
                            comp.strategy = new AddPointStrategyLast();
                            break;
                        }
                    case LinePointAdditionMethod.first:
                        {
                            comp.strategy = new AddPointStrategyFirst();
                            break;
                        }
                }
                comp.Build();

                linePoint.Add(lcp);

                Sphere sphere = new Sphere();
                sphere.r = r;
                sphere.color = colorManager.spline;
                sphere.center = lcp.vert.q;
                AddPrimitive(sphere);
            }
        }

        public void RemovePoint(TPoint lcp)
        {
            if (lcp != null)
            {
                linePoint.Remove(lcp);

                TPoint lcpPrev = lcp.prev as TPoint;
                TPoint lcpNext = lcp.next as TPoint;

                if (lcpPrev != null & lcpNext != null)
                {
                    lcpPrev.next = lcpNext;
                }

                linePoint.Remove(lcp);
                lcp.Unsubcribe();

                IPrimitive prim = primitiveList.Find((p) =>
                    {
                        return (p is Sphere) && ((p as Sphere).center == lcp.vert.q);
                    });

                if (prim != null)
                {
                    RemovePrimitive(prim);
                }
            }
        }
    }
}
