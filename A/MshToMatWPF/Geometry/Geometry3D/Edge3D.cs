using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatWPF.Geometry
{
    class Edge3D : Edge
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

        public Vertex3D Center
        {
            get
            {
                Vertex3D v = new Vertex3D();
                v.X = (V1.X + V2.X) / 2.0;
                v.Y = (V1.Y + V2.Y) / 2.0;
                v.Z = (V1.Z + V2.Z) / 2.0;
                return v;
            }
        }

        public Edge3D(Vertex3D v1, Vertex3D v2) : base(v1, v2)
        {

        }

        public override object Clone()
        {
            Vertex3D v1 = (Vertex3D)V1.Clone();
            Vertex3D v2 = (Vertex3D)V2.Clone();

            Edge3D e = new Edge3D(v1, v2);

            e.extID = extID;

            return e;
        }
    }
}
