using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;

namespace MshToMatWPF.Preferences
{
    public class ColorManager
    {
        public Color grid { get; set; }

        public Color gridSelected { get; set; }

        public Color spline { get; set; }

        public Color splineSelected { get; set; }

        public Color directionHandler { get; set; }

        public Color direction { get; set; }

        public ColorManager()
        {
            grid = Colors.Green;
            gridSelected = Colors.Red;

            spline = Colors.Blue;
            splineSelected = Colors.Aqua;

            directionHandler = Colors.Red;
            direction = Colors.Gray;
        }
    }
}
