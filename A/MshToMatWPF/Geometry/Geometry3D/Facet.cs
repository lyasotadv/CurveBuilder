using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatWPF.Geometry
{
    abstract public class Facet : ICloneable
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

        int _cnt;

        Vertex[] _v;

        Edge[] _e;

        protected Vertex[] v
        {
            get
            {
                return _v;
            }
        }

        protected Edge[] e
        {
            get
            {
                return _e;
            }
        }

        public Facet(int cnt)
        {
            _ID = Guid.NewGuid();

            this._cnt = cnt;

            _v = new Vertex[_cnt];
            _e = new Edge[_cnt];
        }

        abstract public object Clone();
    }
}
