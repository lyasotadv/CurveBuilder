using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace MshToMatConverter
{
    class StepSave : Step
    {
        string _filename;

        public StepSave()
            : base()
        {
            this._filename = string.Empty;
        }

        public override string Name
        {
            get { return "Save matrix"; }
        }

        public void SaveFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "MATLAB datafile|*.mat";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                _filename = sfd.FileName;
                data.Export(_filename);
            }
        }

        public override void Finish()
        {
            base.Finish();
            data.Export(_filename);
        }
    }
}
