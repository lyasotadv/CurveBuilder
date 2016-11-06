using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatConverter.Geometry
{
    public class ElementCutParameters : ICloneable
    {
        public double T1 { get; private set; }

        public double T2 { get; private set; }

        public double T3 { get; private set; }

        public double Eps { get; private set; }

        Edge2D _Cut;

        public Edge2D Cut 
        {
            get
            {
                return _Cut;
            }
            set
            {
                _Cut = value;
                if (_Cut == null)
                {
                    T1 = -1.0;
                    T2 = -1.0;
                    T3 = -1.0;
                    Eps = -1.0;
                }
            }
        }

        public ElementCutParameters()
        {
            Cut = null;
        }

        public void CutLine(Triangle2D triangle, CurveLine curve, bool CalcAreaRelations)
        {
            double t1 = curve.LinearCut(triangle.E1);
            double t2 = curve.LinearCut(triangle.E2);
            double t3 = curve.LinearCut(triangle.E3);

            if (!double.IsNaN(t1))
                T1 = t1;
            if (!double.IsNaN(t2))
                T2 = t2;
            if (!double.IsNaN(t3))
                T3 = t3;

            if ((T1 > 0.0) & (T2 > 0.0))
            {
                Vertex2D v1 = triangle.E1.Verteces2LinCut(T1);
                Vertex2D v2 = triangle.E2.Verteces2LinCut(T2);
                Cut = new Edge2D(v1, v2);
            }

            if ((T2 > 0.0) & (T3 > 0.0))
            {
                Vertex2D v2 = triangle.E2.Verteces2LinCut(T2);
                Vertex2D v3 = triangle.E3.Verteces2LinCut(T3);
                Cut = new Edge2D(v2, v3);
            }
            if ((T3 > 0.0) & (T1 > 0.0))
            {
                Vertex2D v3 = triangle.E3.Verteces2LinCut(T3);
                Vertex2D v1 = triangle.E1.Verteces2LinCut(T1);
                Cut = new Edge2D(v3, v1);
            }

            if (T1 == 0.0)
            {
                Vertex2D v2 = triangle.E2.Verteces2LinCut(T2);
                Vertex2D v0 = (Vertex2D)triangle.V1.Clone();
                Cut = new Edge2D(v2, v0);
            }
            if (T2 == 0.0)
            {
                Vertex2D v3 = triangle.E3.Verteces2LinCut(T3);
                Vertex2D v0 = (Vertex2D)triangle.V2.Clone();
                Cut = new Edge2D(v3, v0);
            }
            if (T3 == 0.0)
            {
                Vertex2D v1 = triangle.E1.Verteces2LinCut(T1);
                Vertex2D v0 = (Vertex2D)triangle.V3.Clone();
                Cut = new Edge2D(v1, v0);
            }

            if (CalcAreaRelations)
                AreaRelationCalculate(triangle, curve);
        }

        private void AreaRelationCalculate(Triangle2D triangle, CurveLine curve)
        {
            if (!(curve is Circle))
                return;

            double centerVal = ((Circle)curve).ValueInCenter();
            double v1 = curve.Calc(triangle.V1.X, triangle.V1.Y);
            double v2 = curve.Calc(triangle.V2.X, triangle.V2.Y);
            double v3 = curve.Calc(triangle.V3.X, triangle.V3.Y);
            
            if ((T1 < 0.0) & (T2 < 0.0) & (T3 < 0.0))
            {
                if ((centerVal * v1 > 0.0) & (centerVal * v2 > 0.0) & (centerVal * v2 > 0.0))
                    Eps = 0.0;
                else
                    Eps = 1.0;
                return;
            }

            

            if (v1 == 0.0)
            {
                Eps = T2;
                if (v2 * centerVal > 0.0)
                    Eps = 1.0 - Eps;
                return;
            }

            if (v2 == 0.0)
            {
                Eps = T3;
                if (v3 * centerVal > 0.0)
                    Eps = 1.0 - Eps;
                return;
            }

            if (v3 == 0.0)
            {
                Eps = T1;
                if (v1 * centerVal > 0.0)
                    Eps = 1.0 - Eps;
                return;
            }

            if ((T1 > 0.0) & (T2 > 0.0))
            {
                Eps = (1.0 - T1) * T2;
                if (v2 * centerVal > 0.0)
                    Eps = 1.0 - Eps;
                return;
            }
            if ((T2 > 0.0) & (T3 > 0.0))
            {
                Eps = (1.0 - T2) * T3;
                if (v3 * centerVal > 0.0)
                    Eps = 1.0 - Eps;
                return;
            }
            if ((T3 > 0.0) & (T1 > 0.0))
            {
                Eps = (1.0 - T3) * T1;
                if (v1 * centerVal > 0.0)
                    Eps = 1.0 - Eps;
                return;
            }
        }

        public object Clone()
        {
            ElementCutParameters p = new ElementCutParameters();

            p.T1 = T1;
            p.T2 = T2;
            p.T3 = T3;
            p.Eps = Eps;

            if (Cut != null)
                p.Cut = (Edge2D)Cut.Clone();

            return p;
        }
    }
}
