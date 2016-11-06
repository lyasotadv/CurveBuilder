using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatWPF.Geometry
{
    class Vertex3D : Vertex
    {
        public double X
        {
            get
            {
                return q[0];
            }
            set
            {
                q[0] = value;
            }
        }

        public double Y
        {
            get
            {
                return q[1];
            }
            set
            {
                q[1] = value;
            }
        }

        public double Z
        {
            get
            {
                return q[2];
            }
            set
            {
                q[2] = value;
            }
        }

        public Vertex3D() : base(3)
        {

        }

        public override object Clone()
        {
            Vertex3D v = new Vertex3D();

            v.X = X;
            v.Y = Y;
            v.Z = Z;

            v.extID = extID;

            return v;
        }
    }
}
