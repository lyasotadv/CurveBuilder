using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LinearMath;

namespace MshToMatWPF.Primitives
{
    class TubeIncline : Tube
    {
        private Vector _StartNormal;

        private Vector _EndNormal;

        public Vector StartNormal
        {
            get
            {
                return _StartNormal;
            }
            set
            {
                _StartNormal = value;
            }
        }

        public Vector EndNormal
        {
            get
            {
                return _EndNormal;
            }
            set
            {
                _EndNormal = value;
            }
        }

        public TubeIncline()
            : base()
        {

        }
    }
}
