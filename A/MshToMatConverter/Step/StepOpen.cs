using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace MshToMatConverter
{
    class StepOpen : Step
    {
        string _filename;

        public StepOpen()
            : base()
        {
            _filename = string.Empty;
        }

        public void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Mesh files|*.msh";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _filename = ofd.FileName;
            }
        }

        public override string Name
        {
            get { return "Open file"; }
        }

        public override void Finish()
        {
            base.Finish();
            data.Import(_filename);
            ((Mesh2DContainer)data).CreateGeomManager(pref.ElementID);
        }
    }
}
