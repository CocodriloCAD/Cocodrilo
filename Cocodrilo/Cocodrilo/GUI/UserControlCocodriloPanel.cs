using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Cocodrilo.Analyses;
using Cocodrilo.Commands;
using Cocodrilo.UserData;
using Cocodrilo.ElementProperties;
using Cocodrilo.Refinement;
using System.Collections.Generic;

namespace Cocodrilo.Panels
{
    [Guid("6E2D0EB9-188C-45D3-97F7-20A8BA6D6F42")]
    public partial class UserControlCocodriloPanel : UserControl
    {
        public UserControlCocodriloPanel()
        {
            InitializeComponent();

            Instance = this;

            CocodriloPlugIn.Instance.materialUpdate += updateMaterialData;
            CocodriloPlugIn.Instance.analysisUpdate += updateAnalysesData;

            comboBoxElementMat.DataSource = CocodriloPlugIn.Instance.Materials;
            comboBoxElementMat.DisplayMember = "Name";
            comboBoxElementMat.ValueMember = "Id";

            comboBoxAnalyses.DataSource = CocodriloPlugIn.Instance.Analyses;
            comboBoxAnalyses.DisplayMember = "Name";

            comboBoxCouplingType.DataSource = Enum.GetValues(typeof(CouplingType));
        }

        public static UserControlCocodriloPanel Instance { get; private set; }

        #region ComboBoxes
        public void updateMaterialData()
        {
            comboBoxElementMat.DataSource = null;
            comboBoxElementMat.DataSource = CocodriloPlugIn.Instance.Materials;
            comboBoxElementMat.DisplayMember = "Name";
            comboBoxElementMat.ValueMember = "Id";
        }

        public void updateAnalysesData()
        {
            comboBoxAnalyses.DataSource = null;
            comboBoxAnalyses.DataSource = CocodriloPlugIn.Instance.Analyses;
            comboBoxAnalyses.DisplayMember = "Name";
        }
        #endregion
        #region Material
        private void buttonAddModifyMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Run(new WindowMaterial());
            }
            catch
            {
                RhinoApp.WriteLine("Material cannot be opened!");
            }
        }
        #endregion
        #region Support
        private void buttonAddEdgeSupports_Click(object sender, EventArgs e)
        {
            try
            {
                RhinoApp.RunScript("Cocodrilo_AddSupports", true);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No support added.");
            }
        }

        public Support getSupport()
        {
            return new Support(
                checkBoxDispX.Checked, checkBoxDispY.Checked, checkBoxDispZ.Checked,
                textBoxSupportTypeDispX.Text, textBoxSupportTypeDispY.Text, textBoxSupportTypeDispY.Text,
                checkBoxRotationSupport.Checked, false,
                getIsSupportStrong(), comboBoxSupportType.Text);
        }


        public GeometryType getGeometryTypeSupport()
        {
            string GeometyTypeSelected = getSupportGeometryType();
            string ObjectTypeSelected = getSupportObjectType();

            switch (GeometyTypeSelected)
            {
                case "Surface":
                    switch (ObjectTypeSelected)
                    {
                        case "Surface":
                            return GeometryType.GeometrySurface;
                        case "Edge":
                            return GeometryType.SurfaceEdge;
                        case "Vertex":
                            return GeometryType.SurfacePoint;
                    }
                    break;
                case "Curve":
                    switch (ObjectTypeSelected)
                    {
                        case "Edge":
                            return GeometryType.CurveEdge;
                        case "Vertex":
                            return GeometryType.CurvePoint;
                    }
                    break;
            }

            RhinoApp.WriteLine("WARNING SUPPORT NOT FOUND!");
            return GeometryType.ErrorType;
        }

        public TimeInterval GetTimeInterval()
        {
            return new TimeInterval(textBoxSupportStartTime.Text, textBoxSupportEndTime.Text);
        }

        public bool getIsSupportStrong()
        {
            return checkBoxSupportStrong.Checked;
        }

        public bool getOverwriteSupport()
        {
            return checkBoxOverwriteSupport.Checked;
        }

        private void buttonDeleteEdgeSupport_Click(object sender, EventArgs e)
        {
            try
            {
                Rhino.RhinoApp.RunScript("Cocodrilo_DeleteSupports", true);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No support deleted. One or more supports did not exist.");
            }
        }
        public string getSupportObjectType()
        {
            if (radioButtonSupportDimFace.Checked)
                return "Surface";
            else if (radioButtonSupportDimLine.Checked)
                return "Edge";
            else if (radioButtonSupportDimVertex.Checked)
                return "Vertex";
            else
                return "";
        }

        public string getSupportGeometryType()
        {
            if (radioButtonSupportSurface.Checked)
                return "Surface";
            else if (radioButtonSupportCurve.Checked)
                return "Curve";
            else
                return "";
        }

        private void radioButtonSupportCurve_CheckedChanged(object sender, EventArgs e)      // TODO: Actions in Windows Forms directly on radioButtonSupport_AdaptChecked()
        {
            radioButtonSupport_AdaptChecked();
        }

        private void radioButtonSupportSurface_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonSupport_AdaptChecked();
        }

        private void radioButtonSupport_AdaptChecked()
        {
            if (radioButtonSupportCurve.Checked)
            {
                radioButtonSupportDimFace.Checked = false;
                radioButtonSupportDimFace.Enabled = false;
                checkBoxSupportStrong.Enabled = true;
            }
            else if (radioButtonSupportSurface.Checked)
            {
                radioButtonSupportDimFace.Enabled = true;
                checkBoxSupportStrong.Enabled = true;
            }
        }
        #endregion
        #region Coupling
        private void comboBoxCouplingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.GlobalCouplingMethod = (CouplingType)Enum.Parse(typeof(CouplingType), comboBoxCouplingType.Text);
        }
        #endregion
        #region Refinement
        private void buttonCheckRefinement_Click(object sender, EventArgs e)
        {
            try
            {
                if (getRefinementElementType() == GeometryType.GeometrySurface)
                {
                    ObjRef objref = null;
                    var rc = RhinoGet.GetOneObject("Select one Surface", false, ObjectType.Surface, out objref);
                    if (rc != Result.Success)
                        new Exception();

                    var user_data_surface = UserDataUtilities.GetOrCreateUserDataSurface(objref.Brep()
                        .Surfaces[objref.Face().FaceIndex]);

                    var refinement = user_data_surface.GetRefinement() as RefinementSurface;
                    textBoxPDeg.Text = refinement.PDeg.ToString();
                    textBoxQDeg.Text = refinement.QDeg.ToString();
                    textBoxKnotSubDivU.Text = refinement.KnotSubDivU.ToString();
                    textBoxKnotSubDivV.Text = refinement.KnotSubDivV.ToString();
                    if (refinement.KnotInsertType == 1)
                        radioButtonRefinementApproxElementSize.Checked = true;
                    else
                        radioButtonRefinementKnotSubdivision.Checked = true;
                }
                else if (getRefinementElementType() == GeometryType.SurfaceEdge)
                {
                    ObjRef objref = null;
                    var rc = RhinoGet.GetOneObject("Select one Edge", false, ObjectType.Curve, out objref);
                    if (rc != Result.Success)
                        new Exception();

                    var user_data_edge = UserDataUtilities.GetOrCreateUserDataEdge(
                        objref.Curve());

                    var refinement = user_data_edge.GetRefinement() as RefinementEdge;
                    textBoxPDeg.Text = refinement.PDeg.ToString();
                    textBoxKnotSubDivU.Text = refinement.KnotSubDivU.ToString();
                    if (refinement.KnotInsertType == 1)
                        radioButtonRefinementApproxElementSize.Checked = true;
                    else
                        radioButtonRefinementKnotSubdivision.Checked = true;
                }
                else if (getRefinementElementType() == GeometryType.GeometryCurve)
                {
                    ObjRef objref = null;
                    var rc = RhinoGet.GetOneObject(
                        "Select one Curve...",
                        false, ObjectType.Curve, out objref);
                    if (rc != Result.Success)
                        new Exception();

                    var user_data_curve = UserDataUtilities.GetOrCreateUserDataCurve(objref.Curve());

                    var refinement = user_data_curve.GetRefinement() as Refinement.RefinementCurve;
                    textBoxPDeg.Text = refinement.PolynomialDegree.ToString();
                    textBoxKnotSubDivU.Text = refinement.KnotSubDivU.ToString();
                    if (refinement.KnotInsertType == 1)
                        radioButtonRefinementApproxElementSize.Checked = true;
                    else
                        radioButtonRefinementKnotSubdivision.Checked = true;
                }
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No element found.");
            }
        }
        private void buttonChangeRefinement_Click(object sender, EventArgs e)
        {
            try
            {
                int knotinserttype = 0;
                if (radioButtonRefinementApproxElementSize.Checked)
                    knotinserttype = 1;

                if (getRefinementElementType() == GeometryType.GeometrySurface)
                {
                    var degree_p = Convert.ToInt32(textBoxPDeg.Text);
                    var degree_q = Convert.ToInt32(textBoxQDeg.Text);
                    var KnotSubDivU = Convert.ToInt32(textBoxKnotSubDivU.Text);
                    var KnotSubDivV = Convert.ToInt32(textBoxKnotSubDivV.Text);

                    var ThisSurfaceRefinement = new RefinementSurface(
                        degree_p,
                        degree_q,
                        KnotSubDivU,
                        KnotSubDivV,
                        knotinserttype);

                    if (CommandUtilities.TryGetUserDataSurface(out var user_data_surface_list))
                    {
                        foreach (var user_data_surface in user_data_surface_list)
                        {
                            user_data_surface.ChangeRefinement(ThisSurfaceRefinement);
                        }
                    }
                }
                else if (getRefinementElementType() == GeometryType.SurfaceEdge)
                {
                    int degree_p = Convert.ToInt32(textBoxPDeg.Text);
                    int KnotSubDivU = Convert.ToInt32(textBoxKnotSubDivU.Text);

                    var ThisEdgeRefinement = new RefinementEdge(
                        degree_p,
                        KnotSubDivU,
                        knotinserttype);

                    if (CommandUtilities.TryGetUserDataEdge(out var user_data_edge_list))
                    {
                        foreach (var user_data_edge in user_data_edge_list)
                        {
                            user_data_edge.ChangeRefinement(ThisEdgeRefinement);
                        }
                    }
                }
                if (getRefinementElementType() == GeometryType.GeometryCurve)
                {
                    int degree_p = Convert.ToInt32(textBoxPDeg.Text);
                    int KnotSubDivU = Convert.ToInt32(textBoxKnotSubDivU.Text);

                    var ThisCurveRefinement = new RefinementCurve(
                        degree_p,
                        KnotSubDivU,
                        knotinserttype);

                    if (CommandUtilities.TryGetUserDataCurve(out var user_data_curve_list))
                    {
                        foreach (var user_data_curve in user_data_curve_list)
                        {
                            user_data_curve.ChangeRefinement(ThisCurveRefinement);
                        }
                    }
                }
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No refinement done.");
            }
        }

        public GeometryType getRefinementElementType()
        {
            if (radioButtonRefinementElementSurf.Checked)
                return GeometryType.GeometrySurface;
            else if (radioButtonRefinementElementCurve.Checked)
                return GeometryType.GeometryCurve;
            else if (radioButtonRefinementElementEdge.Checked)
                return GeometryType.SurfaceEdge;
            else
                return GeometryType.ErrorType;
            //return comboBoxRefinementType.Text.ToString();
        }

        private void radioButtonRefinementCurve_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRefinement_AdaptChecked();
        }

        private void radioButtonRefinementSurface_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRefinement_AdaptChecked();
        }

        private void radioButtonRefinementEdge_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRefinement_AdaptChecked();
        }

        private void radioButtonRefinement_AdaptChecked()
        {
            if (radioButtonRefinementElementCurve.Checked)
            {
                textBoxQDeg.Enabled = false;
                textBoxKnotSubDivV.Enabled = false;
                buttonCheckRefinement.Enabled = true;
                radioButtonRefinementKnotSubdivision.Checked = true;
                radioButtonRefinementApproxElementSize.Enabled = false;
            }
            else if (radioButtonRefinementElementEdge.Checked)
            {
                textBoxQDeg.Enabled = false;
                textBoxKnotSubDivV.Enabled = false;
                buttonCheckRefinement.Enabled = false;
                radioButtonRefinementKnotSubdivision.Checked = true;
                radioButtonRefinementApproxElementSize.Enabled = false;
            }
            else if (radioButtonRefinementElementSurf.Checked)
            {
                textBoxQDeg.Enabled = true;
                textBoxKnotSubDivV.Enabled = true;
                buttonCheckRefinement.Enabled = true;
                radioButtonRefinementApproxElementSize.Enabled = true;
            }
        }
        private void radioButtonRefinementKnotSubdivision_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRefinementElements_AdaptChecked();
        }
        private void radioButtonKnotSubdivision_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRefinementElements_AdaptChecked();
        }
        private void radioButtonRefinementElements_AdaptChecked()
        {
            //if (radioButtonRefinementKnotSubdivision.Checked)
            //{
            //    labelRefinementMinU.Text = "Knot Subdivision U";
            //    labelRefinementMinV.Text = "Knot Subdivision V";
            //}
            //else if (radioButtonRefinementApproxElementSize.Checked)
            //{
            //    labelRefinementMinU.Text = "Approx. EleSize U";
            //    labelRefinementMinV.Text = "Approx. EleSize V";
            //}
        }
        #endregion
        #region Analyses
        //just allow integer in the text box
        private void textBoxMaxSteps_KeyPress(object sender, KeyPressEventArgs e)
        {
            var c = e.KeyChar;

            if (!char.IsDigit(c) && c != 8)
            {
                e.Handled = true;
            }
        }
        private void textBoxMaxIterations_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8) return;
            e.Handled = true;
        }
        //just allow doubles in the text box
        private void textBoxTolerance_KeyPress(object sender, KeyPressEventArgs e)
        {
            var c = e.KeyChar;

            if (!char.IsDigit(c) && c != 8 && c != '.' && c != 'e' && c != '-')
            {
                e.Handled = true;
            }
        }
        //name of analyses must not have spaces.
        private void textBoxFormfindingName_KeyPress(object sender, KeyPressEventArgs e) => e.Handled = e.KeyChar == ' ';
        private void tabControlAnalyses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlAnalyses.SelectedIndex == 1)
                buttonModifyAnalysis.Enabled = false;
            else
                buttonModifyAnalysis.Enabled = true;
        }

        private void buttonCreateAnalysis_Click(object sender, EventArgs e)
        {
            try
            {

                if (tabControlAnalyses.SelectedTab.Text.ToString() == "Formfinding")
                {
                    var Name = textBoxFormfindingName.Text;
                    var analysis = CocodriloPlugIn.Instance.findAnalysis(Name);
                    if (analysis != null)
                        new Exception();
                    var MaxIterations = Convert.ToInt32(textBoxMaxIterations.Text);
                    var MaxSteps = Convert.ToInt32(textBoxMaxSteps.Text);
                    var Tolerance = Convert.ToDouble(textBoxTolerance.Text);

                    var Formfinding = new Analyses.AnalysisFormfinding(Name, MaxSteps, MaxIterations, Tolerance);
                    CocodriloPlugIn.Instance.AddAnalysis(Formfinding);
                }
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "LinStrucAnalysis")
                {
                    var Name = textBoxLinStrucAnalysisName.Text;
                    //var Acc = Convert.ToDouble(textBoxAnalysisStaGeoAcc.Text);
                    //var OnlyIbraPre = checkBoxOnlyIbraPre.Checked;

                    var analysis = CocodriloPlugIn.Instance.findAnalysis(Name);

                    if (analysis != null)
                        throw new Exception();

                    var StaLinAnalysis = new Analyses.AnalysisLinear(Name);
                    CocodriloPlugIn.Instance.AddAnalysis(StaLinAnalysis);
                    analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxLinStrucAnalysisName.Text);
                }
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "NonLinStrucAnalysis")
                {
                    var name = textBoxNonLinStrucAnalysisName.Text;
                    var SolverTolerance = Convert.ToDouble(textBoxNonLinStrucAnalysisAcc.Text);
                    var NumSimulationSteps = Convert.ToInt32(textBoxNonLinStrucAnalysisNumSteps.Text);
                    var StepSize = Convert.ToDouble(textBoxNonLinStruAnalysisStepSize.Text);
                    var MaxSolverIteration = Convert.ToInt32(textBoxNonLinStrucAnalysisNumIter.Text);

                    var Stp_Ctrl = Convert.ToInt32(textBoxNonLinStrucAnalysisAdapStepCntrl.Text);

                    var analysis = CocodriloPlugIn.Instance.findAnalysis(Name);

                    if (analysis != null)
                        throw new Exception();

                    var StaNonLinAnalysis = new Analyses.AnalysisNonLinear(
                        name, NumSimulationSteps, MaxSolverIteration, SolverTolerance, StepSize);
                    //Formfinding.startAnalysis();
                    CocodriloPlugIn.Instance.AddAnalysis(StaNonLinAnalysis);
                }
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "TransientAnalysis")
                {
                    var Name = textBoxTransientAnalysisName.Text;
                    var Acc = Convert.ToDouble(textBoxTransientAnalysisAcc.Text);
                    var Num_steps = Convert.ToInt32(textBoxTransientAnalysisNumSteps.Text);
                    var Num_iter = Convert.ToInt32(textBoxTransientAnalysisNumIter.Text);
                    var Stp_Ctrl = Convert.ToInt32(textBoxTransientAnalysisAdapStepCntrl.Text);
                    var Rayleigh_alpha = Convert.ToDouble(textBoxTransientAnalysisRayleighAlpha.Text);
                    var Rayleigh_beta = Convert.ToDouble(textBoxTransientAnalysisRayleighBeta.Text);
                    var Time_integ = Convert.ToString(comboBoxTransientAnalysisTimeIntegration.Text);
                    var Scheme = Convert.ToString(comboBoxTransientAnalysisScheme.Text);
                    var Automatic_Rayleigh = checkBoxTransientAnalysisAutomaticRayleigh.Checked;
                    var Damping_ratio_0 = Convert.ToDouble(textBoxTransientAnalysisDampingRatio0.Text);
                    var Damping_ratio_1 = Convert.ToDouble(textBoxTransientAnalysisDampingRatio1.Text);
                    var Num_eigen = Convert.ToInt32(textBoxTransientAnalysisNumEigen.Text);

                    var analysis = CocodriloPlugIn.Instance.findAnalysis(Name);

                    if (analysis != null)
                        throw new Exception();

                    var TransientAnalysis = new Analyses.AnalysisTransient(Name, Num_steps, Num_iter, Acc, Num_steps, 1.0, Stp_Ctrl, Rayleigh_alpha, Rayleigh_beta, Time_integ, Scheme, Automatic_Rayleigh, Damping_ratio_0, Damping_ratio_1, Num_eigen);
                    CocodriloPlugIn.Instance.AddAnalysis(TransientAnalysis);
                }
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "EigenvalueAnalysis")
                {
                    //var Name = textBoxEigenvalueAnalysisName.Text;
                    //var Acc = Convert.ToDouble(textBoxEigenvalueAnalysisAcc.Text);
                    //var Num_eigen = Convert.ToInt32(textBoxEigenvalueAnalysisNumEigen.Text);
                    //var Num_iter = Convert.ToInt32(textBoxEigenvalueAnalysisNumIter.Text);
                    //var Solver_type = Convert.ToString(comboBoxEigenvalueAnalysisSolverType.Text);

                    var analysis = CocodriloPlugIn.Instance.findAnalysis(Name);

                    if (analysis != null)
                        throw new Exception();

                    //var EigenvalueAnalysis = new Analyses.AnalysisEigenvalue(Name, Acc, Num_eigen, Num_iter, Solver_type);
                    //CocodriloPlugIn.Instance.AddAnalysis(EigenvalueAnalysis);
                }
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "CutPattern")
                {
                    RhinoApp.WriteLine("WARNING: Cutting Pattern Analysis not yet implemented.");
                    var Name = textBoxCutPatternAnalysisName.Text;
                    var analysis = CocodriloPlugIn.Instance.findAnalysis(Name);
                    if (analysis != null)
                        new Exception();

                    var MaxIterations = Convert.ToInt32(textBoxCutPatternAnalysisMaxIter.Text);
                    var MaxSteps = Convert.ToInt32(textBoxCutPatternAnalysisMaxStep.Text);
                    var Tolerance = Convert.ToDouble(textBoxCutPatternAnalysisTol.Text);
                    //var SolStrat = comboBoxCutPatternAnalysisSolutionStrategy.Text;
                    var SolStrat = "Newton-Raphson";
                    var Prestress = checkBoxCutPatternAnalysisPrestress.Checked;

                    var CutPatt = new Analyses.AnalysisCuttingPattern(Name, MaxSteps, MaxIterations, Tolerance, SolStrat, Prestress);
                    CocodriloPlugIn.Instance.AddAnalysis(CutPatt);
                }
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: Analysis not completed, or name does already exist.");
            }
        }
        private void buttonModifyAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControlAnalyses.SelectedTab.Text.ToString() == "Formfinding")
                {
                    var analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxFormfindingName.Text);
                    if (analysis == null)
                        new Exception();
                    (analysis as AnalysisFormfinding).maxIterations = Convert.ToInt32(textBoxMaxIterations.Text);
                    (analysis as AnalysisFormfinding).maxSteps = Convert.ToInt32(textBoxMaxSteps.Text);
                    (analysis as AnalysisFormfinding).tolerance = Convert.ToDouble(textBoxTolerance.Text);
                }
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "LinStrucAnalysis")
                {
                }
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "NonLinStrucAnalysis")
                {
                    var analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxNonLinStrucAnalysisName.Text);
                    if (analysis == null)
                        new Exception();
                    if (analysis is AnalysisNonLinear)
                    {
                        var non_linear_analysis = analysis as AnalysisNonLinear;
                        non_linear_analysis.mMaxSolverIteration = Convert.ToInt32(textBoxNonLinStrucAnalysisNumIter.Text);
                        non_linear_analysis.mNumSimulationSteps = Convert.ToInt32(textBoxNonLinStrucAnalysisNumSteps.Text);
                        non_linear_analysis.mSolverTolerance = Convert.ToDouble(textBoxNonLinStrucAnalysisAcc.Text);
                        non_linear_analysis.mStepSize = Convert.ToDouble(textBoxNonLinStruAnalysisStepSize.Text);
                    }
                }
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "TransientAnalysis")
                {
                    var analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxTransientAnalysisName.Text);
                    if (analysis == null)
                        new Exception();
                    (analysis as AnalysisTransient).MaxIter = Convert.ToInt32(textBoxTransientAnalysisNumIter.Text);
                    (analysis as AnalysisTransient).NumStep = Convert.ToInt32(textBoxTransientAnalysisNumSteps.Text);
                    (analysis as AnalysisTransient).tolerance = Convert.ToDouble(textBoxTransientAnalysisAcc.Text);
                    (analysis as AnalysisTransient).RayleighAlpha = Convert.ToDouble(textBoxTransientAnalysisRayleighAlpha.Text);
                    (analysis as AnalysisTransient).RayleighBeta = Convert.ToDouble(textBoxTransientAnalysisRayleighBeta.Text);
                    (analysis as AnalysisTransient).TimeInteg = Convert.ToString(comboBoxTransientAnalysisTimeIntegration.Text);
                    (analysis as AnalysisTransient).Scheme = Convert.ToString(comboBoxTransientAnalysisScheme.Text);
                    (analysis as AnalysisTransient).AutomaticRayleigh = checkBoxTransientAnalysisAutomaticRayleigh.Checked;
                    (analysis as AnalysisTransient).DampingRatio0 = Convert.ToDouble(textBoxTransientAnalysisDampingRatio0.Text);
                    (analysis as AnalysisTransient).DampingRatio1 = Convert.ToDouble(textBoxTransientAnalysisDampingRatio1.Text);
                    (analysis as AnalysisTransient).NumEigen = Convert.ToDouble(textBoxTransientAnalysisNumEigen.Text);
                }
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "EigenvalueAnalysis")
                {
                    //var analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxEigenvalueAnalysisName.Text);
                    //if (analysis == null)
                    //    new Exception();
                    //(analysis as AnalysisEigenvalue).MaxIter = Convert.ToInt32(textBoxEigenvalueAnalysisNumIter.Text);
                    //(analysis as AnalysisEigenvalue).NumEigen = Convert.ToInt32(textBoxEigenvalueAnalysisNumEigen.Text);
                    //(analysis as AnalysisEigenvalue).tolerance = Convert.ToDouble(textBoxEigenvalueAnalysisAcc.Text);
                    //(analysis as AnalysisEigenvalue).SolverType = Convert.ToString(comboBoxEigenvalueAnalysisSolverType.Text);
                }
                if (tabControlAnalyses.SelectedTab.Text.ToString() == "CutPattern")
                {
                    var analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxCutPatternAnalysisName.Text);
                    if (analysis == null)
                        new Exception();
                    (analysis as AnalysisCuttingPattern).maxIterations = Convert.ToInt32(textBoxCutPatternAnalysisMaxIter.Text);
                    (analysis as AnalysisCuttingPattern).maxSteps = Convert.ToInt32(textBoxCutPatternAnalysisMaxStep.Text);
                    (analysis as AnalysisCuttingPattern).tolerance = Convert.ToDouble(textBoxCutPatternAnalysisTol.Text);
                    //(analysis as AnalysisCuttingPattern).solution_strategy = comboBoxCutPatternAnalysisSolutionStrategy.Text;
                    (analysis as AnalysisCuttingPattern).Prestress = checkBoxCutPatternAnalysisPrestress.Checked;
                }
            }
            catch (Exception)
            {
                RhinoApp.WriteLine("WARNING: Analysis not found!");
            }
        }
        private void buttonDeleteAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                Analysis analysis = null;
                if (tabControlAnalyses.SelectedTab.Text.ToString() == "Formfinding")
                    analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxFormfindingName.Text);
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "LinStrucAnalysis")
                    analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxLinStrucAnalysisName.Text);
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "NonLinStrucAnalysis")
                    analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxNonLinStrucAnalysisName.Text);
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "TransientAnalysis")
                    analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxTransientAnalysisName.Text);
                //else if (tabControlAnalyses.SelectedTab.Text.ToString() == "EigenvalueAnalysis")
                //    analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxEigenvalueAnalysisName.Text);
                else if (tabControlAnalyses.SelectedTab.Text.ToString() == "CutPattern")
                    analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxCutPatternAnalysisName.Text);

                if (analysis == null)
                    new Exception();

                if (!CocodriloPlugIn.Instance.DeleteAnalysis(analysis))
                    new Exception();
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: Analysis not found!");
            }
        }

        private void comboBoxAnalyses_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var analysis = CocodriloPlugIn.Instance.findAnalysis(comboBoxAnalyses.Text);
                if (analysis != null)
                {
                    if (analysis.GetType() == typeof(Analyses.AnalysisFormfinding))
                    {
                        var formfinding = (Analyses.AnalysisFormfinding)analysis;
                        textBoxFormfindingName.Text = formfinding.Name;
                        textBoxMaxIterations.Text = formfinding.maxIterations.ToString();
                        textBoxMaxSteps.Text = formfinding.maxSteps.ToString();
                        textBoxTolerance.Text = formfinding.tolerance.ToString();
                    }
                }
            }
            catch (Exception)
            {
                RhinoApp.WriteLine("Analysis not found");
            }
        }

        private void radioButtonRunCarat_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRunCarat.Checked)
                radioButtonRunKratos.Checked = false;
            else
                radioButtonRunKratos.Checked = true;
        }
        private void radioButtonRunKratos_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRunKratos.Checked)
                radioButtonRunCarat.Checked = false;
            else
                radioButtonRunCarat.Checked = true;
        }

        private void buttonRunAnalysis_Click(object sender, EventArgs e)
        {
            var analysis = CocodriloPlugIn.Instance.findAnalysis(comboBoxAnalyses.Text);

            if (radioButtonRunCarat.Checked)
            {
                var output = new IO.OutputKratosIGA(analysis);
                output.StartAnalysis();
            }
            else
            {
                var output = new IO.OutputKratosIGA(analysis);
                output.StartAnalysis();
            }
        }

        private void buttonPostprocessing_Click(object sender, EventArgs e)
        {
            try
            {
                //Rhino.RhinoApp.RunScript("CARAT_Clear_Postprocessing", true);
                var is_post = Rhino.RhinoApp.RunScript("CARAT_Is_Loaded_Postprocessing", true);     //BA TODO: ergibt immer true, da script ausgeführt werden kann -> direkt auf CaratInterface zugreifen.
                if (!is_post)
                {
                    var path = Rhino.PlugIns.PlugIn.PathFromId(new Guid("ce983e9d-72de-4a79-8832-7c374e6e26de"));
                    var pathWithSlash = path.Replace("\\", "/");
                    var idOfLastSlash = pathWithSlash.LastIndexOf("/");
                    var pathWithSlash_short = pathWithSlash.Substring(0, idOfLastSlash);
                    var pluginPath = pathWithSlash_short.Replace("/", "\\");
                    var analysis = CocodriloPlugIn.Instance.findAnalysis(comboBoxAnalyses.Text);
                    string input = "";
                    if (radioButtonRunKratos.Checked)
                        input = analysis.Name + "_kratos_0.georhino.json";
                    else
                        input = analysis.Name + ".georhino.txt";
                    pluginPath += "\\" + analysis.Name;
                    Rhino.RhinoApp.RunScript("CARAT_Load_Postprocessing " + pluginPath + " " + input + " ", false);
                }
                Rhino.RhinoApp.RunScript("CARAT_Postprocessing", true);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: CARAT_Postprocessing not found.");
            }
        }

        private void buttonEditOutput_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Run(new WindowOutputOptions());
            }
            catch
            {
                RhinoApp.WriteLine("Output options window is already open!");
            }
        }

        #endregion
        #region Options
        private void checkBoxShowElements_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.visualizer.show_elements = checkBoxShowPreprocessing.Checked;
            //CocodriloPlugIn.Instance.visualizer.Enabled = checkBoxShowPreprocessing.Checked;
            if (checkBoxShowPreprocessing.Checked || checkBoxShowPreSupports.Checked || checkBoxShowPreLoads.Checked
                || checkBoxShowPreCouplings.Checked)
                CocodriloPlugIn.Instance.visualizer.Enabled = true;
            else
                CocodriloPlugIn.Instance.visualizer.Enabled = false;
            var doc = RhinoDoc.ActiveDoc;
            doc.Views.Redraw();
        }

        private void checkBoxShowPreSupports_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.visualizer.show_supports = checkBoxShowPreSupports.Checked;
            if (checkBoxShowPreprocessing.Checked || checkBoxShowPreSupports.Checked || checkBoxShowPreLoads.Checked
                || checkBoxShowPreCouplings.Checked)
                CocodriloPlugIn.Instance.visualizer.Enabled = true;
            else
                CocodriloPlugIn.Instance.visualizer.Enabled = false;
            var doc = RhinoDoc.ActiveDoc;
            doc.Views.Redraw();
        }

        private void checkBoxShowPreLoads_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.visualizer.show_loads = checkBoxShowPreLoads.Checked;
            if (checkBoxShowPreprocessing.Checked || checkBoxShowPreSupports.Checked || checkBoxShowPreLoads.Checked
                || checkBoxShowPreCouplings.Checked)
                CocodriloPlugIn.Instance.visualizer.Enabled = true;
            else
                CocodriloPlugIn.Instance.visualizer.Enabled = false;
            var doc = RhinoDoc.ActiveDoc;
            doc.Views.Redraw();
        }

        private void checkBoxShowPreCouplings_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.visualizer.show_couplings = checkBoxShowPreCouplings.Checked;
            if (checkBoxShowPreprocessing.Checked || checkBoxShowPreSupports.Checked || checkBoxShowPreLoads.Checked
                || checkBoxShowPreCouplings.Checked)
                CocodriloPlugIn.Instance.visualizer.Enabled = true;
            else
                CocodriloPlugIn.Instance.visualizer.Enabled = false;
            var doc = RhinoDoc.ActiveDoc;
            doc.Views.Redraw();
        }

        private void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                Rhino.RhinoApp.RunScript("Cocodrilo_DeleteAll", true);
            }
            catch (Exception)
            {
                RhinoApp.WriteLine("WARNING: No Userdata deleted");
            }

        }

        private void buttonResetInstance_Click(object sender, EventArgs e)
        {
            Rhino.RhinoApp.RunScript("Cocodrilo_DeleteAll", true);
        }
        #endregion
        #region Load
        public Load getLoad()
        {
            return new ElementProperties.Load(
                textBoxLoadX.Text,
                textBoxLoadY.Text,
                textBoxLoadZ.Text,
                "1.0",
                comboBoxLoadType.Text);
        }

        public double[] getLoadPosition()
        {
            double[] positions = new double[2] { -1, -1 };
            if (textBoxLoadPositionU.Text == "")
                positions[0] = -1;
            else
                positions[0] = Convert.ToDouble(textBoxLoadPositionU.Text);

            if (textBoxLoadPositionV.Text == "")
                positions[1] = -1;
            else
                positions[1] = Convert.ToDouble(textBoxLoadPositionV.Text);
            return positions;
        }
        //public string getLoadType()
        //{
        //    return comboBoxLoadType.SelectedText;
        //}
        public bool getLoadBoolOverwrite()
        {
            return checkBoxLoadOverwrite.Checked;
        }
        //public string getLoadObjectType()
        //{
        //    return comboBoxLoadObjectType.Text.ToString();
        //}
        private void buttonAddLoad_Click(object sender, EventArgs e)
        {
            try
            {
                Rhino.RhinoApp.RunScript("Cocodrilo_AddLoad", true);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No Load Added!");
            }
        }
        private void buttonDeleteLoad_Click(object sender, EventArgs e)
        {
            try
            {
                Rhino.RhinoApp.RunScript("Cocodrilo_DeleteLoad", true);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No Load Deleted!");
            }
        }
        private void comboBoxLoadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxLoadType.Text.ToString() == "PRES" || comboBoxLoadType.Text.ToString() == "PRES_FL")
            {
                //labelLoadDirection.Text = "Load:";
                labelLoadDirectionX.Visible = false;
                labelLoadDirectionY.Visible = false;
                labelLoadDirectionZ.Visible = false;
                textBoxLoadY.Visible = false;
                textBoxLoadZ.Visible = false;
            }
            else
            {
                //labelLoadDirection.Text = "Load:";
                labelLoadDirectionX.Visible = true;
                labelLoadDirectionY.Visible = true;
                labelLoadDirectionZ.Visible = true;
                textBoxLoadY.Visible = true;
                textBoxLoadZ.Visible = true;
            }
        }
        public string getLoadObjectType()
        {
            if (radioButtonLoadDimFace.Checked)
                return "Surface";
            else if (radioButtonLoadDimLine.Checked)
                return "Edge";
            else if (radioButtonLoadDimVertex.Checked)
                return "Vertex";
            else
                return "";
            //return comboBoxSupportType.Text.ToString();
        }
        public string getLoadGeometryType()
        {
            if (radioButtonLoadElementSurface.Checked)
                return "Surface";
            else if (radioButtonLoadElementCurve.Checked)
                return "Curve";
            //else if (radioButtonLoadElementEdge.Checked)
            //    return "Edge";
            //else if (radioButtonLoadElementPoint.Checked)
            //    return "Point";
            else
                return "";
            //return comboBoxSupportType.Text.ToString();
        }

        public GeometryType getGeometryTypeLoad()
        {
            string GeometyTypeSelected = getLoadGeometryType();
            string ObjectTypeSelected = getLoadObjectType();

            switch (GeometyTypeSelected)
            {
                case "Surface":
                    switch (ObjectTypeSelected)
                    {
                        case "Surface":
                            return GeometryType.GeometrySurface;
                        case "Edge":
                            return GeometryType.SurfaceEdge;
                        case "Vertex":
                            return GeometryType.SurfacePoint;
                    }
                    break;
                case "Curve":
                    switch (ObjectTypeSelected)
                    {
                        case "Edge":
                            return GeometryType.CurveEdge;
                        case "Vertex":
                            return GeometryType.CurvePoint;
                    }
                    break;
            }

            RhinoApp.WriteLine("WARNING SUPPORT NOT FOUND!");
            return GeometryType.GeometrySurface;
        }

        private void radioButtonLoadCurve_CheckedChanged(object sender, EventArgs e)      // TODO: Actions in Windows Forms directly on radioButtonLoad_AdaptChecked()
        {
            radioButtonLoad_AdaptChecked();
        }

        private void radioButtonLoadSurface_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonLoad_AdaptChecked();
        }

        private void radioButtonLoadDimFace_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonLoad_AdaptChecked();
        }

        private void radioButtonLoadDimLine_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonLoad_AdaptChecked();
        }

        private void radioButtonLoadDimVertex_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonLoad_AdaptChecked();
        }

        private void radioButtonLoad_AdaptChecked()
        {
            if (radioButtonLoadElementCurve.Checked)
            {
                radioButtonLoadDimFace.Checked = false;
                radioButtonLoadDimFace.Enabled = false;
                radioButtonLoadDimLine.Enabled = true;
                if (radioButtonLoadDimVertex.Checked)
                {
                    labelLoadPositionU.Enabled = true;
                    textBoxLoadPositionU.Enabled = true;
                }
                else
                {
                    labelLoadPositionU.Enabled = false;
                    textBoxLoadPositionU.Enabled = false;
                }
                labelLoadPositionV.Enabled = false;
                textBoxLoadPositionV.Enabled = false;
            }
            else if (radioButtonLoadElementSurface.Checked)
            {
                radioButtonLoadDimFace.Enabled = true;
                radioButtonLoadDimLine.Enabled = true;
                if (radioButtonLoadDimFace.Checked)
                {
                    labelLoadPositionU.Enabled = false;
                    textBoxLoadPositionU.Enabled = false;
                    labelLoadPositionV.Enabled = false;
                    textBoxLoadPositionV.Enabled = false;
                }
                labelLoadPositionU.Enabled = true;
                textBoxLoadPositionU.Enabled = true;
                labelLoadPositionV.Enabled = true;
                textBoxLoadPositionV.Enabled = true;
            }
        }
        #endregion
        #region Element
        private void comboBoxBeamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxBeamType.Text.ToString() == "")
            {
                //circular
                labelBeamDiameter.Visible = false;
                //textBoxBeamDiameter.Visible = false;
                //rectangular
                labelBeamHeight.Visible = false;
                //textBoxBeamHeight.Visible = false;
                labelBeamWidth.Visible = false;
                //textBoxBeamWidth.Visible = false;
                //undefined
                labelBeamArea.Visible = false;
                textBoxBeamArea.Visible = false;
                labelBeamIy.Visible = false;
                textBoxBeamIy.Visible = false;
                labelBeamIz.Visible = false;
                textBoxBeamIz.Visible = false;
                labelBeamIt.Visible = false;
                textBoxBeamIt.Visible = false;
            }
            else if (comboBoxBeamType.Text.ToString() == "Circular")
            {
                //circular
                labelBeamDiameter.Visible = true;
                //textBoxBeamDiameter.Visible = true;
                //rectangular
                labelBeamHeight.Visible = false;
                //textBoxBeamHeight.Visible = false;
                labelBeamWidth.Visible = false;
                //textBoxBeamWidth.Visible = false;
                //undefined
                labelBeamArea.Visible = false;
                textBoxBeamArea.Visible = false;
                labelBeamIy.Visible = false;
                textBoxBeamIy.Visible = false;
                labelBeamIz.Visible = false;
                textBoxBeamIz.Visible = false;
                labelBeamIt.Visible = false;
                textBoxBeamIt.Visible = false;
            }
            else if (comboBoxBeamType.Text.ToString() == "Rectangular")
            {
                //circular
                labelBeamDiameter.Visible = false;
                //textBoxBeamDiameter.Visible = false;
                //rectangular
                labelBeamHeight.Visible = true;
                //textBoxBeamHeight.Visible = true;
                labelBeamWidth.Visible = true;
                //textBoxBeamWidth.Visible = true;
                //undefined
                labelBeamArea.Visible = false;
                textBoxBeamArea.Visible = false;
                labelBeamIy.Visible = false;
                textBoxBeamIy.Visible = false;
                labelBeamIz.Visible = false;
                textBoxBeamIz.Visible = false;
                labelBeamIt.Visible = false;
                textBoxBeamIt.Visible = false;
            }
            else if (comboBoxBeamType.Text.ToString() == "Undefined")
            {
                //circular
                labelBeamDiameter.Visible = false;
                //textBoxBeamDiameter.Visible = false;
                //rectangular
                labelBeamHeight.Visible = false;
                //textBoxBeamHeight.Visible = false;
                labelBeamWidth.Visible = false;
                //textBoxBeamWidth.Visible = false;
                //undefined
                labelBeamArea.Visible = true;
                textBoxBeamArea.Visible = true;
                labelBeamIy.Visible = true;
                textBoxBeamIy.Visible = true;
                labelBeamIz.Visible = true;
                textBoxBeamIz.Visible = true;
                labelBeamIt.Visible = true;
                textBoxBeamIt.Visible = true;
            }
            else
            {
            }
        }
        private void comboBoxCableType_SelectedIndex_Changed(object sender, EventArgs e)
        {
            if (comboBoxCableType.Text == "Curve")
            {
                checkBoxCablePrestressCurve.Enabled = true;
            }
            else if (comboBoxCableType.Text == "Edge")
            {
                checkBoxCablePrestressCurve.Enabled = false;
            }
        }

        public MembraneProperties getMembraneProperties(ObjRef Surface)
        {
            var thickness = Convert.ToDouble(textBoxMembraneThick.Text);
            var prestress_1 = Convert.ToDouble(textBoxMembranePrestress1.Text);
            var prestress_2 = Convert.ToDouble(textBoxMembranePrestress2.Text);

            double[] direction_1 = new double[3] { 1, 0, 0 };
            double[] direction_2 = new double[3] { 0, 1, 0 };

            var knots_1 = Surface.Brep().Surfaces[Surface.Face().FaceIndex].GetSpanVector(0);
            var knots_2 = Surface.Brep().Surfaces[Surface.Face().FaceIndex].GetSpanVector(1);

            var point_00 = Surface.Brep().Surfaces[Surface.Face().FaceIndex].PointAt(knots_1[0], knots_2[0]);
            var point_10 = Surface.Brep().Surfaces[Surface.Face().FaceIndex].PointAt(knots_1[knots_1.Length - 1], knots_2[0]);
            var point_01 = Surface.Brep().Surfaces[Surface.Face().FaceIndex].PointAt(knots_1[0], knots_2[knots_2.Length - 1]);

            if ((Math.Abs(point_00[0] - point_10[0]) < 1e-12
                    && Math.Abs(point_00[1] - point_10[1]) < 1e-12)
                || (Math.Abs(point_00[0] - point_01[0]) < 1e-12
                    && Math.Abs(point_00[1] - point_01[1]) < 1e-12))
            {
                direction_2[0] = 0;
                direction_2[1] = 0;
                direction_2[2] = 1;
            }

            return new MembraneProperties(
                thickness,
                direction_1,
                direction_2,
                prestress_1,
                prestress_2);
        }
        public bool getIsMembraneFormFinding() => checkBoxElementMembraneFofi.Checked;

        public ShellProperties GetShellProperties()
        {
            return new ShellProperties(
                Convert.ToDouble(textBoxShellThick.Text),
                true,
                comboBoxShellType.Text);
        }

        public CableProperties GetCableProperties()
        {
            return new CableProperties(
                Convert.ToDouble(textBoxCablePrestress.Text),
                Convert.ToDouble(textBoxCableArea.Text));
        }
        public string getSelectedElementType() => tabControlElement.SelectedTab.Text.ToString();
        public GeometryType getCableTopologyType()
        {
            if (comboBoxCableType.Text == "Curve")
            {
                return GeometryType.GeometryCurve;
            }
            if (comboBoxCableType.Text == "Edge")
            {
                return GeometryType.SurfaceEdge;
            }

            RhinoApp.WriteLine("WARNING: comboBoxCableType.Text not Curve or Edge");
            return GeometryType.CurveEdge;
        }

        public bool getIsCableFormFinding() => checkBoxElementCableFofi.Checked;
        public int getMaterialIdElement() => Convert.ToInt32(comboBoxElementMat.SelectedValue);

        public bool getIsFormFindingElement() => checkBoxElementMembraneFofi.Checked;
        public bool getIsEdgeCoupling() => checkBoxElementMembraneEdgeCoupling.Checked;

        private void buttonAddElement_Click(object sender, EventArgs e)
        {
            try
            {
                RhinoApp.RunScript("Cocodrilo_AddElementFormulation", true);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No element added.");
            }
        }
        private void buttonDeleteElement_Click(object sender, EventArgs e)
        {
            try
            {
                RhinoApp.RunScript("Cocodrilo_DeleteElementFormulation", true);
            }
            catch (Exception)
            {
                RhinoApp.WriteLine("No data deleted!");
            }

        }
        private void buttonLoadElement_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControlElement.SelectedTab.Text.ToString() == "Membrane")
                {
                    ObjRef[] objref = null;
                    var rc = RhinoGet.GetMultipleObjects("Select Surfaces", false, ObjectType.Surface, out objref);
                    if (rc != Result.Success)
                        new Exception();

                    double tmp_thick = 0;
                    double tmp_prestress1 = 0;
                    double tmp_prestress2 = 0;
                    bool tmp_is_fofi = true;
                    bool same_thick = true;
                    bool same_prestress1 = true;
                    bool same_prestress2 = true;
                    bool same_is_fofi = true;

                    //UserDataObject ud;
                    // See if user data of my custom type is attached to the geomtry
                    foreach (var surface in objref)
                    {
                        var ud = surface.Brep().Surfaces[surface.Face().FaceIndex].UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                        if (ud == null)
                        {
                            RhinoApp.WriteLine("Surface has no user data");
                            continue;
                        }
                        //var tmp_ele = ud.myElementDataSurface.element as ElementSurfaceMembrane;
                        //if(tmp_ele==null)
                        //{
                        //    RhinoApp.WriteLine("Surface is not a membrane");
                        //    continue;
                        //}
                        //if (is_first_ele)
                        //{
                        //    tmp_thick = tmp_ele.mMembraneProperties.mThickness;
                        //    tmp_prestress1 = tmp_ele.mMembraneProperties.mPrestress1;
                        //    tmp_prestress2 = tmp_ele.mMembraneProperties.mPrestress2;
                        //    tmp_is_fofi = tmp_ele.mIsFormFinding;
                        //    is_first_ele = false;
                        //}
                        //else
                        //{
                        //    if(tmp_thick != tmp_ele.mMembraneProperties.mThickness)
                        //    { same_thick = false; }
                        //    if (tmp_prestress1 != tmp_ele.mMembraneProperties.mPrestress1)
                        //    { same_prestress1 = false; }
                        //    if (tmp_prestress2 != tmp_ele.mMembraneProperties.mPrestress2)
                        //    { same_prestress2 = false; }
                        //    if (tmp_is_fofi != tmp_ele.mIsFormFinding)
                        //    { same_is_fofi = false; }
                        //}
                    }
                    if (same_thick)
                        textBoxMembraneThick.Text = tmp_thick.ToString();
                    else
                        textBoxMembraneThick.Text = "";

                    if (same_prestress1)
                        textBoxMembranePrestress1.Text = tmp_prestress1.ToString();
                    else
                        textBoxMembranePrestress1.Text = "";

                    if (same_prestress2)
                        textBoxMembranePrestress2.Text = tmp_prestress2.ToString();
                    else
                        textBoxMembranePrestress2.Text = "";

                    if (same_is_fofi)
                        checkBoxElementMembraneFofi.Checked = tmp_is_fofi;
                    else
                    {
                        RhinoApp.WriteLine("Not every membrane element is part of form finding!");
                        checkBoxElementMembraneFofi.Checked = true;
                    }
                }
                else if (tabControlElement.SelectedTab.Text.ToString() == "Shell")
                {
                    ObjRef[] objref = null;
                    var rc = RhinoGet.GetMultipleObjects("Select Surfaces", false, ObjectType.Surface, out objref);
                    if (rc != Result.Success)
                        new Exception();

                    double tmp_thick = 0;
                    bool same_thick = true;

                    //UserDataObject ud;
                    // See if user data of my custom type is attached to the geomtry
                    foreach (var surface in objref)
                    {
                        var ud = surface.Brep().Surfaces[surface.Face().FaceIndex].UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                        if (ud == null)
                        {
                            RhinoApp.WriteLine("Surface has no user data");
                            continue;
                        }
                        //var tmp_ele = ud.myElementDataSurface.element as ElementSurfaceMembrane;
                        //if (tmp_ele == null)
                        //{
                        //    RhinoApp.WriteLine("Surface is not a shell");
                        //    continue;
                        //}
                        //if (is_first_ele)
                        //{
                        //    tmp_thick = tmp_ele.mMembraneProperties.mThickness;
                        //    is_first_ele = false;
                        //}
                        //else
                        //{
                        //    if (tmp_thick != tmp_ele.mMembraneProperties.mThickness)
                        //    { same_thick = false; }
                        //}
                    }
                    if (same_thick)
                        textBoxMembraneThick.Text = tmp_thick.ToString();
                    else
                        textBoxMembraneThick.Text = "";
                }
                else if (tabControlElement.SelectedTab.Text.ToString() == "Beam")
                {
                    ObjRef[] objref = null;
                    var rc = RhinoGet.GetMultipleObjects("Select Curves", false, ObjectType.Curve, out objref);
                    if (rc != Result.Success)
                        new Exception();

                    string tmp_cstype = "";
                    //double tmp_d = 0;
                    //double tmp_w = 0;
                    //double tmp_h = 0;
                    double tmp_it = 0;
                    double tmp_iy = 0;
                    double tmp_iz = 0;
                    double tmp_a = 0;
                    bool same_cstype = true;
                    //bool same_d = true;
                    //bool same_w = true;
                    //bool same_h = true;
                    bool same_it = true;
                    bool same_iy = true;
                    bool same_iz = true;
                    bool same_a = true;

                    bool is_first_ele = true;

                    //UserDataObject ud;
                    // See if user data of my custom type is attached to the geomtry
                    foreach (var crv in objref)
                    {
                        var ud = crv.Curve().UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                        if (ud == null)
                        {
                            RhinoApp.WriteLine("Curve has no user data");
                            continue;
                        }
                        //var tmp_ele = ud.myElementDataCurve.element as ElementCurveBeam;
                        //if (tmp_ele == null)
                        //{
                        //    RhinoApp.WriteLine("Curve is not a beam");
                        //    continue;
                        //}
                        if (is_first_ele)
                        {
                            tmp_cstype = comboBoxBeamType.Text;
                            //tmp_d = Convert.ToDouble(textBoxBeamDiameter.Text);
                            //tmp_w = Convert.ToDouble(textBoxBeamWidth.Text);
                            //tmp_h = Convert.ToDouble(textBoxBeamHeight.Text);
                            tmp_it = Convert.ToDouble(textBoxBeamIt.Text);
                            tmp_iy = Convert.ToDouble(textBoxBeamIy.Text);
                            tmp_iz = Convert.ToDouble(textBoxBeamIz.Text);
                            tmp_a = Convert.ToDouble(textBoxBeamArea.Text);
                            is_first_ele = false;
                        }
                        else
                        {
                            //if (tmp_type != tmp_ele.Height)
                            //{ same_type = false; }
                            //if (tmp_d != tmp_ele.Diameter)
                            //{ same_d = false; }
                            //if (tmp_w != tmp_ele.Width)
                            //{ same_w = false; }
                            //if (tmp_h != tmp_ele.Height)
                            //{ same_h = false; }
                            //if (tmp_a != tmp_ele.Area)
                            //{ same_a = false; }
                            //if (tmp_it != tmp_ele.It)
                            //{ same_it = false; }
                            //if (tmp_iy != tmp_ele.Iy)
                            //{ same_iy = false; }
                            //if (tmp_iz != tmp_ele.Iz)
                            //{ same_iz = false; }

                        }
                    }
                    //if (same_d)
                    //    textBoxBeamDiameter.Text = tmp_d.ToString();
                    //else
                    //    textBoxBeamDiameter.Text = "";

                    //if (same_w)
                    //    textBoxBeamWidth.Text = tmp_w.ToString();
                    //else
                    //    textBoxBeamWidth.Text = "";

                    //if (same_h)
                    //    textBoxBeamHeight.Text = tmp_h.ToString();
                    //else
                    //    textBoxBeamHeight.Text = "";

                    if (same_a)
                        textBoxBeamArea.Text = tmp_a.ToString();
                    else
                        textBoxBeamArea.Text = "";

                    if (same_iy)
                        textBoxBeamIy.Text = tmp_iy.ToString();
                    else
                        textBoxBeamIy.Text = "";

                    if (same_iz)
                        textBoxBeamIz.Text = tmp_iz.ToString();
                    else
                        textBoxBeamIz.Text = "";

                    if (same_it)
                        textBoxBeamIt.Text = tmp_it.ToString();
                    else
                        textBoxBeamIt.Text = "";

                    if (same_cstype)
                        comboBoxBeamType.Text = tmp_cstype;
                    else
                    {
                        RhinoApp.WriteLine("Not every membrane element is part of form finding!");
                        checkBoxElementMembraneFofi.Checked = true;
                    }

                }
                else if (tabControlElement.SelectedTab.Text.ToString() == "Cable")
                {
                    ObjRef[] objref = null;
                    var rc = RhinoGet.GetMultipleObjects("Select Surfaces", false, ObjectType.Surface, out objref);
                    if (rc != Result.Success)
                        new Exception();

                    string tmp_type = "";
                    double tmp_a = 0;
                    double tmp_prestress = 0;
                    bool tmp_hasldcrv = false;
                    bool tmp_is_fofi = true;
                    bool same_type = true;
                    bool same_prestress = true;
                    bool same_a = true;
                    bool same_is_fofi = true;
                    bool same_hasldcrv = true;

                    //UserDataObject ud;
                    // See if user data of my custom type is attached to the geomtry
                    foreach (var crv in objref)
                    {
                        var ud = crv.Curve().UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                        var ud_e = crv.Curve().UserData.Find(typeof(UserDataEdge)) as UserDataEdge;
                        if (ud == null || ud_e == null)
                        {
                            RhinoApp.WriteLine("Surface has no user data");
                            continue;
                        }
                        if (ud_e == null)
                        {
                            //var tmp_ele = ud.myElementDataCurve.element as ElementCurveCable;
                            //if (tmp_ele == null)
                            //{
                            //    RhinoApp.WriteLine("Surface is not a membrane");
                            //    continue;
                            //}
                            //if (is_first_ele)
                            //{
                            //    tmp_type = "Curve";
                            //    tmp_a = tmp_ele.area;
                            //    tmp_prestress = tmp_ele.prestress;
                            //    tmp_is_fofi = tmp_ele.is_fofi;
                            //    tmp_hasldcrv = (tmp_ele.is_load_curve != 0);
                            //    is_first_ele = false;
                            //}
                            //else
                            //{
                            //    if (tmp_type != "Curve")
                            //    { same_type = false; }
                            //    if (tmp_a != tmp_ele.area)
                            //    { same_a = false; }
                            //    if (tmp_prestress != tmp_ele.prestress)
                            //    { same_prestress = false; }
                            //    if (tmp_is_fofi != tmp_ele.is_fofi)
                            //    { same_is_fofi = false; }
                            //    if (tmp_hasldcrv != (tmp_ele.is_load_curve != 0))
                            //    { same_hasldcrv = false; }
                            //}
                        }
                        else if (ud == null)
                        {
                            //var tmp_ele = ud_e.cord as BRepElementEdgeCord;
                            //if (tmp_ele == null)
                            //{
                            //    RhinoApp.WriteLine("Surface is not a membrane");
                            //    continue;
                            //}
                            //if (is_first_ele)
                            //{
                            //    tmp_type = "Edge";
                            //    tmp_a = tmp_ele.area;
                            //    tmp_prestress = tmp_ele.prestress;
                            //    tmp_is_fofi = tmp_ele.is_fofi;
                            //    is_first_ele = false;
                            //}
                            //else
                            //{
                            //    if (tmp_type != "Edge")
                            //    { same_type = false; }
                            //    if (tmp_a != tmp_ele.area)
                            //    { same_a = false; }
                            //    if (tmp_prestress != tmp_ele.prestress)
                            //    { same_prestress = false; }
                            //    if (tmp_is_fofi != tmp_ele.is_fofi)
                            //    { same_is_fofi = false; }
                            //    //if (tmp_hasldcrv != (tmp_ele..is_load_curve != 0))
                            //    //{ same_hasldcrv = false; }
                            //}
                        }
                    }
                    if (same_type)
                        comboBoxCableType.Text = tmp_type;
                    else
                        textBoxMembraneThick.Text = "";

                    if (same_a)
                        textBoxCableArea.Text = tmp_a.ToString();
                    else
                        textBoxCableArea.Text = "";

                    if (same_prestress)
                        textBoxCablePrestress.Text = tmp_prestress.ToString();
                    else
                        textBoxCablePrestress.Text = "";

                    if (same_is_fofi)
                        checkBoxElementCableFofi.Checked = tmp_is_fofi;
                    else
                    {
                        RhinoApp.WriteLine("Not every membrane element is part of form finding!");
                        checkBoxElementMembraneFofi.Checked = true;
                    }

                    if (same_hasldcrv)
                        checkBoxCablePrestressCurve.Checked = tmp_hasldcrv;
                    else
                    {
                        RhinoApp.WriteLine("Not every membrane element has a load curve!");
                        checkBoxElementMembraneFofi.Checked = false;
                    }
                }
                else
                {
                    RhinoApp.WriteLine("No data could be loaded!");
                }
            }
            catch (Exception)
            {
                RhinoApp.WriteLine("No data could be loaded!");
            }
        }
        private void buttonAddAxis_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Run(new WindowAxis());
            }
            catch
            {
                RhinoApp.WriteLine("Axis window is already open!");
            }
        }
        #endregion
        #region Check
        private void buttonAddCheck_Click(object sender, EventArgs e)
        {
            try
            {
                RhinoApp.RunScript("Cocodrilo_AddChecks", true);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No outputs added.");
            }
        }

        public CheckProperties getCheckProperties()
        {
            return new CheckProperties(
                checkBoxOutputDispX.Checked,
                checkBoxOutputDispY.Checked,
                checkBoxOutputDispZ.Checked,
                checkBoxOutputLagrangeMP.Checked);
        }

        public GeometryType getGeometryTypeCheck()
        {
            string GeometyTypeSelected = getCheckGeometryType();
            string ObjectTypeSelected = getCheckObjectType();

            switch (GeometyTypeSelected)
            {
                case "Surface":
                    switch (ObjectTypeSelected)
                    {
                        case "Surface":
                            return GeometryType.GeometrySurface;
                        case "Edge":
                            return GeometryType.SurfaceEdge;
                        case "Vertex":
                            return GeometryType.SurfacePoint;
                    }
                    break;
                case "Curve":
                    switch (ObjectTypeSelected)
                    {
                        case "Edge":
                            return GeometryType.CurveEdge;
                        case "Vertex":
                            return GeometryType.CurvePoint;
                    }
                    break;
            }

            RhinoApp.WriteLine("WARNING OUTPUT LOCATION NOT FOUND!");
            return GeometryType.ErrorType;
        }

        public TimeInterval GetTimeIntervalCheck()
        {
            return new TimeInterval(textBoxCheckStartTime.Text, textBoxCheckEndTime.Text);
        }

        public bool getOverwriteCheck()
        {
            return checkBoxOverwriteChecks.Checked;
        }

        public CheckType GetCheckType()
        {
            return CheckType.ConvergenceCheck;
            //var item = comboBoxCheckType.SelectedItem;
            //return comboBoxCheckType.SelectedItem as CheckType;
        }


        public string getCheckObjectType()
        {
            if (radioButtonCheckFace.Checked)
                return "Surface";
            else if (radioButtonCheckLine.Checked)
                return "Edge";
            else if (radioButtonCheckVertex.Checked)
                return "Vertex";
            else
                return "";
        }

        public string getCheckGeometryType()
        {
            if (radioButtonCheckSurface.Checked)
                return "Surface";
            else if (radioButtonCheckCurve.Checked)
                return "Curve";
            else
                return "";
        }
        #endregion
        #region POST_PROCESSING
        private void open_file_Click(object sender, EventArgs e)
        {
            string open_file_name = "";
            var openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "Postprocessing Files | *.georhino.txt;*.georhino.json";
            DialogResult result = openFileDialog1.ShowDialog();

            // OK button was pressed.
            if (result == DialogResult.OK)
            {
                open_file_name = openFileDialog1.FileName;
                Invalidate();
            }

            // Cancel button was pressed.
            else if (result == DialogResult.Cancel)
            {
                return;
            }

            CocodriloPlugIn.Instance.PostProcessingCocodrilo = new Cocodrilo.PostProcessing.PostProcessing(open_file_name);

            UpdatePostProcessingVariables();

            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGeometry = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGaussPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateCouplingPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;

            Rhino.RhinoApp.WriteLine("Reading results finished!");
        }
        public void SetDefaults()
        {
            this.textBoxDispScale.Text = "1.000e+00";
            this.textBoxResScale.Text = "1.000e+00";
            this.textBoxFlyingNodeLimit.Text = "1.000e+05";
        }
        public void UpdatePostProcessingVariables()
        {
            if (CocodriloPlugIn.Instance.PostProcessingCocodrilo?.ResultList.Count > 0)
            {
                this.comboBoxLoadCaseType.Items.Clear();
                foreach (var result_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseTypes)
                {
                    this.comboBoxLoadCaseType.Items.Add(result_type);
                }

                this.comboBoxResultType.Items.Clear();
                foreach (var result_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.CurrentDistinctResultTypes)
                {
                    this.comboBoxResultType.Items.Add(result_type);
                }
                //int index_result_type = 0;
                //if (CocodriloPlugIn.Instance.PostProcessingCocodrilo.CurrentDistinctResultTypes[0] == "\"DISPLACEMENT\""
                //        || CocodriloPlugIn.Instance.PostProcessingCocodrilo.CurrentDistinctResultTypes[0] == "\"Base Vec of Cross Section\"")
                //    index_result_type++;

                this.domainUpDownAnalysisStep.Items.Clear();
                if (CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers.Count > 0)
                {
                    foreach (var load_case_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers)
                    {
                        this.domainUpDownAnalysisStep.Items.Add(load_case_type);
                    }
                    this.domainUpDownAnalysisStep.SelectedItem = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0];
                }
                else
                {
                    this.domainUpDownAnalysisStep.Items.Add(0.0);
                    this.domainUpDownAnalysisStep.SelectedItem = 0.0;
                }
                if (CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers.Count > 0)
                {
                    this.trackBarAnalysisStep.Minimum = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0];
                    int n_lc = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers.Count;
                    this.trackBarAnalysisStep.Maximum = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[n_lc - 1];
                    this.trackBarAnalysisStep.Value = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0];
                }
                else
                {
                    this.trackBarAnalysisStep.Minimum = 0;
                    this.trackBarAnalysisStep.Maximum = 0;
                    this.trackBarAnalysisStep.Value = 0;
                }

                UpdateMinMax();

                /// Update selected items at end to avoid complications with not filled data bases.
                this.comboBoxLoadCaseType.SelectedItem = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseTypes[0];
                this.comboBoxLoadCaseType.SelectedIndex = 0;

                this.comboBoxResultType.SelectedItem = CocodriloPlugIn.Instance.PostProcessingCocodrilo.CurrentDistinctResultTypes[0];
                this.comboBoxResultType.SelectedIndex = 0;
            }
        }

        private void buttonClearPost_Click(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo?.ClearPostProcessing();

            UpdatePostProcessingVariables();
        }

        private void buttonShowPost_Click(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateCurrentResults(
                comboBoxLoadCaseType.Text, Convert.ToInt32(domainUpDownAnalysisStep.SelectedItem), comboBoxResultType.Text);

            Cocodrilo.PostProcessing.PostProcessing.s_MinMax[0] = Convert.ToDouble(textBoxColorBarMin.Text);
            Cocodrilo.PostProcessing.PostProcessing.s_MinMax[1] = Convert.ToDouble(textBoxColorBarMax.Text);

            Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex = (this.comboBoxPostProcessingDirection.Enabled)
                ? this.comboBoxPostProcessingDirection.SelectedIndex
                : 0;

            CocodriloPlugIn.Instance.PostProcessingCocodrilo.ShowPostProcessing(
                Convert.ToDouble(textBoxDispScale.Text),
                Convert.ToDouble(textBoxFlyingNodeLimit.Text),
                Convert.ToDouble(textBoxResScale.Text),
                checkBoxShowResults.Checked,
                checkBoxShowGaussPoints.Checked,
                checkBoxShowCouplingPoints.Checked,
                checkBoxShowCauchyStresses.Checked,
                checkBoxPK2Stresses.Checked,
                checkBoxPrincipalStresses.Checked,
                checkBoxShowUndeformed.Checked);
        }

        private void comboBoxResultType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (domainUpDownAnalysisStep.Items.Count > 0)
            {
                UpdateComboBoxPostProcessingDirection(
                    CocodriloPlugIn.Instance.PostProcessingCocodrilo.ResultInfo(comboBoxLoadCaseType.Text, Convert.ToInt32(domainUpDownAnalysisStep.SelectedItem), comboBoxResultType.Text));

                CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateCurrentResults(
                    comboBoxLoadCaseType.Text, Convert.ToInt32(domainUpDownAnalysisStep.SelectedItem), comboBoxResultType.Text);
            }
            UpdateMinMax();
        }

        private void comboBoxPostProcessingDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
            Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex = comboBoxPostProcessingDirection.SelectedIndex;

            UpdateMinMax();
        }
        private void UpdateMinMax()
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateCurrentMinMax();
            this.textBoxColorBarMin.Text = CocodriloPlugIn.Instance.PostProcessingCocodrilo.CurrentMinMax[0].ToString("0.0000e+00");
            this.textBoxColorBarMax.Text = CocodriloPlugIn.Instance.PostProcessingCocodrilo.CurrentMinMax[1].ToString("0.0000e+00");
        }
        private void UpdateComboBoxPostProcessingDirection(PostProcessing.RESULT_INFO ThisResultInfo)
        {
            int selected_index = (this.comboBoxPostProcessingDirection.SelectedIndex < 0)
                ? 0
                : this.comboBoxPostProcessingDirection.SelectedIndex;
            this.comboBoxPostProcessingDirection.Items.Clear();
            if (ThisResultInfo.VectorOrScalar == "Vector")
            {
                this.comboBoxPostProcessingDirection.Enabled = true;

                if (ThisResultInfo.NodeOrGauss == "OnNodes" || ThisResultInfo.NodeOrGauss == "\"OnNodes\"")
                {
                    this.comboBoxPostProcessingDirection.Items.Add("X");
                    this.comboBoxPostProcessingDirection.Items.Add("Y");
                    this.comboBoxPostProcessingDirection.Items.Add("Z");
                    this.comboBoxPostProcessingDirection.Items.Add("Length");
                    this.comboBoxPostProcessingDirection.SelectedIndex = selected_index;
                }
                else
                {
                    if (ThisResultInfo.Results.Count > 0)
                    {
                        if (ThisResultInfo.Results[1].GetLength(0) == 3)
                        {
                            this.comboBoxPostProcessingDirection.Items.Add("11");
                            this.comboBoxPostProcessingDirection.Items.Add("22");
                            this.comboBoxPostProcessingDirection.Items.Add("12");
                            this.comboBoxPostProcessingDirection.Items.Add("von Mises");
                            this.comboBoxPostProcessingDirection.SelectedIndex = selected_index;
                        }
                        else
                        {
                            this.comboBoxPostProcessingDirection.Items.Add("11");
                            this.comboBoxPostProcessingDirection.Items.Add("22");
                            this.comboBoxPostProcessingDirection.Items.Add("33");
                            this.comboBoxPostProcessingDirection.Items.Add("12");
                            this.comboBoxPostProcessingDirection.Items.Add("13");
                            this.comboBoxPostProcessingDirection.Items.Add("23");
                            this.comboBoxPostProcessingDirection.Items.Add("von Mises");
                            this.comboBoxPostProcessingDirection.SelectedIndex = selected_index;
                        }
                    }
                }
            }
            else
            {
                this.comboBoxPostProcessingDirection.Items.Add("");
                this.comboBoxPostProcessingDirection.SelectedItem = "";
                this.comboBoxPostProcessingDirection.SelectedIndex = 0;
                this.comboBoxPostProcessingDirection.Enabled = false;
                Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex = 0;
            }

            UpdateMinMax();
        }
        private void UpdateComboBoxResultType()
        {
            bool update_result_type = false;
            foreach (var result_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.CurrentDistinctResultTypes)
            {
                if (!comboBoxResultType.Items.Contains(result_type))
                    update_result_type = true;
            }

            if (update_result_type)
            {
                int result_type_index = this.comboBoxResultType.SelectedIndex;
                this.comboBoxResultType.Items.Clear();
                foreach (var result_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.CurrentDistinctResultTypes)
                {
                    this.comboBoxResultType.Items.Add(result_type);
                }
                comboBoxResultType.SelectedIndex = (result_type_index < comboBoxResultType.Items.Count && result_type_index >= 0)
                        ? result_type_index
                        : 0;
            }
        }

        private void domainUpDownAnalysisStep_SelectedItemChanged(object sender, EventArgs e)
        {
            if (comboBoxResultType.Text == "")
                return;

            // required if change is due to input text
            if (domainUpDownAnalysisStep.SelectedItem == null)
                domainUpDownAnalysisStep.SelectedItem = domainUpDownAnalysisStep.Items.IndexOf(Convert.ToInt32(domainUpDownAnalysisStep.Text));

            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateCurrentResults(
                comboBoxLoadCaseType.Text, Convert.ToInt32(domainUpDownAnalysisStep.SelectedItem), comboBoxResultType.Text);

            UpdateComboBoxResultType();

            if ((int)domainUpDownAnalysisStep.SelectedItem > trackBarAnalysisStep.Maximum)
            {
                trackBarAnalysisStep.Value = trackBarAnalysisStep.Maximum;
                domainUpDownAnalysisStep.SelectedItem = domainUpDownAnalysisStep.Items.IndexOf(trackBarAnalysisStep.Maximum);
            }
            else if ((int)domainUpDownAnalysisStep.SelectedItem < trackBarAnalysisStep.Minimum)
            {
                trackBarAnalysisStep.Value = trackBarAnalysisStep.Minimum;
                domainUpDownAnalysisStep.SelectedItem = domainUpDownAnalysisStep.Items.IndexOf(trackBarAnalysisStep.Minimum);
            }
            else
                trackBarAnalysisStep.Value = (int)domainUpDownAnalysisStep.SelectedItem;

            UpdateMinMax();
        }

        private void comboBoxLoadCaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseTypes.IndexOf((string)this.comboBoxLoadCaseType.SelectedItem);

            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateCurrentResults(
                comboBoxLoadCaseType.Text, Convert.ToInt32(domainUpDownAnalysisStep.SelectedItem), comboBoxResultType.Text);

            UpdateComboBoxResultType();

            this.domainUpDownAnalysisStep.Items.Clear();
            foreach (var load_case_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers)
            {
                this.domainUpDownAnalysisStep.Items.Add(load_case_type);
            }
            if (CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers.Count > 0)
            {
                this.trackBarAnalysisStep.Minimum = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0];
                int n_lc = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers.Count;
                this.trackBarAnalysisStep.Maximum = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[n_lc - 1];
                this.domainUpDownAnalysisStep.SelectedItem = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0];
                this.domainUpDownAnalysisStep.Text = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0].ToString();
                this.trackBarAnalysisStep.Value = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0];
            }
            else
            {
                this.trackBarAnalysisStep.Minimum = 0;
                this.trackBarAnalysisStep.Maximum = 0;
                this.domainUpDownAnalysisStep.SelectedItem = 0;
                this.domainUpDownAnalysisStep.Text = 0.ToString();
                this.trackBarAnalysisStep.Value = 0;
            }

            UpdateComboBoxResultType();
            UpdateMinMax();
        }

        private void textBoxColorBarMin_TextChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        private void textBoxColorBarMax_TextChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        private void checkBoxShowGaussPoints_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGaussPoints = true;
        }

        private void checkBoxShowCouplingPoints_TextChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateCouplingPoints = true;
        }

        private void textBoxFlyingNodeLimit_TextChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGeometry = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGaussPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateCouplingPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        private void textBoxResScale_TextChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        private void textBoxDispScale_TextChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGeometry = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGaussPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateCouplingPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        private void checkBoxShowResults_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        private void trackBarAnalysisStep_Scroll(object sender, EventArgs e)
        {
            if (domainUpDownAnalysisStep.Items.Contains(trackBarAnalysisStep.Value))
                domainUpDownAnalysisStep.SelectedItem = trackBarAnalysisStep.Value;
            else
            {
                double min_dist = 100000;
                int closest_index = -1;
                for (int i = 0; i < domainUpDownAnalysisStep.Items.Count; i++)
                {
                    if (Math.Abs(Convert.ToDouble(domainUpDownAnalysisStep.Items[i]) - trackBarAnalysisStep.Value) < min_dist)
                    {
                        closest_index = i;
                        min_dist = Math.Abs(Convert.ToDouble(domainUpDownAnalysisStep.Items[i]) - trackBarAnalysisStep.Value);
                    }
                }
                domainUpDownAnalysisStep.SelectedItem = domainUpDownAnalysisStep.Items[closest_index];
                trackBarAnalysisStep.Value = (int)domainUpDownAnalysisStep.Items[closest_index];
            }
        }
        #endregion

        private void checkBoxShowKnots_CheckedChanged(object sender, EventArgs e)
        {
            Cocodrilo.PostProcessing.PostProcessing.s_ShowKnotSpanIsoCurves = checkBoxShowKnots.Checked;
        }

        private void checkBoxShowCouplingPoints_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateCouplingPoints = true;
        }

        private void checkBoxShowCauchyStresses_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateStressPatterns = true;
        }

        private void checkBoxPK2Stresses_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateStressPatterns = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateStressPatterns = true;
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void MeshShowPreview(object sender, EventArgs e)
        {

            List<Guid> ids = new List<Guid>();
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.ShowMeshBoundaryPoints(ref ids);

        }

        private void SetMaxDistanceEgdeVertices(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateVisualizationMesh = true;
        }

        private void AutoMinMax(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.RealMinMax();
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
            textBoxColorBarMin.Text = Cocodrilo.PostProcessing.PostProcessing.s_MinMax[0].ToString("0.0000e+00");
            textBoxColorBarMax.Text = Cocodrilo.PostProcessing.PostProcessing.s_MinMax[1].ToString("0.0000e+00");
        }
    }
}