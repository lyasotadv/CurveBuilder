using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatWPF.Geometry
{
    abstract public class Triangle : Facet
    {
        protected Vertex v1
        {
            get
            {
                return v[0];
            }
            private set
            {
                v[0] = value;
            }
        }

        protected Vertex v2
        {
            get
            {
                return v[1];
            }
            private set
            {
                v[1] = value;
            }
        }

        protected Vertex v3
        {
            get
            {
                return v[2];
            }
            private set
            {
                v[2] = value;
            }
        }

        protected Edge e1
        {
            get
            {
                return e[0];
            }
            set
            {
                e[0] = value;
            }
        }

        protected Edge e2
        {
            get
            {
                return e[1];
            }
            set
            {
                e[1] = value;
            }
        }

        protected Edge e3
        {
            get
            {
                return e[2];
            }
            set
            {
                e[2] = value;
            }
        }

        public Triangle(Vertex v1, Vertex v2, Vertex v3)
            : base(3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }
}
