using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MshToMatWPF.Drawing.Primitives
{
    public interface IPrimitive
    {
        GeometryModel3D model { get; }

        Color color { get; set; }

        bool IsSelected { get; set; }

        event EventHandler Selected;
    }
}
