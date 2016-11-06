using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;

namespace MshToMatWPF.ModellingTree.ContextMenuNodeStrategy
{
    public interface IContextMenuNodeStrategy
    {
        ContextMenu menu { get; }

        void Reload();
    }
}
