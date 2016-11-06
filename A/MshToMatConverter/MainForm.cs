using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MshToMatConverter.Geometry;

using System.Threading;

namespace MshToMatConverter
{
    public partial class MainForm : Form, IMainForm
    {
        StepController stepController;

        Thread plotThread;

        Graphics graph;

        private GeometryManager _geomManager;

        public GeometryManager geomManager 
        {
            get
            {
                return _geomManager;
            }
            set
            {
                _geomManager = value;
                ReDraw();
            }
        } 

        public MainForm()
        {
            InitializeComponent();

            stepController = new StepController(this);

            stepController.Start();

            _mousestart = Point.Empty;
            _mouseline = new List<Point>();

            pctMain.Image = new Bitmap(pctMain.Width, pctMain.Height);
            graph = Graphics.FromImage(pctMain.Image);
        }

        Func<double, double, Point> xy2pnt;

        Func<Point, PointF> pnt2xy;

        private void PlotMesh()
        {
            int field = 10;

            int W = pctMain.Size.Width;
            int H = pctMain.Size.Height;

            double Xmin;
            double Xmax;
            double Ymin;
            double Ymax;

            geomManager.RectangleBox(out Xmin, out Xmax, out Ymin, out Ymax);

            double pxX = (W - 2 * field) / (Xmax - Xmin);
            double pxY = (H - 2 * field) / (Ymax - Ymin);

            Func<double, int> X2Px = (x) => { return (int)((x - Xmin) * pxX) + field; };
            Func<double, int> Y2Px = (y) => { return H - (int)((y - Ymin) * pxY) - field; };
            xy2pnt = (x, y) => { return new Point(X2Px(x), Y2Px(y)); };

            Func<int, float> Px2X = (X) => { return (float)((X - field)/pxX + Xmin); };
            Func<int, float> Px2Y = (Y) => { return (float)((H - Y - field)/pxY + Ymin); };
            pnt2xy = (p) => { return new PointF(Px2X(p.X), Px2Y(p.Y)); };

            Pen pen = new Pen(Color.Black, 1);
            graph.Clear(Color.White);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, pctMain.Width, pctMain.Height);
            foreach (var edge in geomManager.edge)
            {
                Point p1 = xy2pnt(edge.V1.X, edge.V1.Y);
                Point p2 = xy2pnt(edge.V2.X, edge.V2.Y);

                graph.DrawLine(pen, p1, p2);
            }

            pctMain.Invalidate();
        }

        public void PlotCutLine()
        {
            Pen pen = new Pen(Color.Red, 3);
            foreach (var p in geomManager.cutParam.Values)
            {
                if (p.Cut != null)
                {
                    Point p1 = xy2pnt(p.Cut.V1.X, p.Cut.V1.Y);
                    Point p2 = xy2pnt(p.Cut.V2.X, p.Cut.V2.Y);

                    graph.DrawLine(pen, p1, p2);
                }
            }

            pctMain.Invalidate();
        }

        private void ReDraw()
        {
            if (plotThread != null)
            {
                if (plotThread.ThreadState == ThreadState.Background)
                    plotThread.Abort();
            }
            plotThread = new Thread(PlotMesh);
            plotThread.IsBackground = true;
            plotThread.Start();
        }

        private void mshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stepController.Open();
        }

        private void matToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stepController.Save();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stepController.PrefForm();
        }

        Point _mousestart;
        List<Point> _mouseline;

        public PointF[] mouseline
        {
            get
            {
                if (_mouseline == null)
                    return null;
                int N = _mouseline.Count;
                PointF[] p = new PointF[N];
                for (int n = 0; n < N; n++)
                    p[n] = pnt2xy(_mouseline[n]);
                return p;
            }
        }

        private void pctMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _mousestart = new Point(e.X, e.Y);
                _mouseline = new List<Point>();
                _mouseline.Add(new Point(e.X, e.Y));
            }
        }

        private void pctMain_MouseMove(object sender, MouseEventArgs e)
        {
            if ((_mousestart != Point.Empty)&(e.Button == System.Windows.Forms.MouseButtons.Left))
            {
                Point p = new Point(e.X, e.Y);
                Pen pen = new Pen(Color.Green, 3);
                graph.DrawLine(pen, _mousestart, p);
                _mouseline.Add(p);
                _mousestart = p;
                pctMain.Invalidate();
            }
        }

        private void pctMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                stepController.DrawLine();
                _mousestart = Point.Empty;
                _mouseline = null;
            }
        }

    }
}
