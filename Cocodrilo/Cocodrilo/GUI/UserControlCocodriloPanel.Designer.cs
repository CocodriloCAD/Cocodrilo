namespace Cocodrilo.Panels
{
    partial class UserControlCocodriloPanel
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlCocodriloPanel));
            this.TeDA_Plugin = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBoxAnalyses = new System.Windows.Forms.GroupBox();
            this.radioButtonRunKratos = new System.Windows.Forms.RadioButton();
            this.radioButtonRunCarat = new System.Windows.Forms.RadioButton();
            this.buttonEditOutput = new System.Windows.Forms.Button();
            this.tabControlAnalyses = new System.Windows.Forms.TabControl();
            this.tabPageFormfinding = new System.Windows.Forms.TabPage();
            this.textBoxFormfindingName = new System.Windows.Forms.TextBox();
            this.textBoxTolerance = new System.Windows.Forms.TextBox();
            this.textBoxMaxIterations = new System.Windows.Forms.TextBox();
            this.textBoxMaxSteps = new System.Windows.Forms.TextBox();
            this.labelMaxIterattions = new System.Windows.Forms.Label();
            this.labelAnalysisFofiName = new System.Windows.Forms.Label();
            this.labelAccuracy = new System.Windows.Forms.Label();
            this.labelMaxSteps = new System.Windows.Forms.Label();
            this.tabPageLinStructuralAnalysis = new System.Windows.Forms.TabPage();
            this.textBoxLinStrucAnalysisName = new System.Windows.Forms.TextBox();
            this.labelAnalysisStaGeoName = new System.Windows.Forms.Label();
            this.tabPageNonLinStrucAnalysis = new System.Windows.Forms.TabPage();
            this.textBoxNonLinStruAnalysisStepSize = new System.Windows.Forms.TextBox();
            this.labelNonLinAnalysisStepSize = new System.Windows.Forms.Label();
            this.textBoxNonLinStrucAnalysisAdapStepCntrl = new System.Windows.Forms.TextBox();
            this.labelNonLinStrucAnalysisAdapStepCntrl = new System.Windows.Forms.Label();
            this.labelNonLinStrucAnalysisNumIter = new System.Windows.Forms.Label();
            this.textBoxNonLinStrucAnalysisNumIter = new System.Windows.Forms.TextBox();
            this.labelNonLinStrucAnalysisNumSteps = new System.Windows.Forms.Label();
            this.textBoxNonLinStrucAnalysisNumSteps = new System.Windows.Forms.TextBox();
            this.textBoxNonLinStrucAnalysisAcc = new System.Windows.Forms.TextBox();
            this.labelNonLinStrucAnalysisAcc = new System.Windows.Forms.Label();
            this.textBoxNonLinStrucAnalysisName = new System.Windows.Forms.TextBox();
            this.labelNonLinStrucAnalysisName = new System.Windows.Forms.Label();
            this.tabPageTransientAnalysis = new System.Windows.Forms.TabPage();
            this.textBoxTransientAnalysisStepSize = new System.Windows.Forms.TextBox();
            this.labelTransientAnalysisStepSize = new System.Windows.Forms.Label();
            this.textBoxTransientAnalysisAdapStepCntrl = new System.Windows.Forms.TextBox();
            this.labelTransientAnalysisAdapStepCntrl = new System.Windows.Forms.Label();
            this.labelTransientAnalysisNumIter = new System.Windows.Forms.Label();
            this.textBoxTransientAnalysisNumIter = new System.Windows.Forms.TextBox();
            this.labelTransientAnalysisNumSteps = new System.Windows.Forms.Label();
            this.textBoxTransientAnalysisNumSteps = new System.Windows.Forms.TextBox();
            this.textBoxTransientAnalysisAcc = new System.Windows.Forms.TextBox();
            this.labelTransientAnalysisAcc = new System.Windows.Forms.Label();
            this.textBoxTransientAnalysisRayleighAlpha = new System.Windows.Forms.TextBox();
            this.labelTransientAnalysisRayleighAlpha = new System.Windows.Forms.Label();
            this.textBoxTransientAnalysisRayleighBeta = new System.Windows.Forms.TextBox();
            this.labelTransientAnalysisRayleighBeta = new System.Windows.Forms.Label();
            this.textBoxTransientAnalysisName = new System.Windows.Forms.TextBox();
            this.labelTransientAnalysisName = new System.Windows.Forms.Label();
            this.comboBoxTransientAnalysisTimeIntegration = new System.Windows.Forms.ComboBox();
            this.labelTransientAnalysisTimeIntegration = new System.Windows.Forms.Label();
            this.comboBoxTransientAnalysisScheme = new System.Windows.Forms.ComboBox();
            this.labelTransientAnalysisScheme = new System.Windows.Forms.Label();
            this.checkBoxTransientAnalysisAutomaticRayleigh = new System.Windows.Forms.CheckBox();
            this.textBoxTransientAnalysisDampingRatio0 = new System.Windows.Forms.TextBox();
            this.labelTransientAnalysisDampingRatio0 = new System.Windows.Forms.Label();
            this.textBoxTransientAnalysisDampingRatio1 = new System.Windows.Forms.TextBox();
            this.labelTransientAnalysisDampingRatio1 = new System.Windows.Forms.Label();
            this.textBoxTransientAnalysisNumEigen = new System.Windows.Forms.TextBox();
            this.labelTransientAnalysisNumEigen = new System.Windows.Forms.Label();
            this.tabPageEigenvalueAnalysis = new System.Windows.Forms.TabPage();
            this.labelEigenvalueAnalysisNumIter = new System.Windows.Forms.Label();
            this.textBoxEigenvalueAnalysisNumIter = new System.Windows.Forms.TextBox();
            this.labelEigenvalueAnalysisNumEigen = new System.Windows.Forms.Label();
            this.textBoxEigenvalueAnalysisNumEigen = new System.Windows.Forms.TextBox();
            this.textBoxEigenvalueAnalysisAcc = new System.Windows.Forms.TextBox();
            this.labelEigenvalueAnalysisAcc = new System.Windows.Forms.Label();
            this.textBoxEigenvalueAnalysisName = new System.Windows.Forms.TextBox();
            this.labelEigenvalueAnalysisName = new System.Windows.Forms.Label();
            this.comboBoxEigenvalueAnalysisSolverType = new System.Windows.Forms.ComboBox();
            this.labelEigenvalueAnalysisSolverType = new System.Windows.Forms.Label();
            this.tabPageCutPatternAnalysis = new System.Windows.Forms.TabPage();
            this.checkBoxCutPatternAnalysisPrestress = new System.Windows.Forms.CheckBox();
            this.textBoxCutPatternAnalysisName = new System.Windows.Forms.TextBox();
            this.textBoxCutPatternAnalysisTol = new System.Windows.Forms.TextBox();
            this.textBoxCutPatternAnalysisMaxIter = new System.Windows.Forms.TextBox();
            this.textBoxCutPatternAnalysisMaxStep = new System.Windows.Forms.TextBox();
            this.labelCutPatternAnalysisMaxIter = new System.Windows.Forms.Label();
            this.labelCutPatternAnalysisName = new System.Windows.Forms.Label();
            this.labelCutPatternAnalysisTol = new System.Windows.Forms.Label();
            this.labelCutPatternAnalysisMaxStep = new System.Windows.Forms.Label();
            this.comboBoxAnalyses = new System.Windows.Forms.ComboBox();
            this.buttonRunAnalysis = new System.Windows.Forms.Button();
            this.buttonDeleteAnalysis = new System.Windows.Forms.Button();
            this.buttonCreateAnalysis = new System.Windows.Forms.Button();
            this.buttonModifyAnalysis = new System.Windows.Forms.Button();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxShowPreLoads = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPreSupports = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPreCouplings = new System.Windows.Forms.CheckBox();
            this.buttonResetInstance = new System.Windows.Forms.Button();
            this.buttonDeleteAll = new System.Windows.Forms.Button();
            this.checkBoxShowPreprocessing = new System.Windows.Forms.CheckBox();
            this.groupBoxMaterials = new System.Windows.Forms.GroupBox();
            this.buttonAddModifyMaterial = new System.Windows.Forms.Button();
            this.groupBoxProperties = new System.Windows.Forms.GroupBox();
            this.tabControlProperties = new System.Windows.Forms.TabControl();
            this.tabPageElement = new System.Windows.Forms.TabPage();
            this.buttonLoadElement = new System.Windows.Forms.Button();
            this.tabControlElement = new System.Windows.Forms.TabControl();
            this.tabPageElementMembrane = new System.Windows.Forms.TabPage();
            this.checkBoxElementMembraneFofi = new System.Windows.Forms.CheckBox();
            this.checkBoxElementMembraneEdgeCoupling = new System.Windows.Forms.CheckBox();
            this.textBoxMembranePrestress2 = new System.Windows.Forms.TextBox();
            this.labelMembranePrestress2 = new System.Windows.Forms.Label();
            this.textBoxMembranePrestress1 = new System.Windows.Forms.TextBox();
            this.labelMembraneThick = new System.Windows.Forms.Label();
            this.labelMembranePrestress1 = new System.Windows.Forms.Label();
            this.textBoxMembraneThick = new System.Windows.Forms.TextBox();
            this.tabPageElementShell = new System.Windows.Forms.TabPage();
            this.comboBoxShellType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelShellThick = new System.Windows.Forms.Label();
            this.textBoxShellThick = new System.Windows.Forms.TextBox();
            this.tabPageElementBeam = new System.Windows.Forms.TabPage();
            this.buttonAddAxis = new System.Windows.Forms.Button();
            this.labelBeamIy = new System.Windows.Forms.Label();
            this.labelBeamWidth = new System.Windows.Forms.Label();
            this.labelBeamIt = new System.Windows.Forms.Label();
            this.textBoxBeamIy = new System.Windows.Forms.TextBox();
            this.textBoxBeamArea = new System.Windows.Forms.TextBox();
            this.labelBeamIz = new System.Windows.Forms.Label();
            this.textBoxBeamIz = new System.Windows.Forms.TextBox();
            this.labelBeamDiameter = new System.Windows.Forms.Label();
            this.labelBeamHeight = new System.Windows.Forms.Label();
            this.textBoxBeamIt = new System.Windows.Forms.TextBox();
            this.labelBeamArea = new System.Windows.Forms.Label();
            this.labelBeamType = new System.Windows.Forms.Label();
            this.comboBoxBeamType = new System.Windows.Forms.ComboBox();
            this.tabPageElementCable = new System.Windows.Forms.TabPage();
            this.checkBoxElementCableFofi = new System.Windows.Forms.CheckBox();
            this.checkBoxCablePrestressCurve = new System.Windows.Forms.CheckBox();
            this.labelCablePrestress = new System.Windows.Forms.Label();
            this.textBoxCablePrestress = new System.Windows.Forms.TextBox();
            this.textBoxCableArea = new System.Windows.Forms.TextBox();
            this.comboBoxCableType = new System.Windows.Forms.ComboBox();
            this.labelCableArea = new System.Windows.Forms.Label();
            this.labelCableType = new System.Windows.Forms.Label();
            this.imageListTabControlElement = new System.Windows.Forms.ImageList(this.components);
            this.labelElementMat = new System.Windows.Forms.Label();
            this.comboBoxElementMat = new System.Windows.Forms.ComboBox();
            this.labelCouplingType = new System.Windows.Forms.Label();
            this.comboBoxCouplingType = new System.Windows.Forms.ComboBox();
            this.buttonDeleteElement = new System.Windows.Forms.Button();
            this.buttonAddElement = new System.Windows.Forms.Button();
            this.tabPageRefinement = new System.Windows.Forms.TabPage();
            this.radioButtonRefinementApproxElementSize = new System.Windows.Forms.RadioButton();
            this.radioButtonRefinementKnotSubdivision = new System.Windows.Forms.RadioButton();
            this.groupBoxRefinementElement = new System.Windows.Forms.GroupBox();
            this.radioButtonRefinementElementEdge = new System.Windows.Forms.RadioButton();
            this.radioButtonRefinementElementCurve = new System.Windows.Forms.RadioButton();
            this.radioButtonRefinementElementSurf = new System.Windows.Forms.RadioButton();
            this.buttonCheckRefinement = new System.Windows.Forms.Button();
            this.buttonChangeRefinement = new System.Windows.Forms.Button();
            this.textBoxKnotSubDivV = new System.Windows.Forms.TextBox();
            this.textBoxKnotSubDivU = new System.Windows.Forms.TextBox();
            this.textBoxQDeg = new System.Windows.Forms.TextBox();
            this.textBoxPDeg = new System.Windows.Forms.TextBox();
            this.labelRefinementMinV = new System.Windows.Forms.Label();
            this.labelRefinementMinU = new System.Windows.Forms.Label();
            this.labelRefinementQDeg = new System.Windows.Forms.Label();
            this.labelRefinementPDeg = new System.Windows.Forms.Label();
            this.tabPagePropSupport = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBoxSupportType = new System.Windows.Forms.ComboBox();
            this.groupBoxInterval = new System.Windows.Forms.GroupBox();
            this.textBoxSupportEndTime = new System.Windows.Forms.TextBox();
            this.textBoxSupportStartTime = new System.Windows.Forms.TextBox();
            this.checkBoxOverwriteSupport = new System.Windows.Forms.CheckBox();
            this.groupBoxSupportType = new System.Windows.Forms.GroupBox();
            this.textBoxSupportTypeDispZ = new System.Windows.Forms.TextBox();
            this.textBoxSupportTypeDispY = new System.Windows.Forms.TextBox();
            this.textBoxSupportTypeDispX = new System.Windows.Forms.TextBox();
            this.checkBoxSupportStrong = new System.Windows.Forms.CheckBox();
            this.checkBoxDispZ = new System.Windows.Forms.CheckBox();
            this.checkBoxDispY = new System.Windows.Forms.CheckBox();
            this.checkBoxRotationSupport = new System.Windows.Forms.CheckBox();
            this.checkBoxDispX = new System.Windows.Forms.CheckBox();
            this.groupBoxSupportDimension = new System.Windows.Forms.GroupBox();
            this.radioButtonSupportDimVertex = new System.Windows.Forms.RadioButton();
            this.radioButtonSupportDimLine = new System.Windows.Forms.RadioButton();
            this.radioButtonSupportDimFace = new System.Windows.Forms.RadioButton();
            this.groupBoxSupportElementType = new System.Windows.Forms.GroupBox();
            this.radioButtonSupportCurve = new System.Windows.Forms.RadioButton();
            this.radioButtonSupportSurface = new System.Windows.Forms.RadioButton();
            this.buttonDeleteEdgeSupport = new System.Windows.Forms.Button();
            this.buttonAddEdgeSupports = new System.Windows.Forms.Button();
            this.tabPageLoad = new System.Windows.Forms.TabPage();
            this.groupBoxLoadInterval = new System.Windows.Forms.GroupBox();
            this.textBoxLoadEndTime = new System.Windows.Forms.TextBox();
            this.textBoxLoadStartTime = new System.Windows.Forms.TextBox();
            this.checkBoxLoadOverwrite = new System.Windows.Forms.CheckBox();
            this.textBoxLoadPositionV = new System.Windows.Forms.TextBox();
            this.textBoxLoadPositionU = new System.Windows.Forms.TextBox();
            this.textBoxLoadZ = new System.Windows.Forms.TextBox();
            this.textBoxLoadY = new System.Windows.Forms.TextBox();
            this.textBoxLoadX = new System.Windows.Forms.TextBox();
            this.labelLoadDirectionZ = new System.Windows.Forms.Label();
            this.labelLoadDirectionY = new System.Windows.Forms.Label();
            this.labelLoadDirectionX = new System.Windows.Forms.Label();
            this.labelLoadPositionV = new System.Windows.Forms.Label();
            this.labelLoadPositionU = new System.Windows.Forms.Label();
            this.labelLoadPosition = new System.Windows.Forms.Label();
            this.labelLoadDirection = new System.Windows.Forms.Label();
            this.groupBoxLoadDimension = new System.Windows.Forms.GroupBox();
            this.radioButtonLoadDimVertex = new System.Windows.Forms.RadioButton();
            this.radioButtonLoadDimLine = new System.Windows.Forms.RadioButton();
            this.radioButtonLoadDimFace = new System.Windows.Forms.RadioButton();
            this.groupBoxLoadElement = new System.Windows.Forms.GroupBox();
            this.radioButtonLoadElementCurve = new System.Windows.Forms.RadioButton();
            this.radioButtonLoadElementSurface = new System.Windows.Forms.RadioButton();
            this.comboBoxLoadType = new System.Windows.Forms.ComboBox();
            this.buttonDeleteLoad = new System.Windows.Forms.Button();
            this.buttonAddLoad = new System.Windows.Forms.Button();
            this.tabPageCheck = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxCheckEndTime = new System.Windows.Forms.TextBox();
            this.textBoxCheckStartTime = new System.Windows.Forms.TextBox();
            this.checkBoxOverwriteChecks = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxOutputLagrangeMP = new System.Windows.Forms.CheckBox();
            this.checkBoxOutputDispZ = new System.Windows.Forms.CheckBox();
            this.checkBoxOutputDispX = new System.Windows.Forms.CheckBox();
            this.checkBoxOutputDispY = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonCheckVertex = new System.Windows.Forms.RadioButton();
            this.radioButtonCheckLine = new System.Windows.Forms.RadioButton();
            this.radioButtonCheckFace = new System.Windows.Forms.RadioButton();
            this.groupBoxCheckStructuralElement = new System.Windows.Forms.GroupBox();
            this.radioButtonCheckCurve = new System.Windows.Forms.RadioButton();
            this.radioButtonCheckSurface = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonAddCheck = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.open_file = new System.Windows.Forms.Button();
            this.buttonShowPost = new System.Windows.Forms.Button();
            this.buttonClearPost = new System.Windows.Forms.Button();
            this.groupBoxVisualization = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBoxShowMesh = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBoxPostProcessingDirection = new System.Windows.Forms.ComboBox();
            this.labelPostProcessingDirection = new System.Windows.Forms.Label();
            this.textBoxColorBarMin = new System.Windows.Forms.TextBox();
            this.textBoxColorBarMax = new System.Windows.Forms.TextBox();
            this.groupBoxScalings = new System.Windows.Forms.GroupBox();
            this.textBoxFlyingNodeLimit = new System.Windows.Forms.TextBox();
            this.textBoxResScale = new System.Windows.Forms.TextBox();
            this.textBoxDispScale = new System.Windows.Forms.TextBox();
            this.labelFlyingNodeLimit = new System.Windows.Forms.Label();
            this.labelDispScale = new System.Windows.Forms.Label();
            this.labelResScale = new System.Windows.Forms.Label();
            this.pictureBoxColorBar = new System.Windows.Forms.PictureBox();
            this.groupBoxShow = new System.Windows.Forms.GroupBox();
            this.checkBoxPrincipalStresses = new System.Windows.Forms.CheckBox();
            this.checkBoxPK2Stresses = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCauchyStresses = new System.Windows.Forms.CheckBox();
            this.checkBoxShowResults = new System.Windows.Forms.CheckBox();
            this.checkBoxShowKnots = new System.Windows.Forms.CheckBox();
            this.checkBoxShowUndeformed = new System.Windows.Forms.CheckBox();
            this.checkBoxShowGaussPoints = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCouplingPoints = new System.Windows.Forms.CheckBox();
            this.comboBoxResultType = new System.Windows.Forms.ComboBox();
            this.groupBoxAnalysisStep = new System.Windows.Forms.GroupBox();
            this.domainUpDownAnalysisStep = new System.Windows.Forms.DomainUpDown();
            this.trackBarAnalysisStep = new System.Windows.Forms.TrackBar();
            this.comboBoxLoadCaseType = new System.Windows.Forms.ComboBox();
            this.toolTipPanel = new System.Windows.Forms.ToolTip(this.components);
            this.CocodriloPlugInBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.propertySupportBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.checkBoxCouplingStresses = new System.Windows.Forms.CheckBox();
            this.TeDA_Plugin.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxAnalyses.SuspendLayout();
            this.tabControlAnalyses.SuspendLayout();
            this.tabPageFormfinding.SuspendLayout();
            this.tabPageLinStructuralAnalysis.SuspendLayout();
            this.tabPageNonLinStrucAnalysis.SuspendLayout();
            this.tabPageTransientAnalysis.SuspendLayout();
            this.tabPageEigenvalueAnalysis.SuspendLayout();
            this.tabPageCutPatternAnalysis.SuspendLayout();
            this.groupBoxOptions.SuspendLayout();
            this.groupBoxMaterials.SuspendLayout();
            this.groupBoxProperties.SuspendLayout();
            this.tabControlProperties.SuspendLayout();
            this.tabPageElement.SuspendLayout();
            this.tabControlElement.SuspendLayout();
            this.tabPageElementMembrane.SuspendLayout();
            this.tabPageElementShell.SuspendLayout();
            this.tabPageElementBeam.SuspendLayout();
            this.tabPageElementCable.SuspendLayout();
            this.tabPageRefinement.SuspendLayout();
            this.groupBoxRefinementElement.SuspendLayout();
            this.tabPagePropSupport.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBoxInterval.SuspendLayout();
            this.groupBoxSupportType.SuspendLayout();
            this.groupBoxSupportDimension.SuspendLayout();
            this.groupBoxSupportElementType.SuspendLayout();
            this.tabPageLoad.SuspendLayout();
            this.groupBoxLoadInterval.SuspendLayout();
            this.groupBoxLoadDimension.SuspendLayout();
            this.groupBoxLoadElement.SuspendLayout();
            this.tabPageCheck.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxCheckStructuralElement.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBoxVisualization.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBoxScalings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColorBar)).BeginInit();
            this.groupBoxShow.SuspendLayout();
            this.groupBoxAnalysisStep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAnalysisStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CocodriloPlugInBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertySupportBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // TeDA_Plugin
            // 
            this.TeDA_Plugin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TeDA_Plugin.Controls.Add(this.tabPage1);
            this.TeDA_Plugin.Controls.Add(this.tabPage2);
            this.TeDA_Plugin.Location = new System.Drawing.Point(0, 0);
            this.TeDA_Plugin.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TeDA_Plugin.Name = "TeDA_Plugin";
            this.TeDA_Plugin.SelectedIndex = 0;
            this.TeDA_Plugin.Size = new System.Drawing.Size(892, 1517);
            this.TeDA_Plugin.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBoxAnalyses);
            this.tabPage1.Controls.Add(this.groupBoxOptions);
            this.tabPage1.Controls.Add(this.groupBoxMaterials);
            this.tabPage1.Controls.Add(this.groupBoxProperties);
            this.tabPage1.Location = new System.Drawing.Point(8, 39);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage1.Size = new System.Drawing.Size(876, 1470);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Pre Processing";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBoxAnalyses
            // 
            this.groupBoxAnalyses.Controls.Add(this.radioButtonRunKratos);
            this.groupBoxAnalyses.Controls.Add(this.radioButtonRunCarat);
            this.groupBoxAnalyses.Controls.Add(this.buttonEditOutput);
            this.groupBoxAnalyses.Controls.Add(this.tabControlAnalyses);
            this.groupBoxAnalyses.Controls.Add(this.comboBoxAnalyses);
            this.groupBoxAnalyses.Controls.Add(this.buttonRunAnalysis);
            this.groupBoxAnalyses.Controls.Add(this.buttonDeleteAnalysis);
            this.groupBoxAnalyses.Controls.Add(this.buttonCreateAnalysis);
            this.groupBoxAnalyses.Controls.Add(this.buttonModifyAnalysis);
            this.groupBoxAnalyses.Location = new System.Drawing.Point(10, 663);
            this.groupBoxAnalyses.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxAnalyses.Name = "groupBoxAnalyses";
            this.groupBoxAnalyses.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxAnalyses.Size = new System.Drawing.Size(686, 537);
            this.groupBoxAnalyses.TabIndex = 99;
            this.groupBoxAnalyses.TabStop = false;
            this.groupBoxAnalyses.Text = "Analyses";
            // 
            // radioButtonRunKratos
            // 
            this.radioButtonRunKratos.AutoSize = true;
            this.radioButtonRunKratos.Checked = true;
            this.radioButtonRunKratos.Location = new System.Drawing.Point(146, 435);
            this.radioButtonRunKratos.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonRunKratos.Name = "radioButtonRunKratos";
            this.radioButtonRunKratos.Size = new System.Drawing.Size(105, 29);
            this.radioButtonRunKratos.TabIndex = 91;
            this.radioButtonRunKratos.TabStop = true;
            this.radioButtonRunKratos.Text = "Kratos";
            this.radioButtonRunKratos.UseVisualStyleBackColor = true;
            this.radioButtonRunKratos.CheckedChanged += new System.EventHandler(this.radioButtonRunKratos_CheckedChanged);
            // 
            // radioButtonRunCarat
            // 
            this.radioButtonRunCarat.AutoSize = true;
            this.radioButtonRunCarat.Enabled = false;
            this.radioButtonRunCarat.Location = new System.Drawing.Point(10, 435);
            this.radioButtonRunCarat.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonRunCarat.Name = "radioButtonRunCarat";
            this.radioButtonRunCarat.Size = new System.Drawing.Size(119, 29);
            this.radioButtonRunCarat.TabIndex = 90;
            this.radioButtonRunCarat.Text = "Carat++";
            this.radioButtonRunCarat.UseVisualStyleBackColor = true;
            this.radioButtonRunCarat.CheckedChanged += new System.EventHandler(this.radioButtonRunCarat_CheckedChanged);
            // 
            // buttonEditOutput
            // 
            this.buttonEditOutput.Location = new System.Drawing.Point(318, 477);
            this.buttonEditOutput.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonEditOutput.Name = "buttonEditOutput";
            this.buttonEditOutput.Size = new System.Drawing.Size(228, 46);
            this.buttonEditOutput.TabIndex = 89;
            this.buttonEditOutput.Text = "Edit Output Options";
            this.buttonEditOutput.UseVisualStyleBackColor = true;
            this.buttonEditOutput.Click += new System.EventHandler(this.buttonEditOutput_Click);
            // 
            // tabControlAnalyses
            // 
            this.tabControlAnalyses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlAnalyses.Controls.Add(this.tabPageFormfinding);
            this.tabControlAnalyses.Controls.Add(this.tabPageLinStructuralAnalysis);
            this.tabControlAnalyses.Controls.Add(this.tabPageNonLinStrucAnalysis);
            this.tabControlAnalyses.Controls.Add(this.tabPageTransientAnalysis);
            this.tabControlAnalyses.Controls.Add(this.tabPageEigenvalueAnalysis);
            this.tabControlAnalyses.Controls.Add(this.tabPageCutPatternAnalysis);
            this.tabControlAnalyses.Location = new System.Drawing.Point(8, 33);
            this.tabControlAnalyses.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControlAnalyses.Name = "tabControlAnalyses";
            this.tabControlAnalyses.SelectedIndex = 0;
            this.tabControlAnalyses.Size = new System.Drawing.Size(670, 288);
            this.tabControlAnalyses.TabIndex = 3;
            this.tabControlAnalyses.SelectedIndexChanged += new System.EventHandler(this.tabControlAnalyses_SelectedIndexChanged);
            // 
            // tabPageFormfinding
            // 
            this.tabPageFormfinding.Controls.Add(this.textBoxFormfindingName);
            this.tabPageFormfinding.Controls.Add(this.textBoxTolerance);
            this.tabPageFormfinding.Controls.Add(this.textBoxMaxIterations);
            this.tabPageFormfinding.Controls.Add(this.textBoxMaxSteps);
            this.tabPageFormfinding.Controls.Add(this.labelMaxIterattions);
            this.tabPageFormfinding.Controls.Add(this.labelAnalysisFofiName);
            this.tabPageFormfinding.Controls.Add(this.labelAccuracy);
            this.tabPageFormfinding.Controls.Add(this.labelMaxSteps);
            this.tabPageFormfinding.Location = new System.Drawing.Point(8, 39);
            this.tabPageFormfinding.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageFormfinding.Name = "tabPageFormfinding";
            this.tabPageFormfinding.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageFormfinding.Size = new System.Drawing.Size(654, 241);
            this.tabPageFormfinding.TabIndex = 0;
            this.tabPageFormfinding.Text = "Formfinding";
            this.tabPageFormfinding.UseVisualStyleBackColor = true;
            // 
            // textBoxFormfindingName
            // 
            this.textBoxFormfindingName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFormfindingName.Location = new System.Drawing.Point(166, 8);
            this.textBoxFormfindingName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxFormfindingName.Name = "textBoxFormfindingName";
            this.textBoxFormfindingName.Size = new System.Drawing.Size(448, 31);
            this.textBoxFormfindingName.TabIndex = 80;
            this.textBoxFormfindingName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFormfindingName_KeyPress);
            // 
            // textBoxTolerance
            // 
            this.textBoxTolerance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTolerance.Location = new System.Drawing.Point(166, 146);
            this.textBoxTolerance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxTolerance.Name = "textBoxTolerance";
            this.textBoxTolerance.Size = new System.Drawing.Size(448, 31);
            this.textBoxTolerance.TabIndex = 83;
            this.textBoxTolerance.Text = "0.001";
            this.textBoxTolerance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxTolerance_KeyPress);
            // 
            // textBoxMaxIterations
            // 
            this.textBoxMaxIterations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMaxIterations.Location = new System.Drawing.Point(166, 100);
            this.textBoxMaxIterations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxMaxIterations.Name = "textBoxMaxIterations";
            this.textBoxMaxIterations.Size = new System.Drawing.Size(448, 31);
            this.textBoxMaxIterations.TabIndex = 82;
            this.textBoxMaxIterations.Text = "1";
            this.textBoxMaxIterations.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMaxIterations_KeyPress);
            // 
            // textBoxMaxSteps
            // 
            this.textBoxMaxSteps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMaxSteps.Location = new System.Drawing.Point(166, 54);
            this.textBoxMaxSteps.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxMaxSteps.Name = "textBoxMaxSteps";
            this.textBoxMaxSteps.Size = new System.Drawing.Size(448, 31);
            this.textBoxMaxSteps.TabIndex = 81;
            this.textBoxMaxSteps.Text = "10";
            this.textBoxMaxSteps.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMaxSteps_KeyPress);
            // 
            // labelMaxIterattions
            // 
            this.labelMaxIterattions.AutoSize = true;
            this.labelMaxIterattions.Location = new System.Drawing.Point(8, 106);
            this.labelMaxIterattions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMaxIterattions.Name = "labelMaxIterattions";
            this.labelMaxIterattions.Size = new System.Drawing.Size(159, 25);
            this.labelMaxIterattions.TabIndex = 3;
            this.labelMaxIterattions.Text = "Max. Iterations:";
            // 
            // labelAnalysisFofiName
            // 
            this.labelAnalysisFofiName.AutoSize = true;
            this.labelAnalysisFofiName.Location = new System.Drawing.Point(8, 15);
            this.labelAnalysisFofiName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAnalysisFofiName.Name = "labelAnalysisFofiName";
            this.labelAnalysisFofiName.Size = new System.Drawing.Size(74, 25);
            this.labelAnalysisFofiName.TabIndex = 3;
            this.labelAnalysisFofiName.Text = "Name:";
            // 
            // labelAccuracy
            // 
            this.labelAccuracy.AutoSize = true;
            this.labelAccuracy.Location = new System.Drawing.Point(8, 152);
            this.labelAccuracy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAccuracy.Name = "labelAccuracy";
            this.labelAccuracy.Size = new System.Drawing.Size(114, 25);
            this.labelAccuracy.TabIndex = 3;
            this.labelAccuracy.Text = "Tolerance:";
            // 
            // labelMaxSteps
            // 
            this.labelMaxSteps.AutoSize = true;
            this.labelMaxSteps.Location = new System.Drawing.Point(8, 60);
            this.labelMaxSteps.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMaxSteps.Name = "labelMaxSteps";
            this.labelMaxSteps.Size = new System.Drawing.Size(126, 25);
            this.labelMaxSteps.TabIndex = 3;
            this.labelMaxSteps.Text = "Max. Steps:";
            // 
            // tabPageLinStructuralAnalysis
            // 
            this.tabPageLinStructuralAnalysis.Controls.Add(this.textBoxLinStrucAnalysisName);
            this.tabPageLinStructuralAnalysis.Controls.Add(this.labelAnalysisStaGeoName);
            this.tabPageLinStructuralAnalysis.Location = new System.Drawing.Point(8, 39);
            this.tabPageLinStructuralAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageLinStructuralAnalysis.Name = "tabPageLinStructuralAnalysis";
            this.tabPageLinStructuralAnalysis.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageLinStructuralAnalysis.Size = new System.Drawing.Size(654, 241);
            this.tabPageLinStructuralAnalysis.TabIndex = 1;
            this.tabPageLinStructuralAnalysis.Text = "LinStrucAnalysis";
            this.tabPageLinStructuralAnalysis.UseVisualStyleBackColor = true;
            // 
            // textBoxLinStrucAnalysisName
            // 
            this.textBoxLinStrucAnalysisName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLinStrucAnalysisName.Location = new System.Drawing.Point(166, 8);
            this.textBoxLinStrucAnalysisName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLinStrucAnalysisName.Name = "textBoxLinStrucAnalysisName";
            this.textBoxLinStrucAnalysisName.Size = new System.Drawing.Size(450, 31);
            this.textBoxLinStrucAnalysisName.TabIndex = 88;
            // 
            // labelAnalysisStaGeoName
            // 
            this.labelAnalysisStaGeoName.AutoSize = true;
            this.labelAnalysisStaGeoName.Location = new System.Drawing.Point(6, 15);
            this.labelAnalysisStaGeoName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAnalysisStaGeoName.Name = "labelAnalysisStaGeoName";
            this.labelAnalysisStaGeoName.Size = new System.Drawing.Size(74, 25);
            this.labelAnalysisStaGeoName.TabIndex = 87;
            this.labelAnalysisStaGeoName.Text = "Name:";
            // 
            // tabPageNonLinStrucAnalysis
            // 
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.textBoxNonLinStruAnalysisStepSize);
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.labelNonLinAnalysisStepSize);
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.textBoxNonLinStrucAnalysisAdapStepCntrl);
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.labelNonLinStrucAnalysisAdapStepCntrl);
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.labelNonLinStrucAnalysisNumIter);
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.textBoxNonLinStrucAnalysisNumIter);
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.labelNonLinStrucAnalysisNumSteps);
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.textBoxNonLinStrucAnalysisNumSteps);
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.textBoxNonLinStrucAnalysisAcc);
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.labelNonLinStrucAnalysisAcc);
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.textBoxNonLinStrucAnalysisName);
            this.tabPageNonLinStrucAnalysis.Controls.Add(this.labelNonLinStrucAnalysisName);
            this.tabPageNonLinStrucAnalysis.Location = new System.Drawing.Point(8, 39);
            this.tabPageNonLinStrucAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageNonLinStrucAnalysis.Name = "tabPageNonLinStrucAnalysis";
            this.tabPageNonLinStrucAnalysis.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageNonLinStrucAnalysis.Size = new System.Drawing.Size(654, 241);
            this.tabPageNonLinStrucAnalysis.TabIndex = 2;
            this.tabPageNonLinStrucAnalysis.Text = "NonLinStrucAnalysis";
            this.tabPageNonLinStrucAnalysis.UseVisualStyleBackColor = true;
            // 
            // textBoxNonLinStruAnalysisStepSize
            // 
            this.textBoxNonLinStruAnalysisStepSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNonLinStruAnalysisStepSize.Location = new System.Drawing.Point(428, 54);
            this.textBoxNonLinStruAnalysisStepSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxNonLinStruAnalysisStepSize.Name = "textBoxNonLinStruAnalysisStepSize";
            this.textBoxNonLinStruAnalysisStepSize.Size = new System.Drawing.Size(190, 31);
            this.textBoxNonLinStruAnalysisStepSize.TabIndex = 102;
            this.textBoxNonLinStruAnalysisStepSize.Text = "0.1";
            // 
            // labelNonLinAnalysisStepSize
            // 
            this.labelNonLinAnalysisStepSize.AutoSize = true;
            this.labelNonLinAnalysisStepSize.Location = new System.Drawing.Point(310, 60);
            this.labelNonLinAnalysisStepSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNonLinAnalysisStepSize.Name = "labelNonLinAnalysisStepSize";
            this.labelNonLinAnalysisStepSize.Size = new System.Drawing.Size(110, 25);
            this.labelNonLinAnalysisStepSize.TabIndex = 101;
            this.labelNonLinAnalysisStepSize.Text = "Step Size:";
            // 
            // textBoxNonLinStrucAnalysisAdapStepCntrl
            // 
            this.textBoxNonLinStrucAnalysisAdapStepCntrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNonLinStrucAnalysisAdapStepCntrl.Location = new System.Drawing.Point(166, 198);
            this.textBoxNonLinStrucAnalysisAdapStepCntrl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxNonLinStrucAnalysisAdapStepCntrl.Name = "textBoxNonLinStrucAnalysisAdapStepCntrl";
            this.textBoxNonLinStrucAnalysisAdapStepCntrl.Size = new System.Drawing.Size(450, 31);
            this.textBoxNonLinStrucAnalysisAdapStepCntrl.TabIndex = 100;
            this.textBoxNonLinStrucAnalysisAdapStepCntrl.Text = "0";
            // 
            // labelNonLinStrucAnalysisAdapStepCntrl
            // 
            this.labelNonLinStrucAnalysisAdapStepCntrl.AutoSize = true;
            this.labelNonLinStrucAnalysisAdapStepCntrl.Location = new System.Drawing.Point(8, 204);
            this.labelNonLinStrucAnalysisAdapStepCntrl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelNonLinStrucAnalysisAdapStepCntrl.Name = "labelNonLinStrucAnalysisAdapStepCntrl";
            this.labelNonLinStrucAnalysisAdapStepCntrl.Size = new System.Drawing.Size(163, 25);
            this.labelNonLinStrucAnalysisAdapStepCntrl.TabIndex = 99;
            this.labelNonLinStrucAnalysisAdapStepCntrl.Text = "Adaptive Steps:";
            // 
            // labelNonLinStrucAnalysisNumIter
            // 
            this.labelNonLinStrucAnalysisNumIter.AutoSize = true;
            this.labelNonLinStrucAnalysisNumIter.Location = new System.Drawing.Point(8, 106);
            this.labelNonLinStrucAnalysisNumIter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNonLinStrucAnalysisNumIter.Name = "labelNonLinStrucAnalysisNumIter";
            this.labelNonLinStrucAnalysisNumIter.Size = new System.Drawing.Size(159, 25);
            this.labelNonLinStrucAnalysisNumIter.TabIndex = 98;
            this.labelNonLinStrucAnalysisNumIter.Text = "Max. Iterations:";
            // 
            // textBoxNonLinStrucAnalysisNumIter
            // 
            this.textBoxNonLinStrucAnalysisNumIter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNonLinStrucAnalysisNumIter.Location = new System.Drawing.Point(166, 100);
            this.textBoxNonLinStrucAnalysisNumIter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxNonLinStrucAnalysisNumIter.Name = "textBoxNonLinStrucAnalysisNumIter";
            this.textBoxNonLinStrucAnalysisNumIter.Size = new System.Drawing.Size(452, 31);
            this.textBoxNonLinStrucAnalysisNumIter.TabIndex = 97;
            this.textBoxNonLinStrucAnalysisNumIter.Text = "100";
            // 
            // labelNonLinStrucAnalysisNumSteps
            // 
            this.labelNonLinStrucAnalysisNumSteps.AutoSize = true;
            this.labelNonLinStrucAnalysisNumSteps.Location = new System.Drawing.Point(8, 60);
            this.labelNonLinStrucAnalysisNumSteps.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNonLinStrucAnalysisNumSteps.Name = "labelNonLinStrucAnalysisNumSteps";
            this.labelNonLinStrucAnalysisNumSteps.Size = new System.Drawing.Size(129, 25);
            this.labelNonLinStrucAnalysisNumSteps.TabIndex = 96;
            this.labelNonLinStrucAnalysisNumSteps.Text = "Num. Steps:";
            // 
            // textBoxNonLinStrucAnalysisNumSteps
            // 
            this.textBoxNonLinStrucAnalysisNumSteps.Location = new System.Drawing.Point(166, 54);
            this.textBoxNonLinStrucAnalysisNumSteps.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxNonLinStrucAnalysisNumSteps.Name = "textBoxNonLinStrucAnalysisNumSteps";
            this.textBoxNonLinStrucAnalysisNumSteps.Size = new System.Drawing.Size(132, 31);
            this.textBoxNonLinStrucAnalysisNumSteps.TabIndex = 95;
            this.textBoxNonLinStrucAnalysisNumSteps.Text = "1";
            // 
            // textBoxNonLinStrucAnalysisAcc
            // 
            this.textBoxNonLinStrucAnalysisAcc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNonLinStrucAnalysisAcc.Location = new System.Drawing.Point(166, 148);
            this.textBoxNonLinStrucAnalysisAcc.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxNonLinStrucAnalysisAcc.Name = "textBoxNonLinStrucAnalysisAcc";
            this.textBoxNonLinStrucAnalysisAcc.Size = new System.Drawing.Size(450, 31);
            this.textBoxNonLinStrucAnalysisAcc.TabIndex = 94;
            this.textBoxNonLinStrucAnalysisAcc.Text = "0.001";
            // 
            // labelNonLinStrucAnalysisAcc
            // 
            this.labelNonLinStrucAnalysisAcc.AutoSize = true;
            this.labelNonLinStrucAnalysisAcc.Location = new System.Drawing.Point(8, 154);
            this.labelNonLinStrucAnalysisAcc.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelNonLinStrucAnalysisAcc.Name = "labelNonLinStrucAnalysisAcc";
            this.labelNonLinStrucAnalysisAcc.Size = new System.Drawing.Size(114, 25);
            this.labelNonLinStrucAnalysisAcc.TabIndex = 93;
            this.labelNonLinStrucAnalysisAcc.Text = "Tolerance:";
            // 
            // textBoxNonLinStrucAnalysisName
            // 
            this.textBoxNonLinStrucAnalysisName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNonLinStrucAnalysisName.Location = new System.Drawing.Point(166, 8);
            this.textBoxNonLinStrucAnalysisName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxNonLinStrucAnalysisName.Name = "textBoxNonLinStrucAnalysisName";
            this.textBoxNonLinStrucAnalysisName.Size = new System.Drawing.Size(452, 31);
            this.textBoxNonLinStrucAnalysisName.TabIndex = 88;
            // 
            // labelNonLinStrucAnalysisName
            // 
            this.labelNonLinStrucAnalysisName.AutoSize = true;
            this.labelNonLinStrucAnalysisName.Location = new System.Drawing.Point(8, 15);
            this.labelNonLinStrucAnalysisName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNonLinStrucAnalysisName.Name = "labelNonLinStrucAnalysisName";
            this.labelNonLinStrucAnalysisName.Size = new System.Drawing.Size(74, 25);
            this.labelNonLinStrucAnalysisName.TabIndex = 87;
            this.labelNonLinStrucAnalysisName.Text = "Name:";
            // 
            // tabPageTransientAnalysis
            // 
            this.tabPageTransientAnalysis.AutoScroll = true;
            this.tabPageTransientAnalysis.Controls.Add(this.textBoxTransientAnalysisStepSize);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisStepSize);
            this.tabPageTransientAnalysis.Controls.Add(this.textBoxTransientAnalysisAdapStepCntrl);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisAdapStepCntrl);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisNumIter);
            this.tabPageTransientAnalysis.Controls.Add(this.textBoxTransientAnalysisNumIter);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisNumSteps);
            this.tabPageTransientAnalysis.Controls.Add(this.textBoxTransientAnalysisNumSteps);
            this.tabPageTransientAnalysis.Controls.Add(this.textBoxTransientAnalysisAcc);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisAcc);
            this.tabPageTransientAnalysis.Controls.Add(this.textBoxTransientAnalysisRayleighAlpha);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisRayleighAlpha);
            this.tabPageTransientAnalysis.Controls.Add(this.textBoxTransientAnalysisRayleighBeta);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisRayleighBeta);
            this.tabPageTransientAnalysis.Controls.Add(this.textBoxTransientAnalysisName);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisName);
            this.tabPageTransientAnalysis.Controls.Add(this.comboBoxTransientAnalysisTimeIntegration);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisTimeIntegration);
            this.tabPageTransientAnalysis.Controls.Add(this.comboBoxTransientAnalysisScheme);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisScheme);
            this.tabPageTransientAnalysis.Controls.Add(this.checkBoxTransientAnalysisAutomaticRayleigh);
            this.tabPageTransientAnalysis.Controls.Add(this.textBoxTransientAnalysisDampingRatio0);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisDampingRatio0);
            this.tabPageTransientAnalysis.Controls.Add(this.textBoxTransientAnalysisDampingRatio1);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisDampingRatio1);
            this.tabPageTransientAnalysis.Controls.Add(this.textBoxTransientAnalysisNumEigen);
            this.tabPageTransientAnalysis.Controls.Add(this.labelTransientAnalysisNumEigen);
            this.tabPageTransientAnalysis.Location = new System.Drawing.Point(8, 39);
            this.tabPageTransientAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageTransientAnalysis.Name = "tabPageTransientAnalysis";
            this.tabPageTransientAnalysis.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageTransientAnalysis.Size = new System.Drawing.Size(654, 241);
            this.tabPageTransientAnalysis.TabIndex = 2;
            this.tabPageTransientAnalysis.Text = "TransientAnalysis";
            this.tabPageTransientAnalysis.UseVisualStyleBackColor = true;
            // 
            // textBoxTransientAnalysisStepSize
            // 
            this.textBoxTransientAnalysisStepSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTransientAnalysisStepSize.Location = new System.Drawing.Point(428, 54);
            this.textBoxTransientAnalysisStepSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxTransientAnalysisStepSize.Name = "textBoxTransientAnalysisStepSize";
            this.textBoxTransientAnalysisStepSize.Size = new System.Drawing.Size(451, 31);
            this.textBoxTransientAnalysisStepSize.TabIndex = 102;
            this.textBoxTransientAnalysisStepSize.Text = "0.1";
            // 
            // labelTransientAnalysisStepSize
            // 
            this.labelTransientAnalysisStepSize.AutoSize = true;
            this.labelTransientAnalysisStepSize.Location = new System.Drawing.Point(310, 60);
            this.labelTransientAnalysisStepSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTransientAnalysisStepSize.Name = "labelTransientAnalysisStepSize";
            this.labelTransientAnalysisStepSize.Size = new System.Drawing.Size(110, 25);
            this.labelTransientAnalysisStepSize.TabIndex = 101;
            this.labelTransientAnalysisStepSize.Text = "Step Size:";
            // 
            // textBoxTransientAnalysisAdapStepCntrl
            // 
            this.textBoxTransientAnalysisAdapStepCntrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTransientAnalysisAdapStepCntrl.Location = new System.Drawing.Point(166, 198);
            this.textBoxTransientAnalysisAdapStepCntrl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxTransientAnalysisAdapStepCntrl.Name = "textBoxTransientAnalysisAdapStepCntrl";
            this.textBoxTransientAnalysisAdapStepCntrl.Size = new System.Drawing.Size(451, 31);
            this.textBoxTransientAnalysisAdapStepCntrl.TabIndex = 100;
            this.textBoxTransientAnalysisAdapStepCntrl.Text = "0";
            // 
            // labelTransientAnalysisAdapStepCntrl
            // 
            this.labelTransientAnalysisAdapStepCntrl.AutoSize = true;
            this.labelTransientAnalysisAdapStepCntrl.Location = new System.Drawing.Point(8, 204);
            this.labelTransientAnalysisAdapStepCntrl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelTransientAnalysisAdapStepCntrl.Name = "labelTransientAnalysisAdapStepCntrl";
            this.labelTransientAnalysisAdapStepCntrl.Size = new System.Drawing.Size(163, 25);
            this.labelTransientAnalysisAdapStepCntrl.TabIndex = 99;
            this.labelTransientAnalysisAdapStepCntrl.Text = "Adaptive Steps:";
            // 
            // labelTransientAnalysisNumIter
            // 
            this.labelTransientAnalysisNumIter.AutoSize = true;
            this.labelTransientAnalysisNumIter.Location = new System.Drawing.Point(8, 106);
            this.labelTransientAnalysisNumIter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTransientAnalysisNumIter.Name = "labelTransientAnalysisNumIter";
            this.labelTransientAnalysisNumIter.Size = new System.Drawing.Size(159, 25);
            this.labelTransientAnalysisNumIter.TabIndex = 98;
            this.labelTransientAnalysisNumIter.Text = "Max. Iterations:";
            // 
            // textBoxTransientAnalysisNumIter
            // 
            this.textBoxTransientAnalysisNumIter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTransientAnalysisNumIter.Location = new System.Drawing.Point(166, 100);
            this.textBoxTransientAnalysisNumIter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxTransientAnalysisNumIter.Name = "textBoxTransientAnalysisNumIter";
            this.textBoxTransientAnalysisNumIter.Size = new System.Drawing.Size(451, 31);
            this.textBoxTransientAnalysisNumIter.TabIndex = 97;
            this.textBoxTransientAnalysisNumIter.Text = "100";
            // 
            // labelTransientAnalysisNumSteps
            // 
            this.labelTransientAnalysisNumSteps.AutoSize = true;
            this.labelTransientAnalysisNumSteps.Location = new System.Drawing.Point(8, 60);
            this.labelTransientAnalysisNumSteps.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTransientAnalysisNumSteps.Name = "labelTransientAnalysisNumSteps";
            this.labelTransientAnalysisNumSteps.Size = new System.Drawing.Size(129, 25);
            this.labelTransientAnalysisNumSteps.TabIndex = 96;
            this.labelTransientAnalysisNumSteps.Text = "Num. Steps:";
            // 
            // textBoxTransientAnalysisNumSteps
            // 
            this.textBoxTransientAnalysisNumSteps.Location = new System.Drawing.Point(166, 54);
            this.textBoxTransientAnalysisNumSteps.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxTransientAnalysisNumSteps.Name = "textBoxTransientAnalysisNumSteps";
            this.textBoxTransientAnalysisNumSteps.Size = new System.Drawing.Size(132, 31);
            this.textBoxTransientAnalysisNumSteps.TabIndex = 95;
            this.textBoxTransientAnalysisNumSteps.Text = "1";
            // 
            // textBoxTransientAnalysisAcc
            // 
            this.textBoxTransientAnalysisAcc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTransientAnalysisAcc.Location = new System.Drawing.Point(166, 148);
            this.textBoxTransientAnalysisAcc.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxTransientAnalysisAcc.Name = "textBoxTransientAnalysisAcc";
            this.textBoxTransientAnalysisAcc.Size = new System.Drawing.Size(451, 31);
            this.textBoxTransientAnalysisAcc.TabIndex = 94;
            this.textBoxTransientAnalysisAcc.Text = "0.001";
            // 
            // labelTransientAnalysisAcc
            // 
            this.labelTransientAnalysisAcc.AutoSize = true;
            this.labelTransientAnalysisAcc.Location = new System.Drawing.Point(8, 154);
            this.labelTransientAnalysisAcc.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelTransientAnalysisAcc.Name = "labelTransientAnalysisAcc";
            this.labelTransientAnalysisAcc.Size = new System.Drawing.Size(114, 25);
            this.labelTransientAnalysisAcc.TabIndex = 93;
            this.labelTransientAnalysisAcc.Text = "Tolerance:";
            // 
            // textBoxTransientAnalysisRayleighAlpha
            // 
            this.textBoxTransientAnalysisRayleighAlpha.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTransientAnalysisRayleighAlpha.Location = new System.Drawing.Point(166, 348);
            this.textBoxTransientAnalysisRayleighAlpha.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.textBoxTransientAnalysisRayleighAlpha.Name = "textBoxTransientAnalysisRayleighAlpha";
            this.textBoxTransientAnalysisRayleighAlpha.Size = new System.Drawing.Size(451, 31);
            this.textBoxTransientAnalysisRayleighAlpha.TabIndex = 94;
            this.textBoxTransientAnalysisRayleighAlpha.Text = "1.0";
            // 
            // labelTransientAnalysisRayleighAlpha
            // 
            this.labelTransientAnalysisRayleighAlpha.AutoSize = true;
            this.labelTransientAnalysisRayleighAlpha.Location = new System.Drawing.Point(8, 354);
            this.labelTransientAnalysisRayleighAlpha.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelTransientAnalysisRayleighAlpha.Name = "labelTransientAnalysisRayleighAlpha";
            this.labelTransientAnalysisRayleighAlpha.Size = new System.Drawing.Size(161, 25);
            this.labelTransientAnalysisRayleighAlpha.TabIndex = 93;
            this.labelTransientAnalysisRayleighAlpha.Text = "Rayleigh alpha:";
            // 
            // textBoxTransientAnalysisRayleighBeta
            // 
            this.textBoxTransientAnalysisRayleighBeta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTransientAnalysisRayleighBeta.Location = new System.Drawing.Point(166, 398);
            this.textBoxTransientAnalysisRayleighBeta.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.textBoxTransientAnalysisRayleighBeta.Name = "textBoxTransientAnalysisRayleighBeta";
            this.textBoxTransientAnalysisRayleighBeta.Size = new System.Drawing.Size(451, 31);
            this.textBoxTransientAnalysisRayleighBeta.TabIndex = 94;
            this.textBoxTransientAnalysisRayleighBeta.Text = "1.0";
            // 
            // labelTransientAnalysisRayleighBeta
            // 
            this.labelTransientAnalysisRayleighBeta.AutoSize = true;
            this.labelTransientAnalysisRayleighBeta.Location = new System.Drawing.Point(8, 404);
            this.labelTransientAnalysisRayleighBeta.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelTransientAnalysisRayleighBeta.Name = "labelTransientAnalysisRayleighBeta";
            this.labelTransientAnalysisRayleighBeta.Size = new System.Drawing.Size(150, 25);
            this.labelTransientAnalysisRayleighBeta.TabIndex = 93;
            this.labelTransientAnalysisRayleighBeta.Text = "Rayleigh beta:";
            // 
            // textBoxTransientAnalysisName
            // 
            this.textBoxTransientAnalysisName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTransientAnalysisName.Location = new System.Drawing.Point(166, 8);
            this.textBoxTransientAnalysisName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxTransientAnalysisName.Name = "textBoxTransientAnalysisName";
            this.textBoxTransientAnalysisName.Size = new System.Drawing.Size(451, 31);
            this.textBoxTransientAnalysisName.TabIndex = 88;
            // 
            // labelTransientAnalysisName
            // 
            this.labelTransientAnalysisName.AutoSize = true;
            this.labelTransientAnalysisName.Location = new System.Drawing.Point(8, 15);
            this.labelTransientAnalysisName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTransientAnalysisName.Name = "labelTransientAnalysisName";
            this.labelTransientAnalysisName.Size = new System.Drawing.Size(74, 25);
            this.labelTransientAnalysisName.TabIndex = 87;
            this.labelTransientAnalysisName.Text = "Name:";
            // 
            // comboBoxTransientAnalysisTimeIntegration
            // 
            this.comboBoxTransientAnalysisTimeIntegration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTransientAnalysisTimeIntegration.AutoCompleteCustomSource.AddRange(new string[] {
            "implicit",
            "explicit"});
            this.comboBoxTransientAnalysisTimeIntegration.FormattingEnabled = true;
            this.comboBoxTransientAnalysisTimeIntegration.Items.AddRange(new object[] {
            "implicit",
            "explicit"});
            this.comboBoxTransientAnalysisTimeIntegration.Location = new System.Drawing.Point(166, 248);
            this.comboBoxTransientAnalysisTimeIntegration.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.comboBoxTransientAnalysisTimeIntegration.Name = "comboBoxTransientAnalysisTimeIntegration";
            this.comboBoxTransientAnalysisTimeIntegration.Size = new System.Drawing.Size(451, 33);
            this.comboBoxTransientAnalysisTimeIntegration.TabIndex = 22;
            this.comboBoxTransientAnalysisTimeIntegration.Text = "implicit";
            // 
            // labelTransientAnalysisTimeIntegration
            // 
            this.labelTransientAnalysisTimeIntegration.AutoSize = true;
            this.labelTransientAnalysisTimeIntegration.Location = new System.Drawing.Point(8, 254);
            this.labelTransientAnalysisTimeIntegration.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelTransientAnalysisTimeIntegration.Name = "labelTransientAnalysisTimeIntegration";
            this.labelTransientAnalysisTimeIntegration.Size = new System.Drawing.Size(124, 25);
            this.labelTransientAnalysisTimeIntegration.TabIndex = 7;
            this.labelTransientAnalysisTimeIntegration.Text = "Time integ.:";
            // 
            // comboBoxTransientAnalysisScheme
            // 
            this.comboBoxTransientAnalysisScheme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTransientAnalysisScheme.AutoCompleteCustomSource.AddRange(new string[] {
            "newmark",
            "bossak",
            "bdf2"});
            this.comboBoxTransientAnalysisScheme.FormattingEnabled = true;
            this.comboBoxTransientAnalysisScheme.Items.AddRange(new object[] {
            "newmark",
            "bossak",
            "bdf2"});
            this.comboBoxTransientAnalysisScheme.Location = new System.Drawing.Point(166, 298);
            this.comboBoxTransientAnalysisScheme.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.comboBoxTransientAnalysisScheme.Name = "comboBoxTransientAnalysisScheme";
            this.comboBoxTransientAnalysisScheme.Size = new System.Drawing.Size(451, 33);
            this.comboBoxTransientAnalysisScheme.TabIndex = 22;
            this.comboBoxTransientAnalysisScheme.Text = "newmark";
            // 
            // labelTransientAnalysisScheme
            // 
            this.labelTransientAnalysisScheme.AutoSize = true;
            this.labelTransientAnalysisScheme.Location = new System.Drawing.Point(8, 304);
            this.labelTransientAnalysisScheme.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelTransientAnalysisScheme.Name = "labelTransientAnalysisScheme";
            this.labelTransientAnalysisScheme.Size = new System.Drawing.Size(96, 25);
            this.labelTransientAnalysisScheme.TabIndex = 7;
            this.labelTransientAnalysisScheme.Text = "Scheme:";
            // 
            // checkBoxTransientAnalysisAutomaticRayleigh
            // 
            this.checkBoxTransientAnalysisAutomaticRayleigh.AutoSize = true;
            this.checkBoxTransientAnalysisAutomaticRayleigh.Location = new System.Drawing.Point(12, 448);
            this.checkBoxTransientAnalysisAutomaticRayleigh.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxTransientAnalysisAutomaticRayleigh.Name = "checkBoxTransientAnalysisAutomaticRayleigh";
            this.checkBoxTransientAnalysisAutomaticRayleigh.Size = new System.Drawing.Size(345, 29);
            this.checkBoxTransientAnalysisAutomaticRayleigh.TabIndex = 92;
            this.checkBoxTransientAnalysisAutomaticRayleigh.Text = "Automatic Rayleigh Parameters";
            this.checkBoxTransientAnalysisAutomaticRayleigh.UseVisualStyleBackColor = true;
            // 
            // textBoxTransientAnalysisDampingRatio0
            // 
            this.textBoxTransientAnalysisDampingRatio0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTransientAnalysisDampingRatio0.Location = new System.Drawing.Point(166, 498);
            this.textBoxTransientAnalysisDampingRatio0.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.textBoxTransientAnalysisDampingRatio0.Name = "textBoxTransientAnalysisDampingRatio0";
            this.textBoxTransientAnalysisDampingRatio0.Size = new System.Drawing.Size(451, 31);
            this.textBoxTransientAnalysisDampingRatio0.TabIndex = 94;
            this.textBoxTransientAnalysisDampingRatio0.Text = "0.001";
            // 
            // labelTransientAnalysisDampingRatio0
            // 
            this.labelTransientAnalysisDampingRatio0.AutoSize = true;
            this.labelTransientAnalysisDampingRatio0.Location = new System.Drawing.Point(8, 504);
            this.labelTransientAnalysisDampingRatio0.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelTransientAnalysisDampingRatio0.Name = "labelTransientAnalysisDampingRatio0";
            this.labelTransientAnalysisDampingRatio0.Size = new System.Drawing.Size(155, 25);
            this.labelTransientAnalysisDampingRatio0.TabIndex = 93;
            this.labelTransientAnalysisDampingRatio0.Text = "Damping val 0:";
            // 
            // textBoxTransientAnalysisDampingRatio1
            // 
            this.textBoxTransientAnalysisDampingRatio1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTransientAnalysisDampingRatio1.Location = new System.Drawing.Point(166, 548);
            this.textBoxTransientAnalysisDampingRatio1.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.textBoxTransientAnalysisDampingRatio1.Name = "textBoxTransientAnalysisDampingRatio1";
            this.textBoxTransientAnalysisDampingRatio1.Size = new System.Drawing.Size(451, 31);
            this.textBoxTransientAnalysisDampingRatio1.TabIndex = 94;
            this.textBoxTransientAnalysisDampingRatio1.Text = "-1.0";
            // 
            // labelTransientAnalysisDampingRatio1
            // 
            this.labelTransientAnalysisDampingRatio1.AutoSize = true;
            this.labelTransientAnalysisDampingRatio1.Location = new System.Drawing.Point(8, 554);
            this.labelTransientAnalysisDampingRatio1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelTransientAnalysisDampingRatio1.Name = "labelTransientAnalysisDampingRatio1";
            this.labelTransientAnalysisDampingRatio1.Size = new System.Drawing.Size(155, 25);
            this.labelTransientAnalysisDampingRatio1.TabIndex = 93;
            this.labelTransientAnalysisDampingRatio1.Text = "Damping val 1:";
            // 
            // textBoxTransientAnalysisNumEigen
            // 
            this.textBoxTransientAnalysisNumEigen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTransientAnalysisNumEigen.Location = new System.Drawing.Point(166, 598);
            this.textBoxTransientAnalysisNumEigen.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.textBoxTransientAnalysisNumEigen.Name = "textBoxTransientAnalysisNumEigen";
            this.textBoxTransientAnalysisNumEigen.Size = new System.Drawing.Size(451, 31);
            this.textBoxTransientAnalysisNumEigen.TabIndex = 94;
            this.textBoxTransientAnalysisNumEigen.Text = "15";
            // 
            // labelTransientAnalysisNumEigen
            // 
            this.labelTransientAnalysisNumEigen.AutoSize = true;
            this.labelTransientAnalysisNumEigen.Location = new System.Drawing.Point(8, 604);
            this.labelTransientAnalysisNumEigen.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelTransientAnalysisNumEigen.Name = "labelTransientAnalysisNumEigen";
            this.labelTransientAnalysisNumEigen.Size = new System.Drawing.Size(129, 25);
            this.labelTransientAnalysisNumEigen.TabIndex = 93;
            this.labelTransientAnalysisNumEigen.Text = "Num. Eigen:";
            // 
            // tabPageEigenvalueAnalysis
            // 
            this.tabPageEigenvalueAnalysis.AutoScroll = true;
            this.tabPageEigenvalueAnalysis.Controls.Add(this.labelEigenvalueAnalysisNumIter);
            this.tabPageEigenvalueAnalysis.Controls.Add(this.textBoxEigenvalueAnalysisNumIter);
            this.tabPageEigenvalueAnalysis.Controls.Add(this.labelEigenvalueAnalysisNumEigen);
            this.tabPageEigenvalueAnalysis.Controls.Add(this.textBoxEigenvalueAnalysisNumEigen);
            this.tabPageEigenvalueAnalysis.Controls.Add(this.textBoxEigenvalueAnalysisAcc);
            this.tabPageEigenvalueAnalysis.Controls.Add(this.labelEigenvalueAnalysisAcc);
            this.tabPageEigenvalueAnalysis.Controls.Add(this.textBoxEigenvalueAnalysisName);
            this.tabPageEigenvalueAnalysis.Controls.Add(this.labelEigenvalueAnalysisName);
            this.tabPageEigenvalueAnalysis.Controls.Add(this.comboBoxEigenvalueAnalysisSolverType);
            this.tabPageEigenvalueAnalysis.Controls.Add(this.labelEigenvalueAnalysisSolverType);
            this.tabPageEigenvalueAnalysis.Location = new System.Drawing.Point(8, 39);
            this.tabPageEigenvalueAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageEigenvalueAnalysis.Name = "tabPageEigenvalueAnalysis";
            this.tabPageEigenvalueAnalysis.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageEigenvalueAnalysis.Size = new System.Drawing.Size(654, 241);
            this.tabPageEigenvalueAnalysis.TabIndex = 2;
            this.tabPageEigenvalueAnalysis.Text = "EigenvalueAnalysis";
            this.tabPageEigenvalueAnalysis.UseVisualStyleBackColor = true;
            // 
            // labelEigenvalueAnalysisNumIter
            // 
            this.labelEigenvalueAnalysisNumIter.AutoSize = true;
            this.labelEigenvalueAnalysisNumIter.Location = new System.Drawing.Point(8, 106);
            this.labelEigenvalueAnalysisNumIter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEigenvalueAnalysisNumIter.Name = "labelEigenvalueAnalysisNumIter";
            this.labelEigenvalueAnalysisNumIter.Size = new System.Drawing.Size(159, 25);
            this.labelEigenvalueAnalysisNumIter.TabIndex = 98;
            this.labelEigenvalueAnalysisNumIter.Text = "Max. Iterations:";
            // 
            // textBoxEigenvalueAnalysisNumIter
            // 
            this.textBoxEigenvalueAnalysisNumIter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEigenvalueAnalysisNumIter.Location = new System.Drawing.Point(166, 100);
            this.textBoxEigenvalueAnalysisNumIter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxEigenvalueAnalysisNumIter.Name = "textBoxEigenvalueAnalysisNumIter";
            this.textBoxEigenvalueAnalysisNumIter.Size = new System.Drawing.Size(778, 31);
            this.textBoxEigenvalueAnalysisNumIter.TabIndex = 97;
            this.textBoxEigenvalueAnalysisNumIter.Text = "100";
            // 
            // labelEigenvalueAnalysisNumEigen
            // 
            this.labelEigenvalueAnalysisNumEigen.AutoSize = true;
            this.labelEigenvalueAnalysisNumEigen.Location = new System.Drawing.Point(8, 60);
            this.labelEigenvalueAnalysisNumEigen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEigenvalueAnalysisNumEigen.Name = "labelEigenvalueAnalysisNumEigen";
            this.labelEigenvalueAnalysisNumEigen.Size = new System.Drawing.Size(129, 25);
            this.labelEigenvalueAnalysisNumEigen.TabIndex = 96;
            this.labelEigenvalueAnalysisNumEigen.Text = "Num. Eigen:";
            // 
            // textBoxEigenvalueAnalysisNumEigen
            // 
            this.textBoxEigenvalueAnalysisNumEigen.Location = new System.Drawing.Point(166, 54);
            this.textBoxEigenvalueAnalysisNumEigen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxEigenvalueAnalysisNumEigen.Name = "textBoxEigenvalueAnalysisNumEigen";
            this.textBoxEigenvalueAnalysisNumEigen.Size = new System.Drawing.Size(462, 31);
            this.textBoxEigenvalueAnalysisNumEigen.TabIndex = 95;
            this.textBoxEigenvalueAnalysisNumEigen.Text = "1";
            // 
            // textBoxEigenvalueAnalysisAcc
            // 
            this.textBoxEigenvalueAnalysisAcc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEigenvalueAnalysisAcc.Location = new System.Drawing.Point(166, 148);
            this.textBoxEigenvalueAnalysisAcc.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxEigenvalueAnalysisAcc.Name = "textBoxEigenvalueAnalysisAcc";
            this.textBoxEigenvalueAnalysisAcc.Size = new System.Drawing.Size(778, 31);
            this.textBoxEigenvalueAnalysisAcc.TabIndex = 94;
            this.textBoxEigenvalueAnalysisAcc.Text = "0.001";
            // 
            // labelEigenvalueAnalysisAcc
            // 
            this.labelEigenvalueAnalysisAcc.AutoSize = true;
            this.labelEigenvalueAnalysisAcc.Location = new System.Drawing.Point(8, 154);
            this.labelEigenvalueAnalysisAcc.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelEigenvalueAnalysisAcc.Name = "labelEigenvalueAnalysisAcc";
            this.labelEigenvalueAnalysisAcc.Size = new System.Drawing.Size(114, 25);
            this.labelEigenvalueAnalysisAcc.TabIndex = 93;
            this.labelEigenvalueAnalysisAcc.Text = "Tolerance:";
            // 
            // textBoxEigenvalueAnalysisName
            // 
            this.textBoxEigenvalueAnalysisName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEigenvalueAnalysisName.Location = new System.Drawing.Point(166, 8);
            this.textBoxEigenvalueAnalysisName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxEigenvalueAnalysisName.Name = "textBoxEigenvalueAnalysisName";
            this.textBoxEigenvalueAnalysisName.Size = new System.Drawing.Size(778, 31);
            this.textBoxEigenvalueAnalysisName.TabIndex = 88;
            // 
            // labelEigenvalueAnalysisName
            // 
            this.labelEigenvalueAnalysisName.AutoSize = true;
            this.labelEigenvalueAnalysisName.Location = new System.Drawing.Point(8, 15);
            this.labelEigenvalueAnalysisName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEigenvalueAnalysisName.Name = "labelEigenvalueAnalysisName";
            this.labelEigenvalueAnalysisName.Size = new System.Drawing.Size(74, 25);
            this.labelEigenvalueAnalysisName.TabIndex = 87;
            this.labelEigenvalueAnalysisName.Text = "Name:";
            // 
            // comboBoxEigenvalueAnalysisSolverType
            // 
            this.comboBoxEigenvalueAnalysisSolverType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxEigenvalueAnalysisSolverType.AutoCompleteCustomSource.AddRange(new string[] {
            "eigen_eigensystem",
            "spectra_sym_g_eigs_shift",
            "feast"});
            this.comboBoxEigenvalueAnalysisSolverType.FormattingEnabled = true;
            this.comboBoxEigenvalueAnalysisSolverType.Items.AddRange(new object[] {
            "eigen_eigensystem",
            "spectra_sym_g_eigs_shift",
            "feast"});
            this.comboBoxEigenvalueAnalysisSolverType.Location = new System.Drawing.Point(166, 198);
            this.comboBoxEigenvalueAnalysisSolverType.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.comboBoxEigenvalueAnalysisSolverType.Name = "comboBoxEigenvalueAnalysisSolverType";
            this.comboBoxEigenvalueAnalysisSolverType.Size = new System.Drawing.Size(778, 33);
            this.comboBoxEigenvalueAnalysisSolverType.TabIndex = 22;
            this.comboBoxEigenvalueAnalysisSolverType.Text = "eigen_eigensystem";
            // 
            // labelEigenvalueAnalysisSolverType
            // 
            this.labelEigenvalueAnalysisSolverType.AutoSize = true;
            this.labelEigenvalueAnalysisSolverType.Location = new System.Drawing.Point(8, 204);
            this.labelEigenvalueAnalysisSolverType.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelEigenvalueAnalysisSolverType.Name = "labelEigenvalueAnalysisSolverType";
            this.labelEigenvalueAnalysisSolverType.Size = new System.Drawing.Size(133, 25);
            this.labelEigenvalueAnalysisSolverType.TabIndex = 7;
            this.labelEigenvalueAnalysisSolverType.Text = "Solver Type:";
            // 
            // tabPageCutPatternAnalysis
            // 
            this.tabPageCutPatternAnalysis.BackColor = System.Drawing.Color.Transparent;
            this.tabPageCutPatternAnalysis.Controls.Add(this.checkBoxCutPatternAnalysisPrestress);
            this.tabPageCutPatternAnalysis.Controls.Add(this.textBoxCutPatternAnalysisName);
            this.tabPageCutPatternAnalysis.Controls.Add(this.textBoxCutPatternAnalysisTol);
            this.tabPageCutPatternAnalysis.Controls.Add(this.textBoxCutPatternAnalysisMaxIter);
            this.tabPageCutPatternAnalysis.Controls.Add(this.textBoxCutPatternAnalysisMaxStep);
            this.tabPageCutPatternAnalysis.Controls.Add(this.labelCutPatternAnalysisMaxIter);
            this.tabPageCutPatternAnalysis.Controls.Add(this.labelCutPatternAnalysisName);
            this.tabPageCutPatternAnalysis.Controls.Add(this.labelCutPatternAnalysisTol);
            this.tabPageCutPatternAnalysis.Controls.Add(this.labelCutPatternAnalysisMaxStep);
            this.tabPageCutPatternAnalysis.Location = new System.Drawing.Point(8, 39);
            this.tabPageCutPatternAnalysis.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageCutPatternAnalysis.Name = "tabPageCutPatternAnalysis";
            this.tabPageCutPatternAnalysis.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageCutPatternAnalysis.Size = new System.Drawing.Size(654, 241);
            this.tabPageCutPatternAnalysis.TabIndex = 3;
            this.tabPageCutPatternAnalysis.Text = "CutPattern";
            this.tabPageCutPatternAnalysis.UseVisualStyleBackColor = true;
            // 
            // checkBoxCutPatternAnalysisPrestress
            // 
            this.checkBoxCutPatternAnalysisPrestress.AutoSize = true;
            this.checkBoxCutPatternAnalysisPrestress.Location = new System.Drawing.Point(12, 194);
            this.checkBoxCutPatternAnalysisPrestress.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxCutPatternAnalysisPrestress.Name = "checkBoxCutPatternAnalysisPrestress";
            this.checkBoxCutPatternAnalysisPrestress.Size = new System.Drawing.Size(227, 29);
            this.checkBoxCutPatternAnalysisPrestress.TabIndex = 92;
            this.checkBoxCutPatternAnalysisPrestress.Text = "Consider Prestress";
            this.checkBoxCutPatternAnalysisPrestress.UseVisualStyleBackColor = true;
            // 
            // textBoxCutPatternAnalysisName
            // 
            this.textBoxCutPatternAnalysisName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCutPatternAnalysisName.Location = new System.Drawing.Point(166, 8);
            this.textBoxCutPatternAnalysisName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxCutPatternAnalysisName.Name = "textBoxCutPatternAnalysisName";
            this.textBoxCutPatternAnalysisName.Size = new System.Drawing.Size(450, 31);
            this.textBoxCutPatternAnalysisName.TabIndex = 88;
            // 
            // textBoxCutPatternAnalysisTol
            // 
            this.textBoxCutPatternAnalysisTol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCutPatternAnalysisTol.Location = new System.Drawing.Point(166, 146);
            this.textBoxCutPatternAnalysisTol.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxCutPatternAnalysisTol.Name = "textBoxCutPatternAnalysisTol";
            this.textBoxCutPatternAnalysisTol.Size = new System.Drawing.Size(450, 31);
            this.textBoxCutPatternAnalysisTol.TabIndex = 91;
            this.textBoxCutPatternAnalysisTol.Text = "0.001";
            // 
            // textBoxCutPatternAnalysisMaxIter
            // 
            this.textBoxCutPatternAnalysisMaxIter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCutPatternAnalysisMaxIter.Location = new System.Drawing.Point(166, 100);
            this.textBoxCutPatternAnalysisMaxIter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxCutPatternAnalysisMaxIter.Name = "textBoxCutPatternAnalysisMaxIter";
            this.textBoxCutPatternAnalysisMaxIter.Size = new System.Drawing.Size(450, 31);
            this.textBoxCutPatternAnalysisMaxIter.TabIndex = 90;
            this.textBoxCutPatternAnalysisMaxIter.Text = "10";
            // 
            // textBoxCutPatternAnalysisMaxStep
            // 
            this.textBoxCutPatternAnalysisMaxStep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCutPatternAnalysisMaxStep.Location = new System.Drawing.Point(166, 54);
            this.textBoxCutPatternAnalysisMaxStep.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxCutPatternAnalysisMaxStep.Name = "textBoxCutPatternAnalysisMaxStep";
            this.textBoxCutPatternAnalysisMaxStep.Size = new System.Drawing.Size(450, 31);
            this.textBoxCutPatternAnalysisMaxStep.TabIndex = 89;
            this.textBoxCutPatternAnalysisMaxStep.Text = "10";
            // 
            // labelCutPatternAnalysisMaxIter
            // 
            this.labelCutPatternAnalysisMaxIter.AutoSize = true;
            this.labelCutPatternAnalysisMaxIter.Location = new System.Drawing.Point(8, 106);
            this.labelCutPatternAnalysisMaxIter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCutPatternAnalysisMaxIter.Name = "labelCutPatternAnalysisMaxIter";
            this.labelCutPatternAnalysisMaxIter.Size = new System.Drawing.Size(159, 25);
            this.labelCutPatternAnalysisMaxIter.TabIndex = 84;
            this.labelCutPatternAnalysisMaxIter.Text = "Max. Iterations:";
            // 
            // labelCutPatternAnalysisName
            // 
            this.labelCutPatternAnalysisName.AutoSize = true;
            this.labelCutPatternAnalysisName.Location = new System.Drawing.Point(8, 15);
            this.labelCutPatternAnalysisName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCutPatternAnalysisName.Name = "labelCutPatternAnalysisName";
            this.labelCutPatternAnalysisName.Size = new System.Drawing.Size(74, 25);
            this.labelCutPatternAnalysisName.TabIndex = 85;
            this.labelCutPatternAnalysisName.Text = "Name:";
            // 
            // labelCutPatternAnalysisTol
            // 
            this.labelCutPatternAnalysisTol.AutoSize = true;
            this.labelCutPatternAnalysisTol.Location = new System.Drawing.Point(8, 152);
            this.labelCutPatternAnalysisTol.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCutPatternAnalysisTol.Name = "labelCutPatternAnalysisTol";
            this.labelCutPatternAnalysisTol.Size = new System.Drawing.Size(114, 25);
            this.labelCutPatternAnalysisTol.TabIndex = 86;
            this.labelCutPatternAnalysisTol.Text = "Tolerance:";
            // 
            // labelCutPatternAnalysisMaxStep
            // 
            this.labelCutPatternAnalysisMaxStep.AutoSize = true;
            this.labelCutPatternAnalysisMaxStep.Location = new System.Drawing.Point(8, 60);
            this.labelCutPatternAnalysisMaxStep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCutPatternAnalysisMaxStep.Name = "labelCutPatternAnalysisMaxStep";
            this.labelCutPatternAnalysisMaxStep.Size = new System.Drawing.Size(126, 25);
            this.labelCutPatternAnalysisMaxStep.TabIndex = 87;
            this.labelCutPatternAnalysisMaxStep.Text = "Max. Steps:";
            // 
            // comboBoxAnalyses
            // 
            this.comboBoxAnalyses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAnalyses.FormattingEnabled = true;
            this.comboBoxAnalyses.Location = new System.Drawing.Point(8, 385);
            this.comboBoxAnalyses.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxAnalyses.Name = "comboBoxAnalyses";
            this.comboBoxAnalyses.Size = new System.Drawing.Size(660, 33);
            this.comboBoxAnalyses.TabIndex = 87;
            this.comboBoxAnalyses.SelectedIndexChanged += new System.EventHandler(this.comboBoxAnalyses_SelectedIndexChanged);
            // 
            // buttonRunAnalysis
            // 
            this.buttonRunAnalysis.Location = new System.Drawing.Point(8, 477);
            this.buttonRunAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonRunAnalysis.Name = "buttonRunAnalysis";
            this.buttonRunAnalysis.Size = new System.Drawing.Size(174, 48);
            this.buttonRunAnalysis.TabIndex = 88;
            this.buttonRunAnalysis.Text = "Run Analysis";
            this.buttonRunAnalysis.UseVisualStyleBackColor = true;
            this.buttonRunAnalysis.Click += new System.EventHandler(this.buttonRunAnalysis_Click);
            // 
            // buttonDeleteAnalysis
            // 
            this.buttonDeleteAnalysis.Location = new System.Drawing.Point(372, 329);
            this.buttonDeleteAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonDeleteAnalysis.Name = "buttonDeleteAnalysis";
            this.buttonDeleteAnalysis.Size = new System.Drawing.Size(174, 48);
            this.buttonDeleteAnalysis.TabIndex = 86;
            this.buttonDeleteAnalysis.Text = "Delete Analysis";
            this.buttonDeleteAnalysis.UseVisualStyleBackColor = true;
            this.buttonDeleteAnalysis.Click += new System.EventHandler(this.buttonDeleteAnalysis_Click);
            // 
            // buttonCreateAnalysis
            // 
            this.buttonCreateAnalysis.Location = new System.Drawing.Point(8, 329);
            this.buttonCreateAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCreateAnalysis.Name = "buttonCreateAnalysis";
            this.buttonCreateAnalysis.Size = new System.Drawing.Size(174, 48);
            this.buttonCreateAnalysis.TabIndex = 84;
            this.buttonCreateAnalysis.Text = "Create Analysis";
            this.buttonCreateAnalysis.UseVisualStyleBackColor = true;
            this.buttonCreateAnalysis.Click += new System.EventHandler(this.buttonCreateAnalysis_Click);
            // 
            // buttonModifyAnalysis
            // 
            this.buttonModifyAnalysis.Location = new System.Drawing.Point(190, 329);
            this.buttonModifyAnalysis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonModifyAnalysis.Name = "buttonModifyAnalysis";
            this.buttonModifyAnalysis.Size = new System.Drawing.Size(174, 48);
            this.buttonModifyAnalysis.TabIndex = 85;
            this.buttonModifyAnalysis.Text = "Modify Analysis";
            this.buttonModifyAnalysis.UseVisualStyleBackColor = true;
            this.buttonModifyAnalysis.Click += new System.EventHandler(this.buttonModifyAnalysis_Click);
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.checkBoxShowPreLoads);
            this.groupBoxOptions.Controls.Add(this.checkBoxShowPreSupports);
            this.groupBoxOptions.Controls.Add(this.checkBoxShowPreCouplings);
            this.groupBoxOptions.Controls.Add(this.buttonResetInstance);
            this.groupBoxOptions.Controls.Add(this.buttonDeleteAll);
            this.groupBoxOptions.Controls.Add(this.checkBoxShowPreprocessing);
            this.groupBoxOptions.Location = new System.Drawing.Point(10, 1208);
            this.groupBoxOptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxOptions.Size = new System.Drawing.Size(686, 188);
            this.groupBoxOptions.TabIndex = 100;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "Options";
            // 
            // checkBoxShowPreLoads
            // 
            this.checkBoxShowPreLoads.AutoSize = true;
            this.checkBoxShowPreLoads.Location = new System.Drawing.Point(422, 33);
            this.checkBoxShowPreLoads.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxShowPreLoads.Name = "checkBoxShowPreLoads";
            this.checkBoxShowPreLoads.Size = new System.Drawing.Size(162, 29);
            this.checkBoxShowPreLoads.TabIndex = 4;
            this.checkBoxShowPreLoads.Text = "Show Loads";
            this.checkBoxShowPreLoads.UseVisualStyleBackColor = true;
            this.checkBoxShowPreLoads.CheckedChanged += new System.EventHandler(this.checkBoxShowPreLoads_CheckedChanged);
            // 
            // checkBoxShowPreSupports
            // 
            this.checkBoxShowPreSupports.AutoSize = true;
            this.checkBoxShowPreSupports.Location = new System.Drawing.Point(216, 33);
            this.checkBoxShowPreSupports.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxShowPreSupports.Name = "checkBoxShowPreSupports";
            this.checkBoxShowPreSupports.Size = new System.Drawing.Size(189, 29);
            this.checkBoxShowPreSupports.TabIndex = 3;
            this.checkBoxShowPreSupports.Text = "Show Supports";
            this.checkBoxShowPreSupports.UseVisualStyleBackColor = true;
            this.checkBoxShowPreSupports.CheckedChanged += new System.EventHandler(this.checkBoxShowPreSupports_CheckedChanged);
            // 
            // checkBoxShowPreCouplings
            // 
            this.checkBoxShowPreCouplings.AutoSize = true;
            this.checkBoxShowPreCouplings.Location = new System.Drawing.Point(8, 73);
            this.checkBoxShowPreCouplings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxShowPreCouplings.Name = "checkBoxShowPreCouplings";
            this.checkBoxShowPreCouplings.Size = new System.Drawing.Size(199, 29);
            this.checkBoxShowPreCouplings.TabIndex = 0;
            this.checkBoxShowPreCouplings.Text = "Show Couplings";
            this.checkBoxShowPreCouplings.UseVisualStyleBackColor = true;
            this.checkBoxShowPreCouplings.CheckedChanged += new System.EventHandler(this.checkBoxShowPreCouplings_CheckedChanged);
            // 
            // buttonResetInstance
            // 
            this.buttonResetInstance.Location = new System.Drawing.Point(308, 113);
            this.buttonResetInstance.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonResetInstance.Name = "buttonResetInstance";
            this.buttonResetInstance.Size = new System.Drawing.Size(292, 46);
            this.buttonResetInstance.TabIndex = 2;
            this.buttonResetInstance.Text = "Reset Instance";
            this.buttonResetInstance.UseVisualStyleBackColor = true;
            this.buttonResetInstance.Click += new System.EventHandler(this.buttonResetInstance_Click);
            // 
            // buttonDeleteAll
            // 
            this.buttonDeleteAll.Location = new System.Drawing.Point(8, 113);
            this.buttonDeleteAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonDeleteAll.Name = "buttonDeleteAll";
            this.buttonDeleteAll.Size = new System.Drawing.Size(290, 48);
            this.buttonDeleteAll.TabIndex = 1;
            this.buttonDeleteAll.Text = "Delete Entire User Data";
            this.buttonDeleteAll.UseVisualStyleBackColor = true;
            this.buttonDeleteAll.Click += new System.EventHandler(this.buttonDeleteAll_Click);
            // 
            // checkBoxShowPreprocessing
            // 
            this.checkBoxShowPreprocessing.AutoSize = true;
            this.checkBoxShowPreprocessing.Location = new System.Drawing.Point(8, 33);
            this.checkBoxShowPreprocessing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxShowPreprocessing.Name = "checkBoxShowPreprocessing";
            this.checkBoxShowPreprocessing.Size = new System.Drawing.Size(192, 29);
            this.checkBoxShowPreprocessing.TabIndex = 0;
            this.checkBoxShowPreprocessing.Text = "Show Elements";
            this.checkBoxShowPreprocessing.UseVisualStyleBackColor = true;
            this.checkBoxShowPreprocessing.CheckedChanged += new System.EventHandler(this.checkBoxShowElements_CheckedChanged);
            // 
            // groupBoxMaterials
            // 
            this.groupBoxMaterials.Controls.Add(this.buttonAddModifyMaterial);
            this.groupBoxMaterials.Location = new System.Drawing.Point(10, 10);
            this.groupBoxMaterials.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxMaterials.Name = "groupBoxMaterials";
            this.groupBoxMaterials.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxMaterials.Size = new System.Drawing.Size(686, 98);
            this.groupBoxMaterials.TabIndex = 101;
            this.groupBoxMaterials.TabStop = false;
            this.groupBoxMaterials.Text = "Materials";
            // 
            // buttonAddModifyMaterial
            // 
            this.buttonAddModifyMaterial.Location = new System.Drawing.Point(24, 40);
            this.buttonAddModifyMaterial.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonAddModifyMaterial.Name = "buttonAddModifyMaterial";
            this.buttonAddModifyMaterial.Size = new System.Drawing.Size(294, 48);
            this.buttonAddModifyMaterial.TabIndex = 0;
            this.buttonAddModifyMaterial.Text = "Add/ Modify Materials";
            this.buttonAddModifyMaterial.UseVisualStyleBackColor = true;
            this.buttonAddModifyMaterial.Click += new System.EventHandler(this.buttonAddModifyMaterial_Click);
            // 
            // groupBoxProperties
            // 
            this.groupBoxProperties.Controls.Add(this.tabControlProperties);
            this.groupBoxProperties.Location = new System.Drawing.Point(10, 115);
            this.groupBoxProperties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxProperties.Name = "groupBoxProperties";
            this.groupBoxProperties.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxProperties.Size = new System.Drawing.Size(686, 540);
            this.groupBoxProperties.TabIndex = 102;
            this.groupBoxProperties.TabStop = false;
            this.groupBoxProperties.Text = "Properties";
            // 
            // tabControlProperties
            // 
            this.tabControlProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlProperties.Controls.Add(this.tabPageElement);
            this.tabControlProperties.Controls.Add(this.tabPageRefinement);
            this.tabControlProperties.Controls.Add(this.tabPagePropSupport);
            this.tabControlProperties.Controls.Add(this.tabPageLoad);
            this.tabControlProperties.Controls.Add(this.tabPageCheck);
            this.tabControlProperties.ImageList = this.imageListTabControlElement;
            this.tabControlProperties.Location = new System.Drawing.Point(8, 37);
            this.tabControlProperties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControlProperties.Name = "tabControlProperties";
            this.tabControlProperties.SelectedIndex = 0;
            this.tabControlProperties.Size = new System.Drawing.Size(670, 496);
            this.tabControlProperties.TabIndex = 3;
            // 
            // tabPageElement
            // 
            this.tabPageElement.AutoScroll = true;
            this.tabPageElement.Controls.Add(this.buttonLoadElement);
            this.tabPageElement.Controls.Add(this.tabControlElement);
            this.tabPageElement.Controls.Add(this.labelElementMat);
            this.tabPageElement.Controls.Add(this.comboBoxElementMat);
            this.tabPageElement.Controls.Add(this.labelCouplingType);
            this.tabPageElement.Controls.Add(this.comboBoxCouplingType);
            this.tabPageElement.Controls.Add(this.buttonDeleteElement);
            this.tabPageElement.Controls.Add(this.buttonAddElement);
            this.tabPageElement.ImageKey = "Teda_icon_element.png";
            this.tabPageElement.Location = new System.Drawing.Point(8, 39);
            this.tabPageElement.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageElement.Name = "tabPageElement";
            this.tabPageElement.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageElement.Size = new System.Drawing.Size(654, 449);
            this.tabPageElement.TabIndex = 8;
            this.tabPageElement.Text = "Element";
            this.tabPageElement.UseVisualStyleBackColor = true;
            // 
            // buttonLoadElement
            // 
            this.buttonLoadElement.Location = new System.Drawing.Point(384, 381);
            this.buttonLoadElement.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonLoadElement.Name = "buttonLoadElement";
            this.buttonLoadElement.Size = new System.Drawing.Size(172, 40);
            this.buttonLoadElement.TabIndex = 39;
            this.buttonLoadElement.Text = "Load Element";
            this.buttonLoadElement.UseVisualStyleBackColor = true;
            this.buttonLoadElement.Click += new System.EventHandler(this.buttonLoadElement_Click);
            // 
            // tabControlElement
            // 
            this.tabControlElement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlElement.Controls.Add(this.tabPageElementMembrane);
            this.tabControlElement.Controls.Add(this.tabPageElementShell);
            this.tabControlElement.Controls.Add(this.tabPageElementBeam);
            this.tabControlElement.Controls.Add(this.tabPageElementCable);
            this.tabControlElement.ImageList = this.imageListTabControlElement;
            this.tabControlElement.Location = new System.Drawing.Point(0, 12);
            this.tabControlElement.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabControlElement.Name = "tabControlElement";
            this.tabControlElement.SelectedIndex = 0;
            this.tabControlElement.Size = new System.Drawing.Size(550, 256);
            this.tabControlElement.TabIndex = 38;
            // 
            // tabPageElementMembrane
            // 
            this.tabPageElementMembrane.Controls.Add(this.checkBoxElementMembraneFofi);
            this.tabPageElementMembrane.Controls.Add(this.checkBoxElementMembraneEdgeCoupling);
            this.tabPageElementMembrane.Controls.Add(this.textBoxMembranePrestress2);
            this.tabPageElementMembrane.Controls.Add(this.labelMembranePrestress2);
            this.tabPageElementMembrane.Controls.Add(this.textBoxMembranePrestress1);
            this.tabPageElementMembrane.Controls.Add(this.labelMembraneThick);
            this.tabPageElementMembrane.Controls.Add(this.labelMembranePrestress1);
            this.tabPageElementMembrane.Controls.Add(this.textBoxMembraneThick);
            this.tabPageElementMembrane.Location = new System.Drawing.Point(8, 39);
            this.tabPageElementMembrane.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageElementMembrane.Name = "tabPageElementMembrane";
            this.tabPageElementMembrane.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageElementMembrane.Size = new System.Drawing.Size(534, 209);
            this.tabPageElementMembrane.TabIndex = 0;
            this.tabPageElementMembrane.Text = "Membrane";
            this.tabPageElementMembrane.UseVisualStyleBackColor = true;
            // 
            // checkBoxElementMembraneFofi
            // 
            this.checkBoxElementMembraneFofi.AutoSize = true;
            this.checkBoxElementMembraneFofi.Checked = true;
            this.checkBoxElementMembraneFofi.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxElementMembraneFofi.Location = new System.Drawing.Point(144, 162);
            this.checkBoxElementMembraneFofi.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxElementMembraneFofi.Name = "checkBoxElementMembraneFofi";
            this.checkBoxElementMembraneFofi.Size = new System.Drawing.Size(157, 29);
            this.checkBoxElementMembraneFofi.TabIndex = 10;
            this.checkBoxElementMembraneFofi.Text = "Formfinding";
            this.checkBoxElementMembraneFofi.UseVisualStyleBackColor = true;
            // 
            // checkBoxElementMembraneEdgeCoupling
            // 
            this.checkBoxElementMembraneEdgeCoupling.AutoSize = true;
            this.checkBoxElementMembraneEdgeCoupling.Location = new System.Drawing.Point(404, 162);
            this.checkBoxElementMembraneEdgeCoupling.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxElementMembraneEdgeCoupling.Name = "checkBoxElementMembraneEdgeCoupling";
            this.checkBoxElementMembraneEdgeCoupling.Size = new System.Drawing.Size(185, 29);
            this.checkBoxElementMembraneEdgeCoupling.TabIndex = 10;
            this.checkBoxElementMembraneEdgeCoupling.Text = "Edge Coupling";
            this.checkBoxElementMembraneEdgeCoupling.UseVisualStyleBackColor = true;
            // 
            // textBoxMembranePrestress2
            // 
            this.textBoxMembranePrestress2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMembranePrestress2.Location = new System.Drawing.Point(144, 112);
            this.textBoxMembranePrestress2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxMembranePrestress2.Name = "textBoxMembranePrestress2";
            this.textBoxMembranePrestress2.Size = new System.Drawing.Size(324, 31);
            this.textBoxMembranePrestress2.TabIndex = 9;
            this.textBoxMembranePrestress2.Text = "1.0";
            // 
            // labelMembranePrestress2
            // 
            this.labelMembranePrestress2.AutoSize = true;
            this.labelMembranePrestress2.Location = new System.Drawing.Point(0, 117);
            this.labelMembranePrestress2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelMembranePrestress2.Name = "labelMembranePrestress2";
            this.labelMembranePrestress2.Size = new System.Drawing.Size(121, 25);
            this.labelMembranePrestress2.TabIndex = 8;
            this.labelMembranePrestress2.Text = "Prestress 2";
            // 
            // textBoxMembranePrestress1
            // 
            this.textBoxMembranePrestress1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMembranePrestress1.Location = new System.Drawing.Point(142, 62);
            this.textBoxMembranePrestress1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxMembranePrestress1.Name = "textBoxMembranePrestress1";
            this.textBoxMembranePrestress1.Size = new System.Drawing.Size(326, 31);
            this.textBoxMembranePrestress1.TabIndex = 7;
            this.textBoxMembranePrestress1.Text = "1.0";
            // 
            // labelMembraneThick
            // 
            this.labelMembraneThick.AutoSize = true;
            this.labelMembraneThick.Location = new System.Drawing.Point(0, 17);
            this.labelMembraneThick.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelMembraneThick.Name = "labelMembraneThick";
            this.labelMembraneThick.Size = new System.Drawing.Size(110, 25);
            this.labelMembraneThick.TabIndex = 2;
            this.labelMembraneThick.Text = "Thickness";
            // 
            // labelMembranePrestress1
            // 
            this.labelMembranePrestress1.AutoSize = true;
            this.labelMembranePrestress1.Location = new System.Drawing.Point(0, 67);
            this.labelMembranePrestress1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelMembranePrestress1.Name = "labelMembranePrestress1";
            this.labelMembranePrestress1.Size = new System.Drawing.Size(121, 25);
            this.labelMembranePrestress1.TabIndex = 6;
            this.labelMembranePrestress1.Text = "Prestress 1";
            // 
            // textBoxMembraneThick
            // 
            this.textBoxMembraneThick.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMembraneThick.Location = new System.Drawing.Point(144, 12);
            this.textBoxMembraneThick.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxMembraneThick.Name = "textBoxMembraneThick";
            this.textBoxMembraneThick.Size = new System.Drawing.Size(324, 31);
            this.textBoxMembraneThick.TabIndex = 3;
            this.textBoxMembraneThick.Text = "1.0";
            // 
            // tabPageElementShell
            // 
            this.tabPageElementShell.Controls.Add(this.comboBoxShellType);
            this.tabPageElementShell.Controls.Add(this.label1);
            this.tabPageElementShell.Controls.Add(this.labelShellThick);
            this.tabPageElementShell.Controls.Add(this.textBoxShellThick);
            this.tabPageElementShell.Location = new System.Drawing.Point(8, 39);
            this.tabPageElementShell.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageElementShell.Name = "tabPageElementShell";
            this.tabPageElementShell.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageElementShell.Size = new System.Drawing.Size(500, 209);
            this.tabPageElementShell.TabIndex = 1;
            this.tabPageElementShell.Text = "Shell";
            this.tabPageElementShell.UseVisualStyleBackColor = true;
            // 
            // comboBoxShellType
            // 
            this.comboBoxShellType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxShellType.AutoCompleteCustomSource.AddRange(new string[] {
            "Kirchhoff-Love",
            "5p Director Shell",
            "5p Hierarchic Shell"});
            this.comboBoxShellType.FormattingEnabled = true;
            this.comboBoxShellType.Items.AddRange(new object[] {
            "Shell3pElement",
            "Shell5pElement",
            "Shell5pHierarchicElement"});
            this.comboBoxShellType.Location = new System.Drawing.Point(144, 62);
            this.comboBoxShellType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxShellType.Name = "comboBoxShellType";
            this.comboBoxShellType.Size = new System.Drawing.Size(408, 33);
            this.comboBoxShellType.TabIndex = 22;
            this.comboBoxShellType.Text = "Shell3pElement";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 67);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "Shell Type";
            // 
            // labelShellThick
            // 
            this.labelShellThick.AutoSize = true;
            this.labelShellThick.Location = new System.Drawing.Point(0, 17);
            this.labelShellThick.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelShellThick.Name = "labelShellThick";
            this.labelShellThick.Size = new System.Drawing.Size(110, 25);
            this.labelShellThick.TabIndex = 5;
            this.labelShellThick.Text = "Thickness";
            // 
            // textBoxShellThick
            // 
            this.textBoxShellThick.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxShellThick.Location = new System.Drawing.Point(144, 12);
            this.textBoxShellThick.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxShellThick.Name = "textBoxShellThick";
            this.textBoxShellThick.Size = new System.Drawing.Size(408, 31);
            this.textBoxShellThick.TabIndex = 4;
            this.textBoxShellThick.Text = "1.0";
            // 
            // tabPageElementBeam
            // 
            this.tabPageElementBeam.Controls.Add(this.buttonAddAxis);
            this.tabPageElementBeam.Controls.Add(this.labelBeamIy);
            this.tabPageElementBeam.Controls.Add(this.labelBeamWidth);
            this.tabPageElementBeam.Controls.Add(this.labelBeamIt);
            this.tabPageElementBeam.Controls.Add(this.textBoxBeamIy);
            this.tabPageElementBeam.Controls.Add(this.textBoxBeamArea);
            this.tabPageElementBeam.Controls.Add(this.labelBeamIz);
            this.tabPageElementBeam.Controls.Add(this.textBoxBeamIz);
            this.tabPageElementBeam.Controls.Add(this.labelBeamDiameter);
            this.tabPageElementBeam.Controls.Add(this.labelBeamHeight);
            this.tabPageElementBeam.Controls.Add(this.textBoxBeamIt);
            this.tabPageElementBeam.Controls.Add(this.labelBeamArea);
            this.tabPageElementBeam.Controls.Add(this.labelBeamType);
            this.tabPageElementBeam.Controls.Add(this.comboBoxBeamType);
            this.tabPageElementBeam.Location = new System.Drawing.Point(8, 39);
            this.tabPageElementBeam.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageElementBeam.Name = "tabPageElementBeam";
            this.tabPageElementBeam.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageElementBeam.Size = new System.Drawing.Size(500, 209);
            this.tabPageElementBeam.TabIndex = 2;
            this.tabPageElementBeam.Text = "Beam";
            this.tabPageElementBeam.UseVisualStyleBackColor = true;
            // 
            // buttonAddAxis
            // 
            this.buttonAddAxis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddAxis.Location = new System.Drawing.Point(142, 163);
            this.buttonAddAxis.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonAddAxis.Name = "buttonAddAxis";
            this.buttonAddAxis.Size = new System.Drawing.Size(410, 42);
            this.buttonAddAxis.TabIndex = 37;
            this.buttonAddAxis.Text = "Add Axis";
            this.buttonAddAxis.UseVisualStyleBackColor = true;
            this.buttonAddAxis.Click += new System.EventHandler(this.buttonAddAxis_Click);
            // 
            // labelBeamIy
            // 
            this.labelBeamIy.AutoSize = true;
            this.labelBeamIy.Location = new System.Drawing.Point(0, 119);
            this.labelBeamIy.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelBeamIy.Name = "labelBeamIy";
            this.labelBeamIy.Size = new System.Drawing.Size(28, 25);
            this.labelBeamIy.TabIndex = 30;
            this.labelBeamIy.Text = "Iy";
            this.labelBeamIy.Visible = false;
            // 
            // labelBeamWidth
            // 
            this.labelBeamWidth.AutoSize = true;
            this.labelBeamWidth.Location = new System.Drawing.Point(0, 119);
            this.labelBeamWidth.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelBeamWidth.Name = "labelBeamWidth";
            this.labelBeamWidth.Size = new System.Drawing.Size(67, 25);
            this.labelBeamWidth.TabIndex = 27;
            this.labelBeamWidth.Text = "Width";
            this.labelBeamWidth.Visible = false;
            // 
            // labelBeamIt
            // 
            this.labelBeamIt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBeamIt.AutoSize = true;
            this.labelBeamIt.Location = new System.Drawing.Point(376, 119);
            this.labelBeamIt.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelBeamIt.Name = "labelBeamIt";
            this.labelBeamIt.Size = new System.Drawing.Size(23, 25);
            this.labelBeamIt.TabIndex = 35;
            this.labelBeamIt.Text = "It";
            this.labelBeamIt.Visible = false;
            // 
            // textBoxBeamIy
            // 
            this.textBoxBeamIy.Location = new System.Drawing.Point(142, 113);
            this.textBoxBeamIy.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxBeamIy.Name = "textBoxBeamIy";
            this.textBoxBeamIy.Size = new System.Drawing.Size(218, 31);
            this.textBoxBeamIy.TabIndex = 32;
            this.textBoxBeamIy.Text = "8.3333e-6";
            this.textBoxBeamIy.Visible = false;
            // 
            // textBoxBeamArea
            // 
            this.textBoxBeamArea.Location = new System.Drawing.Point(142, 63);
            this.textBoxBeamArea.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxBeamArea.Name = "textBoxBeamArea";
            this.textBoxBeamArea.Size = new System.Drawing.Size(218, 31);
            this.textBoxBeamArea.TabIndex = 31;
            this.textBoxBeamArea.Text = "0.01";
            this.textBoxBeamArea.Visible = false;
            // 
            // labelBeamIz
            // 
            this.labelBeamIz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBeamIz.AutoSize = true;
            this.labelBeamIz.Location = new System.Drawing.Point(376, 69);
            this.labelBeamIz.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelBeamIz.Name = "labelBeamIz";
            this.labelBeamIz.Size = new System.Drawing.Size(28, 25);
            this.labelBeamIz.TabIndex = 33;
            this.labelBeamIz.Text = "Iz";
            this.labelBeamIz.Visible = false;
            // 
            // textBoxBeamIz
            // 
            this.textBoxBeamIz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBeamIz.Location = new System.Drawing.Point(418, 63);
            this.textBoxBeamIz.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxBeamIz.Name = "textBoxBeamIz";
            this.textBoxBeamIz.Size = new System.Drawing.Size(130, 31);
            this.textBoxBeamIz.TabIndex = 34;
            this.textBoxBeamIz.Text = "8.3333e-6";
            this.textBoxBeamIz.Visible = false;
            // 
            // labelBeamDiameter
            // 
            this.labelBeamDiameter.AutoSize = true;
            this.labelBeamDiameter.Location = new System.Drawing.Point(-2, 69);
            this.labelBeamDiameter.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelBeamDiameter.Name = "labelBeamDiameter";
            this.labelBeamDiameter.Size = new System.Drawing.Size(98, 25);
            this.labelBeamDiameter.TabIndex = 23;
            this.labelBeamDiameter.Text = "Diameter";
            this.labelBeamDiameter.Visible = false;
            // 
            // labelBeamHeight
            // 
            this.labelBeamHeight.AutoSize = true;
            this.labelBeamHeight.Location = new System.Drawing.Point(0, 69);
            this.labelBeamHeight.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelBeamHeight.Name = "labelBeamHeight";
            this.labelBeamHeight.Size = new System.Drawing.Size(74, 25);
            this.labelBeamHeight.TabIndex = 25;
            this.labelBeamHeight.Text = "Height";
            this.labelBeamHeight.Visible = false;
            // 
            // textBoxBeamIt
            // 
            this.textBoxBeamIt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBeamIt.Location = new System.Drawing.Point(418, 113);
            this.textBoxBeamIt.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxBeamIt.Name = "textBoxBeamIt";
            this.textBoxBeamIt.Size = new System.Drawing.Size(130, 31);
            this.textBoxBeamIt.TabIndex = 36;
            this.textBoxBeamIt.Text = "3.3333e-5";
            this.textBoxBeamIt.Visible = false;
            // 
            // labelBeamArea
            // 
            this.labelBeamArea.AutoSize = true;
            this.labelBeamArea.Location = new System.Drawing.Point(0, 69);
            this.labelBeamArea.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelBeamArea.Name = "labelBeamArea";
            this.labelBeamArea.Size = new System.Drawing.Size(57, 25);
            this.labelBeamArea.TabIndex = 29;
            this.labelBeamArea.Text = "Area";
            this.labelBeamArea.Visible = false;
            // 
            // labelBeamType
            // 
            this.labelBeamType.AutoSize = true;
            this.labelBeamType.Location = new System.Drawing.Point(0, 17);
            this.labelBeamType.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelBeamType.Name = "labelBeamType";
            this.labelBeamType.Size = new System.Drawing.Size(143, 25);
            this.labelBeamType.TabIndex = 22;
            this.labelBeamType.Text = "Cross section";
            // 
            // comboBoxBeamType
            // 
            this.comboBoxBeamType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBeamType.FormattingEnabled = true;
            this.comboBoxBeamType.Items.AddRange(new object[] {
            "Circular",
            "Rectangular",
            "Undefined"});
            this.comboBoxBeamType.Location = new System.Drawing.Point(144, 12);
            this.comboBoxBeamType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxBeamType.Name = "comboBoxBeamType";
            this.comboBoxBeamType.Size = new System.Drawing.Size(404, 33);
            this.comboBoxBeamType.TabIndex = 21;
            this.comboBoxBeamType.SelectedIndexChanged += new System.EventHandler(this.comboBoxBeamType_SelectedIndexChanged);
            // 
            // tabPageElementCable
            // 
            this.tabPageElementCable.Controls.Add(this.checkBoxElementCableFofi);
            this.tabPageElementCable.Controls.Add(this.checkBoxCablePrestressCurve);
            this.tabPageElementCable.Controls.Add(this.labelCablePrestress);
            this.tabPageElementCable.Controls.Add(this.textBoxCablePrestress);
            this.tabPageElementCable.Controls.Add(this.textBoxCableArea);
            this.tabPageElementCable.Controls.Add(this.comboBoxCableType);
            this.tabPageElementCable.Controls.Add(this.labelCableArea);
            this.tabPageElementCable.Controls.Add(this.labelCableType);
            this.tabPageElementCable.Location = new System.Drawing.Point(8, 39);
            this.tabPageElementCable.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageElementCable.Name = "tabPageElementCable";
            this.tabPageElementCable.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageElementCable.Size = new System.Drawing.Size(500, 209);
            this.tabPageElementCable.TabIndex = 3;
            this.tabPageElementCable.Text = "Cable";
            this.tabPageElementCable.UseVisualStyleBackColor = true;
            // 
            // checkBoxElementCableFofi
            // 
            this.checkBoxElementCableFofi.AutoSize = true;
            this.checkBoxElementCableFofi.Checked = true;
            this.checkBoxElementCableFofi.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxElementCableFofi.Location = new System.Drawing.Point(440, 163);
            this.checkBoxElementCableFofi.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxElementCableFofi.Name = "checkBoxElementCableFofi";
            this.checkBoxElementCableFofi.Size = new System.Drawing.Size(157, 29);
            this.checkBoxElementCableFofi.TabIndex = 21;
            this.checkBoxElementCableFofi.Text = "Formfinding";
            this.checkBoxElementCableFofi.UseVisualStyleBackColor = true;
            // 
            // checkBoxCablePrestressCurve
            // 
            this.checkBoxCablePrestressCurve.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCablePrestressCurve.AutoSize = true;
            this.checkBoxCablePrestressCurve.Location = new System.Drawing.Point(144, 163);
            this.checkBoxCablePrestressCurve.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxCablePrestressCurve.Name = "checkBoxCablePrestressCurve";
            this.checkBoxCablePrestressCurve.Size = new System.Drawing.Size(283, 29);
            this.checkBoxCablePrestressCurve.TabIndex = 20;
            this.checkBoxCablePrestressCurve.Text = "Load Curve for Prestress";
            this.checkBoxCablePrestressCurve.UseVisualStyleBackColor = true;
            // 
            // labelCablePrestress
            // 
            this.labelCablePrestress.AutoSize = true;
            this.labelCablePrestress.Location = new System.Drawing.Point(-2, 119);
            this.labelCablePrestress.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelCablePrestress.Name = "labelCablePrestress";
            this.labelCablePrestress.Size = new System.Drawing.Size(103, 25);
            this.labelCablePrestress.TabIndex = 16;
            this.labelCablePrestress.Text = "Prestress";
            // 
            // textBoxCablePrestress
            // 
            this.textBoxCablePrestress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCablePrestress.Location = new System.Drawing.Point(144, 113);
            this.textBoxCablePrestress.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxCablePrestress.Name = "textBoxCablePrestress";
            this.textBoxCablePrestress.Size = new System.Drawing.Size(412, 31);
            this.textBoxCablePrestress.TabIndex = 17;
            this.textBoxCablePrestress.Text = "1.0";
            // 
            // textBoxCableArea
            // 
            this.textBoxCableArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCableArea.Location = new System.Drawing.Point(144, 63);
            this.textBoxCableArea.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxCableArea.Name = "textBoxCableArea";
            this.textBoxCableArea.Size = new System.Drawing.Size(412, 31);
            this.textBoxCableArea.TabIndex = 15;
            this.textBoxCableArea.Text = "1.0";
            // 
            // comboBoxCableType
            // 
            this.comboBoxCableType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCableType.FormattingEnabled = true;
            this.comboBoxCableType.Items.AddRange(new object[] {
            "Curve",
            "Edge"});
            this.comboBoxCableType.Location = new System.Drawing.Point(144, 12);
            this.comboBoxCableType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxCableType.Name = "comboBoxCableType";
            this.comboBoxCableType.Size = new System.Drawing.Size(412, 33);
            this.comboBoxCableType.TabIndex = 19;
            this.comboBoxCableType.SelectedIndexChanged += new System.EventHandler(this.comboBoxCableType_SelectedIndex_Changed);
            // 
            // labelCableArea
            // 
            this.labelCableArea.AutoSize = true;
            this.labelCableArea.Location = new System.Drawing.Point(0, 69);
            this.labelCableArea.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelCableArea.Name = "labelCableArea";
            this.labelCableArea.Size = new System.Drawing.Size(57, 25);
            this.labelCableArea.TabIndex = 14;
            this.labelCableArea.Text = "Area";
            // 
            // labelCableType
            // 
            this.labelCableType.AutoSize = true;
            this.labelCableType.Location = new System.Drawing.Point(0, 17);
            this.labelCableType.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelCableType.Name = "labelCableType";
            this.labelCableType.Size = new System.Drawing.Size(60, 25);
            this.labelCableType.TabIndex = 18;
            this.labelCableType.Text = "Type";
            // 
            // imageListTabControlElement
            // 
            this.imageListTabControlElement.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabControlElement.ImageStream")));
            this.imageListTabControlElement.Tag = "";
            this.imageListTabControlElement.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabControlElement.Images.SetKeyName(0, "Teda_icon_element.png");
            this.imageListTabControlElement.Images.SetKeyName(1, "Teda_icon_ref.png");
            this.imageListTabControlElement.Images.SetKeyName(2, "Teda_icon_supp.png");
            this.imageListTabControlElement.Images.SetKeyName(3, "Teda_icon_load.png");
            this.imageListTabControlElement.Images.SetKeyName(4, "Teda_icon_inidisp.png");
            // 
            // labelElementMat
            // 
            this.labelElementMat.AutoSize = true;
            this.labelElementMat.Location = new System.Drawing.Point(6, 285);
            this.labelElementMat.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelElementMat.Name = "labelElementMat";
            this.labelElementMat.Size = new System.Drawing.Size(89, 25);
            this.labelElementMat.TabIndex = 10;
            this.labelElementMat.Text = "Material";
            // 
            // comboBoxElementMat
            // 
            this.comboBoxElementMat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxElementMat.FormattingEnabled = true;
            this.comboBoxElementMat.Location = new System.Drawing.Point(150, 279);
            this.comboBoxElementMat.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxElementMat.Name = "comboBoxElementMat";
            this.comboBoxElementMat.Size = new System.Drawing.Size(396, 33);
            this.comboBoxElementMat.TabIndex = 11;
            // 
            // labelCouplingType
            // 
            this.labelCouplingType.AutoSize = true;
            this.labelCouplingType.Location = new System.Drawing.Point(6, 335);
            this.labelCouplingType.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelCouplingType.Name = "labelCouplingType";
            this.labelCouplingType.Size = new System.Drawing.Size(97, 25);
            this.labelCouplingType.TabIndex = 10;
            this.labelCouplingType.Text = "Coupling";
            // 
            // comboBoxCouplingType
            // 
            this.comboBoxCouplingType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCouplingType.FormattingEnabled = true;
            this.comboBoxCouplingType.Location = new System.Drawing.Point(150, 329);
            this.comboBoxCouplingType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxCouplingType.Name = "comboBoxCouplingType";
            this.comboBoxCouplingType.Size = new System.Drawing.Size(396, 33);
            this.comboBoxCouplingType.TabIndex = 11;
            this.comboBoxCouplingType.Text = "CouplingPenaltyCondition";
            this.comboBoxCouplingType.SelectedIndexChanged += new System.EventHandler(this.comboBoxCouplingType_SelectedIndexChanged);
            // 
            // buttonDeleteElement
            // 
            this.buttonDeleteElement.Location = new System.Drawing.Point(198, 381);
            this.buttonDeleteElement.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonDeleteElement.Name = "buttonDeleteElement";
            this.buttonDeleteElement.Size = new System.Drawing.Size(174, 40);
            this.buttonDeleteElement.TabIndex = 13;
            this.buttonDeleteElement.Text = "Delete Element";
            this.buttonDeleteElement.UseVisualStyleBackColor = true;
            this.buttonDeleteElement.Click += new System.EventHandler(this.buttonDeleteElement_Click);
            // 
            // buttonAddElement
            // 
            this.buttonAddElement.Location = new System.Drawing.Point(14, 381);
            this.buttonAddElement.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonAddElement.Name = "buttonAddElement";
            this.buttonAddElement.Size = new System.Drawing.Size(172, 40);
            this.buttonAddElement.TabIndex = 12;
            this.buttonAddElement.Text = "Add Element";
            this.buttonAddElement.UseVisualStyleBackColor = true;
            this.buttonAddElement.Click += new System.EventHandler(this.buttonAddElement_Click);
            // 
            // tabPageRefinement
            // 
            this.tabPageRefinement.Controls.Add(this.radioButtonRefinementApproxElementSize);
            this.tabPageRefinement.Controls.Add(this.radioButtonRefinementKnotSubdivision);
            this.tabPageRefinement.Controls.Add(this.groupBoxRefinementElement);
            this.tabPageRefinement.Controls.Add(this.buttonCheckRefinement);
            this.tabPageRefinement.Controls.Add(this.buttonChangeRefinement);
            this.tabPageRefinement.Controls.Add(this.textBoxKnotSubDivV);
            this.tabPageRefinement.Controls.Add(this.textBoxKnotSubDivU);
            this.tabPageRefinement.Controls.Add(this.textBoxQDeg);
            this.tabPageRefinement.Controls.Add(this.textBoxPDeg);
            this.tabPageRefinement.Controls.Add(this.labelRefinementMinV);
            this.tabPageRefinement.Controls.Add(this.labelRefinementMinU);
            this.tabPageRefinement.Controls.Add(this.labelRefinementQDeg);
            this.tabPageRefinement.Controls.Add(this.labelRefinementPDeg);
            this.tabPageRefinement.ImageKey = "Teda_icon_ref.png";
            this.tabPageRefinement.Location = new System.Drawing.Point(8, 39);
            this.tabPageRefinement.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageRefinement.Name = "tabPageRefinement";
            this.tabPageRefinement.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabPageRefinement.Size = new System.Drawing.Size(654, 449);
            this.tabPageRefinement.TabIndex = 4;
            this.tabPageRefinement.Text = "Refinement";
            this.tabPageRefinement.UseVisualStyleBackColor = true;
            // 
            // radioButtonRefinementApproxElementSize
            // 
            this.radioButtonRefinementApproxElementSize.AutoSize = true;
            this.radioButtonRefinementApproxElementSize.Location = new System.Drawing.Point(238, 183);
            this.radioButtonRefinementApproxElementSize.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonRefinementApproxElementSize.Name = "radioButtonRefinementApproxElementSize";
            this.radioButtonRefinementApproxElementSize.Size = new System.Drawing.Size(249, 29);
            this.radioButtonRefinementApproxElementSize.TabIndex = 85;
            this.radioButtonRefinementApproxElementSize.TabStop = true;
            this.radioButtonRefinementApproxElementSize.Text = "Approx. Element Size";
            this.radioButtonRefinementApproxElementSize.UseVisualStyleBackColor = true;
            this.radioButtonRefinementApproxElementSize.CheckedChanged += new System.EventHandler(this.radioButtonKnotSubdivision_CheckedChanged);
            // 
            // radioButtonRefinementKnotSubdivision
            // 
            this.radioButtonRefinementKnotSubdivision.AutoSize = true;
            this.radioButtonRefinementKnotSubdivision.Checked = true;
            this.radioButtonRefinementKnotSubdivision.Location = new System.Drawing.Point(18, 183);
            this.radioButtonRefinementKnotSubdivision.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonRefinementKnotSubdivision.Name = "radioButtonRefinementKnotSubdivision";
            this.radioButtonRefinementKnotSubdivision.Size = new System.Drawing.Size(204, 29);
            this.radioButtonRefinementKnotSubdivision.TabIndex = 84;
            this.radioButtonRefinementKnotSubdivision.TabStop = true;
            this.radioButtonRefinementKnotSubdivision.Text = "Knot Subdivision";
            this.radioButtonRefinementKnotSubdivision.UseVisualStyleBackColor = true;
            this.radioButtonRefinementKnotSubdivision.CheckedChanged += new System.EventHandler(this.radioButtonRefinementKnotSubdivision_CheckedChanged);
            // 
            // groupBoxRefinementElement
            // 
            this.groupBoxRefinementElement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRefinementElement.Controls.Add(this.radioButtonRefinementElementEdge);
            this.groupBoxRefinementElement.Controls.Add(this.radioButtonRefinementElementCurve);
            this.groupBoxRefinementElement.Controls.Add(this.radioButtonRefinementElementSurf);
            this.groupBoxRefinementElement.Location = new System.Drawing.Point(0, 6);
            this.groupBoxRefinementElement.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxRefinementElement.Name = "groupBoxRefinementElement";
            this.groupBoxRefinementElement.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxRefinementElement.Size = new System.Drawing.Size(624, 79);
            this.groupBoxRefinementElement.TabIndex = 83;
            this.groupBoxRefinementElement.TabStop = false;
            this.groupBoxRefinementElement.Text = "Structural Element";
            // 
            // radioButtonRefinementElementEdge
            // 
            this.radioButtonRefinementElementEdge.AutoSize = true;
            this.radioButtonRefinementElementEdge.Location = new System.Drawing.Point(274, 35);
            this.radioButtonRefinementElementEdge.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonRefinementElementEdge.Name = "radioButtonRefinementElementEdge";
            this.radioButtonRefinementElementEdge.Size = new System.Drawing.Size(93, 29);
            this.radioButtonRefinementElementEdge.TabIndex = 2;
            this.radioButtonRefinementElementEdge.Text = "Edge";
            this.radioButtonRefinementElementEdge.UseVisualStyleBackColor = true;
            this.radioButtonRefinementElementEdge.CheckedChanged += new System.EventHandler(this.radioButtonRefinementEdge_CheckedChanged);
            // 
            // radioButtonRefinementElementCurve
            // 
            this.radioButtonRefinementElementCurve.AutoSize = true;
            this.radioButtonRefinementElementCurve.Location = new System.Drawing.Point(156, 35);
            this.radioButtonRefinementElementCurve.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonRefinementElementCurve.Name = "radioButtonRefinementElementCurve";
            this.radioButtonRefinementElementCurve.Size = new System.Drawing.Size(100, 29);
            this.radioButtonRefinementElementCurve.TabIndex = 1;
            this.radioButtonRefinementElementCurve.Text = "Curve";
            this.radioButtonRefinementElementCurve.UseVisualStyleBackColor = true;
            this.radioButtonRefinementElementCurve.CheckedChanged += new System.EventHandler(this.radioButtonRefinementCurve_CheckedChanged);
            // 
            // radioButtonRefinementElementSurf
            // 
            this.radioButtonRefinementElementSurf.AutoSize = true;
            this.radioButtonRefinementElementSurf.Checked = true;
            this.radioButtonRefinementElementSurf.Location = new System.Drawing.Point(20, 35);
            this.radioButtonRefinementElementSurf.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonRefinementElementSurf.Name = "radioButtonRefinementElementSurf";
            this.radioButtonRefinementElementSurf.Size = new System.Drawing.Size(117, 29);
            this.radioButtonRefinementElementSurf.TabIndex = 0;
            this.radioButtonRefinementElementSurf.TabStop = true;
            this.radioButtonRefinementElementSurf.Text = "Surface";
            this.radioButtonRefinementElementSurf.UseVisualStyleBackColor = true;
            this.radioButtonRefinementElementSurf.CheckedChanged += new System.EventHandler(this.radioButtonRefinementSurface_CheckedChanged);
            // 
            // buttonCheckRefinement
            // 
            this.buttonCheckRefinement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCheckRefinement.Location = new System.Drawing.Point(296, 321);
            this.buttonCheckRefinement.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonCheckRefinement.Name = "buttonCheckRefinement";
            this.buttonCheckRefinement.Size = new System.Drawing.Size(318, 50);
            this.buttonCheckRefinement.TabIndex = 55;
            this.buttonCheckRefinement.Text = "Check Refinement";
            this.buttonCheckRefinement.UseVisualStyleBackColor = true;
            this.buttonCheckRefinement.Click += new System.EventHandler(this.buttonCheckRefinement_Click);
            // 
            // buttonChangeRefinement
            // 
            this.buttonChangeRefinement.Location = new System.Drawing.Point(6, 321);
            this.buttonChangeRefinement.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.buttonChangeRefinement.Name = "buttonChangeRefinement";
            this.buttonChangeRefinement.Size = new System.Drawing.Size(276, 48);
            this.buttonChangeRefinement.TabIndex = 54;
            this.buttonChangeRefinement.Text = "Change Refinement";
            this.buttonChangeRefinement.UseVisualStyleBackColor = true;
            this.buttonChangeRefinement.Click += new System.EventHandler(this.buttonChangeRefinement_Click);
            // 
            // textBoxKnotSubDivV
            // 
            this.textBoxKnotSubDivV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxKnotSubDivV.Location = new System.Drawing.Point(126, 265);
            this.textBoxKnotSubDivV.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxKnotSubDivV.Name = "textBoxKnotSubDivV";
            this.textBoxKnotSubDivV.Size = new System.Drawing.Size(484, 31);
            this.textBoxKnotSubDivV.TabIndex = 53;
            this.textBoxKnotSubDivV.Text = "4";
            // 
            // textBoxKnotSubDivU
            // 
            this.textBoxKnotSubDivU.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxKnotSubDivU.Location = new System.Drawing.Point(126, 225);
            this.textBoxKnotSubDivU.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxKnotSubDivU.Name = "textBoxKnotSubDivU";
            this.textBoxKnotSubDivU.Size = new System.Drawing.Size(484, 31);
            this.textBoxKnotSubDivU.TabIndex = 52;
            this.textBoxKnotSubDivU.Text = "4";
            // 
            // textBoxQDeg
            // 
            this.textBoxQDeg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxQDeg.Location = new System.Drawing.Point(126, 135);
            this.textBoxQDeg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxQDeg.Name = "textBoxQDeg";
            this.textBoxQDeg.Size = new System.Drawing.Size(484, 31);
            this.textBoxQDeg.TabIndex = 51;
            this.textBoxQDeg.Text = "3";
            // 
            // textBoxPDeg
            // 
            this.textBoxPDeg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPDeg.Location = new System.Drawing.Point(126, 94);
            this.textBoxPDeg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxPDeg.Name = "textBoxPDeg";
            this.textBoxPDeg.Size = new System.Drawing.Size(484, 31);
            this.textBoxPDeg.TabIndex = 50;
            this.textBoxPDeg.Text = "3";
            // 
            // labelRefinementMinV
            // 
            this.labelRefinementMinV.AutoSize = true;
            this.labelRefinementMinV.Location = new System.Drawing.Point(12, 271);
            this.labelRefinementMinV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRefinementMinV.Name = "labelRefinementMinV";
            this.labelRefinementMinV.Size = new System.Drawing.Size(65, 25);
            this.labelRefinementMinV.TabIndex = 11;
            this.labelRefinementMinV.Text = "Dir V:";
            // 
            // labelRefinementMinU
            // 
            this.labelRefinementMinU.AutoSize = true;
            this.labelRefinementMinU.Location = new System.Drawing.Point(14, 231);
            this.labelRefinementMinU.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRefinementMinU.Name = "labelRefinementMinU";
            this.labelRefinementMinU.Size = new System.Drawing.Size(66, 25);
            this.labelRefinementMinU.TabIndex = 11;
            this.labelRefinementMinU.Text = "Dir U:";
            // 
            // labelRefinementQDeg
            // 
            this.labelRefinementQDeg.AutoSize = true;
            this.labelRefinementQDeg.Location = new System.Drawing.Point(12, 138);
            this.labelRefinementQDeg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRefinementQDeg.Name = "labelRefinementQDeg";
            this.labelRefinementQDeg.Size = new System.Drawing.Size(79, 25);
            this.labelRefinementQDeg.TabIndex = 11;
            this.labelRefinementQDeg.Text = "Q Deg:";
            // 
            // labelRefinementPDeg
            // 
            this.labelRefinementPDeg.AutoSize = true;
            this.labelRefinementPDeg.Location = new System.Drawing.Point(12, 98);
            this.labelRefinementPDeg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRefinementPDeg.Name = "labelRefinementPDeg";
            this.labelRefinementPDeg.Size = new System.Drawing.Size(77, 25);
            this.labelRefinementPDeg.TabIndex = 11;
            this.labelRefinementPDeg.Text = "P Deg:";
            // 
            // tabPagePropSupport
            // 
            this.tabPagePropSupport.Controls.Add(this.groupBox4);
            this.tabPagePropSupport.Controls.Add(this.groupBoxInterval);
            this.tabPagePropSupport.Controls.Add(this.checkBoxOverwriteSupport);
            this.tabPagePropSupport.Controls.Add(this.groupBoxSupportType);
            this.tabPagePropSupport.Controls.Add(this.groupBoxSupportDimension);
            this.tabPagePropSupport.Controls.Add(this.groupBoxSupportElementType);
            this.tabPagePropSupport.Controls.Add(this.buttonDeleteEdgeSupport);
            this.tabPagePropSupport.Controls.Add(this.buttonAddEdgeSupports);
            this.tabPagePropSupport.ImageIndex = 2;
            this.tabPagePropSupport.Location = new System.Drawing.Point(8, 39);
            this.tabPagePropSupport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPagePropSupport.Name = "tabPagePropSupport";
            this.tabPagePropSupport.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPagePropSupport.Size = new System.Drawing.Size(654, 449);
            this.tabPagePropSupport.TabIndex = 0;
            this.tabPagePropSupport.Text = "Support";
            this.tabPagePropSupport.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox4.Controls.Add(this.comboBoxSupportType);
            this.groupBox4.Location = new System.Drawing.Point(360, 100);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox4.Size = new System.Drawing.Size(278, 88);
            this.groupBox4.TabIndex = 96;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Support Type";
            // 
            // comboBoxSupportType
            // 
            this.comboBoxSupportType.FormattingEnabled = true;
            this.comboBoxSupportType.Items.AddRange(new object[] {
            "SupportPenaltyCondition",
            "SupportLagrangeCondition",
            "SupportNitscheCondition",
            "DirectorInc5pShellSupport"});
            this.comboBoxSupportType.Location = new System.Drawing.Point(12, 35);
            this.comboBoxSupportType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxSupportType.Name = "comboBoxSupportType";
            this.comboBoxSupportType.Size = new System.Drawing.Size(250, 33);
            this.comboBoxSupportType.TabIndex = 0;
            this.comboBoxSupportType.Text = "SupportPenaltyCondition";
            // 
            // groupBoxInterval
            // 
            this.groupBoxInterval.Controls.Add(this.textBoxSupportEndTime);
            this.groupBoxInterval.Controls.Add(this.textBoxSupportStartTime);
            this.groupBoxInterval.Location = new System.Drawing.Point(302, 6);
            this.groupBoxInterval.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxInterval.Name = "groupBoxInterval";
            this.groupBoxInterval.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxInterval.Size = new System.Drawing.Size(336, 83);
            this.groupBoxInterval.TabIndex = 83;
            this.groupBoxInterval.TabStop = false;
            this.groupBoxInterval.Text = "Interval: Start Time - End Time";
            // 
            // textBoxSupportEndTime
            // 
            this.textBoxSupportEndTime.Location = new System.Drawing.Point(182, 35);
            this.textBoxSupportEndTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxSupportEndTime.Name = "textBoxSupportEndTime";
            this.textBoxSupportEndTime.Size = new System.Drawing.Size(138, 31);
            this.textBoxSupportEndTime.TabIndex = 93;
            this.textBoxSupportEndTime.Text = "End";
            // 
            // textBoxSupportStartTime
            // 
            this.textBoxSupportStartTime.Location = new System.Drawing.Point(10, 35);
            this.textBoxSupportStartTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxSupportStartTime.Name = "textBoxSupportStartTime";
            this.textBoxSupportStartTime.Size = new System.Drawing.Size(160, 31);
            this.textBoxSupportStartTime.TabIndex = 92;
            this.textBoxSupportStartTime.Text = "0.0";
            // 
            // checkBoxOverwriteSupport
            // 
            this.checkBoxOverwriteSupport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxOverwriteSupport.AutoSize = true;
            this.checkBoxOverwriteSupport.Checked = true;
            this.checkBoxOverwriteSupport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOverwriteSupport.Location = new System.Drawing.Point(8, 335);
            this.checkBoxOverwriteSupport.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxOverwriteSupport.Name = "checkBoxOverwriteSupport";
            this.checkBoxOverwriteSupport.Size = new System.Drawing.Size(135, 29);
            this.checkBoxOverwriteSupport.TabIndex = 95;
            this.checkBoxOverwriteSupport.Text = "Overwrite";
            this.checkBoxOverwriteSupport.UseVisualStyleBackColor = true;
            // 
            // groupBoxSupportType
            // 
            this.groupBoxSupportType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSupportType.Controls.Add(this.textBoxSupportTypeDispZ);
            this.groupBoxSupportType.Controls.Add(this.textBoxSupportTypeDispY);
            this.groupBoxSupportType.Controls.Add(this.textBoxSupportTypeDispX);
            this.groupBoxSupportType.Controls.Add(this.checkBoxSupportStrong);
            this.groupBoxSupportType.Controls.Add(this.checkBoxDispZ);
            this.groupBoxSupportType.Controls.Add(this.checkBoxDispY);
            this.groupBoxSupportType.Controls.Add(this.checkBoxRotationSupport);
            this.groupBoxSupportType.Controls.Add(this.checkBoxDispX);
            this.groupBoxSupportType.Location = new System.Drawing.Point(2, 200);
            this.groupBoxSupportType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxSupportType.Name = "groupBoxSupportType";
            this.groupBoxSupportType.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxSupportType.Size = new System.Drawing.Size(612, 115);
            this.groupBoxSupportType.TabIndex = 3;
            this.groupBoxSupportType.TabStop = false;
            this.groupBoxSupportType.Text = "Support Direction";
            // 
            // textBoxSupportTypeDispZ
            // 
            this.textBoxSupportTypeDispZ.Location = new System.Drawing.Point(254, 67);
            this.textBoxSupportTypeDispZ.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxSupportTypeDispZ.Name = "textBoxSupportTypeDispZ";
            this.textBoxSupportTypeDispZ.Size = new System.Drawing.Size(110, 31);
            this.textBoxSupportTypeDispZ.TabIndex = 91;
            this.textBoxSupportTypeDispZ.Text = "0.0";
            // 
            // textBoxSupportTypeDispY
            // 
            this.textBoxSupportTypeDispY.Location = new System.Drawing.Point(132, 67);
            this.textBoxSupportTypeDispY.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxSupportTypeDispY.Name = "textBoxSupportTypeDispY";
            this.textBoxSupportTypeDispY.Size = new System.Drawing.Size(110, 31);
            this.textBoxSupportTypeDispY.TabIndex = 90;
            this.textBoxSupportTypeDispY.Text = "0.0";
            // 
            // textBoxSupportTypeDispX
            // 
            this.textBoxSupportTypeDispX.Location = new System.Drawing.Point(10, 67);
            this.textBoxSupportTypeDispX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxSupportTypeDispX.Name = "textBoxSupportTypeDispX";
            this.textBoxSupportTypeDispX.Size = new System.Drawing.Size(110, 31);
            this.textBoxSupportTypeDispX.TabIndex = 89;
            this.textBoxSupportTypeDispX.Text = "0.0";
            // 
            // checkBoxSupportStrong
            // 
            this.checkBoxSupportStrong.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxSupportStrong.AutoSize = true;
            this.checkBoxSupportStrong.Location = new System.Drawing.Point(526, 35);
            this.checkBoxSupportStrong.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxSupportStrong.Name = "checkBoxSupportStrong";
            this.checkBoxSupportStrong.Size = new System.Drawing.Size(107, 29);
            this.checkBoxSupportStrong.TabIndex = 43;
            this.checkBoxSupportStrong.Text = "Strong";
            this.checkBoxSupportStrong.UseVisualStyleBackColor = true;
            // 
            // checkBoxDispZ
            // 
            this.checkBoxDispZ.AutoSize = true;
            this.checkBoxDispZ.Checked = true;
            this.checkBoxDispZ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDispZ.Location = new System.Drawing.Point(254, 35);
            this.checkBoxDispZ.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxDispZ.Name = "checkBoxDispZ";
            this.checkBoxDispZ.Size = new System.Drawing.Size(106, 29);
            this.checkBoxDispZ.TabIndex = 42;
            this.checkBoxDispZ.Text = "Disp Z";
            this.checkBoxDispZ.UseVisualStyleBackColor = true;
            // 
            // checkBoxDispY
            // 
            this.checkBoxDispY.AutoSize = true;
            this.checkBoxDispY.Checked = true;
            this.checkBoxDispY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDispY.Location = new System.Drawing.Point(132, 35);
            this.checkBoxDispY.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxDispY.Name = "checkBoxDispY";
            this.checkBoxDispY.Size = new System.Drawing.Size(108, 29);
            this.checkBoxDispY.TabIndex = 41;
            this.checkBoxDispY.Text = "Disp Y";
            this.checkBoxDispY.UseVisualStyleBackColor = true;
            // 
            // checkBoxRotationSupport
            // 
            this.checkBoxRotationSupport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxRotationSupport.AutoSize = true;
            this.checkBoxRotationSupport.Location = new System.Drawing.Point(376, 35);
            this.checkBoxRotationSupport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxRotationSupport.Name = "checkBoxRotationSupport";
            this.checkBoxRotationSupport.Size = new System.Drawing.Size(124, 29);
            this.checkBoxRotationSupport.TabIndex = 42;
            this.checkBoxRotationSupport.Text = "Rotation";
            this.checkBoxRotationSupport.UseVisualStyleBackColor = true;
            // 
            // checkBoxDispX
            // 
            this.checkBoxDispX.AutoSize = true;
            this.checkBoxDispX.Checked = true;
            this.checkBoxDispX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDispX.Location = new System.Drawing.Point(10, 35);
            this.checkBoxDispX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxDispX.Name = "checkBoxDispX";
            this.checkBoxDispX.Size = new System.Drawing.Size(107, 29);
            this.checkBoxDispX.TabIndex = 40;
            this.checkBoxDispX.Text = "Disp X";
            this.checkBoxDispX.UseVisualStyleBackColor = true;
            // 
            // groupBoxSupportDimension
            // 
            this.groupBoxSupportDimension.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxSupportDimension.Controls.Add(this.radioButtonSupportDimVertex);
            this.groupBoxSupportDimension.Controls.Add(this.radioButtonSupportDimLine);
            this.groupBoxSupportDimension.Controls.Add(this.radioButtonSupportDimFace);
            this.groupBoxSupportDimension.Location = new System.Drawing.Point(0, 100);
            this.groupBoxSupportDimension.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxSupportDimension.Name = "groupBoxSupportDimension";
            this.groupBoxSupportDimension.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxSupportDimension.Size = new System.Drawing.Size(348, 88);
            this.groupBoxSupportDimension.TabIndex = 83;
            this.groupBoxSupportDimension.TabStop = false;
            this.groupBoxSupportDimension.Text = "Support Dimension";
            // 
            // radioButtonSupportDimVertex
            // 
            this.radioButtonSupportDimVertex.AutoSize = true;
            this.radioButtonSupportDimVertex.Location = new System.Drawing.Point(230, 37);
            this.radioButtonSupportDimVertex.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonSupportDimVertex.Name = "radioButtonSupportDimVertex";
            this.radioButtonSupportDimVertex.Size = new System.Drawing.Size(105, 29);
            this.radioButtonSupportDimVertex.TabIndex = 2;
            this.radioButtonSupportDimVertex.TabStop = true;
            this.radioButtonSupportDimVertex.Text = "Vertex";
            this.radioButtonSupportDimVertex.UseVisualStyleBackColor = true;
            // 
            // radioButtonSupportDimLine
            // 
            this.radioButtonSupportDimLine.AutoSize = true;
            this.radioButtonSupportDimLine.Location = new System.Drawing.Point(128, 37);
            this.radioButtonSupportDimLine.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonSupportDimLine.Name = "radioButtonSupportDimLine";
            this.radioButtonSupportDimLine.Size = new System.Drawing.Size(84, 29);
            this.radioButtonSupportDimLine.TabIndex = 1;
            this.radioButtonSupportDimLine.TabStop = true;
            this.radioButtonSupportDimLine.Text = "Line";
            this.radioButtonSupportDimLine.UseVisualStyleBackColor = true;
            // 
            // radioButtonSupportDimFace
            // 
            this.radioButtonSupportDimFace.AutoSize = true;
            this.radioButtonSupportDimFace.Location = new System.Drawing.Point(18, 37);
            this.radioButtonSupportDimFace.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonSupportDimFace.Name = "radioButtonSupportDimFace";
            this.radioButtonSupportDimFace.Size = new System.Drawing.Size(91, 29);
            this.radioButtonSupportDimFace.TabIndex = 0;
            this.radioButtonSupportDimFace.TabStop = true;
            this.radioButtonSupportDimFace.Text = "Face";
            this.radioButtonSupportDimFace.UseVisualStyleBackColor = true;
            // 
            // groupBoxSupportElementType
            // 
            this.groupBoxSupportElementType.Controls.Add(this.radioButtonSupportCurve);
            this.groupBoxSupportElementType.Controls.Add(this.radioButtonSupportSurface);
            this.groupBoxSupportElementType.Location = new System.Drawing.Point(0, 6);
            this.groupBoxSupportElementType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxSupportElementType.Name = "groupBoxSupportElementType";
            this.groupBoxSupportElementType.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxSupportElementType.Size = new System.Drawing.Size(290, 83);
            this.groupBoxSupportElementType.TabIndex = 82;
            this.groupBoxSupportElementType.TabStop = false;
            this.groupBoxSupportElementType.Text = "Structural Element";
            // 
            // radioButtonSupportCurve
            // 
            this.radioButtonSupportCurve.AutoSize = true;
            this.radioButtonSupportCurve.Location = new System.Drawing.Point(150, 37);
            this.radioButtonSupportCurve.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonSupportCurve.Name = "radioButtonSupportCurve";
            this.radioButtonSupportCurve.Size = new System.Drawing.Size(100, 29);
            this.radioButtonSupportCurve.TabIndex = 1;
            this.radioButtonSupportCurve.TabStop = true;
            this.radioButtonSupportCurve.Text = "Curve";
            this.radioButtonSupportCurve.UseVisualStyleBackColor = true;
            this.radioButtonSupportCurve.CheckedChanged += new System.EventHandler(this.radioButtonSupportCurve_CheckedChanged);
            // 
            // radioButtonSupportSurface
            // 
            this.radioButtonSupportSurface.AutoSize = true;
            this.radioButtonSupportSurface.Location = new System.Drawing.Point(18, 37);
            this.radioButtonSupportSurface.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonSupportSurface.Name = "radioButtonSupportSurface";
            this.radioButtonSupportSurface.Size = new System.Drawing.Size(117, 29);
            this.radioButtonSupportSurface.TabIndex = 0;
            this.radioButtonSupportSurface.TabStop = true;
            this.radioButtonSupportSurface.Text = "Surface";
            this.radioButtonSupportSurface.UseVisualStyleBackColor = true;
            this.radioButtonSupportSurface.CheckedChanged += new System.EventHandler(this.radioButtonSupportSurface_CheckedChanged);
            // 
            // buttonDeleteEdgeSupport
            // 
            this.buttonDeleteEdgeSupport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteEdgeSupport.Location = new System.Drawing.Point(426, 325);
            this.buttonDeleteEdgeSupport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonDeleteEdgeSupport.Name = "buttonDeleteEdgeSupport";
            this.buttonDeleteEdgeSupport.Size = new System.Drawing.Size(188, 48);
            this.buttonDeleteEdgeSupport.TabIndex = 5;
            this.buttonDeleteEdgeSupport.Text = "Delete Support";
            this.buttonDeleteEdgeSupport.UseVisualStyleBackColor = true;
            this.buttonDeleteEdgeSupport.Click += new System.EventHandler(this.buttonDeleteEdgeSupport_Click);
            // 
            // buttonAddEdgeSupports
            // 
            this.buttonAddEdgeSupports.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddEdgeSupports.Location = new System.Drawing.Point(218, 325);
            this.buttonAddEdgeSupports.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonAddEdgeSupports.Name = "buttonAddEdgeSupports";
            this.buttonAddEdgeSupports.Size = new System.Drawing.Size(200, 48);
            this.buttonAddEdgeSupports.TabIndex = 2;
            this.buttonAddEdgeSupports.Text = "Add Supports";
            this.buttonAddEdgeSupports.UseVisualStyleBackColor = true;
            this.buttonAddEdgeSupports.Click += new System.EventHandler(this.buttonAddEdgeSupports_Click);
            // 
            // tabPageLoad
            // 
            this.tabPageLoad.Controls.Add(this.groupBoxLoadInterval);
            this.tabPageLoad.Controls.Add(this.checkBoxLoadOverwrite);
            this.tabPageLoad.Controls.Add(this.textBoxLoadPositionV);
            this.tabPageLoad.Controls.Add(this.textBoxLoadPositionU);
            this.tabPageLoad.Controls.Add(this.textBoxLoadZ);
            this.tabPageLoad.Controls.Add(this.textBoxLoadY);
            this.tabPageLoad.Controls.Add(this.textBoxLoadX);
            this.tabPageLoad.Controls.Add(this.labelLoadDirectionZ);
            this.tabPageLoad.Controls.Add(this.labelLoadDirectionY);
            this.tabPageLoad.Controls.Add(this.labelLoadDirectionX);
            this.tabPageLoad.Controls.Add(this.labelLoadPositionV);
            this.tabPageLoad.Controls.Add(this.labelLoadPositionU);
            this.tabPageLoad.Controls.Add(this.labelLoadPosition);
            this.tabPageLoad.Controls.Add(this.labelLoadDirection);
            this.tabPageLoad.Controls.Add(this.groupBoxLoadDimension);
            this.tabPageLoad.Controls.Add(this.groupBoxLoadElement);
            this.tabPageLoad.Controls.Add(this.comboBoxLoadType);
            this.tabPageLoad.Controls.Add(this.buttonDeleteLoad);
            this.tabPageLoad.Controls.Add(this.buttonAddLoad);
            this.tabPageLoad.ImageIndex = 3;
            this.tabPageLoad.Location = new System.Drawing.Point(8, 39);
            this.tabPageLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageLoad.Name = "tabPageLoad";
            this.tabPageLoad.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageLoad.Size = new System.Drawing.Size(654, 449);
            this.tabPageLoad.TabIndex = 7;
            this.tabPageLoad.Text = "Load";
            this.tabPageLoad.UseVisualStyleBackColor = true;
            // 
            // groupBoxLoadInterval
            // 
            this.groupBoxLoadInterval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLoadInterval.Controls.Add(this.textBoxLoadEndTime);
            this.groupBoxLoadInterval.Controls.Add(this.textBoxLoadStartTime);
            this.groupBoxLoadInterval.Location = new System.Drawing.Point(304, 6);
            this.groupBoxLoadInterval.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxLoadInterval.Name = "groupBoxLoadInterval";
            this.groupBoxLoadInterval.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxLoadInterval.Size = new System.Drawing.Size(316, 83);
            this.groupBoxLoadInterval.TabIndex = 95;
            this.groupBoxLoadInterval.TabStop = false;
            this.groupBoxLoadInterval.Text = "Interval: Start Time - End Time";
            // 
            // textBoxLoadEndTime
            // 
            this.textBoxLoadEndTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLoadEndTime.Location = new System.Drawing.Point(158, 35);
            this.textBoxLoadEndTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLoadEndTime.Name = "textBoxLoadEndTime";
            this.textBoxLoadEndTime.Size = new System.Drawing.Size(144, 31);
            this.textBoxLoadEndTime.TabIndex = 93;
            this.textBoxLoadEndTime.Text = "End";
            // 
            // textBoxLoadStartTime
            // 
            this.textBoxLoadStartTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLoadStartTime.Location = new System.Drawing.Point(10, 35);
            this.textBoxLoadStartTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLoadStartTime.Name = "textBoxLoadStartTime";
            this.textBoxLoadStartTime.Size = new System.Drawing.Size(112, 31);
            this.textBoxLoadStartTime.TabIndex = 92;
            this.textBoxLoadStartTime.Text = "0.0";
            // 
            // checkBoxLoadOverwrite
            // 
            this.checkBoxLoadOverwrite.AutoSize = true;
            this.checkBoxLoadOverwrite.Checked = true;
            this.checkBoxLoadOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLoadOverwrite.Location = new System.Drawing.Point(16, 344);
            this.checkBoxLoadOverwrite.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxLoadOverwrite.Name = "checkBoxLoadOverwrite";
            this.checkBoxLoadOverwrite.Size = new System.Drawing.Size(135, 29);
            this.checkBoxLoadOverwrite.TabIndex = 94;
            this.checkBoxLoadOverwrite.Text = "Overwrite";
            this.checkBoxLoadOverwrite.UseVisualStyleBackColor = true;
            // 
            // textBoxLoadPositionV
            // 
            this.textBoxLoadPositionV.Location = new System.Drawing.Point(384, 196);
            this.textBoxLoadPositionV.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxLoadPositionV.Name = "textBoxLoadPositionV";
            this.textBoxLoadPositionV.Size = new System.Drawing.Size(132, 31);
            this.textBoxLoadPositionV.TabIndex = 93;
            // 
            // textBoxLoadPositionU
            // 
            this.textBoxLoadPositionU.Location = new System.Drawing.Point(172, 196);
            this.textBoxLoadPositionU.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxLoadPositionU.Name = "textBoxLoadPositionU";
            this.textBoxLoadPositionU.Size = new System.Drawing.Size(132, 31);
            this.textBoxLoadPositionU.TabIndex = 92;
            // 
            // textBoxLoadZ
            // 
            this.textBoxLoadZ.Location = new System.Drawing.Point(520, 288);
            this.textBoxLoadZ.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLoadZ.Name = "textBoxLoadZ";
            this.textBoxLoadZ.Size = new System.Drawing.Size(120, 31);
            this.textBoxLoadZ.TabIndex = 77;
            this.textBoxLoadZ.Text = "1.0";
            // 
            // textBoxLoadY
            // 
            this.textBoxLoadY.Location = new System.Drawing.Point(336, 288);
            this.textBoxLoadY.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLoadY.Name = "textBoxLoadY";
            this.textBoxLoadY.Size = new System.Drawing.Size(132, 31);
            this.textBoxLoadY.TabIndex = 77;
            this.textBoxLoadY.Text = "0.0";
            // 
            // textBoxLoadX
            // 
            this.textBoxLoadX.Location = new System.Drawing.Point(152, 288);
            this.textBoxLoadX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLoadX.Name = "textBoxLoadX";
            this.textBoxLoadX.Size = new System.Drawing.Size(132, 31);
            this.textBoxLoadX.TabIndex = 76;
            this.textBoxLoadX.Text = "0.0";
            // 
            // labelLoadDirectionZ
            // 
            this.labelLoadDirectionZ.AutoSize = true;
            this.labelLoadDirectionZ.Location = new System.Drawing.Point(482, 294);
            this.labelLoadDirectionZ.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLoadDirectionZ.Name = "labelLoadDirectionZ";
            this.labelLoadDirectionZ.Size = new System.Drawing.Size(25, 25);
            this.labelLoadDirectionZ.TabIndex = 91;
            this.labelLoadDirectionZ.Text = "Z";
            // 
            // labelLoadDirectionY
            // 
            this.labelLoadDirectionY.AutoSize = true;
            this.labelLoadDirectionY.Location = new System.Drawing.Point(298, 294);
            this.labelLoadDirectionY.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLoadDirectionY.Name = "labelLoadDirectionY";
            this.labelLoadDirectionY.Size = new System.Drawing.Size(27, 25);
            this.labelLoadDirectionY.TabIndex = 90;
            this.labelLoadDirectionY.Text = "Y";
            // 
            // labelLoadDirectionX
            // 
            this.labelLoadDirectionX.AutoSize = true;
            this.labelLoadDirectionX.Location = new System.Drawing.Point(114, 294);
            this.labelLoadDirectionX.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLoadDirectionX.Name = "labelLoadDirectionX";
            this.labelLoadDirectionX.Size = new System.Drawing.Size(26, 25);
            this.labelLoadDirectionX.TabIndex = 89;
            this.labelLoadDirectionX.Text = "X";
            // 
            // labelLoadPositionV
            // 
            this.labelLoadPositionV.AutoSize = true;
            this.labelLoadPositionV.Location = new System.Drawing.Point(320, 202);
            this.labelLoadPositionV.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLoadPositionV.Name = "labelLoadPositionV";
            this.labelLoadPositionV.Size = new System.Drawing.Size(50, 25);
            this.labelLoadPositionV.TabIndex = 88;
            this.labelLoadPositionV.Text = "V = ";
            // 
            // labelLoadPositionU
            // 
            this.labelLoadPositionU.AutoSize = true;
            this.labelLoadPositionU.Location = new System.Drawing.Point(106, 202);
            this.labelLoadPositionU.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLoadPositionU.Name = "labelLoadPositionU";
            this.labelLoadPositionU.Size = new System.Drawing.Size(51, 25);
            this.labelLoadPositionU.TabIndex = 87;
            this.labelLoadPositionU.Text = "U = ";
            // 
            // labelLoadPosition
            // 
            this.labelLoadPosition.AutoSize = true;
            this.labelLoadPosition.Location = new System.Drawing.Point(10, 202);
            this.labelLoadPosition.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLoadPosition.Name = "labelLoadPosition";
            this.labelLoadPosition.Size = new System.Drawing.Size(95, 25);
            this.labelLoadPosition.TabIndex = 86;
            this.labelLoadPosition.Text = "Position:";
            // 
            // labelLoadDirection
            // 
            this.labelLoadDirection.AutoSize = true;
            this.labelLoadDirection.Location = new System.Drawing.Point(8, 294);
            this.labelLoadDirection.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLoadDirection.Name = "labelLoadDirection";
            this.labelLoadDirection.Size = new System.Drawing.Size(66, 25);
            this.labelLoadDirection.TabIndex = 85;
            this.labelLoadDirection.Text = "Load:";
            // 
            // groupBoxLoadDimension
            // 
            this.groupBoxLoadDimension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLoadDimension.Controls.Add(this.radioButtonLoadDimVertex);
            this.groupBoxLoadDimension.Controls.Add(this.radioButtonLoadDimLine);
            this.groupBoxLoadDimension.Controls.Add(this.radioButtonLoadDimFace);
            this.groupBoxLoadDimension.Location = new System.Drawing.Point(6, 100);
            this.groupBoxLoadDimension.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxLoadDimension.Name = "groupBoxLoadDimension";
            this.groupBoxLoadDimension.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxLoadDimension.Size = new System.Drawing.Size(614, 88);
            this.groupBoxLoadDimension.TabIndex = 84;
            this.groupBoxLoadDimension.TabStop = false;
            this.groupBoxLoadDimension.Text = "Load Dimension";
            // 
            // radioButtonLoadDimVertex
            // 
            this.radioButtonLoadDimVertex.AutoSize = true;
            this.radioButtonLoadDimVertex.Location = new System.Drawing.Point(270, 37);
            this.radioButtonLoadDimVertex.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonLoadDimVertex.Name = "radioButtonLoadDimVertex";
            this.radioButtonLoadDimVertex.Size = new System.Drawing.Size(105, 29);
            this.radioButtonLoadDimVertex.TabIndex = 2;
            this.radioButtonLoadDimVertex.TabStop = true;
            this.radioButtonLoadDimVertex.Text = "Vertex";
            this.radioButtonLoadDimVertex.UseVisualStyleBackColor = true;
            this.radioButtonLoadDimVertex.CheckedChanged += new System.EventHandler(this.radioButtonLoadDimVertex_CheckedChanged);
            // 
            // radioButtonLoadDimLine
            // 
            this.radioButtonLoadDimLine.AutoSize = true;
            this.radioButtonLoadDimLine.Location = new System.Drawing.Point(150, 37);
            this.radioButtonLoadDimLine.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonLoadDimLine.Name = "radioButtonLoadDimLine";
            this.radioButtonLoadDimLine.Size = new System.Drawing.Size(84, 29);
            this.radioButtonLoadDimLine.TabIndex = 1;
            this.radioButtonLoadDimLine.TabStop = true;
            this.radioButtonLoadDimLine.Text = "Line";
            this.radioButtonLoadDimLine.UseVisualStyleBackColor = true;
            this.radioButtonLoadDimLine.CheckedChanged += new System.EventHandler(this.radioButtonLoadDimLine_CheckedChanged);
            // 
            // radioButtonLoadDimFace
            // 
            this.radioButtonLoadDimFace.AutoSize = true;
            this.radioButtonLoadDimFace.Location = new System.Drawing.Point(18, 37);
            this.radioButtonLoadDimFace.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonLoadDimFace.Name = "radioButtonLoadDimFace";
            this.radioButtonLoadDimFace.Size = new System.Drawing.Size(91, 29);
            this.radioButtonLoadDimFace.TabIndex = 0;
            this.radioButtonLoadDimFace.TabStop = true;
            this.radioButtonLoadDimFace.Text = "Face";
            this.radioButtonLoadDimFace.UseVisualStyleBackColor = true;
            this.radioButtonLoadDimFace.CheckedChanged += new System.EventHandler(this.radioButtonLoadDimFace_CheckedChanged);
            // 
            // groupBoxLoadElement
            // 
            this.groupBoxLoadElement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLoadElement.Controls.Add(this.radioButtonLoadElementCurve);
            this.groupBoxLoadElement.Controls.Add(this.radioButtonLoadElementSurface);
            this.groupBoxLoadElement.Location = new System.Drawing.Point(6, 6);
            this.groupBoxLoadElement.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxLoadElement.Name = "groupBoxLoadElement";
            this.groupBoxLoadElement.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxLoadElement.Size = new System.Drawing.Size(268, 83);
            this.groupBoxLoadElement.TabIndex = 83;
            this.groupBoxLoadElement.TabStop = false;
            this.groupBoxLoadElement.Text = "Structural Element";
            // 
            // radioButtonLoadElementCurve
            // 
            this.radioButtonLoadElementCurve.AutoSize = true;
            this.radioButtonLoadElementCurve.Location = new System.Drawing.Point(150, 37);
            this.radioButtonLoadElementCurve.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonLoadElementCurve.Name = "radioButtonLoadElementCurve";
            this.radioButtonLoadElementCurve.Size = new System.Drawing.Size(100, 29);
            this.radioButtonLoadElementCurve.TabIndex = 1;
            this.radioButtonLoadElementCurve.TabStop = true;
            this.radioButtonLoadElementCurve.Text = "Curve";
            this.radioButtonLoadElementCurve.UseVisualStyleBackColor = true;
            this.radioButtonLoadElementCurve.CheckedChanged += new System.EventHandler(this.radioButtonLoadCurve_CheckedChanged);
            // 
            // radioButtonLoadElementSurface
            // 
            this.radioButtonLoadElementSurface.AutoSize = true;
            this.radioButtonLoadElementSurface.Location = new System.Drawing.Point(18, 37);
            this.radioButtonLoadElementSurface.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonLoadElementSurface.Name = "radioButtonLoadElementSurface";
            this.radioButtonLoadElementSurface.Size = new System.Drawing.Size(117, 29);
            this.radioButtonLoadElementSurface.TabIndex = 0;
            this.radioButtonLoadElementSurface.TabStop = true;
            this.radioButtonLoadElementSurface.Text = "Surface";
            this.radioButtonLoadElementSurface.UseVisualStyleBackColor = true;
            this.radioButtonLoadElementSurface.CheckedChanged += new System.EventHandler(this.radioButtonLoadSurface_CheckedChanged);
            // 
            // comboBoxLoadType
            // 
            this.comboBoxLoadType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLoadType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBoxLoadType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxLoadType.FormattingEnabled = true;
            this.comboBoxLoadType.Items.AddRange(new object[] {
            "DEAD",
            "PRES",
            "PRES_FL",
            "SNOW",
            "MOMENT",
            "MOMENT_5P_DIRECTOR"});
            this.comboBoxLoadType.Location = new System.Drawing.Point(14, 240);
            this.comboBoxLoadType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxLoadType.Name = "comboBoxLoadType";
            this.comboBoxLoadType.Size = new System.Drawing.Size(602, 33);
            this.comboBoxLoadType.TabIndex = 81;
            this.comboBoxLoadType.Text = "DEAD";
            this.comboBoxLoadType.UseWaitCursor = true;
            this.comboBoxLoadType.SelectedIndexChanged += new System.EventHandler(this.comboBoxLoadType_SelectedIndexChanged);
            // 
            // buttonDeleteLoad
            // 
            this.buttonDeleteLoad.Location = new System.Drawing.Point(402, 335);
            this.buttonDeleteLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonDeleteLoad.Name = "buttonDeleteLoad";
            this.buttonDeleteLoad.Size = new System.Drawing.Size(242, 48);
            this.buttonDeleteLoad.TabIndex = 78;
            this.buttonDeleteLoad.Text = "Delete Load";
            this.buttonDeleteLoad.UseVisualStyleBackColor = true;
            this.buttonDeleteLoad.Click += new System.EventHandler(this.buttonDeleteLoad_Click);
            // 
            // buttonAddLoad
            // 
            this.buttonAddLoad.Location = new System.Drawing.Point(158, 335);
            this.buttonAddLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonAddLoad.Name = "buttonAddLoad";
            this.buttonAddLoad.Size = new System.Drawing.Size(236, 48);
            this.buttonAddLoad.TabIndex = 79;
            this.buttonAddLoad.Text = "Add Load";
            this.buttonAddLoad.UseVisualStyleBackColor = true;
            this.buttonAddLoad.Click += new System.EventHandler(this.buttonAddLoad_Click);
            // 
            // tabPageCheck
            // 
            this.tabPageCheck.Controls.Add(this.groupBox1);
            this.tabPageCheck.Controls.Add(this.checkBoxOverwriteChecks);
            this.tabPageCheck.Controls.Add(this.groupBox2);
            this.tabPageCheck.Controls.Add(this.groupBox3);
            this.tabPageCheck.Controls.Add(this.groupBoxCheckStructuralElement);
            this.tabPageCheck.Controls.Add(this.button1);
            this.tabPageCheck.Controls.Add(this.buttonAddCheck);
            this.tabPageCheck.Location = new System.Drawing.Point(8, 39);
            this.tabPageCheck.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageCheck.Name = "tabPageCheck";
            this.tabPageCheck.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPageCheck.Size = new System.Drawing.Size(654, 449);
            this.tabPageCheck.TabIndex = 10;
            this.tabPageCheck.Text = "Output";
            this.tabPageCheck.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxCheckEndTime);
            this.groupBox1.Controls.Add(this.textBoxCheckStartTime);
            this.groupBox1.Location = new System.Drawing.Point(306, 6);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox1.Size = new System.Drawing.Size(346, 83);
            this.groupBox1.TabIndex = 100;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Interval: Start Time - End Time";
            // 
            // textBoxCheckEndTime
            // 
            this.textBoxCheckEndTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCheckEndTime.Location = new System.Drawing.Point(182, 35);
            this.textBoxCheckEndTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxCheckEndTime.Name = "textBoxCheckEndTime";
            this.textBoxCheckEndTime.Size = new System.Drawing.Size(128, 31);
            this.textBoxCheckEndTime.TabIndex = 93;
            this.textBoxCheckEndTime.Text = "End";
            // 
            // textBoxCheckStartTime
            // 
            this.textBoxCheckStartTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCheckStartTime.Location = new System.Drawing.Point(10, 35);
            this.textBoxCheckStartTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxCheckStartTime.Name = "textBoxCheckStartTime";
            this.textBoxCheckStartTime.Size = new System.Drawing.Size(96, 31);
            this.textBoxCheckStartTime.TabIndex = 92;
            this.textBoxCheckStartTime.Text = "0.0";
            // 
            // checkBoxOverwriteChecks
            // 
            this.checkBoxOverwriteChecks.AutoSize = true;
            this.checkBoxOverwriteChecks.Checked = true;
            this.checkBoxOverwriteChecks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOverwriteChecks.Location = new System.Drawing.Point(10, 342);
            this.checkBoxOverwriteChecks.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxOverwriteChecks.Name = "checkBoxOverwriteChecks";
            this.checkBoxOverwriteChecks.Size = new System.Drawing.Size(135, 29);
            this.checkBoxOverwriteChecks.TabIndex = 102;
            this.checkBoxOverwriteChecks.Text = "Overwrite";
            this.checkBoxOverwriteChecks.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBoxOutputLagrangeMP);
            this.groupBox2.Controls.Add(this.checkBoxOutputDispZ);
            this.groupBox2.Controls.Add(this.checkBoxOutputDispX);
            this.groupBox2.Controls.Add(this.checkBoxOutputDispY);
            this.groupBox2.Location = new System.Drawing.Point(6, 200);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox2.Size = new System.Drawing.Size(646, 115);
            this.groupBox2.TabIndex = 97;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Check Type";
            // 
            // checkBoxOutputLagrangeMP
            // 
            this.checkBoxOutputLagrangeMP.AutoSize = true;
            this.checkBoxOutputLagrangeMP.Location = new System.Drawing.Point(12, 71);
            this.checkBoxOutputLagrangeMP.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxOutputLagrangeMP.Name = "checkBoxOutputLagrangeMP";
            this.checkBoxOutputLagrangeMP.Size = new System.Drawing.Size(204, 29);
            this.checkBoxOutputLagrangeMP.TabIndex = 3;
            this.checkBoxOutputLagrangeMP.Text = "LAGRANGE_MP";
            this.checkBoxOutputLagrangeMP.UseVisualStyleBackColor = true;
            // 
            // checkBoxOutputDispZ
            // 
            this.checkBoxOutputDispZ.AutoSize = true;
            this.checkBoxOutputDispZ.Location = new System.Drawing.Point(292, 37);
            this.checkBoxOutputDispZ.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxOutputDispZ.Name = "checkBoxOutputDispZ";
            this.checkBoxOutputDispZ.Size = new System.Drawing.Size(117, 29);
            this.checkBoxOutputDispZ.TabIndex = 2;
            this.checkBoxOutputDispZ.Text = "DISP_Z";
            this.checkBoxOutputDispZ.UseVisualStyleBackColor = true;
            // 
            // checkBoxOutputDispX
            // 
            this.checkBoxOutputDispX.AutoSize = true;
            this.checkBoxOutputDispX.Location = new System.Drawing.Point(12, 37);
            this.checkBoxOutputDispX.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxOutputDispX.Name = "checkBoxOutputDispX";
            this.checkBoxOutputDispX.Size = new System.Drawing.Size(118, 29);
            this.checkBoxOutputDispX.TabIndex = 1;
            this.checkBoxOutputDispX.Text = "DISP_X";
            this.checkBoxOutputDispX.UseVisualStyleBackColor = true;
            // 
            // checkBoxOutputDispY
            // 
            this.checkBoxOutputDispY.AutoSize = true;
            this.checkBoxOutputDispY.Location = new System.Drawing.Point(152, 37);
            this.checkBoxOutputDispY.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxOutputDispY.Name = "checkBoxOutputDispY";
            this.checkBoxOutputDispY.Size = new System.Drawing.Size(119, 29);
            this.checkBoxOutputDispY.TabIndex = 0;
            this.checkBoxOutputDispY.Text = "DISP_Y";
            this.checkBoxOutputDispY.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.radioButtonCheckVertex);
            this.groupBox3.Controls.Add(this.radioButtonCheckLine);
            this.groupBox3.Controls.Add(this.radioButtonCheckFace);
            this.groupBox3.Location = new System.Drawing.Point(4, 100);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox3.Size = new System.Drawing.Size(648, 88);
            this.groupBox3.TabIndex = 101;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Dimension";
            // 
            // radioButtonCheckVertex
            // 
            this.radioButtonCheckVertex.AutoSize = true;
            this.radioButtonCheckVertex.Location = new System.Drawing.Point(270, 37);
            this.radioButtonCheckVertex.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonCheckVertex.Name = "radioButtonCheckVertex";
            this.radioButtonCheckVertex.Size = new System.Drawing.Size(105, 29);
            this.radioButtonCheckVertex.TabIndex = 2;
            this.radioButtonCheckVertex.TabStop = true;
            this.radioButtonCheckVertex.Text = "Vertex";
            this.radioButtonCheckVertex.UseVisualStyleBackColor = true;
            // 
            // radioButtonCheckLine
            // 
            this.radioButtonCheckLine.AutoSize = true;
            this.radioButtonCheckLine.Location = new System.Drawing.Point(150, 37);
            this.radioButtonCheckLine.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonCheckLine.Name = "radioButtonCheckLine";
            this.radioButtonCheckLine.Size = new System.Drawing.Size(84, 29);
            this.radioButtonCheckLine.TabIndex = 1;
            this.radioButtonCheckLine.TabStop = true;
            this.radioButtonCheckLine.Text = "Line";
            this.radioButtonCheckLine.UseVisualStyleBackColor = true;
            // 
            // radioButtonCheckFace
            // 
            this.radioButtonCheckFace.AutoSize = true;
            this.radioButtonCheckFace.Location = new System.Drawing.Point(18, 37);
            this.radioButtonCheckFace.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonCheckFace.Name = "radioButtonCheckFace";
            this.radioButtonCheckFace.Size = new System.Drawing.Size(91, 29);
            this.radioButtonCheckFace.TabIndex = 0;
            this.radioButtonCheckFace.TabStop = true;
            this.radioButtonCheckFace.Text = "Face";
            this.radioButtonCheckFace.UseVisualStyleBackColor = true;
            // 
            // groupBoxCheckStructuralElement
            // 
            this.groupBoxCheckStructuralElement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCheckStructuralElement.Controls.Add(this.radioButtonCheckCurve);
            this.groupBoxCheckStructuralElement.Controls.Add(this.radioButtonCheckSurface);
            this.groupBoxCheckStructuralElement.Location = new System.Drawing.Point(4, 6);
            this.groupBoxCheckStructuralElement.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxCheckStructuralElement.Name = "groupBoxCheckStructuralElement";
            this.groupBoxCheckStructuralElement.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxCheckStructuralElement.Size = new System.Drawing.Size(278, 83);
            this.groupBoxCheckStructuralElement.TabIndex = 99;
            this.groupBoxCheckStructuralElement.TabStop = false;
            this.groupBoxCheckStructuralElement.Text = "Structural Element";
            // 
            // radioButtonCheckCurve
            // 
            this.radioButtonCheckCurve.AutoSize = true;
            this.radioButtonCheckCurve.Location = new System.Drawing.Point(150, 37);
            this.radioButtonCheckCurve.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonCheckCurve.Name = "radioButtonCheckCurve";
            this.radioButtonCheckCurve.Size = new System.Drawing.Size(100, 29);
            this.radioButtonCheckCurve.TabIndex = 1;
            this.radioButtonCheckCurve.TabStop = true;
            this.radioButtonCheckCurve.Text = "Curve";
            this.radioButtonCheckCurve.UseVisualStyleBackColor = true;
            // 
            // radioButtonCheckSurface
            // 
            this.radioButtonCheckSurface.AutoSize = true;
            this.radioButtonCheckSurface.Location = new System.Drawing.Point(18, 37);
            this.radioButtonCheckSurface.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioButtonCheckSurface.Name = "radioButtonCheckSurface";
            this.radioButtonCheckSurface.Size = new System.Drawing.Size(117, 29);
            this.radioButtonCheckSurface.TabIndex = 0;
            this.radioButtonCheckSurface.TabStop = true;
            this.radioButtonCheckSurface.Text = "Surface";
            this.radioButtonCheckSurface.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(412, 325);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(244, 48);
            this.button1.TabIndex = 98;
            this.button1.Text = "Check";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // buttonAddCheck
            // 
            this.buttonAddCheck.Location = new System.Drawing.Point(184, 325);
            this.buttonAddCheck.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonAddCheck.Name = "buttonAddCheck";
            this.buttonAddCheck.Size = new System.Drawing.Size(220, 48);
            this.buttonAddCheck.TabIndex = 96;
            this.buttonAddCheck.Text = "Add Checks";
            this.buttonAddCheck.UseVisualStyleBackColor = true;
            this.buttonAddCheck.Click += new System.EventHandler(this.buttonAddCheck_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.open_file);
            this.tabPage2.Controls.Add(this.buttonShowPost);
            this.tabPage2.Controls.Add(this.buttonClearPost);
            this.tabPage2.Controls.Add(this.groupBoxVisualization);
            this.tabPage2.Controls.Add(this.groupBoxAnalysisStep);
            this.tabPage2.Location = new System.Drawing.Point(8, 39);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage2.Size = new System.Drawing.Size(876, 1470);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Post Processing";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // open_file
            // 
            this.open_file.Location = new System.Drawing.Point(420, 1035);
            this.open_file.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.open_file.Name = "open_file";
            this.open_file.Size = new System.Drawing.Size(202, 46);
            this.open_file.TabIndex = 13;
            this.open_file.Text = "Open File";
            this.open_file.UseVisualStyleBackColor = true;
            this.open_file.Click += new System.EventHandler(this.open_file_Click);
            // 
            // buttonShowPost
            // 
            this.buttonShowPost.Location = new System.Drawing.Point(6, 1035);
            this.buttonShowPost.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonShowPost.Name = "buttonShowPost";
            this.buttonShowPost.Size = new System.Drawing.Size(202, 46);
            this.buttonShowPost.TabIndex = 11;
            this.buttonShowPost.Text = "Show";
            this.buttonShowPost.UseVisualStyleBackColor = true;
            this.buttonShowPost.Click += new System.EventHandler(this.buttonShowPost_Click);
            // 
            // buttonClearPost
            // 
            this.buttonClearPost.Location = new System.Drawing.Point(214, 1035);
            this.buttonClearPost.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonClearPost.Name = "buttonClearPost";
            this.buttonClearPost.Size = new System.Drawing.Size(202, 46);
            this.buttonClearPost.TabIndex = 12;
            this.buttonClearPost.Text = "Clear";
            this.buttonClearPost.UseVisualStyleBackColor = true;
            this.buttonClearPost.Click += new System.EventHandler(this.buttonClearPost_Click);
            // 
            // groupBoxVisualization
            // 
            this.groupBoxVisualization.Controls.Add(this.button3);
            this.groupBoxVisualization.Controls.Add(this.groupBox5);
            this.groupBoxVisualization.Controls.Add(this.comboBoxPostProcessingDirection);
            this.groupBoxVisualization.Controls.Add(this.labelPostProcessingDirection);
            this.groupBoxVisualization.Controls.Add(this.textBoxColorBarMin);
            this.groupBoxVisualization.Controls.Add(this.textBoxColorBarMax);
            this.groupBoxVisualization.Controls.Add(this.groupBoxScalings);
            this.groupBoxVisualization.Controls.Add(this.pictureBoxColorBar);
            this.groupBoxVisualization.Controls.Add(this.groupBoxShow);
            this.groupBoxVisualization.Controls.Add(this.comboBoxResultType);
            this.groupBoxVisualization.Location = new System.Drawing.Point(6, 198);
            this.groupBoxVisualization.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxVisualization.Name = "groupBoxVisualization";
            this.groupBoxVisualization.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxVisualization.Size = new System.Drawing.Size(616, 825);
            this.groupBoxVisualization.TabIndex = 10;
            this.groupBoxVisualization.TabStop = false;
            this.groupBoxVisualization.Text = "Visualization";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(196, 131);
            this.button3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(214, 38);
            this.button3.TabIndex = 15;
            this.button3.Text = "Auto Min/Max";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.AutoMinMax);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBoxShowMesh);
            this.groupBox5.Controls.Add(this.button2);
            this.groupBox5.Location = new System.Drawing.Point(12, 612);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox5.Size = new System.Drawing.Size(592, 98);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Visualization Mesh";
            // 
            // checkBoxShowMesh
            // 
            this.checkBoxShowMesh.AutoSize = true;
            this.checkBoxShowMesh.Location = new System.Drawing.Point(314, 46);
            this.checkBoxShowMesh.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxShowMesh.Name = "checkBoxShowMesh";
            this.checkBoxShowMesh.Size = new System.Drawing.Size(156, 29);
            this.checkBoxShowMesh.TabIndex = 15;
            this.checkBoxShowMesh.Text = "Show Mesh";
            this.checkBoxShowMesh.UseVisualStyleBackColor = true;
            this.checkBoxShowMesh.CheckedChanged += new System.EventHandler(this.checkBoxShowMesh_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 37);
            this.button2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(214, 46);
            this.button2.TabIndex = 14;
            this.button2.Text = "Refinement";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.MeshShowPreview);
            // 
            // comboBoxPostProcessingDirection
            // 
            this.comboBoxPostProcessingDirection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPostProcessingDirection.FormattingEnabled = true;
            this.comboBoxPostProcessingDirection.Location = new System.Drawing.Point(436, 37);
            this.comboBoxPostProcessingDirection.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxPostProcessingDirection.Name = "comboBoxPostProcessingDirection";
            this.comboBoxPostProcessingDirection.Size = new System.Drawing.Size(164, 33);
            this.comboBoxPostProcessingDirection.TabIndex = 21;
            this.comboBoxPostProcessingDirection.SelectedIndexChanged += new System.EventHandler(this.comboBoxPostProcessingDirection_SelectedIndexChanged);
            // 
            // labelPostProcessingDirection
            // 
            this.labelPostProcessingDirection.AutoSize = true;
            this.labelPostProcessingDirection.Location = new System.Drawing.Point(320, 42);
            this.labelPostProcessingDirection.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelPostProcessingDirection.Name = "labelPostProcessingDirection";
            this.labelPostProcessingDirection.Size = new System.Drawing.Size(103, 25);
            this.labelPostProcessingDirection.TabIndex = 20;
            this.labelPostProcessingDirection.Text = "Direction:";
            // 
            // textBoxColorBarMin
            // 
            this.textBoxColorBarMin.Location = new System.Drawing.Point(10, 131);
            this.textBoxColorBarMin.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxColorBarMin.Name = "textBoxColorBarMin";
            this.textBoxColorBarMin.Size = new System.Drawing.Size(130, 31);
            this.textBoxColorBarMin.TabIndex = 19;
            this.textBoxColorBarMin.Text = "0";
            this.textBoxColorBarMin.TextChanged += new System.EventHandler(this.textBoxColorBarMin_TextChanged);
            // 
            // textBoxColorBarMax
            // 
            this.textBoxColorBarMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxColorBarMax.Location = new System.Drawing.Point(466, 131);
            this.textBoxColorBarMax.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxColorBarMax.Name = "textBoxColorBarMax";
            this.textBoxColorBarMax.Size = new System.Drawing.Size(134, 31);
            this.textBoxColorBarMax.TabIndex = 18;
            this.textBoxColorBarMax.Text = "1";
            this.textBoxColorBarMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxColorBarMax.TextChanged += new System.EventHandler(this.textBoxColorBarMax_TextChanged);
            // 
            // groupBoxScalings
            // 
            this.groupBoxScalings.Controls.Add(this.textBoxFlyingNodeLimit);
            this.groupBoxScalings.Controls.Add(this.textBoxResScale);
            this.groupBoxScalings.Controls.Add(this.textBoxDispScale);
            this.groupBoxScalings.Controls.Add(this.labelFlyingNodeLimit);
            this.groupBoxScalings.Controls.Add(this.labelDispScale);
            this.groupBoxScalings.Controls.Add(this.labelResScale);
            this.groupBoxScalings.Location = new System.Drawing.Point(10, 181);
            this.groupBoxScalings.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxScalings.Name = "groupBoxScalings";
            this.groupBoxScalings.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxScalings.Size = new System.Drawing.Size(482, 192);
            this.groupBoxScalings.TabIndex = 17;
            this.groupBoxScalings.TabStop = false;
            this.groupBoxScalings.Text = "Scalings";
            // 
            // textBoxFlyingNodeLimit
            // 
            this.textBoxFlyingNodeLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFlyingNodeLimit.Location = new System.Drawing.Point(248, 137);
            this.textBoxFlyingNodeLimit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxFlyingNodeLimit.Name = "textBoxFlyingNodeLimit";
            this.textBoxFlyingNodeLimit.Size = new System.Drawing.Size(214, 31);
            this.textBoxFlyingNodeLimit.TabIndex = 13;
            this.textBoxFlyingNodeLimit.Text = "1.000e+05";
            this.textBoxFlyingNodeLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxFlyingNodeLimit.TextChanged += new System.EventHandler(this.textBoxFlyingNodeLimit_TextChanged);
            // 
            // textBoxResScale
            // 
            this.textBoxResScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResScale.Location = new System.Drawing.Point(248, 87);
            this.textBoxResScale.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxResScale.Name = "textBoxResScale";
            this.textBoxResScale.Size = new System.Drawing.Size(214, 31);
            this.textBoxResScale.TabIndex = 12;
            this.textBoxResScale.Text = "1.000e+00";
            this.textBoxResScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxResScale.TextChanged += new System.EventHandler(this.textBoxResScale_TextChanged);
            // 
            // textBoxDispScale
            // 
            this.textBoxDispScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDispScale.Location = new System.Drawing.Point(248, 37);
            this.textBoxDispScale.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxDispScale.Name = "textBoxDispScale";
            this.textBoxDispScale.Size = new System.Drawing.Size(214, 31);
            this.textBoxDispScale.TabIndex = 11;
            this.textBoxDispScale.Text = "1.000e+00";
            this.textBoxDispScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxDispScale.TextChanged += new System.EventHandler(this.textBoxDispScale_TextChanged);
            // 
            // labelFlyingNodeLimit
            // 
            this.labelFlyingNodeLimit.AutoSize = true;
            this.labelFlyingNodeLimit.Location = new System.Drawing.Point(12, 142);
            this.labelFlyingNodeLimit.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelFlyingNodeLimit.Name = "labelFlyingNodeLimit";
            this.labelFlyingNodeLimit.Size = new System.Drawing.Size(184, 25);
            this.labelFlyingNodeLimit.TabIndex = 10;
            this.labelFlyingNodeLimit.Text = "Flying Node Limit:";
            // 
            // labelDispScale
            // 
            this.labelDispScale.AutoSize = true;
            this.labelDispScale.Location = new System.Drawing.Point(12, 42);
            this.labelDispScale.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDispScale.Name = "labelDispScale";
            this.labelDispScale.Size = new System.Drawing.Size(225, 25);
            this.labelDispScale.TabIndex = 8;
            this.labelDispScale.Text = "Displacement Scaling:";
            // 
            // labelResScale
            // 
            this.labelResScale.AutoSize = true;
            this.labelResScale.Location = new System.Drawing.Point(12, 92);
            this.labelResScale.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelResScale.Name = "labelResScale";
            this.labelResScale.Size = new System.Drawing.Size(162, 25);
            this.labelResScale.TabIndex = 9;
            this.labelResScale.Text = "Result Scaling: ";
            // 
            // pictureBoxColorBar
            // 
            this.pictureBoxColorBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxColorBar.Image = global::Cocodrilo.Properties.Resources.color_bar;
            this.pictureBoxColorBar.Location = new System.Drawing.Point(12, 88);
            this.pictureBoxColorBar.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pictureBoxColorBar.Name = "pictureBoxColorBar";
            this.pictureBoxColorBar.Size = new System.Drawing.Size(592, 31);
            this.pictureBoxColorBar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxColorBar.TabIndex = 16;
            this.pictureBoxColorBar.TabStop = false;
            // 
            // groupBoxShow
            // 
            this.groupBoxShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxShow.Controls.Add(this.checkBoxCouplingStresses);
            this.groupBoxShow.Controls.Add(this.checkBoxPrincipalStresses);
            this.groupBoxShow.Controls.Add(this.checkBoxPK2Stresses);
            this.groupBoxShow.Controls.Add(this.checkBoxShowCauchyStresses);
            this.groupBoxShow.Controls.Add(this.checkBoxShowResults);
            this.groupBoxShow.Controls.Add(this.checkBoxShowKnots);
            this.groupBoxShow.Controls.Add(this.checkBoxShowUndeformed);
            this.groupBoxShow.Controls.Add(this.checkBoxShowGaussPoints);
            this.groupBoxShow.Controls.Add(this.checkBoxShowCouplingPoints);
            this.groupBoxShow.Location = new System.Drawing.Point(12, 383);
            this.groupBoxShow.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxShow.Name = "groupBoxShow";
            this.groupBoxShow.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxShow.Size = new System.Drawing.Size(592, 217);
            this.groupBoxShow.TabIndex = 15;
            this.groupBoxShow.TabStop = false;
            this.groupBoxShow.Text = "Show";
            // 
            // checkBoxPrincipalStresses
            // 
            this.checkBoxPrincipalStresses.AutoSize = true;
            this.checkBoxPrincipalStresses.Location = new System.Drawing.Point(388, 125);
            this.checkBoxPrincipalStresses.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxPrincipalStresses.Name = "checkBoxPrincipalStresses";
            this.checkBoxPrincipalStresses.Size = new System.Drawing.Size(217, 29);
            this.checkBoxPrincipalStresses.TabIndex = 9;
            this.checkBoxPrincipalStresses.Text = "Principal Stresses";
            this.checkBoxPrincipalStresses.UseVisualStyleBackColor = true;
            this.checkBoxPrincipalStresses.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBoxPK2Stresses
            // 
            this.checkBoxPK2Stresses.AutoSize = true;
            this.checkBoxPK2Stresses.Location = new System.Drawing.Point(388, 81);
            this.checkBoxPK2Stresses.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxPK2Stresses.Name = "checkBoxPK2Stresses";
            this.checkBoxPK2Stresses.Size = new System.Drawing.Size(174, 29);
            this.checkBoxPK2Stresses.TabIndex = 8;
            this.checkBoxPK2Stresses.Text = "PK2 Stresses";
            this.checkBoxPK2Stresses.UseVisualStyleBackColor = true;
            this.checkBoxPK2Stresses.CheckedChanged += new System.EventHandler(this.checkBoxPK2Stresses_CheckedChanged);
            // 
            // checkBoxShowCauchyStresses
            // 
            this.checkBoxShowCauchyStresses.AutoSize = true;
            this.checkBoxShowCauchyStresses.Location = new System.Drawing.Point(388, 37);
            this.checkBoxShowCauchyStresses.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxShowCauchyStresses.Name = "checkBoxShowCauchyStresses";
            this.checkBoxShowCauchyStresses.Size = new System.Drawing.Size(207, 29);
            this.checkBoxShowCauchyStresses.TabIndex = 7;
            this.checkBoxShowCauchyStresses.Text = "Cauchy Stresses";
            this.checkBoxShowCauchyStresses.UseVisualStyleBackColor = true;
            this.checkBoxShowCauchyStresses.CheckedChanged += new System.EventHandler(this.checkBoxShowCauchyStresses_CheckedChanged);
            // 
            // checkBoxShowResults
            // 
            this.checkBoxShowResults.AutoSize = true;
            this.checkBoxShowResults.Checked = true;
            this.checkBoxShowResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowResults.Location = new System.Drawing.Point(12, 37);
            this.checkBoxShowResults.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxShowResults.Name = "checkBoxShowResults";
            this.checkBoxShowResults.Size = new System.Drawing.Size(116, 29);
            this.checkBoxShowResults.TabIndex = 0;
            this.checkBoxShowResults.Text = "Results";
            this.checkBoxShowResults.UseVisualStyleBackColor = true;
            this.checkBoxShowResults.CheckedChanged += new System.EventHandler(this.checkBoxShowResults_CheckedChanged);
            // 
            // checkBoxShowKnots
            // 
            this.checkBoxShowKnots.AutoSize = true;
            this.checkBoxShowKnots.Location = new System.Drawing.Point(12, 81);
            this.checkBoxShowKnots.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxShowKnots.Name = "checkBoxShowKnots";
            this.checkBoxShowKnots.Size = new System.Drawing.Size(99, 29);
            this.checkBoxShowKnots.TabIndex = 1;
            this.checkBoxShowKnots.Text = "Knots";
            this.checkBoxShowKnots.UseVisualStyleBackColor = true;
            this.checkBoxShowKnots.CheckedChanged += new System.EventHandler(this.checkBoxShowKnots_CheckedChanged);
            // 
            // checkBoxShowUndeformed
            // 
            this.checkBoxShowUndeformed.AutoSize = true;
            this.checkBoxShowUndeformed.Location = new System.Drawing.Point(12, 173);
            this.checkBoxShowUndeformed.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxShowUndeformed.Name = "checkBoxShowUndeformed";
            this.checkBoxShowUndeformed.Size = new System.Drawing.Size(161, 29);
            this.checkBoxShowUndeformed.TabIndex = 2;
            this.checkBoxShowUndeformed.Text = "Undeformed";
            this.checkBoxShowUndeformed.UseVisualStyleBackColor = true;
            this.checkBoxShowUndeformed.CheckedChanged += new System.EventHandler(this.checkBoxShowUndeformed_CheckedChanged);
            // 
            // checkBoxShowGaussPoints
            // 
            this.checkBoxShowGaussPoints.AutoSize = true;
            this.checkBoxShowGaussPoints.Location = new System.Drawing.Point(184, 37);
            this.checkBoxShowGaussPoints.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxShowGaussPoints.Name = "checkBoxShowGaussPoints";
            this.checkBoxShowGaussPoints.Size = new System.Drawing.Size(172, 29);
            this.checkBoxShowGaussPoints.TabIndex = 4;
            this.checkBoxShowGaussPoints.Text = "Gauss Points";
            this.checkBoxShowGaussPoints.UseVisualStyleBackColor = true;
            this.checkBoxShowGaussPoints.CheckedChanged += new System.EventHandler(this.checkBoxShowGaussPoints_CheckedChanged);
            // 
            // checkBoxShowCouplingPoints
            // 
            this.checkBoxShowCouplingPoints.AutoSize = true;
            this.checkBoxShowCouplingPoints.Location = new System.Drawing.Point(184, 81);
            this.checkBoxShowCouplingPoints.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxShowCouplingPoints.Name = "checkBoxShowCouplingPoints";
            this.checkBoxShowCouplingPoints.Size = new System.Drawing.Size(195, 29);
            this.checkBoxShowCouplingPoints.TabIndex = 5;
            this.checkBoxShowCouplingPoints.Text = "Coupling Points";
            this.checkBoxShowCouplingPoints.UseVisualStyleBackColor = true;
            this.checkBoxShowCouplingPoints.CheckedChanged += new System.EventHandler(this.checkBoxShowCouplingPoints_CheckedChanged);
            this.checkBoxShowCouplingPoints.TextChanged += new System.EventHandler(this.checkBoxShowCouplingPoints_TextChanged);
            // 
            // comboBoxResultType
            // 
            this.comboBoxResultType.FormattingEnabled = true;
            this.comboBoxResultType.Location = new System.Drawing.Point(10, 37);
            this.comboBoxResultType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxResultType.Name = "comboBoxResultType";
            this.comboBoxResultType.Size = new System.Drawing.Size(294, 33);
            this.comboBoxResultType.TabIndex = 7;
            this.comboBoxResultType.SelectedIndexChanged += new System.EventHandler(this.comboBoxResultType_SelectedIndexChanged);
            // 
            // groupBoxAnalysisStep
            // 
            this.groupBoxAnalysisStep.Controls.Add(this.domainUpDownAnalysisStep);
            this.groupBoxAnalysisStep.Controls.Add(this.trackBarAnalysisStep);
            this.groupBoxAnalysisStep.Controls.Add(this.comboBoxLoadCaseType);
            this.groupBoxAnalysisStep.Location = new System.Drawing.Point(6, 12);
            this.groupBoxAnalysisStep.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxAnalysisStep.Name = "groupBoxAnalysisStep";
            this.groupBoxAnalysisStep.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxAnalysisStep.Size = new System.Drawing.Size(616, 187);
            this.groupBoxAnalysisStep.TabIndex = 9;
            this.groupBoxAnalysisStep.TabStop = false;
            this.groupBoxAnalysisStep.Text = "Analysis/Step";
            // 
            // domainUpDownAnalysisStep
            // 
            this.domainUpDownAnalysisStep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.domainUpDownAnalysisStep.Location = new System.Drawing.Point(352, 38);
            this.domainUpDownAnalysisStep.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.domainUpDownAnalysisStep.Name = "domainUpDownAnalysisStep";
            this.domainUpDownAnalysisStep.Size = new System.Drawing.Size(252, 31);
            this.domainUpDownAnalysisStep.TabIndex = 2;
            this.domainUpDownAnalysisStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.domainUpDownAnalysisStep.SelectedItemChanged += new System.EventHandler(this.domainUpDownAnalysisStep_SelectedItemChanged);
            // 
            // trackBarAnalysisStep
            // 
            this.trackBarAnalysisStep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarAnalysisStep.Location = new System.Drawing.Point(12, 88);
            this.trackBarAnalysisStep.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.trackBarAnalysisStep.Name = "trackBarAnalysisStep";
            this.trackBarAnalysisStep.Size = new System.Drawing.Size(592, 90);
            this.trackBarAnalysisStep.TabIndex = 1;
            this.trackBarAnalysisStep.Scroll += new System.EventHandler(this.trackBarAnalysisStep_Scroll);
            // 
            // comboBoxLoadCaseType
            // 
            this.comboBoxLoadCaseType.FormattingEnabled = true;
            this.comboBoxLoadCaseType.Location = new System.Drawing.Point(12, 37);
            this.comboBoxLoadCaseType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxLoadCaseType.Name = "comboBoxLoadCaseType";
            this.comboBoxLoadCaseType.Size = new System.Drawing.Size(264, 33);
            this.comboBoxLoadCaseType.TabIndex = 0;
            this.comboBoxLoadCaseType.SelectedIndexChanged += new System.EventHandler(this.comboBoxLoadCaseType_SelectedIndexChanged);
            // 
            // CocodriloPlugInBindingSource
            // 
            this.CocodriloPlugInBindingSource.DataSource = typeof(Cocodrilo.CocodriloPlugIn);
            // 
            // propertySupportBindingSource
            // 
            this.propertySupportBindingSource.DataSource = typeof(Cocodrilo.ElementProperties.PropertySupport);
            // 
            // checkBoxCouplingStresses
            // 
            this.checkBoxCouplingStresses.AutoSize = true;
            this.checkBoxCouplingStresses.Location = new System.Drawing.Point(388, 166);
            this.checkBoxCouplingStresses.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxCouplingStresses.Name = "checkBoxCouplingStresses";
            this.checkBoxCouplingStresses.Size = new System.Drawing.Size(219, 29);
            this.checkBoxCouplingStresses.TabIndex = 10;
            this.checkBoxCouplingStresses.Text = "Coupling Stresses";
            this.checkBoxCouplingStresses.UseVisualStyleBackColor = true;
            this.checkBoxCouplingStresses.CheckedChanged += new System.EventHandler(this.checkBoxCouplingStresses_CheckedChanged);
            // 
            // UserControlCocodriloPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.TeDA_Plugin);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UserControlCocodriloPanel";
            this.Size = new System.Drawing.Size(864, 1558);
            this.TeDA_Plugin.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBoxAnalyses.ResumeLayout(false);
            this.groupBoxAnalyses.PerformLayout();
            this.tabControlAnalyses.ResumeLayout(false);
            this.tabPageFormfinding.ResumeLayout(false);
            this.tabPageFormfinding.PerformLayout();
            this.tabPageLinStructuralAnalysis.ResumeLayout(false);
            this.tabPageLinStructuralAnalysis.PerformLayout();
            this.tabPageNonLinStrucAnalysis.ResumeLayout(false);
            this.tabPageNonLinStrucAnalysis.PerformLayout();
            this.tabPageTransientAnalysis.ResumeLayout(false);
            this.tabPageTransientAnalysis.PerformLayout();
            this.tabPageEigenvalueAnalysis.ResumeLayout(false);
            this.tabPageEigenvalueAnalysis.PerformLayout();
            this.tabPageCutPatternAnalysis.ResumeLayout(false);
            this.tabPageCutPatternAnalysis.PerformLayout();
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            this.groupBoxMaterials.ResumeLayout(false);
            this.groupBoxProperties.ResumeLayout(false);
            this.tabControlProperties.ResumeLayout(false);
            this.tabPageElement.ResumeLayout(false);
            this.tabPageElement.PerformLayout();
            this.tabControlElement.ResumeLayout(false);
            this.tabPageElementMembrane.ResumeLayout(false);
            this.tabPageElementMembrane.PerformLayout();
            this.tabPageElementShell.ResumeLayout(false);
            this.tabPageElementShell.PerformLayout();
            this.tabPageElementBeam.ResumeLayout(false);
            this.tabPageElementBeam.PerformLayout();
            this.tabPageElementCable.ResumeLayout(false);
            this.tabPageElementCable.PerformLayout();
            this.tabPageRefinement.ResumeLayout(false);
            this.tabPageRefinement.PerformLayout();
            this.groupBoxRefinementElement.ResumeLayout(false);
            this.groupBoxRefinementElement.PerformLayout();
            this.tabPagePropSupport.ResumeLayout(false);
            this.tabPagePropSupport.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBoxInterval.ResumeLayout(false);
            this.groupBoxInterval.PerformLayout();
            this.groupBoxSupportType.ResumeLayout(false);
            this.groupBoxSupportType.PerformLayout();
            this.groupBoxSupportDimension.ResumeLayout(false);
            this.groupBoxSupportDimension.PerformLayout();
            this.groupBoxSupportElementType.ResumeLayout(false);
            this.groupBoxSupportElementType.PerformLayout();
            this.tabPageLoad.ResumeLayout(false);
            this.tabPageLoad.PerformLayout();
            this.groupBoxLoadInterval.ResumeLayout(false);
            this.groupBoxLoadInterval.PerformLayout();
            this.groupBoxLoadDimension.ResumeLayout(false);
            this.groupBoxLoadDimension.PerformLayout();
            this.groupBoxLoadElement.ResumeLayout(false);
            this.groupBoxLoadElement.PerformLayout();
            this.tabPageCheck.ResumeLayout(false);
            this.tabPageCheck.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBoxCheckStructuralElement.ResumeLayout(false);
            this.groupBoxCheckStructuralElement.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBoxVisualization.ResumeLayout(false);
            this.groupBoxVisualization.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBoxScalings.ResumeLayout(false);
            this.groupBoxScalings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColorBar)).EndInit();
            this.groupBoxShow.ResumeLayout(false);
            this.groupBoxShow.PerformLayout();
            this.groupBoxAnalysisStep.ResumeLayout(false);
            this.groupBoxAnalysisStep.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAnalysisStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CocodriloPlugInBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertySupportBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TeDA_Plugin;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBoxAnalyses;
        private System.Windows.Forms.RadioButton radioButtonRunKratos;
        private System.Windows.Forms.RadioButton radioButtonRunCarat;
        private System.Windows.Forms.Button buttonEditOutput;
        private System.Windows.Forms.TabControl tabControlAnalyses;
        private System.Windows.Forms.TabPage tabPageFormfinding;
        private System.Windows.Forms.TextBox textBoxFormfindingName;
        private System.Windows.Forms.TextBox textBoxTolerance;
        private System.Windows.Forms.TextBox textBoxMaxIterations;
        private System.Windows.Forms.TextBox textBoxMaxSteps;
        private System.Windows.Forms.Label labelMaxIterattions;
        private System.Windows.Forms.Label labelAnalysisFofiName;
        private System.Windows.Forms.Label labelAccuracy;
        private System.Windows.Forms.Label labelMaxSteps;
        private System.Windows.Forms.TabPage tabPageLinStructuralAnalysis;
        private System.Windows.Forms.TextBox textBoxLinStrucAnalysisName;
        private System.Windows.Forms.Label labelAnalysisStaGeoName;
        private System.Windows.Forms.TabPage tabPageNonLinStrucAnalysis;
        private System.Windows.Forms.TextBox textBoxNonLinStruAnalysisStepSize;
        private System.Windows.Forms.Label labelNonLinAnalysisStepSize;
        private System.Windows.Forms.TextBox textBoxNonLinStrucAnalysisAdapStepCntrl;
        private System.Windows.Forms.Label labelNonLinStrucAnalysisAdapStepCntrl;
        private System.Windows.Forms.Label labelNonLinStrucAnalysisNumIter;
        private System.Windows.Forms.TextBox textBoxNonLinStrucAnalysisNumIter;
        private System.Windows.Forms.Label labelNonLinStrucAnalysisNumSteps;
        private System.Windows.Forms.TextBox textBoxNonLinStrucAnalysisNumSteps;
        private System.Windows.Forms.TextBox textBoxNonLinStrucAnalysisAcc;
        private System.Windows.Forms.Label labelNonLinStrucAnalysisAcc;
        private System.Windows.Forms.TextBox textBoxNonLinStrucAnalysisName;
        private System.Windows.Forms.Label labelNonLinStrucAnalysisName;
        private System.Windows.Forms.TabPage tabPageTransientAnalysis;
        private System.Windows.Forms.TextBox textBoxTransientAnalysisStepSize;
        private System.Windows.Forms.Label labelTransientAnalysisStepSize;
        private System.Windows.Forms.TextBox textBoxTransientAnalysisAdapStepCntrl;
        private System.Windows.Forms.Label labelTransientAnalysisAdapStepCntrl;
        private System.Windows.Forms.Label labelTransientAnalysisNumIter;
        private System.Windows.Forms.TextBox textBoxTransientAnalysisNumIter;
        private System.Windows.Forms.Label labelTransientAnalysisNumSteps;
        private System.Windows.Forms.TextBox textBoxTransientAnalysisNumSteps;
        private System.Windows.Forms.TextBox textBoxTransientAnalysisAcc;
        private System.Windows.Forms.Label labelTransientAnalysisAcc;
        private System.Windows.Forms.TextBox textBoxTransientAnalysisRayleighAlpha;
        private System.Windows.Forms.Label labelTransientAnalysisRayleighAlpha;
        private System.Windows.Forms.TextBox textBoxTransientAnalysisRayleighBeta;
        private System.Windows.Forms.Label labelTransientAnalysisRayleighBeta;
        private System.Windows.Forms.TextBox textBoxTransientAnalysisName;
        private System.Windows.Forms.Label labelTransientAnalysisName;
        private System.Windows.Forms.ComboBox comboBoxTransientAnalysisTimeIntegration;
        private System.Windows.Forms.Label labelTransientAnalysisTimeIntegration;
        private System.Windows.Forms.ComboBox comboBoxTransientAnalysisScheme;
        private System.Windows.Forms.Label labelTransientAnalysisScheme;
        private System.Windows.Forms.CheckBox checkBoxTransientAnalysisAutomaticRayleigh;
        private System.Windows.Forms.TextBox textBoxTransientAnalysisDampingRatio0;
        private System.Windows.Forms.Label labelTransientAnalysisDampingRatio0;
        private System.Windows.Forms.TextBox textBoxTransientAnalysisDampingRatio1;
        private System.Windows.Forms.Label labelTransientAnalysisDampingRatio1;
        private System.Windows.Forms.TextBox textBoxTransientAnalysisNumEigen;
        private System.Windows.Forms.Label labelTransientAnalysisNumEigen;
        private System.Windows.Forms.TabPage tabPageEigenvalueAnalysis;
        private System.Windows.Forms.Label labelEigenvalueAnalysisNumIter;
        private System.Windows.Forms.TextBox textBoxEigenvalueAnalysisNumIter;
        private System.Windows.Forms.Label labelEigenvalueAnalysisNumEigen;
        private System.Windows.Forms.TextBox textBoxEigenvalueAnalysisNumEigen;
        private System.Windows.Forms.TextBox textBoxEigenvalueAnalysisAcc;
        private System.Windows.Forms.Label labelEigenvalueAnalysisAcc;
        private System.Windows.Forms.TextBox textBoxEigenvalueAnalysisName;
        private System.Windows.Forms.Label labelEigenvalueAnalysisName;
        private System.Windows.Forms.ComboBox comboBoxEigenvalueAnalysisSolverType;
        private System.Windows.Forms.Label labelEigenvalueAnalysisSolverType;
        private System.Windows.Forms.TabPage tabPageCutPatternAnalysis;
        private System.Windows.Forms.CheckBox checkBoxCutPatternAnalysisPrestress;
        private System.Windows.Forms.TextBox textBoxCutPatternAnalysisName;
        private System.Windows.Forms.TextBox textBoxCutPatternAnalysisTol;
        private System.Windows.Forms.TextBox textBoxCutPatternAnalysisMaxIter;
        private System.Windows.Forms.TextBox textBoxCutPatternAnalysisMaxStep;
        private System.Windows.Forms.Label labelCutPatternAnalysisMaxIter;
        private System.Windows.Forms.Label labelCutPatternAnalysisName;
        private System.Windows.Forms.Label labelCutPatternAnalysisTol;
        private System.Windows.Forms.Label labelCutPatternAnalysisMaxStep;
        private System.Windows.Forms.ComboBox comboBoxAnalyses;
        private System.Windows.Forms.Button buttonRunAnalysis;
        private System.Windows.Forms.Button buttonDeleteAnalysis;
        private System.Windows.Forms.Button buttonCreateAnalysis;
        private System.Windows.Forms.Button buttonModifyAnalysis;
        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.CheckBox checkBoxShowPreLoads;
        private System.Windows.Forms.CheckBox checkBoxShowPreSupports;
        private System.Windows.Forms.CheckBox checkBoxShowPreCouplings;
        private System.Windows.Forms.Button buttonResetInstance;
        private System.Windows.Forms.Button buttonDeleteAll;
        private System.Windows.Forms.CheckBox checkBoxShowPreprocessing;
        private System.Windows.Forms.GroupBox groupBoxMaterials;
        private System.Windows.Forms.Button buttonAddModifyMaterial;
        private System.Windows.Forms.GroupBox groupBoxProperties;
        private System.Windows.Forms.TabControl tabControlProperties;
        private System.Windows.Forms.TabPage tabPageElement;
        private System.Windows.Forms.Button buttonLoadElement;
        private System.Windows.Forms.TabControl tabControlElement;
        private System.Windows.Forms.TabPage tabPageElementMembrane;
        private System.Windows.Forms.CheckBox checkBoxElementMembraneFofi;
        private System.Windows.Forms.CheckBox checkBoxElementMembraneEdgeCoupling;
        private System.Windows.Forms.TextBox textBoxMembranePrestress2;
        private System.Windows.Forms.Label labelMembranePrestress2;
        private System.Windows.Forms.TextBox textBoxMembranePrestress1;
        private System.Windows.Forms.Label labelMembraneThick;
        private System.Windows.Forms.Label labelMembranePrestress1;
        private System.Windows.Forms.TextBox textBoxMembraneThick;
        private System.Windows.Forms.TabPage tabPageElementShell;
        private System.Windows.Forms.ComboBox comboBoxShellType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelShellThick;
        private System.Windows.Forms.TextBox textBoxShellThick;
        private System.Windows.Forms.TabPage tabPageElementBeam;
        private System.Windows.Forms.Button buttonAddAxis;
        private System.Windows.Forms.Label labelBeamIy;
        private System.Windows.Forms.Label labelBeamWidth;
        private System.Windows.Forms.Label labelBeamIt;
        private System.Windows.Forms.TextBox textBoxBeamIy;
        private System.Windows.Forms.TextBox textBoxBeamArea;
        private System.Windows.Forms.Label labelBeamIz;
        private System.Windows.Forms.TextBox textBoxBeamIz;
        private System.Windows.Forms.Label labelBeamDiameter;
        private System.Windows.Forms.Label labelBeamHeight;
        private System.Windows.Forms.TextBox textBoxBeamIt;
        private System.Windows.Forms.Label labelBeamArea;
        private System.Windows.Forms.Label labelBeamType;
        private System.Windows.Forms.ComboBox comboBoxBeamType;
        private System.Windows.Forms.TabPage tabPageElementCable;
        private System.Windows.Forms.CheckBox checkBoxElementCableFofi;
        private System.Windows.Forms.CheckBox checkBoxCablePrestressCurve;
        private System.Windows.Forms.Label labelCablePrestress;
        private System.Windows.Forms.TextBox textBoxCablePrestress;
        private System.Windows.Forms.TextBox textBoxCableArea;
        private System.Windows.Forms.ComboBox comboBoxCableType;
        private System.Windows.Forms.Label labelCableArea;
        private System.Windows.Forms.Label labelCableType;
        private System.Windows.Forms.ImageList imageListTabControlElement;
        private System.Windows.Forms.Label labelElementMat;
        private System.Windows.Forms.ComboBox comboBoxElementMat;
        private System.Windows.Forms.Label labelCouplingType;
        private System.Windows.Forms.ComboBox comboBoxCouplingType;
        private System.Windows.Forms.Button buttonDeleteElement;
        private System.Windows.Forms.Button buttonAddElement;
        private System.Windows.Forms.TabPage tabPageRefinement;
        private System.Windows.Forms.RadioButton radioButtonRefinementApproxElementSize;
        private System.Windows.Forms.RadioButton radioButtonRefinementKnotSubdivision;
        private System.Windows.Forms.GroupBox groupBoxRefinementElement;
        private System.Windows.Forms.RadioButton radioButtonRefinementElementEdge;
        private System.Windows.Forms.RadioButton radioButtonRefinementElementCurve;
        private System.Windows.Forms.RadioButton radioButtonRefinementElementSurf;
        private System.Windows.Forms.Button buttonCheckRefinement;
        private System.Windows.Forms.Button buttonChangeRefinement;
        private System.Windows.Forms.TextBox textBoxKnotSubDivV;
        private System.Windows.Forms.TextBox textBoxKnotSubDivU;
        private System.Windows.Forms.TextBox textBoxQDeg;
        private System.Windows.Forms.TextBox textBoxPDeg;
        private System.Windows.Forms.Label labelRefinementMinV;
        private System.Windows.Forms.Label labelRefinementMinU;
        private System.Windows.Forms.Label labelRefinementQDeg;
        private System.Windows.Forms.Label labelRefinementPDeg;
        private System.Windows.Forms.TabPage tabPagePropSupport;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox comboBoxSupportType;
        private System.Windows.Forms.GroupBox groupBoxInterval;
        private System.Windows.Forms.TextBox textBoxSupportEndTime;
        private System.Windows.Forms.TextBox textBoxSupportStartTime;
        private System.Windows.Forms.CheckBox checkBoxOverwriteSupport;
        private System.Windows.Forms.GroupBox groupBoxSupportType;
        private System.Windows.Forms.TextBox textBoxSupportTypeDispZ;
        private System.Windows.Forms.TextBox textBoxSupportTypeDispY;
        private System.Windows.Forms.TextBox textBoxSupportTypeDispX;
        private System.Windows.Forms.CheckBox checkBoxSupportStrong;
        private System.Windows.Forms.CheckBox checkBoxDispZ;
        private System.Windows.Forms.CheckBox checkBoxDispY;
        private System.Windows.Forms.CheckBox checkBoxRotationSupport;
        private System.Windows.Forms.CheckBox checkBoxDispX;
        private System.Windows.Forms.GroupBox groupBoxSupportDimension;
        private System.Windows.Forms.RadioButton radioButtonSupportDimVertex;
        private System.Windows.Forms.RadioButton radioButtonSupportDimLine;
        private System.Windows.Forms.RadioButton radioButtonSupportDimFace;
        private System.Windows.Forms.GroupBox groupBoxSupportElementType;
        private System.Windows.Forms.RadioButton radioButtonSupportCurve;
        private System.Windows.Forms.RadioButton radioButtonSupportSurface;
        private System.Windows.Forms.Button buttonDeleteEdgeSupport;
        private System.Windows.Forms.Button buttonAddEdgeSupports;
        private System.Windows.Forms.TabPage tabPageLoad;
        private System.Windows.Forms.GroupBox groupBoxLoadInterval;
        private System.Windows.Forms.TextBox textBoxLoadEndTime;
        private System.Windows.Forms.TextBox textBoxLoadStartTime;
        private System.Windows.Forms.CheckBox checkBoxLoadOverwrite;
        private System.Windows.Forms.TextBox textBoxLoadPositionV;
        private System.Windows.Forms.TextBox textBoxLoadPositionU;
        private System.Windows.Forms.TextBox textBoxLoadZ;
        private System.Windows.Forms.TextBox textBoxLoadY;
        private System.Windows.Forms.TextBox textBoxLoadX;
        private System.Windows.Forms.Label labelLoadDirectionZ;
        private System.Windows.Forms.Label labelLoadDirectionY;
        private System.Windows.Forms.Label labelLoadDirectionX;
        private System.Windows.Forms.Label labelLoadPositionV;
        private System.Windows.Forms.Label labelLoadPositionU;
        private System.Windows.Forms.Label labelLoadPosition;
        private System.Windows.Forms.Label labelLoadDirection;
        private System.Windows.Forms.GroupBox groupBoxLoadDimension;
        private System.Windows.Forms.RadioButton radioButtonLoadDimVertex;
        private System.Windows.Forms.RadioButton radioButtonLoadDimLine;
        private System.Windows.Forms.RadioButton radioButtonLoadDimFace;
        private System.Windows.Forms.GroupBox groupBoxLoadElement;
        private System.Windows.Forms.RadioButton radioButtonLoadElementCurve;
        private System.Windows.Forms.RadioButton radioButtonLoadElementSurface;
        private System.Windows.Forms.ComboBox comboBoxLoadType;
        private System.Windows.Forms.Button buttonDeleteLoad;
        private System.Windows.Forms.Button buttonAddLoad;
        private System.Windows.Forms.TabPage tabPageCheck;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxCheckEndTime;
        private System.Windows.Forms.TextBox textBoxCheckStartTime;
        private System.Windows.Forms.CheckBox checkBoxOverwriteChecks;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxOutputLagrangeMP;
        private System.Windows.Forms.CheckBox checkBoxOutputDispZ;
        private System.Windows.Forms.CheckBox checkBoxOutputDispX;
        private System.Windows.Forms.CheckBox checkBoxOutputDispY;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonCheckVertex;
        private System.Windows.Forms.RadioButton radioButtonCheckLine;
        private System.Windows.Forms.RadioButton radioButtonCheckFace;
        private System.Windows.Forms.GroupBox groupBoxCheckStructuralElement;
        private System.Windows.Forms.RadioButton radioButtonCheckCurve;
        private System.Windows.Forms.RadioButton radioButtonCheckSurface;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonAddCheck;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.BindingSource CocodriloPlugInBindingSource;
        private System.Windows.Forms.BindingSource propertySupportBindingSource;
        private System.Windows.Forms.ToolTip toolTipPanel;
        private System.Windows.Forms.Button open_file;
        private System.Windows.Forms.Button buttonShowPost;
        private System.Windows.Forms.Button buttonClearPost;
        private System.Windows.Forms.GroupBox groupBoxVisualization;
        private System.Windows.Forms.TextBox textBoxColorBarMin;
        private System.Windows.Forms.TextBox textBoxColorBarMax;
        private System.Windows.Forms.GroupBox groupBoxScalings;
        private System.Windows.Forms.TextBox textBoxFlyingNodeLimit;
        private System.Windows.Forms.TextBox textBoxResScale;
        private System.Windows.Forms.TextBox textBoxDispScale;
        private System.Windows.Forms.Label labelFlyingNodeLimit;
        private System.Windows.Forms.Label labelDispScale;
        private System.Windows.Forms.Label labelResScale;
        private System.Windows.Forms.PictureBox pictureBoxColorBar;
        private System.Windows.Forms.GroupBox groupBoxShow;
        private System.Windows.Forms.CheckBox checkBoxShowResults;
        private System.Windows.Forms.CheckBox checkBoxShowKnots;
        private System.Windows.Forms.CheckBox checkBoxShowUndeformed;
        private System.Windows.Forms.CheckBox checkBoxShowGaussPoints;
        private System.Windows.Forms.CheckBox checkBoxShowCouplingPoints;
        private System.Windows.Forms.ComboBox comboBoxResultType;
        private System.Windows.Forms.GroupBox groupBoxAnalysisStep;
        private System.Windows.Forms.DomainUpDown domainUpDownAnalysisStep;
        private System.Windows.Forms.TrackBar trackBarAnalysisStep;
        private System.Windows.Forms.ComboBox comboBoxLoadCaseType;
        private System.Windows.Forms.ComboBox comboBoxPostProcessingDirection;
        private System.Windows.Forms.Label labelPostProcessingDirection;
        private System.Windows.Forms.CheckBox checkBoxPrincipalStresses;
        private System.Windows.Forms.CheckBox checkBoxPK2Stresses;
        private System.Windows.Forms.CheckBox checkBoxShowCauchyStresses;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBoxShowMesh;
        private System.Windows.Forms.CheckBox checkBoxCouplingStresses;
    }
}
