using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;

using MshToMatConverter.Geometry;

namespace MshToMatConverter
{
    class StepController
    {
        Step[] step;
        
        int activeStepID;

        int StepCnt;

        Preferences _pref;

        IMainForm _form;


        public StepController(IMainForm form)
        {

            this._form = form;
            this._pref = new Preferences();

            StepCnt = 3;
            step = new Step[StepCnt];

            step[0] = new StepOpen();
            step[1] = new StepProcessing();
            step[2] = new StepSave();

            foreach (var s in step)
            {
                s.pref = _pref;
                s.form = _form;
            }

            activeStepID = -1;
        }

        public void Start()
        {
            activeStepID = -1;
            NextStep();
        }

        private void NextStep()
        {
            if (activeStepID == StepCnt - 1)
                return;
            
            if (activeStepID > -1)
            {
                step[activeStepID].Finish();
            }
            
            activeStepID++;
            
            if (activeStepID < StepCnt)
            {
                if (activeStepID > 0)
                    step[activeStepID].data.Copy(step[activeStepID - 1].data);
                step[activeStepID].Start();
            }
        }

        private void GoToStep(string name)
        {
            for (int n = 0; n < StepCnt; n++)
            {
                if (step[n].Name == name)
                {
                    activeStepID = n;
                    if (activeStepID > 0)
                        step[activeStepID].data.Copy(step[activeStepID - 1].data);
                    step[activeStepID].Start();
                    break;
                }
            }
        }

        public void Open()
        {
            Start();
            if (step[activeStepID] is StepOpen)
            {
                ((StepOpen)step[activeStepID]).OpenFile();
                NextStep();
            }
        }

        public void Save()
        {
            if (step[activeStepID] is StepProcessing)
            {
                NextStep();
            }
            if (step[activeStepID] is StepSave)
            {
                ((StepSave)step[activeStepID]).SaveFile();
            }
        }

        public void DrawLine()
        {
            if (step[activeStepID] is StepSave)
            {
                GoToStep("Processing");
            }
            if (step[activeStepID] is StepProcessing)
            {
                NextStep();
            }
        }

        public void PrefForm()
        {
            _pref.Show(_form);
        }
    }

    public interface IMainForm
    {
        GeometryManager geomManager { get; set; }

        PointF[] mouseline { get; }

        void PlotCutLine();
    }
}
