using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LinearMath;

namespace MshToMatWPF.Geometry
{
    abstract public class Vertex : ICloneable
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

        int _dim;

        Vector _q;

        public Vector q
        {
            get
            {
                return _q;
            }
        }

        protected Vertex(int dim)
        {
            _ID = Guid.NewGuid();
            _extID = -1;

            this._dim = dim;
            _q = new Vector(_dim);
        }

        abstract public object Clone();

        public override string ToString()
        {
            return q.ToString();
        }
    }
}
