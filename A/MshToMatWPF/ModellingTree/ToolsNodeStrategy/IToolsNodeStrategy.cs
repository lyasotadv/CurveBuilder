using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;

namespace MshToMatWPF.ModellingTree.ToolsNodeStrategy
{
    public interface IToolsNodeStrategy
    {
        List<Control> controlList { get; }
    }
}
