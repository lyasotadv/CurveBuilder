using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Media3D;

using LinearMath;

namespace MshToMatWPF.Drawing.Primitives
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
                _StartNormal.Changed += OnVectorChanged;
                Update();
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
                _EndNormal.Changed += OnVectorChanged;
                Update();
            }
        }

        public TubeIncline()
            : base()
        {

        }

        protected override void Update()
        {
            if ((StartVector == null) | (EndVector == null))
                return;

            if ((StartNormal == null) | (EndNormal == null))
                return;
            
            Vector dir = EndVector - StartVector;
            if (dir.norm() < 1E-3)
                return;
            
            dir.normalize();

            if ((dir * StartNormal < 0.0) | (dir * EndNormal < 0.0))
                return;

            Point3D[] start = Circle(StartVector, StartNormal, dir);
            Point3D[] end = Circle(EndVector, EndNormal, dir);

            CreateMesh(start, end);
        }
    }
}
