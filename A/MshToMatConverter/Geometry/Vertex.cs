using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatConverter.Geometry
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

        double[] _q;

        protected double[] q
        {
            get
            {
                return _q;
            }
        }

        public Vertex(int dim)
        {
            _ID = Guid.NewGuid();

            this._dim = dim;
            _q = new double[_dim];

            for (int n = 0; n < _dim; n++)
                _q[n] = 0.0;
        }

        abstract public object Clone();
    }
}
