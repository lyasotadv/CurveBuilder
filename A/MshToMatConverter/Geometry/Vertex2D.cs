using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatConverter.Geometry
{
    public class Vertex2D : Vertex
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

        public Vertex2D() : base(2)
        {

        }

        public override object Clone()
        {
            Vertex2D v = new Vertex2D();

            v.X = X;
            v.Y = Y;

            v.extID = extID;

            return v;
        }
    }
}
