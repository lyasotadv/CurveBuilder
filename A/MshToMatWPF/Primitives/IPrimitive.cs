using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MshToMatWPF.Primitives
{
    public interface IPrimitive
    {
        GeometryModel3D model { get; }
    }
}
