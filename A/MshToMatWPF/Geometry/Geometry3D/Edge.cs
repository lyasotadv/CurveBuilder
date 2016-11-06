using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatWPF.Geometry
{
    abstract public class Edge : ICloneable
    {
        Guid _ID;

        public Guid ID
        {
            get
            {
                return _ID;
            }
        }

        int _extID;

        public int extID
        {
            get
            {
                return _extID;
            }
            set
            {
                _extID = value;
            }
        }

        Vertex _v1;

        Vertex _v2;

        protected Vertex v1
        {
            get
            {
                return _v1;
            }
        }

        protected Vertex v2
        {
            get
            {
                return _v2;
            }
        }

        public Edge(Vertex v1, Vertex v2)
        {
            _ID = Guid.NewGuid();

            this._v1 = v1;
            this._v2 = v2;
        }

        abstract public object Clone();
    }
}
