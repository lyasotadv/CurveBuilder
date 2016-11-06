using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatWPF.Geometry
{
    class Triangle3D : Triangle
    {
        public Vertex3D V1
        {
            get
            {
                return (Vertex3D)base.v1;
            }
        }

        public Vertex3D V2
        {
            get
            {
                return (Vertex3D)base.v2;
            }
        }

        public Vertex3D V3
        {
            get
            {
                return (Vertex3D)base.v3;
            }
        }

        public Edge3D E1
        {
            get
            {
                return (Edge3D)base.e1;
            }
            set
            {
                e1 = value;
            }
        }

        public Edge3D E2
        {
            get
            {
                return (Edge3D)base.e2;
            }
            set
            {
                e2 = value;
            }
        }

        public Edge3D E3
        {
            get
            {
                return (Edge3D)base.e3;
            }
            set
            {
                e3 = value;
            }
        }

        public Triangle3D(Vertex3D v1, Vertex3D v2, Vertex3D v3)
            : base(v1, v2, v3)
        {

        }

        public override object Clone()
        {
            Vertex3D v1 = (Vertex3D)V1.Clone();
            Vertex3D v2 = (Vertex3D)V2.Clone();
            Vertex3D v3 = (Vertex3D)V3.Clone();

            Triangle3D tr = new Triangle3D(v1, v2, v3);

            tr.extID = extID;

            tr.E1 = (Edge3D)E1.Clone();
            tr.E2 = (Edge3D)E2.Clone();
            tr.E3 = (Edge3D)E3.Clone();

            return tr;
        }
    }
}
