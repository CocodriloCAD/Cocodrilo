using System;
using System.Runtime.InteropServices;
using Eto.Forms;
using Eto.Drawing;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.UI;
using Cocodrilo.Analyses;
using Cocodrilo.Commands;
using Cocodrilo.UserData;
using Cocodrilo.ElementProperties;
using Cocodrilo.Refinement;
using System.Collections.Generic;
using System.Linq;

namespace Cocodrilo.Panels
{
    [Guid("6E2D0EB9-188C-45D3-97F7-20A8BA6D6F42")]
    public class UserControlCocodriloPanel : Panel, IPanel
    {
        public static UserControlCocodriloPanel Instance { get; private set; }

        // Analyses
        TabControl tabControlAnalyses;
        RadioButton radioButtonRunKratos, radioButtonRunCarat;
        DropDown comboBoxAnalyses;
        Button buttonRunAnalysis, buttonDeleteAnalysis, buttonCreateAnalysis, buttonModifyAnalysis, buttonEditOutput;
        TextBox textBoxFormfindingName, textBoxTolerance, textBoxMaxIterations, textBoxMaxSteps;
        TextBox textBoxLinStrucAnalysisName;
        TextBox textBoxNonLinStrucAnalysisName, textBoxNonLinStruAnalysisStepSize, textBoxNonLinStrucAnalysisAdapStepCntrl,
            textBoxNonLinStrucAnalysisNumIter, textBoxNonLinStrucAnalysisNumSteps, textBoxNonLinStrucAnalysisAcc;
        TextBox textBoxTransientAnalysisName, textBoxTransientAnalysisStepSize, textBoxTransientAnalysisAdapStepCntrl,
            textBoxTransientAnalysisNumIter, textBoxTransientAnalysisNumSteps, textBoxTransientAnalysisAcc,
            textBoxTransientAnalysisRayleighAlpha, textBoxTransientAnalysisRayleighBeta,
            textBoxTransientAnalysisDampingRatio0, textBoxTransientAnalysisDampingRatio1, textBoxTransientAnalysisNumEigen;
        DropDown comboBoxTransientAnalysisTimeIntegration, comboBoxTransientAnalysisScheme;
        CheckBox checkBoxTransientAnalysisAutomaticRayleigh;
        TextBox textBoxEigenvalueAnalysisName, textBoxEigenvalueAnalysisAcc, textBoxEigenvalueAnalysisNumEigen, textBoxEigenvalueAnalysisNumIter;
        DropDown comboBoxEigenvalueAnalysisSolverType;
        TextBox textBoxCutPatternAnalysisName, textBoxCutPatternAnalysisTol, textBoxCutPatternAnalysisMaxIter, textBoxCutPatternAnalysisMaxStep;
        CheckBox checkBoxCutPatternAnalysisPrestress;

        // Options
        CheckBox checkBoxShowPreLoads, checkBoxShowPreSupports, checkBoxShowPreCouplings, checkBoxShowPreprocessing;
        Button buttonResetInstance, buttonDeleteAll;

        // Materials
        Button buttonAddModifyMaterial;

        // Element
        DropDown comboBoxElementMat, comboBoxCouplingType;
        Button buttonLoadElement, buttonDeleteElement, buttonAddElement;
        TabControl tabControlElement;
        CheckBox checkBoxElementMembraneFofi, checkBoxElementMembraneEdgeCoupling;
        TextBox textBoxMembranePrestress1, textBoxMembranePrestress2, textBoxMembraneThick;
        DropDown comboBoxShellType;
        TextBox textBoxShellThick;
        Button buttonAddAxis;
        TextBox textBoxBeamIy, textBoxBeamArea, textBoxBeamIz, textBoxBeamIt;
        DropDown comboBoxBeamType;
        Label labelBeamDiameter, labelBeamHeight, labelBeamWidth, labelBeamArea, labelBeamIy, labelBeamIz, labelBeamIt;
        CheckBox checkBoxElementCableFofi, checkBoxCablePrestressCurve;
        TextBox textBoxCablePrestress, textBoxCableArea;
        DropDown comboBoxCableType;

        // Refinement
        RadioButton radioButtonRefinementApproxElementSize, radioButtonRefinementKnotSubdivision;
        RadioButton radioButtonRefinementElementEdge, radioButtonRefinementElementCurve, radioButtonRefinementElementSurf;
        Button buttonCheckRefinement, buttonChangeRefinement;
        TextBox textBoxKnotSubDivV, textBoxKnotSubDivU, textBoxQDeg, textBoxPDeg;

        // Support
        DropDown comboBoxSupportType;
        TextBox textBoxSupportEndTime, textBoxSupportStartTime;
        CheckBox checkBoxOverwriteSupport;
        TextBox textBoxSupportTypeDispZ, textBoxSupportTypeDispY, textBoxSupportTypeDispX;
        CheckBox checkBoxSupportStrong, checkBoxDispZ, checkBoxDispY, checkBoxRotationSupport, checkBoxDispX;
        RadioButton radioButtonSupportDimVertex, radioButtonSupportDimLine, radioButtonSupportDimFace;
        RadioButton radioButtonSupportCurve, radioButtonSupportSurface;
        Button buttonDeleteEdgeSupport, buttonAddEdgeSupports;

        // Load
        TextBox textBoxLoadEndTime, textBoxLoadStartTime;
        CheckBox checkBoxLoadOverwrite;
        TextBox textBoxLoadPositionV, textBoxLoadPositionU, textBoxLoadZ, textBoxLoadY, textBoxLoadX;
        Label labelLoadDirectionZ, labelLoadDirectionY, labelLoadDirectionX, labelLoadPositionV, labelLoadPositionU;
        RadioButton radioButtonLoadDimVertex, radioButtonLoadDimLine, radioButtonLoadDimFace;
        RadioButton radioButtonLoadElementCurve, radioButtonLoadElementSurface;
        DropDown comboBoxLoadType;
        Button buttonDeleteLoad, buttonAddLoad;

        // Check
        TextBox textBoxCheckEndTime, textBoxCheckStartTime;
        CheckBox checkBoxOverwriteChecks;
        CheckBox checkBoxOutputLagrangeMP, checkBoxOutputDispZ, checkBoxOutputDispX, checkBoxOutputDispY;
        RadioButton radioButtonCheckVertex, radioButtonCheckLine, radioButtonCheckFace;
        RadioButton radioButtonCheckCurve, radioButtonCheckSurface;
        Button buttonAddCheck;

        // Post Processing
        Button buttonOpenFile, buttonShowPost, buttonClearPost, buttonAutoMinMax, buttonMeshShowPreview;
        CheckBox checkBoxShowMesh;
        DropDown comboBoxPostProcessingDirection;
        TextBox textBoxColorBarMin, textBoxColorBarMax, textBoxFlyingNodeLimit, textBoxResScale, textBoxDispScale;
        ImageView pictureBoxColorBar;
        CheckBox checkBoxPrincipalStresses, checkBoxPK2Stresses, checkBoxShowCauchyStresses, checkBoxShowResults,
            checkBoxShowKnots, checkBoxShowUndeformed, checkBoxShowGaussPoints, checkBoxShowCouplingPoints, checkBoxCouplingStresses;
        DropDown comboBoxResultType, comboBoxLoadCaseType;
        DropDown domainUpDownAnalysisStep;
        Slider trackBarAnalysisStep;

        public UserControlCocodriloPanel(uint documentSerialNumber)
        {
            Instance = this;
            InitializeComponent();

            CocodriloPlugIn.Instance.materialUpdate += updateMaterialData;
            CocodriloPlugIn.Instance.analysisUpdate += updateAnalysesData;

            comboBoxElementMat.DataStore = CocodriloPlugIn.Instance.Materials;
            comboBoxAnalyses.DataStore = CocodriloPlugIn.Instance.Analyses;
            comboBoxCouplingType.DataStore = Enum.GetValues(typeof(CouplingType)).Cast<object>();
        }

        public void PanelShown(uint documentSerialNumber, ShowPanelReason reason) { }
        public void PanelHidden(uint documentSerialNumber, ShowPanelReason reason) { }
        public void PanelClosing(uint documentSerialNumber, bool onCloseDocument) { }

        void InitializeComponent()
        {
            var outerTabs = new TabControl();
            outerTabs.Pages.Add(new TabPage { Text = "Pre Processing", Content = BuildPreProcessingTab() });
            outerTabs.Pages.Add(new TabPage { Text = "Post Processing", Content = BuildPostProcessingTab() });
            Content = outerTabs;
        }

        Control BuildPreProcessingTab()
        {
            return new StackLayout
            {
                Orientation = Orientation.Vertical,
                Spacing = 6,
                Padding = 6,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Items =
                {
                    BuildAnalysesGroup(),
                    BuildOptionsGroup(),
                    BuildMaterialsGroup(),
                    BuildPropertiesGroup()
                }
            };
        }

        static Control LabeledRow(string label, Control control)
        {
            return new TableRow(new TableCell(new Label { Text = label, VerticalAlignment = VerticalAlignment.Center }, false), new TableCell(control, true));
        }

        GroupBox BuildAnalysesGroup()
        {
            tabControlAnalyses = new TabControl();
            tabControlAnalyses.SelectedIndexChanged += tabControlAnalyses_SelectedIndexChanged;
            tabControlAnalyses.Pages.Add(BuildFormfindingTab());
            tabControlAnalyses.Pages.Add(BuildLinStrucAnalysisTab());
            tabControlAnalyses.Pages.Add(BuildNonLinStrucAnalysisTab());
            tabControlAnalyses.Pages.Add(BuildTransientAnalysisTab());
            tabControlAnalyses.Pages.Add(BuildEigenvalueAnalysisTab());
            tabControlAnalyses.Pages.Add(BuildCutPatternAnalysisTab());

            comboBoxAnalyses = new DropDown { ItemTextBinding = Eto.Forms.Binding.Property<Analysis, string>(a => a.Name) };
            comboBoxAnalyses.SelectedIndexChanged += comboBoxAnalyses_SelectedIndexChanged;

            buttonCreateAnalysis = new Button { Text = "Create Analysis" };
            buttonCreateAnalysis.Click += buttonCreateAnalysis_Click;
            buttonModifyAnalysis = new Button { Text = "Modify Analysis" };
            buttonModifyAnalysis.Click += buttonModifyAnalysis_Click;
            buttonDeleteAnalysis = new Button { Text = "Delete Analysis" };
            buttonDeleteAnalysis.Click += buttonDeleteAnalysis_Click;
            buttonRunAnalysis = new Button { Text = "Run Analysis" };
            buttonRunAnalysis.Click += buttonRunAnalysis_Click;
            buttonEditOutput = new Button { Text = "Edit Output Options" };
            buttonEditOutput.Click += buttonEditOutput_Click;

            radioButtonRunCarat = new RadioButton { Text = "Carat++", Enabled = false };
            radioButtonRunKratos = new RadioButton(radioButtonRunCarat) { Text = "Kratos", Checked = true };
            radioButtonRunCarat.CheckedChanged += radioButtonRunCarat_CheckedChanged;
            radioButtonRunKratos.CheckedChanged += radioButtonRunKratos_CheckedChanged;

            return new GroupBox
            {
                Text = "Analyses",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        new TableRow(tabControlAnalyses) { ScaleHeight = true },
                        new TableRow(comboBoxAnalyses),
                        new TableRow(new StackLayout
                        {
                            Orientation = Orientation.Horizontal,
                            Spacing = 6,
                            Items = { buttonCreateAnalysis, buttonModifyAnalysis, buttonDeleteAnalysis, buttonRunAnalysis }
                        }),
                        new TableRow(new StackLayout
                        {
                            Orientation = Orientation.Horizontal,
                            Spacing = 12,
                            Items = { radioButtonRunCarat, radioButtonRunKratos, buttonEditOutput }
                        })
                    }
                }
            };
        }

        TabPage BuildFormfindingTab()
        {
            textBoxFormfindingName = new TextBox();
            textBoxFormfindingName.TextInput += textBoxFormfindingName_KeyPress;
            textBoxMaxSteps = new TextBox { Text = "10" };
            textBoxMaxSteps.TextInput += textBoxMaxSteps_KeyPress;
            textBoxMaxIterations = new TextBox { Text = "1" };
            textBoxMaxIterations.TextInput += textBoxMaxIterations_KeyPress;
            textBoxTolerance = new TextBox { Text = "0.001" };
            textBoxTolerance.TextInput += textBoxTolerance_KeyPress;

            return new TabPage
            {
                Text = "Formfinding",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        LabeledRow("Name:", textBoxFormfindingName),
                        LabeledRow("Max. Steps:", textBoxMaxSteps),
                        LabeledRow("Max. Iterations:", textBoxMaxIterations),
                        LabeledRow("Tolerance:", textBoxTolerance),
                        null
                    }
                }
            };
        }

        TabPage BuildLinStrucAnalysisTab()
        {
            textBoxLinStrucAnalysisName = new TextBox();
            return new TabPage
            {
                Text = "LinStrucAnalysis",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows = { LabeledRow("Name:", textBoxLinStrucAnalysisName), null }
                }
            };
        }

        TabPage BuildNonLinStrucAnalysisTab()
        {
            textBoxNonLinStrucAnalysisName = new TextBox();
            textBoxNonLinStrucAnalysisNumSteps = new TextBox { Text = "1" };
            textBoxNonLinStruAnalysisStepSize = new TextBox { Text = "0.1" };
            textBoxNonLinStrucAnalysisNumIter = new TextBox { Text = "100" };
            textBoxNonLinStrucAnalysisAcc = new TextBox { Text = "0.001" };
            textBoxNonLinStrucAnalysisAdapStepCntrl = new TextBox { Text = "0" };

            return new TabPage
            {
                Text = "NonLinStrucAnalysis",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        LabeledRow("Name:", textBoxNonLinStrucAnalysisName),
                        LabeledRow("Num. Steps:", textBoxNonLinStrucAnalysisNumSteps),
                        LabeledRow("Step Size:", textBoxNonLinStruAnalysisStepSize),
                        LabeledRow("Max. Iterations:", textBoxNonLinStrucAnalysisNumIter),
                        LabeledRow("Tolerance:", textBoxNonLinStrucAnalysisAcc),
                        LabeledRow("Adaptive Steps:", textBoxNonLinStrucAnalysisAdapStepCntrl),
                        null
                    }
                }
            };
        }

        TabPage BuildTransientAnalysisTab()
        {
            textBoxTransientAnalysisName = new TextBox();
            textBoxTransientAnalysisNumSteps = new TextBox { Text = "1" };
            textBoxTransientAnalysisStepSize = new TextBox { Text = "0.1" };
            textBoxTransientAnalysisNumIter = new TextBox { Text = "100" };
            textBoxTransientAnalysisAcc = new TextBox { Text = "0.001" };
            textBoxTransientAnalysisAdapStepCntrl = new TextBox { Text = "0" };
            comboBoxTransientAnalysisTimeIntegration = new DropDown();
            comboBoxTransientAnalysisTimeIntegration.Items.Add("implicit");
            comboBoxTransientAnalysisTimeIntegration.Items.Add("explicit");
            comboBoxTransientAnalysisTimeIntegration.SelectedKey = "implicit";
            comboBoxTransientAnalysisScheme = new DropDown();
            comboBoxTransientAnalysisScheme.Items.Add("newmark");
            comboBoxTransientAnalysisScheme.Items.Add("bossak");
            comboBoxTransientAnalysisScheme.Items.Add("bdf2");
            comboBoxTransientAnalysisScheme.SelectedKey = "newmark";
            checkBoxTransientAnalysisAutomaticRayleigh = new CheckBox { Text = "Automatic Rayleigh Parameters" };
            textBoxTransientAnalysisRayleighAlpha = new TextBox { Text = "1.0" };
            textBoxTransientAnalysisRayleighBeta = new TextBox { Text = "1.0" };
            textBoxTransientAnalysisDampingRatio0 = new TextBox { Text = "0.001" };
            textBoxTransientAnalysisDampingRatio1 = new TextBox { Text = "-1.0" };
            textBoxTransientAnalysisNumEigen = new TextBox { Text = "15" };

            return new TabPage
            {
                Text = "TransientAnalysis",
                Content = new Scrollable
                {
                    Content = new TableLayout
                    {
                        Padding = 6,
                        Spacing = new Size(6, 6),
                        Rows =
                        {
                            LabeledRow("Name:", textBoxTransientAnalysisName),
                            LabeledRow("Num. Steps:", textBoxTransientAnalysisNumSteps),
                            LabeledRow("Step Size:", textBoxTransientAnalysisStepSize),
                            LabeledRow("Max. Iterations:", textBoxTransientAnalysisNumIter),
                            LabeledRow("Tolerance:", textBoxTransientAnalysisAcc),
                            LabeledRow("Time integ.:", comboBoxTransientAnalysisTimeIntegration),
                            LabeledRow("Scheme:", comboBoxTransientAnalysisScheme),
                            LabeledRow("Adaptive Steps:", textBoxTransientAnalysisAdapStepCntrl),
                            new TableRow(checkBoxTransientAnalysisAutomaticRayleigh),
                            LabeledRow("Rayleigh alpha:", textBoxTransientAnalysisRayleighAlpha),
                            LabeledRow("Rayleigh beta:", textBoxTransientAnalysisRayleighBeta),
                            LabeledRow("Damping val 0:", textBoxTransientAnalysisDampingRatio0),
                            LabeledRow("Damping val 1:", textBoxTransientAnalysisDampingRatio1),
                            LabeledRow("Num. Eigen:", textBoxTransientAnalysisNumEigen),
                            null
                        }
                    }
                }
            };
        }

        TabPage BuildEigenvalueAnalysisTab()
        {
            textBoxEigenvalueAnalysisName = new TextBox();
            textBoxEigenvalueAnalysisNumEigen = new TextBox { Text = "1" };
            textBoxEigenvalueAnalysisNumIter = new TextBox { Text = "100" };
            textBoxEigenvalueAnalysisAcc = new TextBox { Text = "0.001" };
            comboBoxEigenvalueAnalysisSolverType = new DropDown();
            comboBoxEigenvalueAnalysisSolverType.Items.Add("eigen_eigensystem");
            comboBoxEigenvalueAnalysisSolverType.Items.Add("spectra_sym_g_eigs_shift");
            comboBoxEigenvalueAnalysisSolverType.Items.Add("feast");
            comboBoxEigenvalueAnalysisSolverType.SelectedKey = "eigen_eigensystem";

            return new TabPage
            {
                Text = "EigenvalueAnalysis",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        LabeledRow("Name:", textBoxEigenvalueAnalysisName),
                        LabeledRow("Num. Eigen:", textBoxEigenvalueAnalysisNumEigen),
                        LabeledRow("Max. Iterations:", textBoxEigenvalueAnalysisNumIter),
                        LabeledRow("Tolerance:", textBoxEigenvalueAnalysisAcc),
                        LabeledRow("Solver Type:", comboBoxEigenvalueAnalysisSolverType),
                        null
                    }
                }
            };
        }

        TabPage BuildCutPatternAnalysisTab()
        {
            textBoxCutPatternAnalysisName = new TextBox();
            textBoxCutPatternAnalysisMaxStep = new TextBox { Text = "10" };
            textBoxCutPatternAnalysisMaxIter = new TextBox { Text = "10" };
            textBoxCutPatternAnalysisTol = new TextBox { Text = "0.001" };
            checkBoxCutPatternAnalysisPrestress = new CheckBox { Text = "Consider Prestress" };

            return new TabPage
            {
                Text = "CutPattern",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        LabeledRow("Name:", textBoxCutPatternAnalysisName),
                        LabeledRow("Max. Steps:", textBoxCutPatternAnalysisMaxStep),
                        LabeledRow("Max. Iterations:", textBoxCutPatternAnalysisMaxIter),
                        LabeledRow("Tolerance:", textBoxCutPatternAnalysisTol),
                        new TableRow(checkBoxCutPatternAnalysisPrestress),
                        null
                    }
                }
            };
        }

        GroupBox BuildOptionsGroup()
        {
            checkBoxShowPreprocessing = new CheckBox { Text = "Show Elements" };
            checkBoxShowPreprocessing.CheckedChanged += checkBoxShowElements_CheckedChanged;
            checkBoxShowPreSupports = new CheckBox { Text = "Show Supports" };
            checkBoxShowPreSupports.CheckedChanged += checkBoxShowPreSupports_CheckedChanged;
            checkBoxShowPreLoads = new CheckBox { Text = "Show Loads" };
            checkBoxShowPreLoads.CheckedChanged += checkBoxShowPreLoads_CheckedChanged;
            checkBoxShowPreCouplings = new CheckBox { Text = "Show Couplings" };
            checkBoxShowPreCouplings.CheckedChanged += checkBoxShowPreCouplings_CheckedChanged;
            buttonResetInstance = new Button { Text = "Reset Instance" };
            buttonResetInstance.Click += buttonResetInstance_Click;
            buttonDeleteAll = new Button { Text = "Delete Entire User Data" };
            buttonDeleteAll.Click += buttonDeleteAll_Click;

            return new GroupBox
            {
                Text = "Options",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 12,
                    Padding = 6,
                    Items =
                    {
                        checkBoxShowPreprocessing, checkBoxShowPreSupports, checkBoxShowPreLoads, checkBoxShowPreCouplings,
                        buttonResetInstance, buttonDeleteAll
                    }
                }
            };
        }

        GroupBox BuildMaterialsGroup()
        {
            buttonAddModifyMaterial = new Button { Text = "Add/ Modify Materials" };
            buttonAddModifyMaterial.Click += buttonAddModifyMaterial_Click;
            return new GroupBox { Text = "Materials", Content = buttonAddModifyMaterial };
        }

        GroupBox BuildPropertiesGroup()
        {
            var tabControlProperties = new TabControl();
            tabControlProperties.Pages.Add(BuildElementTab());
            tabControlProperties.Pages.Add(BuildRefinementTab());
            tabControlProperties.Pages.Add(BuildSupportTab());
            tabControlProperties.Pages.Add(BuildLoadTab());
            tabControlProperties.Pages.Add(BuildCheckTab());

            return new GroupBox { Text = "Properties", Content = tabControlProperties };
        }

        void comboBoxCouplingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.GlobalCouplingMethod = (CouplingType)comboBoxCouplingType.SelectedValue;
        }

        #region Element (layout)
        TabPage BuildElementTab()
        {
            buttonLoadElement = new Button { Text = "Load Element" };
            buttonLoadElement.Click += buttonLoadElement_Click;

            comboBoxElementMat = new DropDown { ItemTextBinding = Eto.Forms.Binding.Property<Materials.Material, string>(m => m.Id.ToString()) };
            comboBoxCouplingType = new DropDown { ItemTextBinding = Eto.Forms.Binding.Property<object, string>(o => o.ToString()) };
            comboBoxCouplingType.SelectedIndexChanged += comboBoxCouplingType_SelectedIndexChanged;

            buttonAddElement = new Button { Text = "Add Element" };
            buttonAddElement.Click += buttonAddElement_Click;
            buttonDeleteElement = new Button { Text = "Delete Element" };
            buttonDeleteElement.Click += buttonDeleteElement_Click;

            tabControlElement = new TabControl();
            tabControlElement.Pages.Add(BuildMembraneTab());
            tabControlElement.Pages.Add(BuildShellTab());
            tabControlElement.Pages.Add(BuildBeamTab());
            tabControlElement.Pages.Add(BuildCableTab());

            return new TabPage
            {
                Text = "Element",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        new TableRow(buttonLoadElement),
                        new TableRow(tabControlElement) { ScaleHeight = true },
                        LabeledRow("Material", comboBoxElementMat),
                        LabeledRow("Coupling", comboBoxCouplingType),
                        new TableRow(new StackLayout
                        {
                            Orientation = Orientation.Horizontal,
                            Spacing = 6,
                            Items = { buttonAddElement, buttonDeleteElement }
                        })
                    }
                }
            };
        }

        TabPage BuildMembraneTab()
        {
            checkBoxElementMembraneFofi = new CheckBox { Text = "Formfinding", Checked = true };
            checkBoxElementMembraneEdgeCoupling = new CheckBox { Text = "Edge Coupling" };
            textBoxMembraneThick = new TextBox { Text = "1.0" };
            textBoxMembranePrestress1 = new TextBox { Text = "1.0" };
            textBoxMembranePrestress2 = new TextBox { Text = "1.0" };

            return new TabPage
            {
                Text = "Membrane",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        LabeledRow("Thickness", textBoxMembraneThick),
                        LabeledRow("Prestress 1", textBoxMembranePrestress1),
                        LabeledRow("Prestress 2", textBoxMembranePrestress2),
                        new TableRow(checkBoxElementMembraneFofi),
                        new TableRow(checkBoxElementMembraneEdgeCoupling),
                        null
                    }
                }
            };
        }

        TabPage BuildShellTab()
        {
            comboBoxShellType = new DropDown();
            comboBoxShellType.Items.Add("Shell3pElement");
            comboBoxShellType.Items.Add("Shell5pElement");
            comboBoxShellType.Items.Add("Shell5pHierarchicElement");
            comboBoxShellType.SelectedKey = "Shell3pElement";
            textBoxShellThick = new TextBox { Text = "1.0" };

            return new TabPage
            {
                Text = "Shell",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        LabeledRow("Shell Type", comboBoxShellType),
                        LabeledRow("Thickness", textBoxShellThick),
                        null
                    }
                }
            };
        }

        TabPage BuildBeamTab()
        {
            comboBoxBeamType = new DropDown();
            comboBoxBeamType.Items.Add("Circular");
            comboBoxBeamType.Items.Add("Rectangular");
            comboBoxBeamType.Items.Add("Undefined");
            comboBoxBeamType.SelectedIndexChanged += comboBoxBeamType_SelectedIndexChanged;

            buttonAddAxis = new Button { Text = "Add Axis" };
            buttonAddAxis.Click += buttonAddAxis_Click;

            labelBeamDiameter = new Label { Text = "Diameter", Visible = false };
            labelBeamHeight = new Label { Text = "Height", Visible = false };
            labelBeamWidth = new Label { Text = "Width", Visible = false };
            labelBeamArea = new Label { Text = "Area", Visible = false };
            labelBeamIy = new Label { Text = "Iy", Visible = false };
            labelBeamIz = new Label { Text = "Iz", Visible = false };
            labelBeamIt = new Label { Text = "It", Visible = false };
            textBoxBeamArea = new TextBox { Text = "0.01", Visible = false };
            textBoxBeamIy = new TextBox { Text = "8.3333e-6", Visible = false };
            textBoxBeamIz = new TextBox { Text = "8.3333e-6", Visible = false };
            textBoxBeamIt = new TextBox { Text = "3.3333e-5", Visible = false };

            return new TabPage
            {
                Text = "Beam",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        LabeledRow("Cross section", comboBoxBeamType),
                        new TableRow(labelBeamDiameter),
                        new TableRow(labelBeamWidth),
                        new TableRow(labelBeamHeight),
                        LabeledRow("Area", textBoxBeamArea),
                        LabeledRow("Iy", textBoxBeamIy),
                        LabeledRow("Iz", textBoxBeamIz),
                        LabeledRow("It", textBoxBeamIt),
                        new TableRow(buttonAddAxis),
                        null
                    }
                }
            };
        }

        TabPage BuildCableTab()
        {
            comboBoxCableType = new DropDown();
            comboBoxCableType.Items.Add("Curve");
            comboBoxCableType.Items.Add("Edge");
            comboBoxCableType.SelectedIndexChanged += comboBoxCableType_SelectedIndex_Changed;
            checkBoxElementCableFofi = new CheckBox { Text = "Formfinding", Checked = true };
            checkBoxCablePrestressCurve = new CheckBox { Text = "Load Curve for Prestress" };
            textBoxCablePrestress = new TextBox { Text = "1.0" };
            textBoxCableArea = new TextBox { Text = "1.0" };

            return new TabPage
            {
                Text = "Cable",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        LabeledRow("Type", comboBoxCableType),
                        LabeledRow("Area", textBoxCableArea),
                        LabeledRow("Prestress", textBoxCablePrestress),
                        new TableRow(checkBoxElementCableFofi),
                        new TableRow(checkBoxCablePrestressCurve),
                        null
                    }
                }
            };
        }
        #endregion

        #region Refinement (layout)
        TabPage BuildRefinementTab()
        {
            radioButtonRefinementElementSurf = new RadioButton { Text = "Surface", Checked = true };
            radioButtonRefinementElementCurve = new RadioButton(radioButtonRefinementElementSurf) { Text = "Curve" };
            radioButtonRefinementElementEdge = new RadioButton(radioButtonRefinementElementSurf) { Text = "Edge" };
            radioButtonRefinementElementSurf.CheckedChanged += radioButtonRefinementSurface_CheckedChanged;
            radioButtonRefinementElementCurve.CheckedChanged += radioButtonRefinementCurve_CheckedChanged;
            radioButtonRefinementElementEdge.CheckedChanged += radioButtonRefinementEdge_CheckedChanged;
            var groupBoxRefinementElement = new GroupBox
            {
                Text = "Structural Element",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 12,
                    Padding = 6,
                    Items = { radioButtonRefinementElementSurf, radioButtonRefinementElementCurve, radioButtonRefinementElementEdge }
                }
            };

            radioButtonRefinementKnotSubdivision = new RadioButton { Text = "Knot Subdivision", Checked = true };
            radioButtonRefinementApproxElementSize = new RadioButton(radioButtonRefinementKnotSubdivision) { Text = "Approx. Element Size" };
            radioButtonRefinementKnotSubdivision.CheckedChanged += radioButtonRefinementKnotSubdivision_CheckedChanged;
            radioButtonRefinementApproxElementSize.CheckedChanged += radioButtonKnotSubdivision_CheckedChanged;

            textBoxPDeg = new TextBox { Text = "3" };
            textBoxQDeg = new TextBox { Text = "3" };
            textBoxKnotSubDivU = new TextBox { Text = "4" };
            textBoxKnotSubDivV = new TextBox { Text = "4" };

            buttonCheckRefinement = new Button { Text = "Check Refinement" };
            buttonCheckRefinement.Click += buttonCheckRefinement_Click;
            buttonChangeRefinement = new Button { Text = "Change Refinement" };
            buttonChangeRefinement.Click += buttonChangeRefinement_Click;

            return new TabPage
            {
                Text = "Refinement",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        new TableRow(groupBoxRefinementElement),
                        new TableRow(new StackLayout
                        {
                            Orientation = Orientation.Horizontal,
                            Spacing = 12,
                            Items = { radioButtonRefinementKnotSubdivision, radioButtonRefinementApproxElementSize }
                        }),
                        LabeledRow("P Deg:", textBoxPDeg),
                        LabeledRow("Q Deg:", textBoxQDeg),
                        LabeledRow("Dir U:", textBoxKnotSubDivU),
                        LabeledRow("Dir V:", textBoxKnotSubDivV),
                        new TableRow(new StackLayout
                        {
                            Orientation = Orientation.Horizontal,
                            Spacing = 6,
                            Items = { buttonCheckRefinement, buttonChangeRefinement }
                        }),
                        null
                    }
                }
            };
        }
        #endregion

        #region Support (layout)
        TabPage BuildSupportTab()
        {
            comboBoxSupportType = new DropDown();
            foreach (var item in new[] { "SupportPenaltyCondition", "SupportLagrangeCondition", "SupportNitscheCondition", "DirectorInc5pShellSupport" })
                comboBoxSupportType.Items.Add(item);
            comboBoxSupportType.SelectedKey = "SupportPenaltyCondition";
            var groupBox4 = new GroupBox { Text = "Support Type", Content = comboBoxSupportType };

            textBoxSupportStartTime = new TextBox { Text = "0.0" };
            textBoxSupportEndTime = new TextBox { Text = "End" };
            var groupBoxInterval = new GroupBox
            {
                Text = "Interval: Start Time - End Time",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 6,
                    Padding = 6,
                    Items = { textBoxSupportStartTime, textBoxSupportEndTime }
                }
            };
            checkBoxOverwriteSupport = new CheckBox { Text = "Overwrite", Checked = true };

            checkBoxDispX = new CheckBox { Text = "Disp X", Checked = true };
            checkBoxDispY = new CheckBox { Text = "Disp Y", Checked = true };
            checkBoxDispZ = new CheckBox { Text = "Disp Z", Checked = true };
            checkBoxRotationSupport = new CheckBox { Text = "Rotation" };
            checkBoxSupportStrong = new CheckBox { Text = "Strong" };
            textBoxSupportTypeDispX = new TextBox { Text = "0.0" };
            textBoxSupportTypeDispY = new TextBox { Text = "0.0" };
            textBoxSupportTypeDispZ = new TextBox { Text = "0.0" };
            var groupBoxSupportType = new GroupBox
            {
                Text = "Support Direction",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        new TableRow(checkBoxDispX, textBoxSupportTypeDispX),
                        new TableRow(checkBoxDispY, textBoxSupportTypeDispY),
                        new TableRow(checkBoxDispZ, textBoxSupportTypeDispZ),
                        new TableRow(checkBoxRotationSupport, checkBoxSupportStrong)
                    }
                }
            };

            radioButtonSupportDimFace = new RadioButton { Text = "Face" };
            radioButtonSupportDimLine = new RadioButton(radioButtonSupportDimFace) { Text = "Line" };
            radioButtonSupportDimVertex = new RadioButton(radioButtonSupportDimFace) { Text = "Vertex" };
            var groupBoxSupportDimension = new GroupBox
            {
                Text = "Support Dimension",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 12,
                    Padding = 6,
                    Items = { radioButtonSupportDimFace, radioButtonSupportDimLine, radioButtonSupportDimVertex }
                }
            };

            radioButtonSupportSurface = new RadioButton { Text = "Surface" };
            radioButtonSupportCurve = new RadioButton(radioButtonSupportSurface) { Text = "Curve" };
            radioButtonSupportSurface.CheckedChanged += radioButtonSupportSurface_CheckedChanged;
            radioButtonSupportCurve.CheckedChanged += radioButtonSupportCurve_CheckedChanged;
            var groupBoxSupportElementType = new GroupBox
            {
                Text = "Structural Element",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 12,
                    Padding = 6,
                    Items = { radioButtonSupportSurface, radioButtonSupportCurve }
                }
            };

            buttonAddEdgeSupports = new Button { Text = "Add Supports" };
            buttonAddEdgeSupports.Click += buttonAddEdgeSupports_Click;
            buttonDeleteEdgeSupport = new Button { Text = "Delete Support" };
            buttonDeleteEdgeSupport.Click += buttonDeleteEdgeSupport_Click;

            return new TabPage
            {
                Text = "Support",
                Content = new Scrollable
                {
                    Content = new TableLayout
                    {
                        Padding = 6,
                        Spacing = new Size(6, 6),
                        Rows =
                        {
                            new TableRow(groupBox4),
                            new TableRow(groupBoxInterval),
                            new TableRow(checkBoxOverwriteSupport),
                            new TableRow(groupBoxSupportType),
                            new TableRow(groupBoxSupportDimension),
                            new TableRow(groupBoxSupportElementType),
                            new TableRow(new StackLayout
                            {
                                Orientation = Orientation.Horizontal,
                                Spacing = 6,
                                Items = { buttonAddEdgeSupports, buttonDeleteEdgeSupport }
                            }),
                            null
                        }
                    }
                }
            };
        }
        #endregion

        #region Load (layout)
        TabPage BuildLoadTab()
        {
            textBoxLoadStartTime = new TextBox { Text = "0.0" };
            textBoxLoadEndTime = new TextBox { Text = "End" };
            var groupBoxLoadInterval = new GroupBox
            {
                Text = "Interval: Start Time - End Time",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 6,
                    Padding = 6,
                    Items = { textBoxLoadStartTime, textBoxLoadEndTime }
                }
            };
            checkBoxLoadOverwrite = new CheckBox { Text = "Overwrite", Checked = true };

            labelLoadDirectionX = new Label { Text = "X" };
            labelLoadDirectionY = new Label { Text = "Y" };
            labelLoadDirectionZ = new Label { Text = "Z" };
            textBoxLoadX = new TextBox { Text = "0.0" };
            textBoxLoadY = new TextBox { Text = "0.0" };
            textBoxLoadZ = new TextBox { Text = "1.0" };

            labelLoadPositionU = new Label { Text = "U = " };
            labelLoadPositionV = new Label { Text = "V = " };
            textBoxLoadPositionU = new TextBox();
            textBoxLoadPositionV = new TextBox();

            radioButtonLoadDimFace = new RadioButton { Text = "Face" };
            radioButtonLoadDimLine = new RadioButton(radioButtonLoadDimFace) { Text = "Line" };
            radioButtonLoadDimVertex = new RadioButton(radioButtonLoadDimFace) { Text = "Vertex" };
            radioButtonLoadDimFace.CheckedChanged += radioButtonLoadDimFace_CheckedChanged;
            radioButtonLoadDimLine.CheckedChanged += radioButtonLoadDimLine_CheckedChanged;
            radioButtonLoadDimVertex.CheckedChanged += radioButtonLoadDimVertex_CheckedChanged;
            var groupBoxLoadDimension = new GroupBox
            {
                Text = "Load Dimension",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 12,
                    Padding = 6,
                    Items = { radioButtonLoadDimFace, radioButtonLoadDimLine, radioButtonLoadDimVertex }
                }
            };

            radioButtonLoadElementSurface = new RadioButton { Text = "Surface" };
            radioButtonLoadElementCurve = new RadioButton(radioButtonLoadElementSurface) { Text = "Curve" };
            radioButtonLoadElementSurface.CheckedChanged += radioButtonLoadSurface_CheckedChanged;
            radioButtonLoadElementCurve.CheckedChanged += radioButtonLoadCurve_CheckedChanged;
            var groupBoxLoadElement = new GroupBox
            {
                Text = "Structural Element",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 12,
                    Padding = 6,
                    Items = { radioButtonLoadElementSurface, radioButtonLoadElementCurve }
                }
            };

            comboBoxLoadType = new DropDown();
            foreach (var item in new[] { "DEAD", "PRES", "PRES_FL", "SNOW", "MOMENT", "MOMENT_5P_DIRECTOR" })
                comboBoxLoadType.Items.Add(item);
            comboBoxLoadType.SelectedKey = "DEAD";
            comboBoxLoadType.SelectedIndexChanged += comboBoxLoadType_SelectedIndexChanged;

            buttonAddLoad = new Button { Text = "Add Load" };
            buttonAddLoad.Click += buttonAddLoad_Click;
            buttonDeleteLoad = new Button { Text = "Delete Load" };
            buttonDeleteLoad.Click += buttonDeleteLoad_Click;

            return new TabPage
            {
                Text = "Load",
                Content = new Scrollable
                {
                    Content = new TableLayout
                    {
                        Padding = 6,
                        Spacing = new Size(6, 6),
                        Rows =
                        {
                            new TableRow(groupBoxLoadInterval),
                            new TableRow(checkBoxLoadOverwrite),
                            new TableRow(groupBoxLoadDimension),
                            new TableRow(groupBoxLoadElement),
                            LabeledRow("Type:", comboBoxLoadType),
                            new TableRow(new Label { Text = "Load:" },
                                new TableLayout(new TableRow(labelLoadDirectionX, textBoxLoadX, labelLoadDirectionY, textBoxLoadY, labelLoadDirectionZ, textBoxLoadZ))),
                            new TableRow(new Label { Text = "Position:" },
                                new TableLayout(new TableRow(labelLoadPositionU, textBoxLoadPositionU, labelLoadPositionV, textBoxLoadPositionV))),
                            new TableRow(new StackLayout
                            {
                                Orientation = Orientation.Horizontal,
                                Spacing = 6,
                                Items = { buttonAddLoad, buttonDeleteLoad }
                            }),
                            null
                        }
                    }
                }
            };
        }
        #endregion

        #region Check (layout)
        TabPage BuildCheckTab()
        {
            textBoxCheckStartTime = new TextBox { Text = "0.0" };
            textBoxCheckEndTime = new TextBox { Text = "End" };
            var groupBox1 = new GroupBox
            {
                Text = "Interval: Start Time - End Time",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 6,
                    Padding = 6,
                    Items = { textBoxCheckStartTime, textBoxCheckEndTime }
                }
            };
            checkBoxOverwriteChecks = new CheckBox { Text = "Overwrite", Checked = true };

            checkBoxOutputDispX = new CheckBox { Text = "DISP_X" };
            checkBoxOutputDispY = new CheckBox { Text = "DISP_Y" };
            checkBoxOutputDispZ = new CheckBox { Text = "DISP_Z" };
            checkBoxOutputLagrangeMP = new CheckBox { Text = "LAGRANGE_MP" };
            var groupBox2 = new GroupBox
            {
                Text = "Check Type",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 12,
                    Padding = 6,
                    Items = { checkBoxOutputDispX, checkBoxOutputDispY, checkBoxOutputDispZ, checkBoxOutputLagrangeMP }
                }
            };

            radioButtonCheckFace = new RadioButton { Text = "Face" };
            radioButtonCheckLine = new RadioButton(radioButtonCheckFace) { Text = "Line" };
            radioButtonCheckVertex = new RadioButton(radioButtonCheckFace) { Text = "Vertex" };
            var groupBox3 = new GroupBox
            {
                Text = "Dimension",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 12,
                    Padding = 6,
                    Items = { radioButtonCheckFace, radioButtonCheckLine, radioButtonCheckVertex }
                }
            };

            radioButtonCheckSurface = new RadioButton { Text = "Surface" };
            radioButtonCheckCurve = new RadioButton(radioButtonCheckSurface) { Text = "Curve" };
            var groupBoxCheckStructuralElement = new GroupBox
            {
                Text = "Structural Element",
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 12,
                    Padding = 6,
                    Items = { radioButtonCheckSurface, radioButtonCheckCurve }
                }
            };

            buttonAddCheck = new Button { Text = "Add Checks" };
            buttonAddCheck.Click += buttonAddCheck_Click;

            return new TabPage
            {
                Text = "Output",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        new TableRow(groupBox1),
                        new TableRow(checkBoxOverwriteChecks),
                        new TableRow(groupBox2),
                        new TableRow(groupBox3),
                        new TableRow(groupBoxCheckStructuralElement),
                        new TableRow(buttonAddCheck),
                        null
                    }
                }
            };
        }
        #endregion

        static Bitmap ToEtoBitmap(System.Drawing.Bitmap bmp)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;
                return new Bitmap(stream);
            }
        }

        Control BuildPostProcessingTab()
        {
            buttonOpenFile = new Button { Text = "Open File" };
            buttonOpenFile.Click += open_file_Click;
            buttonShowPost = new Button { Text = "Show" };
            buttonShowPost.Click += buttonShowPost_Click;
            buttonClearPost = new Button { Text = "Clear" };
            buttonClearPost.Click += buttonClearPost_Click;

            buttonAutoMinMax = new Button { Text = "Auto Min/Max" };
            buttonAutoMinMax.Click += AutoMinMax;
            textBoxColorBarMin = new TextBox { Text = "0" };
            textBoxColorBarMin.TextChanged += textBoxColorBarMin_TextChanged;
            textBoxColorBarMax = new TextBox { Text = "1" };
            textBoxColorBarMax.TextChanged += textBoxColorBarMax_TextChanged;
            comboBoxPostProcessingDirection = new DropDown();
            comboBoxPostProcessingDirection.SelectedIndexChanged += comboBoxPostProcessingDirection_SelectedIndexChanged;
            pictureBoxColorBar = new ImageView();
            try { pictureBoxColorBar.Image = ToEtoBitmap(Cocodrilo.Properties.Resources.color_bar); } catch { }

            checkBoxShowMesh = new CheckBox { Text = "Show Mesh" };
            checkBoxShowMesh.CheckedChanged += checkBoxShowMesh_CheckedChanged;
            buttonMeshShowPreview = new Button { Text = "Refinement" };
            buttonMeshShowPreview.Click += MeshShowPreview;
            var groupBox5 = new GroupBox
            {
                Text = "Visualization Mesh",
                Content = new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, Padding = 6, Items = { checkBoxShowMesh, buttonMeshShowPreview } }
            };

            var groupBoxVisualization = new GroupBox
            {
                Text = "Visualization",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        new TableRow(groupBox5),
                        LabeledRow("Direction:", comboBoxPostProcessingDirection),
                        LabeledRow("Color Min:", textBoxColorBarMin),
                        LabeledRow("Color Max:", textBoxColorBarMax),
                        new TableRow(buttonAutoMinMax),
                        new TableRow(pictureBoxColorBar)
                    }
                }
            };

            textBoxDispScale = new TextBox { Text = "1.000e+00" };
            textBoxDispScale.TextChanged += textBoxDispScale_TextChanged;
            textBoxResScale = new TextBox { Text = "1.000e+00" };
            textBoxResScale.TextChanged += textBoxResScale_TextChanged;
            textBoxFlyingNodeLimit = new TextBox { Text = "1.000e+05" };
            textBoxFlyingNodeLimit.TextChanged += textBoxFlyingNodeLimit_TextChanged;
            var groupBoxScalings = new GroupBox
            {
                Text = "Scalings",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        LabeledRow("Displacement Scaling:", textBoxDispScale),
                        LabeledRow("Result Scaling: ", textBoxResScale),
                        LabeledRow("Flying Node Limit:", textBoxFlyingNodeLimit)
                    }
                }
            };

            checkBoxShowResults = new CheckBox { Text = "Results", Checked = true };
            checkBoxShowResults.CheckedChanged += checkBoxShowResults_CheckedChanged;
            checkBoxShowGaussPoints = new CheckBox { Text = "Gauss Points" };
            checkBoxShowGaussPoints.CheckedChanged += checkBoxShowGaussPoints_CheckedChanged;
            checkBoxShowCouplingPoints = new CheckBox { Text = "Coupling Points" };
            checkBoxShowCouplingPoints.CheckedChanged += checkBoxShowCouplingPoints_CheckedChanged;
            checkBoxShowCauchyStresses = new CheckBox { Text = "Cauchy Stresses" };
            checkBoxShowCauchyStresses.CheckedChanged += checkBoxShowCauchyStresses_CheckedChanged;
            checkBoxPK2Stresses = new CheckBox { Text = "PK2 Stresses" };
            checkBoxPK2Stresses.CheckedChanged += checkBoxPK2Stresses_CheckedChanged;
            checkBoxPrincipalStresses = new CheckBox { Text = "Principal Stresses" };
            checkBoxPrincipalStresses.CheckedChanged += checkBoxPrincipalStresses_CheckedChanged;
            checkBoxShowUndeformed = new CheckBox { Text = "Undeformed" };
            checkBoxShowUndeformed.CheckedChanged += checkBoxShowUndeformed_CheckedChanged;
            checkBoxShowKnots = new CheckBox { Text = "Knots" };
            checkBoxShowKnots.CheckedChanged += checkBoxShowKnots_CheckedChanged;
            checkBoxCouplingStresses = new CheckBox { Text = "Coupling Stresses" };
            checkBoxCouplingStresses.CheckedChanged += checkBoxCouplingStresses_CheckedChanged;
            var groupBoxShow = new GroupBox
            {
                Text = "Show",
                Content = new StackLayout
                {
                    Orientation = Orientation.Vertical,
                    Spacing = 4,
                    Padding = 6,
                    Items =
                    {
                        checkBoxShowResults, checkBoxShowGaussPoints, checkBoxShowCouplingPoints, checkBoxShowCauchyStresses,
                        checkBoxPK2Stresses, checkBoxPrincipalStresses, checkBoxShowUndeformed, checkBoxShowKnots, checkBoxCouplingStresses
                    }
                }
            };

            comboBoxResultType = new DropDown();
            comboBoxResultType.SelectedIndexChanged += comboBoxResultType_SelectedIndexChanged;
            comboBoxLoadCaseType = new DropDown();
            comboBoxLoadCaseType.SelectedIndexChanged += comboBoxLoadCaseType_SelectedIndexChanged;
            domainUpDownAnalysisStep = new DropDown();
            domainUpDownAnalysisStep.SelectedIndexChanged += domainUpDownAnalysisStep_SelectedItemChanged;
            trackBarAnalysisStep = new Slider { Orientation = Orientation.Horizontal };
            trackBarAnalysisStep.ValueChanged += trackBarAnalysisStep_Scroll;
            var groupBoxAnalysisStep = new GroupBox
            {
                Text = "Analysis/Step",
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        LabeledRow("Load Case:", comboBoxLoadCaseType),
                        LabeledRow("Result Type:", comboBoxResultType),
                        LabeledRow("Step:", domainUpDownAnalysisStep),
                        new TableRow(trackBarAnalysisStep)
                    }
                }
            };

            return new Scrollable
            {
                Content = new TableLayout
                {
                    Padding = 6,
                    Spacing = new Size(6, 6),
                    Rows =
                    {
                        new TableRow(new StackLayout
                        {
                            Orientation = Orientation.Horizontal,
                            Spacing = 6,
                            Items = { buttonOpenFile, buttonShowPost, buttonClearPost }
                        }),
                        new TableRow(groupBoxAnalysisStep),
                        new TableRow(groupBoxVisualization),
                        new TableRow(groupBoxScalings),
                        new TableRow(groupBoxShow),
                        null
                    }
                }
            };
        }

        #region ComboBoxes
        public void updateMaterialData()
        {
            comboBoxElementMat.DataStore = null;
            comboBoxElementMat.DataStore = CocodriloPlugIn.Instance.Materials;
        }

        public void updateAnalysesData()
        {
            comboBoxAnalyses.DataStore = null;
            comboBoxAnalyses.DataStore = CocodriloPlugIn.Instance.Analyses;
        }
        #endregion

        #region Material
        void buttonAddModifyMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                new WindowMaterial().ShowModal();
            }
            catch
            {
                RhinoApp.WriteLine("Material cannot be opened!");
            }
        }
        #endregion

        #region Options
        void checkBoxShowElements_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.visualizer.show_elements = checkBoxShowPreprocessing.Checked == true;
            UpdateVisualizerEnabled();
        }

        void checkBoxShowPreSupports_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.visualizer.show_supports = checkBoxShowPreSupports.Checked == true;
            UpdateVisualizerEnabled();
        }

        void checkBoxShowPreLoads_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.visualizer.show_loads = checkBoxShowPreLoads.Checked == true;
            UpdateVisualizerEnabled();
        }

        void checkBoxShowPreCouplings_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.visualizer.show_couplings = checkBoxShowPreCouplings.Checked == true;
            UpdateVisualizerEnabled();
        }

        void UpdateVisualizerEnabled()
        {
            CocodriloPlugIn.Instance.visualizer.Enabled =
                checkBoxShowPreprocessing.Checked == true || checkBoxShowPreSupports.Checked == true ||
                checkBoxShowPreLoads.Checked == true || checkBoxShowPreCouplings.Checked == true;
            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                RhinoApp.RunScript("Cocodrilo_DeleteAll", true);
            }
            catch (Exception)
            {
                RhinoApp.WriteLine("WARNING: No Userdata deleted");
            }
        }

        void buttonResetInstance_Click(object sender, EventArgs e)
        {
            RhinoApp.RunScript("Cocodrilo_DeleteAll", true);
        }
        #endregion

        #region Analyses
        void textBoxMaxSteps_KeyPress(object sender, TextInputEventArgs e)
        {
            RestrictToDigits(e);
        }
        void textBoxMaxIterations_KeyPress(object sender, TextInputEventArgs e)
        {
            RestrictToDigits(e);
        }
        void textBoxTolerance_KeyPress(object sender, TextInputEventArgs e)
        {
            RestrictToDouble(e);
        }
        void textBoxFormfindingName_KeyPress(object sender, TextInputEventArgs e)
        {
            if (e.Text == " ")
                e.Cancel = true;
        }

        static void RestrictToDigits(TextInputEventArgs e)
        {
            foreach (var c in e.Text)
            {
                if (!char.IsDigit(c)) { e.Cancel = true; return; }
            }
        }
        static void RestrictToDouble(TextInputEventArgs e)
        {
            foreach (var c in e.Text)
            {
                if (!char.IsDigit(c) && c != '.' && c != 'e' && c != '-') { e.Cancel = true; return; }
            }
        }

        void tabControlAnalyses_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonModifyAnalysis.Enabled = tabControlAnalyses.SelectedIndex != 1;
        }

        void buttonCreateAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                switch (tabControlAnalyses.SelectedPage.Text)
                {
                    case "Formfinding":
                        {
                            var Name = textBoxFormfindingName.Text;
                            var analysis = CocodriloPlugIn.Instance.findAnalysis(Name);
                            if (analysis != null)
                                new Exception();
                            var MaxIterations = Convert.ToInt32(textBoxMaxIterations.Text);
                            var MaxSteps = Convert.ToInt32(textBoxMaxSteps.Text);
                            var Tolerance = Convert.ToDouble(textBoxTolerance.Text);

                            var Formfinding = new AnalysisFormfinding(Name, MaxSteps, MaxIterations, Tolerance);
                            CocodriloPlugIn.Instance.AddAnalysis(Formfinding);
                            break;
                        }
                    case "LinStrucAnalysis":
                        {
                            var Name = textBoxLinStrucAnalysisName.Text;
                            var analysis = CocodriloPlugIn.Instance.findAnalysis(Name);
                            if (analysis != null)
                                throw new Exception();

                            var StaLinAnalysis = new AnalysisLinear(Name);
                            CocodriloPlugIn.Instance.AddAnalysis(StaLinAnalysis);
                            break;
                        }
                    case "NonLinStrucAnalysis":
                        {
                            var name = textBoxNonLinStrucAnalysisName.Text;
                            var SolverTolerance = Convert.ToDouble(textBoxNonLinStrucAnalysisAcc.Text);
                            var NumSimulationSteps = Convert.ToInt32(textBoxNonLinStrucAnalysisNumSteps.Text);
                            var StepSize = Convert.ToDouble(textBoxNonLinStruAnalysisStepSize.Text);
                            var MaxSolverIteration = Convert.ToInt32(textBoxNonLinStrucAnalysisNumIter.Text);

                            var analysis = CocodriloPlugIn.Instance.findAnalysis(name);
                            if (analysis != null)
                                throw new Exception();

                            var StaNonLinAnalysis = new AnalysisNonLinear(
                                name, NumSimulationSteps, MaxSolverIteration, SolverTolerance, StepSize);
                            CocodriloPlugIn.Instance.AddAnalysis(StaNonLinAnalysis);
                            break;
                        }
                    case "TransientAnalysis":
                        {
                            var Name = textBoxTransientAnalysisName.Text;
                            var Acc = Convert.ToDouble(textBoxTransientAnalysisAcc.Text);
                            var Num_steps = Convert.ToInt32(textBoxTransientAnalysisNumSteps.Text);
                            var Num_iter = Convert.ToInt32(textBoxTransientAnalysisNumIter.Text);
                            var Stp_Ctrl = Convert.ToInt32(textBoxTransientAnalysisAdapStepCntrl.Text);
                            var Rayleigh_alpha = Convert.ToDouble(textBoxTransientAnalysisRayleighAlpha.Text);
                            var Rayleigh_beta = Convert.ToDouble(textBoxTransientAnalysisRayleighBeta.Text);
                            var Time_integ = Convert.ToString(comboBoxTransientAnalysisTimeIntegration.SelectedKey);
                            var Scheme = Convert.ToString(comboBoxTransientAnalysisScheme.SelectedKey);
                            var Automatic_Rayleigh = checkBoxTransientAnalysisAutomaticRayleigh.Checked == true;
                            var Damping_ratio_0 = Convert.ToDouble(textBoxTransientAnalysisDampingRatio0.Text);
                            var Damping_ratio_1 = Convert.ToDouble(textBoxTransientAnalysisDampingRatio1.Text);
                            var Num_eigen = Convert.ToInt32(textBoxTransientAnalysisNumEigen.Text);

                            var analysis = CocodriloPlugIn.Instance.findAnalysis(Name);
                            if (analysis != null)
                                throw new Exception();

                            var TransientAnalysis = new AnalysisTransient(Name, Num_steps, Num_iter, Acc, Num_steps, 1.0, Stp_Ctrl, Rayleigh_alpha, Rayleigh_beta, Time_integ, Scheme, Automatic_Rayleigh, Damping_ratio_0, Damping_ratio_1, Num_eigen);
                            CocodriloPlugIn.Instance.AddAnalysis(TransientAnalysis);
                            break;
                        }
                    case "EigenvalueAnalysis":
                        {
                            var Name = textBoxEigenvalueAnalysisName.Text;
                            var Acc = Convert.ToDouble(textBoxEigenvalueAnalysisAcc.Text);
                            var Num_eigen = Convert.ToInt32(textBoxEigenvalueAnalysisNumEigen.Text);
                            var Num_iter = Convert.ToInt32(textBoxEigenvalueAnalysisNumIter.Text);
                            var Solver_type = Convert.ToString(comboBoxEigenvalueAnalysisSolverType.SelectedKey);

                            var analysis = CocodriloPlugIn.Instance.findAnalysis(Name);
                            if (analysis != null)
                                throw new Exception();

                            var EigenvalueAnalysis = new AnalysisEigenvalue(Name, Acc, Num_eigen, Num_iter, Solver_type);
                            CocodriloPlugIn.Instance.AddAnalysis(EigenvalueAnalysis);
                            break;
                        }
                    case "CutPattern":
                        {
                            RhinoApp.WriteLine("WARNING: Cutting Pattern Analysis not yet implemented.");
                            var Name = textBoxCutPatternAnalysisName.Text;
                            var analysis = CocodriloPlugIn.Instance.findAnalysis(Name);
                            if (analysis != null)
                                new Exception();

                            var MaxIterations = Convert.ToInt32(textBoxCutPatternAnalysisMaxIter.Text);
                            var MaxSteps = Convert.ToInt32(textBoxCutPatternAnalysisMaxStep.Text);
                            var Tolerance = Convert.ToDouble(textBoxCutPatternAnalysisTol.Text);
                            var SolStrat = "Newton-Raphson";
                            var Prestress = checkBoxCutPatternAnalysisPrestress.Checked == true;

                            var CutPatt = new AnalysisCuttingPattern(Name, MaxSteps, MaxIterations, Tolerance, SolStrat, Prestress);
                            CocodriloPlugIn.Instance.AddAnalysis(CutPatt);
                            break;
                        }
                }
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: Analysis not completed, or name does already exist.");
            }
        }

        void buttonModifyAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                switch (tabControlAnalyses.SelectedPage.Text)
                {
                    case "Formfinding":
                        {
                            var analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxFormfindingName.Text);
                            if (analysis == null)
                                new Exception();
                            (analysis as AnalysisFormfinding).maxIterations = Convert.ToInt32(textBoxMaxIterations.Text);
                            (analysis as AnalysisFormfinding).maxSteps = Convert.ToInt32(textBoxMaxSteps.Text);
                            (analysis as AnalysisFormfinding).tolerance = Convert.ToDouble(textBoxTolerance.Text);
                            break;
                        }
                    case "NonLinStrucAnalysis":
                        {
                            var analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxNonLinStrucAnalysisName.Text);
                            if (analysis == null)
                                new Exception();
                            if (analysis is AnalysisNonLinear non_linear_analysis)
                            {
                                non_linear_analysis.mMaxSolverIteration = Convert.ToInt32(textBoxNonLinStrucAnalysisNumIter.Text);
                                non_linear_analysis.mNumSimulationSteps = Convert.ToInt32(textBoxNonLinStrucAnalysisNumSteps.Text);
                                non_linear_analysis.mSolverTolerance = Convert.ToDouble(textBoxNonLinStrucAnalysisAcc.Text);
                                non_linear_analysis.mStepSize = Convert.ToDouble(textBoxNonLinStruAnalysisStepSize.Text);
                            }
                            break;
                        }
                    case "TransientAnalysis":
                        {
                            var analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxTransientAnalysisName.Text);
                            if (analysis == null)
                                new Exception();
                            (analysis as AnalysisTransient).MaxIter = Convert.ToInt32(textBoxTransientAnalysisNumIter.Text);
                            (analysis as AnalysisTransient).NumStep = Convert.ToInt32(textBoxTransientAnalysisNumSteps.Text);
                            (analysis as AnalysisTransient).tolerance = Convert.ToDouble(textBoxTransientAnalysisAcc.Text);
                            (analysis as AnalysisTransient).RayleighAlpha = Convert.ToDouble(textBoxTransientAnalysisRayleighAlpha.Text);
                            (analysis as AnalysisTransient).RayleighBeta = Convert.ToDouble(textBoxTransientAnalysisRayleighBeta.Text);
                            (analysis as AnalysisTransient).TimeInteg = Convert.ToString(comboBoxTransientAnalysisTimeIntegration.SelectedKey);
                            (analysis as AnalysisTransient).Scheme = Convert.ToString(comboBoxTransientAnalysisScheme.SelectedKey);
                            (analysis as AnalysisTransient).AutomaticRayleigh = checkBoxTransientAnalysisAutomaticRayleigh.Checked == true;
                            (analysis as AnalysisTransient).DampingRatio0 = Convert.ToDouble(textBoxTransientAnalysisDampingRatio0.Text);
                            (analysis as AnalysisTransient).DampingRatio1 = Convert.ToDouble(textBoxTransientAnalysisDampingRatio1.Text);
                            (analysis as AnalysisTransient).NumEigen = Convert.ToDouble(textBoxTransientAnalysisNumEigen.Text);
                            break;
                        }
                    case "EigenvalueAnalysis":
                        {
                            var analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxEigenvalueAnalysisName.Text);
                            if (analysis == null)
                                new Exception();
                            (analysis as AnalysisEigenvalue).mMaximumIterations = Convert.ToInt32(textBoxEigenvalueAnalysisNumIter.Text);
                            (analysis as AnalysisEigenvalue).mNumEigenvalues = Convert.ToInt32(textBoxEigenvalueAnalysisNumEigen.Text);
                            (analysis as AnalysisEigenvalue).mTolerance = Convert.ToDouble(textBoxEigenvalueAnalysisAcc.Text);
                            (analysis as AnalysisEigenvalue).mSolverType = Convert.ToString(comboBoxEigenvalueAnalysisSolverType.SelectedKey);
                            break;
                        }
                    case "CutPattern":
                        {
                            var analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxCutPatternAnalysisName.Text);
                            if (analysis == null)
                                new Exception();
                            (analysis as AnalysisCuttingPattern).maxIterations = Convert.ToInt32(textBoxCutPatternAnalysisMaxIter.Text);
                            (analysis as AnalysisCuttingPattern).maxSteps = Convert.ToInt32(textBoxCutPatternAnalysisMaxStep.Text);
                            (analysis as AnalysisCuttingPattern).tolerance = Convert.ToDouble(textBoxCutPatternAnalysisTol.Text);
                            (analysis as AnalysisCuttingPattern).Prestress = checkBoxCutPatternAnalysisPrestress.Checked == true;
                            break;
                        }
                }
            }
            catch (Exception)
            {
                RhinoApp.WriteLine("WARNING: Analysis not found!");
            }
        }

        void buttonDeleteAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                Analysis analysis = null;
                switch (tabControlAnalyses.SelectedPage.Text)
                {
                    case "Formfinding": analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxFormfindingName.Text); break;
                    case "LinStrucAnalysis": analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxLinStrucAnalysisName.Text); break;
                    case "NonLinStrucAnalysis": analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxNonLinStrucAnalysisName.Text); break;
                    case "TransientAnalysis": analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxTransientAnalysisName.Text); break;
                    case "EigenvalueAnalysis": analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxEigenvalueAnalysisName.Text); break;
                    case "CutPattern": analysis = CocodriloPlugIn.Instance.findAnalysis(textBoxCutPatternAnalysisName.Text); break;
                }

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

        void comboBoxAnalyses_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var analysis = comboBoxAnalyses.SelectedValue as Analysis;
                if (analysis != null)
                {
                    if (analysis.GetType() == typeof(AnalysisFormfinding))
                    {
                        var formfinding = (AnalysisFormfinding)analysis;
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

        void radioButtonRunCarat_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRunCarat.Checked)
                radioButtonRunKratos.Checked = false;
            else
                radioButtonRunKratos.Checked = true;
        }
        void radioButtonRunKratos_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRunKratos.Checked)
                radioButtonRunCarat.Checked = false;
            else
                radioButtonRunCarat.Checked = true;
        }

        void buttonRunAnalysis_Click(object sender, EventArgs e)
        {
            var analysis = comboBoxAnalyses.SelectedValue as Analysis;
            var output = new IO.OutputKratosIGA(analysis);
            output.StartAnalysis();
        }

        void buttonEditOutput_Click(object sender, EventArgs e)
        {
            try
            {
                new WindowOutputOptions().ShowModal();
            }
            catch
            {
                RhinoApp.WriteLine("Output options window is already open!");
            }
        }
        #endregion

        #region Support
        void buttonAddEdgeSupports_Click(object sender, EventArgs e)
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
                checkBoxDispX.Checked == true, checkBoxDispY.Checked == true, checkBoxDispZ.Checked == true,
                textBoxSupportTypeDispX.Text, textBoxSupportTypeDispY.Text, textBoxSupportTypeDispY.Text,
                checkBoxRotationSupport.Checked == true, false,
                getIsSupportStrong(), comboBoxSupportType.SelectedKey);
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
                        case "Surface": return GeometryType.GeometrySurface;
                        case "Edge": return GeometryType.SurfaceEdge;
                        case "Vertex": return GeometryType.SurfacePoint;
                    }
                    break;
                case "Curve":
                    switch (ObjectTypeSelected)
                    {
                        case "Edge": return GeometryType.CurveEdge;
                        case "Vertex": return GeometryType.CurvePoint;
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

        public bool getIsSupportStrong() => checkBoxSupportStrong.Checked == true;
        public bool getOverwriteSupport() => checkBoxOverwriteSupport.Checked == true;

        void buttonDeleteEdgeSupport_Click(object sender, EventArgs e)
        {
            try
            {
                RhinoApp.RunScript("Cocodrilo_DeleteSupports", true);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No support deleted. One or more supports did not exist.");
            }
        }

        public string getSupportObjectType()
        {
            if (radioButtonSupportDimFace.Checked) return "Surface";
            if (radioButtonSupportDimLine.Checked) return "Edge";
            if (radioButtonSupportDimVertex.Checked) return "Vertex";
            return "";
        }

        public string getSupportGeometryType()
        {
            if (radioButtonSupportSurface.Checked) return "Surface";
            if (radioButtonSupportCurve.Checked) return "Curve";
            return "";
        }

        void radioButtonSupportCurve_CheckedChanged(object sender, EventArgs e) => radioButtonSupport_AdaptChecked();
        void radioButtonSupportSurface_CheckedChanged(object sender, EventArgs e) => radioButtonSupport_AdaptChecked();

        void radioButtonSupport_AdaptChecked()
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

        #region Refinement
        void buttonCheckRefinement_Click(object sender, EventArgs e)
        {
            try
            {
                if (getRefinementElementType() == GeometryType.GeometrySurface)
                {
                    ObjRef objref = null;
                    var rc = RhinoGet.GetOneObject("Select one Surface", false, ObjectType.Surface, out objref);
                    if (rc != Result.Success)
                        new Exception();

                    var user_data_surface = UserDataUtilities.GetOrCreateUserDataSurface(objref.Brep().Surfaces[objref.Face().FaceIndex]);
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

                    var user_data_edge = UserDataUtilities.GetOrCreateUserDataEdge(objref.Curve());
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
                    var rc = RhinoGet.GetOneObject("Select one Curve...", false, ObjectType.Curve, out objref);
                    if (rc != Result.Success)
                        new Exception();

                    var user_data_curve = UserDataUtilities.GetOrCreateUserDataCurve(objref.Curve());
                    var refinement = user_data_curve.GetRefinement() as RefinementCurve;
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

        void buttonChangeRefinement_Click(object sender, EventArgs e)
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

                    var ThisSurfaceRefinement = new RefinementSurface(degree_p, degree_q, KnotSubDivU, KnotSubDivV, knotinserttype);

                    if (CommandUtilities.TryGetUserDataSurface(out var user_data_surface_list))
                        foreach (var user_data_surface in user_data_surface_list)
                            user_data_surface.ChangeRefinement(ThisSurfaceRefinement);
                }
                else if (getRefinementElementType() == GeometryType.SurfaceEdge)
                {
                    int degree_p = Convert.ToInt32(textBoxPDeg.Text);
                    int KnotSubDivU = Convert.ToInt32(textBoxKnotSubDivU.Text);

                    var ThisEdgeRefinement = new RefinementEdge(degree_p, KnotSubDivU, knotinserttype);

                    if (CommandUtilities.TryGetUserDataEdge(out var user_data_edge_list))
                        foreach (var user_data_edge in user_data_edge_list)
                            user_data_edge.ChangeRefinement(ThisEdgeRefinement);
                }
                if (getRefinementElementType() == GeometryType.GeometryCurve)
                {
                    int degree_p = Convert.ToInt32(textBoxPDeg.Text);
                    int KnotSubDivU = Convert.ToInt32(textBoxKnotSubDivU.Text);

                    var ThisCurveRefinement = new RefinementCurve(degree_p, KnotSubDivU, knotinserttype);

                    if (CommandUtilities.TryGetUserDataCurve(out var user_data_curve_list))
                        foreach (var user_data_curve in user_data_curve_list)
                            user_data_curve.ChangeRefinement(ThisCurveRefinement);
                }
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No refinement done.");
            }
        }

        public GeometryType getRefinementElementType()
        {
            if (radioButtonRefinementElementSurf.Checked) return GeometryType.GeometrySurface;
            if (radioButtonRefinementElementCurve.Checked) return GeometryType.GeometryCurve;
            if (radioButtonRefinementElementEdge.Checked) return GeometryType.SurfaceEdge;
            return GeometryType.ErrorType;
        }

        void radioButtonRefinementCurve_CheckedChanged(object sender, EventArgs e) => radioButtonRefinement_AdaptChecked();
        void radioButtonRefinementSurface_CheckedChanged(object sender, EventArgs e) => radioButtonRefinement_AdaptChecked();
        void radioButtonRefinementEdge_CheckedChanged(object sender, EventArgs e) => radioButtonRefinement_AdaptChecked();

        void radioButtonRefinement_AdaptChecked()
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

        void radioButtonRefinementKnotSubdivision_CheckedChanged(object sender, EventArgs e) { }
        void radioButtonKnotSubdivision_CheckedChanged(object sender, EventArgs e) { }
        #endregion

        #region Element
        void comboBoxBeamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var type = comboBoxBeamType.SelectedKey;
            labelBeamDiameter.Visible = type == "Circular";
            labelBeamHeight.Visible = type == "Rectangular";
            labelBeamWidth.Visible = type == "Rectangular";
            var undefined = type == "Undefined";
            labelBeamArea.Visible = textBoxBeamArea.Visible = undefined;
            labelBeamIy.Visible = textBoxBeamIy.Visible = undefined;
            labelBeamIz.Visible = textBoxBeamIz.Visible = undefined;
            labelBeamIt.Visible = textBoxBeamIt.Visible = undefined;
        }

        void comboBoxCableType_SelectedIndex_Changed(object sender, EventArgs e)
        {
            checkBoxCablePrestressCurve.Enabled = comboBoxCableType.SelectedKey == "Curve";
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

            if ((Math.Abs(point_00[0] - point_10[0]) < 1e-12 && Math.Abs(point_00[1] - point_10[1]) < 1e-12)
                || (Math.Abs(point_00[0] - point_01[0]) < 1e-12 && Math.Abs(point_00[1] - point_01[1]) < 1e-12))
            {
                direction_2[0] = 0;
                direction_2[1] = 0;
                direction_2[2] = 1;
            }

            return new MembraneProperties(thickness, direction_1, direction_2, prestress_1, prestress_2);
        }

        public ShellProperties GetShellProperties()
        {
            return new ShellProperties(Convert.ToDouble(textBoxShellThick.Text), true, comboBoxShellType.SelectedKey, false);
        }

        public CableProperties GetCableProperties()
        {
            return new CableProperties(
                Convert.ToDouble(textBoxCablePrestress.Text),
                Convert.ToDouble(textBoxCableArea.Text),
                CableCouplingType.EntireCurve);
        }

        public string getSelectedElementType() => tabControlElement.SelectedPage.Text;

        public GeometryType getCableTopologyType()
        {
            if (comboBoxCableType.SelectedKey == "Curve") return GeometryType.GeometryCurve;
            if (comboBoxCableType.SelectedKey == "Edge") return GeometryType.SurfaceEdge;

            RhinoApp.WriteLine("WARNING: comboBoxCableType.Text not Curve or Edge");
            return GeometryType.CurveEdge;
        }

        public bool getIsCableFormFinding() => checkBoxElementCableFofi.Checked == true;
        public int getMaterialIdElement() => Convert.ToInt32(comboBoxElementMat.SelectedValue);
        public bool getIsFormFindingElement() => checkBoxElementMembraneFofi.Checked == true;
        public bool getIsEdgeCoupling() => checkBoxElementMembraneEdgeCoupling.Checked == true;

        void buttonAddElement_Click(object sender, EventArgs e)
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

        void buttonAddAxis_Click(object sender, EventArgs e)
        {
            try
            {
                new WindowAxis().ShowModal();
            }
            catch
            {
                RhinoApp.WriteLine("Axis window is already open!");
            }
        }

        void buttonDeleteElement_Click(object sender, EventArgs e)
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

        // NOTE: in the original WinForms code, this handler's per-element data readback (from
        // each selected object's UserData) was entirely commented out - only the object-selection
        // and "has no user data" checks were live. Ported as-is: it verifies selection/user-data,
        // without actually reading properties back into the fields.
        void buttonLoadElement_Click(object sender, EventArgs e)
        {
            try
            {
                var elementType = tabControlElement.SelectedPage.Text;
                if (elementType == "Membrane" || elementType == "Shell")
                {
                    ObjRef[] objref = null;
                    var rc = RhinoGet.GetMultipleObjects("Select Surfaces", false, ObjectType.Surface, out objref);
                    if (rc != Result.Success)
                        new Exception();
                    foreach (var surface in objref)
                    {
                        var ud = surface.Brep().Surfaces[surface.Face().FaceIndex].UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                        if (ud == null)
                            RhinoApp.WriteLine("Surface has no user data");
                    }
                }
                else if (elementType == "Beam")
                {
                    ObjRef[] objref = null;
                    var rc = RhinoGet.GetMultipleObjects("Select Curves", false, ObjectType.Curve, out objref);
                    if (rc != Result.Success)
                        new Exception();
                    foreach (var crv in objref)
                    {
                        var ud = crv.Curve().UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                        if (ud == null)
                            RhinoApp.WriteLine("Curve has no user data");
                    }
                }
                else if (elementType == "Cable")
                {
                    ObjRef[] objref = null;
                    var rc = RhinoGet.GetMultipleObjects("Select Surfaces", false, ObjectType.Surface, out objref);
                    if (rc != Result.Success)
                        new Exception();
                    foreach (var crv in objref)
                    {
                        var ud = crv.Curve().UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                        var ud_e = crv.Curve().UserData.Find(typeof(UserDataEdge)) as UserDataEdge;
                        if (ud == null || ud_e == null)
                            RhinoApp.WriteLine("Surface has no user data");
                    }
                }
            }
            catch (Exception)
            {
                RhinoApp.WriteLine("WARNING: No element loaded.");
            }
        }
        #endregion

        #region Load
        public Load getLoad()
        {
            return new ElementProperties.Load(
                textBoxLoadX.Text, textBoxLoadY.Text, textBoxLoadZ.Text, "1.0", comboBoxLoadType.SelectedKey);
        }

        public double[] getLoadPosition()
        {
            double[] positions = new double[2] { -1, -1 };
            positions[0] = textBoxLoadPositionU.Text == "" ? -1 : Convert.ToDouble(textBoxLoadPositionU.Text);
            positions[1] = textBoxLoadPositionV.Text == "" ? -1 : Convert.ToDouble(textBoxLoadPositionV.Text);
            return positions;
        }

        public bool getLoadBoolOverwrite() => checkBoxLoadOverwrite.Checked == true;

        void buttonAddLoad_Click(object sender, EventArgs e)
        {
            try
            {
                RhinoApp.RunScript("Cocodrilo_AddLoad", true);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No Load Added!");
            }
        }

        void buttonDeleteLoad_Click(object sender, EventArgs e)
        {
            try
            {
                RhinoApp.RunScript("Cocodrilo_DeleteLoad", true);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No Load Deleted!");
            }
        }

        void comboBoxLoadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pressureType = comboBoxLoadType.SelectedKey == "PRES" || comboBoxLoadType.SelectedKey == "PRES_FL";
            labelLoadDirectionX.Visible = !pressureType;
            labelLoadDirectionY.Visible = !pressureType;
            labelLoadDirectionZ.Visible = !pressureType;
            textBoxLoadY.Visible = !pressureType;
            textBoxLoadZ.Visible = !pressureType;
        }

        public string getLoadObjectType()
        {
            if (radioButtonLoadDimFace.Checked) return "Surface";
            if (radioButtonLoadDimLine.Checked) return "Edge";
            if (radioButtonLoadDimVertex.Checked) return "Vertex";
            return "";
        }

        public string getLoadGeometryType()
        {
            if (radioButtonLoadElementSurface.Checked) return "Surface";
            if (radioButtonLoadElementCurve.Checked) return "Curve";
            return "";
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
                        case "Surface": return GeometryType.GeometrySurface;
                        case "Edge": return GeometryType.SurfaceEdge;
                        case "Vertex": return GeometryType.SurfacePoint;
                    }
                    break;
                case "Curve":
                    switch (ObjectTypeSelected)
                    {
                        case "Edge": return GeometryType.CurveEdge;
                        case "Vertex": return GeometryType.CurvePoint;
                    }
                    break;
            }

            RhinoApp.WriteLine("WARNING SUPPORT NOT FOUND!");
            return GeometryType.GeometrySurface;
        }

        void radioButtonLoadCurve_CheckedChanged(object sender, EventArgs e) => radioButtonLoad_AdaptChecked();
        void radioButtonLoadSurface_CheckedChanged(object sender, EventArgs e) => radioButtonLoad_AdaptChecked();
        void radioButtonLoadDimFace_CheckedChanged(object sender, EventArgs e) => radioButtonLoad_AdaptChecked();
        void radioButtonLoadDimLine_CheckedChanged(object sender, EventArgs e) => radioButtonLoad_AdaptChecked();
        void radioButtonLoadDimVertex_CheckedChanged(object sender, EventArgs e) => radioButtonLoad_AdaptChecked();

        void radioButtonLoad_AdaptChecked()
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

        #region Check
        void buttonAddCheck_Click(object sender, EventArgs e)
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
                checkBoxOutputDispX.Checked == true,
                checkBoxOutputDispY.Checked == true,
                checkBoxOutputDispZ.Checked == true,
                checkBoxOutputLagrangeMP.Checked == true,
                new List<string>());
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
                        case "Surface": return GeometryType.GeometrySurface;
                        case "Edge": return GeometryType.SurfaceEdge;
                        case "Vertex": return GeometryType.SurfacePoint;
                    }
                    break;
                case "Curve":
                    switch (ObjectTypeSelected)
                    {
                        case "Edge": return GeometryType.CurveEdge;
                        case "Vertex": return GeometryType.CurvePoint;
                    }
                    break;
            }

            RhinoApp.WriteLine("WARNING OUTPUT LOCATION NOT FOUND!");
            return GeometryType.ErrorType;
        }

        public TimeInterval GetTimeIntervalCheck() => new TimeInterval(textBoxCheckStartTime.Text, textBoxCheckEndTime.Text);
        public bool getOverwriteCheck() => checkBoxOverwriteChecks.Checked == true;
        public CheckType GetCheckType() => CheckType.ConvergenceCheck;

        public string getCheckObjectType()
        {
            if (radioButtonCheckFace.Checked) return "Surface";
            if (radioButtonCheckLine.Checked) return "Edge";
            if (radioButtonCheckVertex.Checked) return "Vertex";
            return "";
        }

        public string getCheckGeometryType()
        {
            if (radioButtonCheckSurface.Checked) return "Surface";
            if (radioButtonCheckCurve.Checked) return "Curve";
            return "";
        }
        #endregion

        #region Post Processing
        void open_file_Click(object sender, EventArgs e)
        {
            string open_file_name = "";
            var openFileDialog1 = new Eto.Forms.OpenFileDialog { Filters = { new FileFilter("Postprocessing Files", new[] { ".georhino.txt", ".georhino.json" }) } };
            var result = openFileDialog1.ShowDialog(this);

            if (result == DialogResult.Ok)
            {
                open_file_name = openFileDialog1.FileName;
            }
            else
            {
                return;
            }

            CocodriloPlugIn.Instance.PostProcessingCocodrilo = new PostProcessing.PostProcessing(open_file_name, true);

            UpdatePostProcessingVariables();

            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGeometry = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGaussPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateCouplingPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;

            RhinoApp.WriteLine("Reading results finished!");
        }

        public void SetDefaults()
        {
            textBoxDispScale.Text = "1.000e+00";
            textBoxResScale.Text = "1.000e+00";
            textBoxFlyingNodeLimit.Text = "1.000e+05";
        }

        public void UpdatePostProcessingVariables()
        {
            if (CocodriloPlugIn.Instance.PostProcessingCocodrilo?.ResultList.Count > 0)
            {
                comboBoxLoadCaseType.Items.Clear();
                foreach (var result_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseTypes)
                    comboBoxLoadCaseType.Items.Add(result_type);

                comboBoxResultType.Items.Clear();
                foreach (var result_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.CurrentDistinctResultTypes)
                    comboBoxResultType.Items.Add(result_type);

                domainUpDownAnalysisStep.Items.Clear();
                if (CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers.Count > 0)
                {
                    foreach (var load_case_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers)
                        domainUpDownAnalysisStep.Items.Add(load_case_type.ToString());
                    domainUpDownAnalysisStep.SelectedKey = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0].ToString();
                }
                else
                {
                    domainUpDownAnalysisStep.Items.Add("0");
                    domainUpDownAnalysisStep.SelectedKey = "0";
                }
                if (CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers.Count > 0)
                {
                    trackBarAnalysisStep.MinValue = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0];
                    int n_lc = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers.Count;
                    trackBarAnalysisStep.MaxValue = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[n_lc - 1];
                    trackBarAnalysisStep.Value = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0];
                }
                else
                {
                    trackBarAnalysisStep.MinValue = 0;
                    trackBarAnalysisStep.MaxValue = 0;
                    trackBarAnalysisStep.Value = 0;
                }

                UpdateMinMax();

                comboBoxLoadCaseType.SelectedIndex = 0;
                comboBoxResultType.SelectedIndex = 0;
            }
        }

        void buttonClearPost_Click(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo?.ClearPostProcessing();
            UpdatePostProcessingVariables();
        }

        void buttonShowPost_Click(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateCurrentResults(
                comboBoxLoadCaseType.SelectedKey, Convert.ToInt32(domainUpDownAnalysisStep.SelectedKey), comboBoxResultType.SelectedKey);

            PostProcessing.PostProcessing.s_MinMax[0] = Convert.ToDouble(textBoxColorBarMin.Text);
            PostProcessing.PostProcessing.s_MinMax[1] = Convert.ToDouble(textBoxColorBarMax.Text);

            PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex = comboBoxPostProcessingDirection.Enabled
                ? comboBoxPostProcessingDirection.SelectedIndex
                : 0;

            CocodriloPlugIn.Instance.PostProcessingCocodrilo.ShowPostProcessing(
                Convert.ToDouble(textBoxDispScale.Text),
                Convert.ToDouble(textBoxFlyingNodeLimit.Text),
                Convert.ToDouble(textBoxResScale.Text),
                checkBoxShowResults.Checked == true,
                checkBoxShowGaussPoints.Checked == true,
                checkBoxShowCouplingPoints.Checked == true,
                checkBoxShowCauchyStresses.Checked == true,
                checkBoxPK2Stresses.Checked == true,
                checkBoxPrincipalStresses.Checked == true,
                checkBoxShowUndeformed.Checked == true);
        }

        void comboBoxResultType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (domainUpDownAnalysisStep.Items.Count > 0)
            {
                UpdateComboBoxPostProcessingDirection(
                    CocodriloPlugIn.Instance.PostProcessingCocodrilo.ResultInfo(comboBoxLoadCaseType.SelectedKey, Convert.ToInt32(domainUpDownAnalysisStep.SelectedKey), comboBoxResultType.SelectedKey));

                CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateCurrentResults(
                    comboBoxLoadCaseType.SelectedKey, Convert.ToInt32(domainUpDownAnalysisStep.SelectedKey), comboBoxResultType.SelectedKey);
            }
            UpdateMinMax();
        }

        void comboBoxPostProcessingDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
            PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex = comboBoxPostProcessingDirection.SelectedIndex;
            UpdateMinMax();
        }

        void UpdateMinMax()
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateCurrentMinMax();
            textBoxColorBarMin.Text = CocodriloPlugIn.Instance.PostProcessingCocodrilo.mCurrentMinMax[0].ToString("0.0000e+00");
            textBoxColorBarMax.Text = CocodriloPlugIn.Instance.PostProcessingCocodrilo.mCurrentMinMax[1].ToString("0.0000e+00");
        }

        void UpdateComboBoxPostProcessingDirection(PostProcessing.RESULT_INFO ThisResultInfo)
        {
            int selected_index = comboBoxPostProcessingDirection.SelectedIndex < 0 ? 0 : comboBoxPostProcessingDirection.SelectedIndex;
            comboBoxPostProcessingDirection.Items.Clear();

            if (ThisResultInfo.VectorOrScalar == "Vector")
            {
                comboBoxPostProcessingDirection.Enabled = true;

                var result_indices = PostProcessing.PostProcessingUtilities.GetResultIndices(ThisResultInfo);

                result_indices.ForEach(item => comboBoxPostProcessingDirection.Items.Add(item.ToString()));
                selected_index = selected_index >= comboBoxPostProcessingDirection.Items.Count
                    ? comboBoxPostProcessingDirection.Items.Count - 1
                    : selected_index;
                comboBoxPostProcessingDirection.SelectedIndex = selected_index;
            }
            else
            {
                comboBoxPostProcessingDirection.Items.Add("");
                comboBoxPostProcessingDirection.SelectedIndex = 0;
                comboBoxPostProcessingDirection.Enabled = false;
                PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex = 0;
            }

            UpdateMinMax();
        }

        void UpdateComboBoxResultType()
        {
            bool update_result_type = false;
            foreach (var result_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.CurrentDistinctResultTypes)
                if (!comboBoxResultType.Items.Any(i => i.Text == result_type))
                    update_result_type = true;

            if (update_result_type)
            {
                int result_type_index = comboBoxResultType.SelectedIndex;
                comboBoxResultType.Items.Clear();
                foreach (var result_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.CurrentDistinctResultTypes)
                    comboBoxResultType.Items.Add(result_type);
                comboBoxResultType.SelectedIndex = (result_type_index < comboBoxResultType.Items.Count && result_type_index >= 0)
                        ? result_type_index
                        : 0;
            }
        }

        void domainUpDownAnalysisStep_SelectedItemChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxResultType.SelectedKey))
                return;
            if (domainUpDownAnalysisStep.SelectedKey == null)
                return;

            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateCurrentResults(
                comboBoxLoadCaseType.SelectedKey, Convert.ToInt32(domainUpDownAnalysisStep.SelectedKey), comboBoxResultType.SelectedKey);

            UpdateComboBoxResultType();

            var step = Convert.ToInt32(domainUpDownAnalysisStep.SelectedKey);
            if (step > trackBarAnalysisStep.MaxValue)
            {
                trackBarAnalysisStep.Value = trackBarAnalysisStep.MaxValue;
                domainUpDownAnalysisStep.SelectedKey = trackBarAnalysisStep.MaxValue.ToString();
            }
            else if (step < trackBarAnalysisStep.MinValue)
            {
                trackBarAnalysisStep.Value = trackBarAnalysisStep.MinValue;
                domainUpDownAnalysisStep.SelectedKey = trackBarAnalysisStep.MinValue.ToString();
            }
            else
                trackBarAnalysisStep.Value = step;

            UpdateMinMax();
        }

        void comboBoxLoadCaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateCurrentResults(
                comboBoxLoadCaseType.SelectedKey, Convert.ToInt32(domainUpDownAnalysisStep.SelectedKey ?? "0"), comboBoxResultType.SelectedKey);

            UpdateComboBoxResultType();

            domainUpDownAnalysisStep.Items.Clear();
            foreach (var load_case_type in CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers)
                domainUpDownAnalysisStep.Items.Add(load_case_type.ToString());
            if (CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers.Count > 0)
            {
                trackBarAnalysisStep.MinValue = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0];
                int n_lc = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers.Count;
                trackBarAnalysisStep.MaxValue = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[n_lc - 1];
                domainUpDownAnalysisStep.SelectedKey = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0].ToString();
                trackBarAnalysisStep.Value = CocodriloPlugIn.Instance.PostProcessingCocodrilo.DistinctLoadCaseNumbers[0];
            }
            else
            {
                trackBarAnalysisStep.MinValue = 0;
                trackBarAnalysisStep.MaxValue = 0;
                domainUpDownAnalysisStep.SelectedKey = "0";
                trackBarAnalysisStep.Value = 0;
            }

            UpdateComboBoxResultType();
            UpdateMinMax();
        }

        void textBoxColorBarMin_TextChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        void textBoxColorBarMax_TextChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        void checkBoxShowGaussPoints_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGaussPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateEvaluationPoints(checkBoxShowGaussPoints.Checked == true, true);
        }

        void textBoxFlyingNodeLimit_TextChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGeometry = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGaussPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateCouplingPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        void textBoxResScale_TextChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        void textBoxDispScale_TextChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGeometry = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateGaussPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateCouplingPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        void checkBoxShowResults_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
        }

        void trackBarAnalysisStep_Scroll(object sender, EventArgs e)
        {
            if (domainUpDownAnalysisStep.Items.Any(i => i.Text == trackBarAnalysisStep.Value.ToString()))
            {
                domainUpDownAnalysisStep.SelectedKey = trackBarAnalysisStep.Value.ToString();
            }
            else
            {
                double min_dist = 100000;
                int closest_index = -1;
                var items = domainUpDownAnalysisStep.Items;
                for (int i = 0; i < items.Count; i++)
                {
                    var dist = Math.Abs(Convert.ToDouble(items[i].Text) - trackBarAnalysisStep.Value);
                    if (dist < min_dist) { closest_index = i; min_dist = dist; }
                }
                if (closest_index >= 0)
                {
                    domainUpDownAnalysisStep.SelectedIndex = closest_index;
                    trackBarAnalysisStep.Value = Convert.ToInt32(items[closest_index].Text);
                }
            }
        }

        void checkBoxShowKnots_CheckedChanged(object sender, EventArgs e)
        {
            PostProcessing.PostProcessing.s_ShowKnotSpanIsoCurves = checkBoxShowKnots.Checked == true;
        }

        void checkBoxShowCouplingPoints_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateCouplingPoints = true;
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateCouplingPoints(checkBoxShowCouplingPoints.Checked == true, true);
        }

        void checkBoxShowCauchyStresses_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateStressPatterns = true;
        }

        void checkBoxPK2Stresses_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateStressPatterns = true;
        }

        void checkBoxPrincipalStresses_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateStressPatterns = true;
        }

        void MeshShowPreview(object sender, EventArgs e)
        {
            List<Guid> ids = new List<Guid>();
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.ShowMeshBoundaryPoints(ref ids);
        }

        void AutoMinMax(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateComputeMinMax();
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.mUpdateResultPlot = true;
            textBoxColorBarMin.Text = PostProcessing.PostProcessing.s_MinMax[0].ToString("0.0000e+00");
            textBoxColorBarMax.Text = PostProcessing.PostProcessing.s_MinMax[1].ToString("0.0000e+00");
        }

        void checkBoxShowMesh_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdatePostProcessingMeshes(checkBoxShowMesh.Checked == true);
        }

        void checkBoxShowUndeformed_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.UpdateInitialGeometry(checkBoxShowUndeformed.Checked == true);
        }

        void checkBoxCouplingStresses_CheckedChanged(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.PostProcessingCocodrilo.VisualizeCouplingStresses(true, Convert.ToDouble(textBoxResScale.Text));
        }
        #endregion
    }
}
