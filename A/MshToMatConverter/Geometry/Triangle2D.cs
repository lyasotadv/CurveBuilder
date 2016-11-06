using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatConverter.Geometry
{
    public class Triangle2D : Triangle
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

        public Vertex2D V3
        {
            get
            {
                return (Vertex2D)base.v3;
            }
        }

        public Edge2D E1
        {
            get
            {
                return (Edge2D)base.e1;
            }
            set
            {
                e1 = value;
            }
        }

        public Edge2D E2
        {
            get
            {
                return (Edge2D)base.e2;
            }
            set
            {
                e2 = value;
            }
        }

        public Edge2D E3
        {
            get
            {
                return (Edge2D)base.e3;
            }
            set
            {
                e3 = value;
            }
        }

        public Triangle2D(Vertex2D v1, Vertex2D v2, Vertex2D v3)
            : base(v1, v2, v3)
        {

        }

        public override object Clone()
        {
            Vertex2D v1 = (Vertex2D)V1.Clone();
            Vertex2D v2 = (Vertex2D)V2.Clone();
            Vertex2D v3 = (Vertex2D)V3.Clone();

            Triangle2D tr = new Triangle2D(v1, v2, v3);

            tr.extID = extID;

            tr.E1 = (Edge2D)E1.Clone();
            tr.E2 = (Edge2D)E2.Clone();
            tr.E3 = (Edge2D)E3.Clone();

            return tr;
        }
    }
}
