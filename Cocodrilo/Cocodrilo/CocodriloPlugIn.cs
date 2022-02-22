using System;
using System.Collections.Generic;
using Rhino;
using System.Drawing;
using System.Linq;
using System.Web.Script.Serialization;
using Cocodrilo.Analyses;
using Cocodrilo.ElementProperties;
using Cocodrilo.Materials;
using Cocodrilo.UserData;

namespace Cocodrilo
{
    public delegate void MaterialChanged();
    public delegate void AnalysesChanged();
    public delegate void StageAdded();

    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class CocodriloPlugIn : Rhino.PlugIns.PlugIn
    {
        private int mCurrentStage;
        public StageAdded NewStageAdded;

        /// <summary>
        /// List of points, which connect independent breps.
        /// Stored for a more efficient output and for visualization.
        /// </summary>
        public List<Rhino.Geometry.Point> IntersectionPointList = new List<Rhino.Geometry.Point>();
        /// <summary>
        /// List of curves, which connect independent breps, or inner brep connections which are
        /// not detected by Rhino, as eg. T-Junctions, 3 intersections, double intersections...
        /// 
        /// Stored for a more efficient output and for visualization.
        /// </summary>
        public List<Rhino.Geometry.Curve> IntersectionCurveList = new List<Rhino.Geometry.Curve>();

        public List<Property> Properties = new List<Property>();

        /// <summary>
        /// Global material settings.
        /// </summary>
        public List<Material> Materials = new List<Material>();
        public MaterialChanged materialUpdate; // for combo boxes to get update.

        public List<Analyses.Analysis> Analyses = new List<Analyses.Analysis>();
        public AnalysesChanged analysisUpdate; // for combo boxes to get update.

        /// <summary>
        /// Global output settings.
        /// </summary>
        public IO.OutputOptions OutputOptions;

        /// <summary>
        /// Global output settings.
        /// </summary>
        public PostProcessing.PostProcessing PostProcessingCocodrilo;

        /// <summary>
        /// Controllong the visualization of enhanced user data information.
        /// </summary>
        public Visualizer.Visualizer visualizer = new Visualizer.Visualizer();

        /// <summary>
        /// Global penalty factor
        /// </summary>
        public double GlobPenaltyFactor = 1e7;
        /// <summary>
        /// Global coupling method
        /// </summary>
        public CouplingType GlobalCouplingMethod = CouplingType.CouplingPenaltyCondition;

        public CocodriloPlugIn()
        {
            Instance = this;

            /// Kommas to points.
            System.Globalization.CultureInfo us_culture = new System.Globalization.CultureInfo("en-us");
            System.Threading.Thread.CurrentThread.CurrentCulture = us_culture;

            /// Add default materials and a default formfinding.
            AddDefaults();

            /// Default output settings.
            OutputOptions = new IO.OutputOptions();

            /// For modifications of the elements.
            Rhino.RhinoDoc.ReplaceRhinoObject += EventWatcher.onReplaceObject;

            /// Panel and icon definition.
            Icon icon1 = new Icon(SystemIcons.Asterisk, 10, 10);
            Icon icon_carat4 = new Icon(Cocodrilo.Properties.Resources.carat10, 10,10);

            mCurrentStage = 0;

            /// Register available panels.
            Rhino.UI.Panels.RegisterPanel(this, typeof(Panels.UserControlCocodriloPanel), "Cocodrilo", icon_carat4);
        }

        ///<summary>Gets the unique instance of the CocodriloPlugIn plug-in.</summary>
        public static CocodriloPlugIn Instance
        {
            get; private set;
        }

        #region Defaults
        public void AddDefaults()
        {
            //Default material and analysis to have fast start.
            this.AddMaterial(new MaterialLinearElasticIsotropic("Steel", 200000, 0.0));
            this.AddMaterial(new MaterialOrthotropicDamage("MasonryEindhoven"));
            this.AddMaterial(new MaterialOrthotropicDamage("MasonryBrisbane"));

            this.AddAnalysis(new AnalysisFormfinding("Default_Formfinding"));
        }
        #endregion
        #region Properties
        public int AddProperty(ElementProperties.Property property)
        {
            foreach (var prop in Properties)
            {
                if (prop.GetType() == property.GetType())
                {
                    if (property.Equals(prop))
                    {
                        property.mPropertyId = prop.mPropertyId;
                        return prop.mPropertyId;
                    }
                }
            }
            if (Properties.Count > 0)
                property.mPropertyId = Properties.Last().mPropertyId + 1;
            else
                property.mPropertyId = 1;
            Properties.Add(property);
            return property.mPropertyId;
        }

        /// <summary>
        /// Returns the property for a given Id.
        /// </summary>
        /// <param name="PropertyId"></param>
        /// <param name="Success">True if property was found, False if not.</param>
        /// <returns></returns>
        public Property GetProperty(int PropertyId, out bool Success)
        {
            var property = Properties.Find(i => i.mPropertyId == PropertyId);
            Success = (property != null);
            return property;
        }
        public bool HasProperty(int PropertyId) => Properties.Any(i => i.mPropertyId == PropertyId);

        /// <summary>
        /// Deletes all properties.
        /// </summary>
        public void DeleteAllProperties()
        {
            Properties.Clear();
            Properties.TrimExcess();
        }

        public void ClearUnusedProperties(List<int> UsedPropertyIds)
        {
            for(int i = 0; i < Properties.Count; i++)
            {
                if (!UsedPropertyIds.Contains(Properties[i].mPropertyId))
                {
                    Properties.RemoveAt(i);
                    i--;
                }
            }
        }
        #endregion
        #region Materials
        public void AddMaterial(int ID, String Name, String Type, double YoungsModulus = 200000, double Nue = 0.3, double _Density = 1.0, double _Alpha_T = 0.0)
        {
            AddMaterial(new MaterialLinearElasticIsotropic(Name, YoungsModulus, Nue, _Density));
        }

        public void AddMaterial(Material material)
        {
            material.SetId((Materials.Count > 0)
                ? material.Id = Materials.Last().GetLastId() + 1
                : 1);

            Materials.Add(material);

            materialUpdate?.Invoke();
        }

        public Material GetMaterial(int Id)
            => Materials.Find(material => material.Id == Id);

        public void ModifyMaterialWithMaterialID(int Id, String _Name_ID, String _Type, double _YoungsModulus, double _Nue, double _Density, double _Alpha_T)
        {
            if (Materials.Exists(material => material.Id == Id))
            {
                var index = Materials.FindIndex(material => material.Id == Id);

                materialUpdate?.Invoke();
            }
            else
            {
                RhinoApp.Write("WARNING: Material ID does not exist yet!");
            }
        }
        public bool CheckExistingMaterialID(int CheckId) 
            => Materials.Exists(material => material.Id == CheckId);
        public bool DeleteMaterial(int Id)
            => Materials.Remove(Materials.Find(material => material.Id == Id));
        public void DeleteAllMaterials()
        {
            Materials.Clear();
            Materials.TrimExcess();

            materialUpdate?.Invoke();
        }

        #endregion
        #region Stages
        public int GetCurrentStage()
        {
            return mCurrentStage;
        }

        //public void AddStage(int StageId)
        //{
        //    if (!mStages.Contains(StageId))
        //        mStages.Add(StageId);
        //}
        #endregion
        #region Analysis
        public void AddAnalysis(Analyses.Analysis analysis)
        {
            Analyses.Add(analysis);

            analysisUpdate?.Invoke();
        }

        public bool DeleteAnalysis(Analyses.Analysis analysis)
        {
            var check = Analyses.Remove(analysis);

            analysisUpdate?.Invoke();

            return check;
        }
        public Analysis findAnalysis(string name) => Analyses.FirstOrDefault(analysis => analysis.Name == name);

        public void DeleteAllAnalyses()
        {
            Analyses.Clear();
            Analyses.TrimExcess();

            analysisUpdate?.Invoke();
        }
#endregion

        public void DeleteAll()
        {
            var active_breps = IO.GeometryUtilities.GetActiveBrepList();
            var active_curves = IO.GeometryUtilities.GetActiveCurveList();
            foreach (var element in active_breps)
            {
                foreach (var surface in element.Surfaces)
                {
                    var ud = surface.UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                    if (ud == null)
                        break;
                    if (!surface.UserData.Remove(ud))
                        break;
                }
                foreach (var trim in element.Curves2D)
                {
                    if (trim.UserData.Find(typeof(UserDataEdge)) is UserDataEdge)
                    {
                        var ud = trim.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;
                        if (!trim.UserData.Remove(ud))
                            break;
                    }
                }
            }
            foreach (var element in active_curves)
            {
                var ud = element.UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                if (ud != null)
                    element.UserData.Remove(ud);
                else
                { 
                    var udcrv = element.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                    if (udcrv == null)
                        break;
                    element.UserData.Remove(udcrv);
                }
            }

            IntersectionCurveList.Clear();
            IntersectionPointList.Clear();

            DeleteAllAnalyses();
            DeleteAllProperties();
            DeleteAllMaterials();

            /// Reset defaults.
            AddDefaults();
            OutputOptions = new IO.OutputOptions();
        }

#region Read/Write Options
        protected override bool ShouldCallWriteDocument(Rhino.FileIO.FileWriteOptions options)
        {
            // Only write document data if there is something to write
            bool rc = (Materials.Count > 0  || Analyses.Count > 0 || Properties.Count > 0)
                ? true
                : false;

            // Only write document data if the user is exporting selected geometry
            if (rc) {
                rc = (options.WriteSelectedObjectsOnly)
                    ? false
                    : true;
            }

            return rc;
        }
        protected override void ReadDocument(RhinoDoc doc, Rhino.FileIO.BinaryArchiveReader archive, Rhino.FileIO.FileReadOptions options)
        {
            Rhino.Collections.ArchivableDictionary dict = archive.ReadDictionary();
            if (dict.ContainsKey("Materials"))
            {
                var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
                string StringJsonMaterials = (String)dict["Materials"];
                Materials = serializer.Deserialize<List<Material>>(StringJsonMaterials);

                materialUpdate?.Invoke();
            }
            if (dict.ContainsKey("Analyses"))
            {
                var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
                string StringJsonAnalyses = (String)dict["Analyses"];
                Analyses = serializer.Deserialize<List<Analyses.Analysis>>(StringJsonAnalyses);

                analysisUpdate?.Invoke();
            }
            if (dict.ContainsKey("Properties"))
            {
                var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
                string StringJsonProperties = (String)dict["Properties"];
                Properties = serializer.Deserialize<List<Property>>(StringJsonProperties);
            }
            if (dict.ContainsKey("Output"))
            {
                var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
                string StringJsonOutput = (String)dict["Output"];
                OutputOptions = serializer.Deserialize<IO.OutputOptions>(StringJsonOutput);
            }
            if (dict.ContainsKey("Glob_Penalty_Fac"))
            {
                var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
                string StringJsonGlobPenFac = (String)dict["Glob_Penalty_Fac"];
                GlobPenaltyFactor = serializer.Deserialize<double>(StringJsonGlobPenFac);
            }
            if (dict.ContainsKey("Coupling_Tolerance"))
            {
                var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
                string StringCoupTol = (String)dict["Coupling_Tolerance"];
            }
        }
        protected override void WriteDocument(RhinoDoc doc, Rhino.FileIO.BinaryArchiveWriter archive, Rhino.FileIO.FileWriteOptions options)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            string StringJsonMaterials = serializer.Serialize(Materials);
            string StringJsonAnalyses = serializer.Serialize(Analyses);
            string StringJsonProperties = serializer.Serialize(Properties);
            string StringJsonOutput = serializer.Serialize(OutputOptions);
            string StringGlobPenFac = serializer.Serialize(GlobPenaltyFactor);

            var dict = new Rhino.Collections.ArchivableDictionary(1, "Physical");
            dict.Set("Materials", StringJsonMaterials);
            dict.Set("Analyses", StringJsonAnalyses);
            dict.Set("Properties", StringJsonProperties);
            dict.Set("Output", StringJsonOutput);
            dict.Set("Glob_Penalty_Fac", StringGlobPenFac); 
            archive.WriteDictionary(dict);
        } 
#endregion
    }
}