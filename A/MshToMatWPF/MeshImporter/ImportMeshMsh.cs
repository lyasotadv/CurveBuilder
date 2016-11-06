using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using MshToMatWPF.Drawing;

namespace MshToMatWPF.MeshImporter
{
    class ImportMeshMsh : ImportMesh
    {
        private enum DataPart { none, node, element }

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

        public ImportMeshMsh(string filename)
            : base(filename)
        {
            nodeline = new List<NodeLine>();
            elementline = new List<ElementLine>();
        }

        private void FillLine()
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

        private void CreateGeomManager()
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
                int[] x = line.Triangle;

                if (x == null)
                    continue;

                e.Add(x);
            }

            double[,] point = ListToArray<double>(n);
            int[,] element = ListToArray<int>(e);

            geomManager = new MeshContainer(point, element);
        }

        public override void Build()
        {
            FillLine();
            CreateGeomManager();
        }
    }
}
