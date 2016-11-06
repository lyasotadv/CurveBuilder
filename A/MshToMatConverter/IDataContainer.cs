using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MshToMatConverter
{
    interface IDataContainer
    {
        void Copy(IDataContainer data);

        void Import(string filename);

        void Export(string filename);

        Preferences pref { get; set; }
    }
}
