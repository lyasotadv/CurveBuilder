using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace MshToMatConverter
{
    abstract class Step
    {
        IDataContainer _data;

        public IDataContainer data
        {
            get
            {
                return _data;
            }
        }

        public IMainForm form { get; set; }

        private enum StepState { NotReady, Active, Ready}

        StepState state;

        public abstract string Name { get; }

        private Preferences _pref;

        public Preferences pref 
        { 
            get
            {
                return _pref;
            }
            set
            {
                _pref = value;
                _data.pref = _pref;
            }
        }

        public Step()
        {
            state = StepState.NotReady;
            _data = new Mesh2DContainer();
        }

        public virtual void Finish()
        {
            state = StepState.Ready;
        }

        public virtual void Start()
        {
            state = StepState.Active;
        }
    }
}
