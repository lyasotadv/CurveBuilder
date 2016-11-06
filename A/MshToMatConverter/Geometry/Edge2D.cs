using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatConverter.Geometry
{
    public class Edge2D : Edge
    {
        public Vertex2D V1
        {
            get
            {
                return (Vertex2D)base.v1;
            }
        }

        public Vertex2D V2
        {
            get
            {
                return (Vertex2D)base.v2;
            }
        }

        public Edge2D(Vertex2D v1, Vertex2D v2) : base(v1, v2)
        {

        }

        public Vertex2D Verteces2LinCut(double t)
        {
            double x = V1.X + t * (V2.X - V1.X);
            double y = V1.Y + t * (V2.Y - V1.Y);

            Vertex2D v = new Vertex2D();
            v.X = x;
            v.Y = y;

            return v;
        }

        public override object Clone()
        {
            Vertex2D v1 = (Vertex2D)V1.Clone();
            Vertex2D v2 = (Vertex2D)V2.Clone();

            Edge2D e = new Edge2D(v1, v2);

            e.extID = extID;

            return e;
        }

        public double Length()
        {
            double dx = V2.X - V1.X;
            double dy = V2.Y - V1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public double Distance(Vertex2D vert)
        {
            double l = this.Length();

            double dx0 = V2.X - V1.X;
            double dy0 = V2.Y - V1.Y;

            double dx = vert.X - V1.X;
            double dy = vert.Y - V1.Y;

            double p = dx * dx0 + dy * dy0;
            p /= l;

            if (p < 0.0)
                return Math.Sqrt(dx * dx + dy * dy);

            if (p > l)
            {
                double dx1 = vert.X - V2.X;
                double dy1 = vert.Y - V2.Y;
                return Math.Sqrt(dx1 * dx1 + dy1 * dy1);
            }

            double L = Math.Sqrt(dx * dx + dy * dy);
            return Math.Sqrt(L * L - p * p);
        }
    }
}
