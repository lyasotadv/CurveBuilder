using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Windows.Interop;

namespace MshToMatConverter
{
    public class Preferences
    {
        string _VertexMatrixName = "vert";

        public string VertexMatrixName
        {
            get
            {
                return _VertexMatrixName;
            }
            set
            {
                _VertexMatrixName = value;
            }
        }

        string _ElementMatrixName = "el";

        public string ElementMatrixName
        {
            get
            {
                return _ElementMatrixName;
            }
            set
            {
                _ElementMatrixName = value;
            }
        }

        string _CutTriangleMatrixName = "cut";

        public string CutTriangleMatrixName
        {
            get
            {
                return _CutTriangleMatrixName;
            }
            set
            {
                _CutTriangleMatrixName = value;
            }
        }

        int _ElementID = 2;

        public int ElementID
        { 
            get
            {
                return _ElementID;
            }
            set
            {
                _ElementID = value;
            }
        }

        public enum CurveType { SecondOrderCurve, Circle}

        CurveType _Curve = CurveType.Circle;

        public CurveType Curve
        {
            get
            {
                return _Curve;
            }
            set
            {
                _Curve = value;
            }
        }

        string _AreaRelationMatrixName = "areaRel";

        public string AreaRelationMatrixName
        {
            get
            {
                return _AreaRelationMatrixName;
            }
            set
            {
                _AreaRelationMatrixName = value;
            }
        }

        bool _CalcAreaRelation = false;

        public bool CalcAreaRelation
        {
            get
            {
                if (Curve != CurveType.Circle)
                    return false;
                return _CalcAreaRelation;
            }
            set
            {
                _CalcAreaRelation = value;
            }
        }

        string _WeightFunctionMatrixName = "qfunc";

        public string WeightFunctionMatrixName
        {
            get
            {
                return _WeightFunctionMatrixName;
            }
            set
            {
                _WeightFunctionMatrixName = value;
            }
        }

        bool _WeightFunctionCalculate = false;

        public bool WeightFunctionCalculate
        {
            get
            {
                if (Curve != CurveType.Circle)
                    return false;
                return _WeightFunctionCalculate;
            }
            set
            {
                _WeightFunctionCalculate = value;
            }
        }

        double _RadiusesDifference = 10.0;

        public double RadiusesDifference
        {
            get
            {
                return _RadiusesDifference;
            }
            set
            {
                _RadiusesDifference = value;
            }
        }

        public Preferences()
        {
            Import();
        }

        public void Show(IMainForm mainform)
        {
            PreferencesForm form = new PreferencesForm(this);

            if (mainform is Form)
            {
                if (form.ShowDialog((Form)mainform) == System.Windows.Forms.DialogResult.OK)
                {
                    Export();
                }
            }
        }

        private void Export()
        {
            XmlSerializer x = new XmlSerializer(this.GetType());
            TextWriter txt = File.CreateText(@"pref.xml");
            x.Serialize(txt, this);
        }

        private void Import()
        {
            try
            {
                XmlSerializer x = new XmlSerializer(this.GetType());
                FileStream txt = new FileStream(@"pref.xml", FileMode.Open);
                Preferences p = (Preferences)x.Deserialize(txt);

                this._VertexMatrixName = p._VertexMatrixName;
                this._ElementMatrixName = p._ElementMatrixName;
                this._CutTriangleMatrixName = p._CutTriangleMatrixName;
                this._ElementID = p._ElementID;
                this._Curve = p._Curve;
                this._CalcAreaRelation = p._CalcAreaRelation;
                this._AreaRelationMatrixName = p._AreaRelationMatrixName;
                this._WeightFunctionCalculate = p._WeightFunctionCalculate;
                this._RadiusesDifference = p._RadiusesDifference;
            }
            catch
            {

            }
        }
    }
}
