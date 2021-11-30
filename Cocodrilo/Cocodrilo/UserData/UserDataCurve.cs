using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Cocodrilo.Elements;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using Rhino.Geometry;
using Cocodrilo.ElementProperties;
using Cocodrilo.Refinement;

namespace Cocodrilo.UserData
{
    [Guid("EDCDDDB5-051C-4DB8-8B39-AFC21EC6CD08")]
    public class UserDataCurve : Rhino.DocObjects.Custom.UserData
    {
        public int BrepId { get; set; }

        private Dictionary<int, ElementDataCurve> mStageElementData { get; set; }

        public List<double[]> base_vecs { get; set; }

        public UserDataCurve()
        {
            BrepId = -1;

            mStageElementData = new Dictionary<int, ElementDataCurve>
            {
                { CocodriloPlugIn.Instance.GetCurrentStage(), new ElementDataCurve() }
            };

            base_vecs = new List<double[]>();
        }
        #region Get/Set-Methods

        public void addBaseVec(double _u, double _nx, double _ny, double _nz)
        {
            double[] base_vec = new double[4] { _u, _nx, _ny, _nz };
            if (base_vecs.Count == 0)
            {
                base_vecs.Add(base_vec);
            }
            else if (base_vecs[base_vecs.Count - 1][0] < _u)
                base_vecs.Add(base_vec);
            else
            {
                for (int i = 0; i < base_vecs.Count - 1; i++)
                {
                    if (base_vecs[i][0] < _u && base_vecs[i + 1][0] >= _u)
                    {
                        base_vecs.Insert(i, base_vec);
                        break;
                    }
                    else if (base_vecs[i + 1][0] == _u)
                    {
                        RhinoApp.WriteLine("U not allowed. Already defined");
                    }
                }
            }
            RhinoApp.WriteLine("New base vector added!");
        }

        public List<double[]> getBaseVecs()
        {
            return base_vecs;
        }

        #endregion
        #region Staging
        public ElementDataCurve GetCurrentElementData(int StageId = -1)
        {
            int current_stage_id = StageId;
            if (StageId == -1)
                current_stage_id = CocodriloPlugIn.Instance.GetCurrentStage();
            if (mStageElementData.ContainsKey(current_stage_id))
            {
                return mStageElementData[current_stage_id];
            }

            mStageElementData.Add(current_stage_id, new ElementDataCurve());
            return mStageElementData[current_stage_id];
        }
        #endregion
        #region BrepCoupling
        public void AddBrepGroupCouplingIds(List<int> BrepCouplingIds, int StageId = -1)
        {
            GetCurrentElementData(StageId).AddBrepGroupCouplingIds(BrepCouplingIds);
        }
        public void ChangeBrepGroupCouplingIds(List<int> BrepCouplingIds, int StageId = -1)
        {
            GetCurrentElementData(StageId).mBrepGroupCouplingIds = BrepCouplingIds;
        }
        public int GetThisBrepGroupCouplingId(int StageId = -1)
        {
            return GetCurrentElementData(StageId).mBrepGoupId;
        }
        public int ChangeThisBrepGroupCouplingId(int BrepGroupId, int StageId = -1)
        {
            return GetCurrentElementData(StageId).mBrepGoupId = BrepGroupId;
        }
        public bool IsBrepGroupCoupledWith(int BrepCouplingId, int StageId = -1)
        {
            return GetCurrentElementData(StageId).IsBrepGroupCoupledWith(BrepCouplingId);
        }
        #endregion
        #region BrepElementCurve
        public void AddBrepElementCurveVertex(
            Property ThisProperty,
            ParameterLocationCurve ThisParameterLocationCurve,
            bool Overwrite = true,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).AddBrepElementCurveVertex(
                ThisProperty,
                ThisParameterLocationCurve,
                Overwrite);
        }
        public void DeleteBrepElementCurveVertex(
            Property ThisProperty,
            ParameterLocationCurve ThisParameterLocationCurve,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteBrepElementCurveVertex(
                ThisProperty,
                ThisParameterLocationCurve);
        }

        public void DeleteBrepElementCurveVertexPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteBrepElementCurveVertexPropertyType(
                ThisPropertyType);
        }
        public void DeleteBrepElementCurveVertices(
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteBrepElementCurveVertices();
        }

        public List<BrepElementCurveVertex> GetBrepElementCurveVerticesOfPropertyType(
            Type ThisPropertyType,
            ParameterLocationCurve ThisParameterLocationCurve,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetBrepElementCurveVerticesOfPropertyType(
                ThisPropertyType,
                ThisParameterLocationCurve);
        }
        public List<BrepElementCurveVertex> GetBrepElementCurveVerticesOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetBrepElementCurveVerticesOfPropertyType(
                ThisPropertyType);
        }
        public List<BrepElementCurveVertex> GetBrepElementCurveVertices(
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).mBrepElementCurveVertices;
        }
        #endregion
        #region GeometryElementCurve
        public void AddGeometryElementCurve(
            Property ThisProperty,
            ParameterLocationCurve ThisParameterLocationCurve,
            bool Overwrite = true,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).AddGeometryElementCurve(
                ThisProperty,
                ThisParameterLocationCurve,
                Overwrite);
        }
        public void AddGeometryElementCurve(
            Property ThisProperty,
            bool Overwrite = true,
            int StageId = -1)
        {
            var parameter_location_curve = new ParameterLocationCurve(-1, -1);

            GetCurrentElementData(StageId).AddGeometryElementCurve(
                ThisProperty,
                parameter_location_curve,
                Overwrite);
        }


        public void DeleteGeometryElementCurve(
            Property ThisProperty,
            ParameterLocationCurve ThisParameterLocationCurve,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteGeometryElementCurve(
                ThisProperty,
                ThisParameterLocationCurve);
        }

        public void DeleteGeometryElementCurveOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteGeometryElementCurvePropertyType(
                ThisPropertyType);
        }

        public void DeleteGeometryElementCurves(
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteGeometryElementCurves();
        }

        public List<GeometryElementCurve> GetGeometryElementCurvesOfPropertyType(
            Type ThisPropertyType,
            ParameterLocationCurve ThisParameterLocationCurve,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetGeometryElementCurvesOfPropertyType(
                ThisPropertyType,
                ThisParameterLocationCurve);
        }
        public bool HasGeometryElementCurvesOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).HasGeometryElementCurvesOfPropertyType(
                ThisPropertyType);
        }
        public List<GeometryElementCurve> GetGeometryElementCurvesOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetGeometryElementCurvesOfPropertyType(
                ThisPropertyType);
        }
        #endregion
        #region Refinement
        public RefinementCurve GetRefinement(int StageId = -1)
        {
            return GetCurrentElementData(StageId).mRefinementCurve;
        }

        public void ChangeRefinement(
            RefinementCurve ThisRefinementCurve,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).mRefinementCurve = ThisRefinementCurve;
        }
        #endregion
        #region KRATOS input
        public bool TryGetKratosPropertiesBrepIds(
            ref Dictionary<int, List<int>> rPropertyElements,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).TryAddPropertyIdsBrepIds(
                ref rPropertyElements,
                BrepId);
        }
        #endregion
        #region Rhino methods
        protected override void OnDuplicate(Rhino.DocObjects.Custom.UserData source)
        {
            if (source is UserDataCurve src)
            {
                mStageElementData = src.mStageElementData;
                BrepId = src.BrepId;
            }
        }
        #endregion
        #region Read/Write
        public override bool ShouldWrite
        {
            get
            {
                return true;
            }
        }

        protected override bool Read(Rhino.FileIO.BinaryArchiveReader archive)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            var dict = archive.ReadDictionary();

            if (dict.ContainsKey("BrepId"))
            {
                BrepId = (int)dict["BrepId"];
            }
            if (dict.ContainsKey("ElementDataDict"))
            {
                mStageElementData = new Dictionary<int, ElementDataCurve>();
                if (dict.TryGetDictionary("ElementDataDict", out Rhino.Collections.ArchivableDictionary element_data_dict))
                {
                    foreach (var element_data_string in element_data_dict)
                    {
                        string StageElementDataString = (String)element_data_string.Value;
                        var stage_element_data = serializer.Deserialize<ElementDataCurve>(StageElementDataString);
                        mStageElementData.Add(Convert.ToInt32(element_data_string.Key), stage_element_data);
                    }
                }
            }

            return true;
        }
        protected override bool Write(Rhino.FileIO.BinaryArchiveWriter archive)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());

            var dict = new Rhino.Collections.ArchivableDictionary(1, "Physical");

            dict.Set("BrepId", BrepId);

            Rhino.Collections.ArchivableDictionary element_data_dict = new Rhino.Collections.ArchivableDictionary();
            if (mStageElementData != null)
            {
                foreach (var element_data in mStageElementData)
                {
                    string StageElementData = serializer.Serialize((object)element_data.Value);
                    element_data_dict.Set(element_data.Key.ToString(), StageElementData);
                }
            }
            dict.Set("ElementDataDict", element_data_dict);

            archive.WriteDictionary(dict);

            return true;
        }
        #endregion
    }
}
