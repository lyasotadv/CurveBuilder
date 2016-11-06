using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatConverter.Geometry
{
    class SecondOrderCurve : CurveLine
    {
        public SecondOrderCurve()
            : base(5)
        {
            phi[0] = (x, y) => { return x * x; };
            phi[1] = (x, y) => { return x * y; };
            phi[2] = (x, y) => { return y * y; };
            phi[3] = (x, y) => { return x; };
            phi[4] = (x, y) => { return y; };
            C = 1.0;
        }


    }
}
