using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MshToMatConverter
{
    public partial class PreferencesForm : Form
    {
        Preferences pref;

        Dictionary<string, Preferences.CurveType> curveName;

        public PreferencesForm(Preferences pref)
        {
            InitializeComponent();
            this.pref = pref;

            curveName = new Dictionary<string, Preferences.CurveType>();
            curveName.Add("2nd order curve", Preferences.CurveType.SecondOrderCurve);
            curveName.Add("Circle", Preferences.CurveType.Circle);

            Init();
            Refresh();
        }

        private void Init()
        {
            txtVertexMatrixName.Text = pref.VertexMatrixName;
            txtElementMatrixName.Text = pref.ElementMatrixName;
            txtCutMatrixName.Text = pref.CutTriangleMatrixName;
            txtAreaRelMatrixName.Text = pref.AreaRelationMatrixName;
            txtWeightFunctionMatrixName.Text = pref.WeightFunctionMatrixName;

            numRadiusesDifference.Value = Convert.ToDecimal(Math.Round(pref.RadiusesDifference, 1));

            CurveTypeListSet();
            chkAreaRelAllow.Checked = pref.CalcAreaRelation;
            chkWeightFunctionCalculate.Checked = pref.WeightFunctionCalculate;
        }

        private new void Refresh()
        {
            chkAreaRelAllow.Enabled = pref.Curve == Preferences.CurveType.Circle;
            txtAreaRelMatrixName.Enabled = pref.CalcAreaRelation;

            chkWeightFunctionCalculate.Enabled = pref.Curve == Preferences.CurveType.Circle;
            txtWeightFunctionMatrixName.Enabled = pref.WeightFunctionCalculate;
            numRadiusesDifference.Enabled = pref.WeightFunctionCalculate;
        }

        private void CurveTypeListSet()
        {
            cmbCurveType.Items.Clear();
            int n = 0;
            int selectedid = -1;
            foreach (var item in curveName)
            {
                cmbCurveType.Items.Add(item.Key);
                if (pref.Curve == item.Value)
                    selectedid = n;
                n++;
            }
            if (selectedid >= 0)
                cmbCurveType.SelectedIndex = selectedid;
        }

        private Preferences.CurveType getSelectedCurveType()
        {
            Preferences.CurveType curve;
            curveName.TryGetValue((string)cmbCurveType.SelectedItem, out curve);
            return curve;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            pref.VertexMatrixName = txtVertexMatrixName.Text;
            pref.ElementMatrixName = txtElementMatrixName.Text;
            pref.CutTriangleMatrixName = txtCutMatrixName.Text;
            pref.AreaRelationMatrixName = txtAreaRelMatrixName.Text;
            pref.WeightFunctionMatrixName = txtWeightFunctionMatrixName.Text;

            pref.RadiusesDifference = Convert.ToDouble(numRadiusesDifference.Value);
        }

        private void cmbCurveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pref.Curve = getSelectedCurveType();
            Refresh();
        }

        private void chkAreaRelAllow_CheckedChanged(object sender, EventArgs e)
        {
            pref.CalcAreaRelation = chkAreaRelAllow.Checked;
            Refresh();
        }

        private void chkWeightFunctionCalculate_CheckedChanged(object sender, EventArgs e)
        {
            pref.WeightFunctionCalculate = chkWeightFunctionCalculate.Checked;
            Refresh();
        }
    }
}
