using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MshToMatConverter.Geometry;

using System.IO;

using csmatio.io;
using csmatio.types;

namespace MshToMatConverter
{
    class Mesh2DContainer : IDataContainer
    {
        private enum DataPart { none, node, element}
        
        private struct NodeLine
        {
            private string str;

            private double[] data;

            public NodeLine(string str)
            {
                this.str = str;
                this.data = null;
                Parse();
            }

            private void Parse()
            {
                string[] strSplit = str.Split(' ');
                this.data = new double[strSplit.Length];
                for (int n = 0; n < strSplit.Length; n++)
                {
                    strSplit[n] = strSplit[n].Replace('.', ',');
                    data[n] = Convert.ToDouble(strSplit[n]);
                }
            }

            public override string ToString()
            {
                return str;
            }

            public double[] Coordinate
            {
                get
                {
                    if (data.Length == 4)
                    {
                        double[] p = new double[2];
                        p[0] = data[1];
                        p[1] = data[2];
                        return p;
                    }
                    return null;
                }
            }

            public object Clone()
            {
                return new NodeLine(str);
            }
        }

        private struct ElementLine
        {
            private string str;

            private int[] data;

            public ElementLine(string str)
            {
                this.str = str;
                this.data = null;
                Parse();
            }

            private void Parse()
            {
                string[] strSplit = str.Split(' ');
                this.data = new int[strSplit.Length];
                for (int n = 0; n < strSplit.Length; n++)
                    data[n] = Convert.ToInt32(strSplit[n]);
            }

            public override string ToString()
            {
                return str;
            }

            public int[] Triangle
            {
                get
                {
                    if (data.Length != 8)
                        return null;

                    if ((data.Length == 8) & (data[1] == 2))
                    {
                        int[] id = new int[3];

                        id[0] = data[5];
                        id[1] = data[6];
                        id[2] = data[7];

                        return id;
                    }
                    return null;
                }
            }

            public int[] Vertex
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public int[] Edge
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public object Clone()
            {
                return new ElementLine(str);
            }
        }

        List<NodeLine> nodeline;

        List<ElementLine> elementline;

        GeometryManager _geomManager;

        public GeometryManager geomManager
        {
            get
            {
                return _geomManager;
            }
        }

        private Dictionary<Triangle2D, ElementCutParameters> cutParam
        {
            get
            {
                return geomManager.cutParam;
            }
        }

        private Dictionary<Vertex2D, WeightFunc2D> weightfunc
        {
            get
            {
                return geomManager.weightfunc;
            }
        }

        double[,] point;

        int[,] element;

        public Preferences pref { get; set; }

        public Mesh2DContainer()
        {
            nodeline = new List<NodeLine>();
            elementline = new List<ElementLine>();
        }

        public void CutLine(CurveLine curve)
        {
            foreach (var tr in geomManager.triangle)
            {
                ElementCutParameters p = new ElementCutParameters();
                p.CutLine(tr, curve, pref.CalcAreaRelation);
                cutParam.Add(tr, p);
            }

            if ((pref.WeightFunctionCalculate) & (curve is Circle))
            {
                foreach (var v in geomManager.vertex)
                {
                    weightfunc.Add(v, ((Circle)curve).CalcWeightFunc(v.X, v.Y));
                }
            }
        }

        public PolyLineGroup2D GroupCutLine()
        {
            PolyLineGroup2D polyline = new PolyLineGroup2D();
            foreach (var p in cutParam.Values)
            {
                if (p.Cut != null)
                    polyline.Add(p.Cut);
            }
            return polyline;
        }

        public void FilterCut(Edge2D[] filt)
        {
            foreach (var p in cutParam.Values)
            {
                if (p.Cut == null)
                    continue;
                if (!filt.Contains(p.Cut))
                    p.Cut = null;
            }
        }

        public void Copy(IDataContainer data)
        {
            Mesh2DContainer mshdata = (Mesh2DContainer)data;

            if ((mshdata.point != null) & (mshdata.element != null))
            {
                point = (double[,])mshdata.point.Clone();
                element = (int[,])mshdata.element.Clone();

                _geomManager = (GeometryManager)mshdata.geomManager.Clone();
            }

            nodeline = new List<NodeLine>();
            foreach (var line in mshdata.nodeline)
                nodeline.Add((NodeLine)line.Clone());

            elementline = new List<ElementLine>();
            foreach (var line in mshdata.elementline)
                elementline.Add((ElementLine)line.Clone());
        }

        public void Import(string filename)
        {
            DataPart part = DataPart.none;
            
            foreach (var str in File.ReadLines(filename))
            {
                switch (str)
                {
                    case "$Nodes":
                        {
                            part = DataPart.node;
                            break;
                        }
                    case "$EndNodes":
                        {
                            part = DataPart.none;
                            break;
                        }
                    case "$Elements":
                        {
                            part = DataPart.element;
                            break;
                        }
                    case "$EndElements":
                        {
                            part = DataPart.none;
                            break;
                        }
                    default:
                        {
                            switch (part)
                            {
                                case DataPart.element:
                                    {
                                        elementline.Add(new ElementLine(str));
                                        break;
                                    }
                                case DataPart.node:
                                    {
                                        nodeline.Add(new NodeLine(str));
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
        }

        public void Export(string filename)
        {
            double[][] p = new double[point.GetLength(0)][];
            for (int n = 0; n < point.GetLength(0); n++)
            {
                p[n] = new double[2];
                for (int m = 0; m < 2; m++)
                    p[n][m] = point[n, m];
            }

            int[][] e = new int[element.GetLength(0)][];
            for (int n = 0; n < element.GetLength(0); n++)
            {
                e[n] = new int[3];
                for (int m = 0; m < 3; m++)
                    e[n][m] = element[n, m];
            }


            int N = 0;
            foreach (var paramet in cutParam.Values)
            {
                if (paramet.Cut != null)
                    N++;
            }

            int k = 0;
            int k0 = 0;
            double[][] c = new double[N][];
            double[][] eps = new double[element.GetLength(0)][];
            foreach (var item in cutParam)
            {
                if (item.Value.Cut != null)
                {
                    c[k] = new double[4];
                    c[k][0] = item.Key.extID;
                    c[k][1] = item.Value.T1;
                    c[k][2] = item.Value.T2;
                    c[k][3] = item.Value.T3;
                    k++;
                }

                eps[k0] = new double[2];
                eps[k0][0] = item.Key.extID;
                eps[k0][1] = item.Value.Eps;

                k0++;
            }

            double[][] q = new double[point.GetLength(0)][];
            if (pref.WeightFunctionCalculate)
            {
                k = 0;
                foreach (var item in weightfunc)
                {
                    q[k] = new double[4];

                    q[k][0] = item.Key.extID;
                    q[k][1] = item.Value.q;
                    q[k][2] = item.Value.qx;
                    q[k][3] = item.Value.qy;

                    k++;
                }
            }

            MLDouble mlPoint = new MLDouble(pref.VertexMatrixName, p);
            MLInt32 mlElement = new MLInt32(pref.ElementMatrixName, e);
            MLDouble mlCut = new MLDouble(pref.CutTriangleMatrixName, c);

            List<MLArray> mlList = new List<MLArray>();
            mlList.Add(mlPoint);
            mlList.Add(mlElement);
            mlList.Add(mlCut);

            if (pref.CalcAreaRelation)
            {
                MLDouble mlEps = new MLDouble(pref.AreaRelationMatrixName, eps);
                mlList.Add(mlEps);
            }

            if (pref.WeightFunctionCalculate)
            {
                MLDouble mlq = new MLDouble(pref.WeightFunctionMatrixName, q);
                mlList.Add(mlq);
            }

            MatFileWriter mfr = new MatFileWriter(filename, mlList, false);
        }

        private T[,] ListToArray<T>(List<T[]> lst)
        {
            int N = lst.Count;
            int M = lst[0].Length;

            T[,] a = new T[N, M];
            for (int n = 0; n < N; n++)
                for (int m = 0; m < M; m++)
                {
                    a[n, m] = lst[n][m];
                }

            return a;
        }

        public void CreateGeomManager(int elementID)
        {
            List<double[]> n = new List<double[]>();
            foreach (var line in nodeline)
            {
                double[] x = line.Coordinate;
                if (x == null)
                    continue;
                n.Add(x);
            }

            List<int[]> e = new List<int[]>();
            foreach (var line in elementline)
            {
                int[] x = null;
                switch (elementID)
                {
                    case 2:
                    {
                        x = line.Triangle;
                        break;
                    }
                    case 1:
                    {
                        x = line.Edge;
                        break;
                    }
                    case 15:
                    {
                        x = line.Vertex;
                        break;
                    }
                }

                if (x == null)
                    continue;
                e.Add(x);
            }

            point = ListToArray<double>(n);
            element = ListToArray<int>(e);

            _geomManager = new GeometryManager(point, element);
        }
    }
}
