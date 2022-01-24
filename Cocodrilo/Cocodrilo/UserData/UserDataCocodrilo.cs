using Cocodrilo.ElementProperties;
using Cocodrilo.Elements;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.UserData
{
    public abstract class UserDataCocodrilo : Rhino.DocObjects.Custom.UserData
    {
        public int BrepId { get; set; }

        protected Coupling mCoupling { get; set; }

        private Dictionary<int, ElementData> mStageElementData { get; set; }

        public UserDataCocodrilo()
        {
            BrepId = -1;

            mStageElementData = new Dictionary<int, ElementData>
            {
                { CocodriloPlugIn.Instance.GetCurrentStage(), new ElementData() }
            };

            mCoupling = new Coupling(0);
        }

        #region Coupling Functions
        public bool TryAddTrimIndex(int BrepId, int TrimIndex)
        {
            if (mCoupling.IsCoupledWith(BrepId))
                if (mCoupling.TryAddTrimIndexToBrepId(BrepId, TrimIndex))
                    return true;
            return false;
        }
        //public bool TryAddLocalCoordinates(int BrepId, double x, double y, double z)
        //{
        //    if (mCoupling.IsCoupledWith(BrepId))
        //        if (mCoupling.TryAddTrimIndexToBrepId(BrepId, x, y, z))
        //            return true;
        //    return false;
        //}
        public bool IsCoupledWith(int BrepId)
        {
            return mCoupling.IsCoupledWith(BrepId);
        }
        public bool AddCoupling(int MasterBrepId, int SlaveBrepId)
        {
            bool success1 = mCoupling.TryAddCoupling(MasterBrepId);
            bool success2 = mCoupling.TryAddCoupling(SlaveBrepId);
            return (success1 || success2);
        }
        public bool AddCoupling(int MasterBrepId)
        {
            bool success1 = mCoupling.TryAddCoupling(MasterBrepId);
            return (success1);
        }
        public Coupling GetCoupling()
        {
            return mCoupling;
        }
        #endregion
        #region BrepCoupling
        public void AddBrepGroupCouplingId(int BrepCouplingId, int StageId = -1)
        {
            GetCurrentElementData(StageId).AddBrepGroupCouplingId(BrepCouplingId);
        }
        public int GetThisBrepGroupCouplingId(int StageId = -1)
        {
            return GetCurrentElementData(StageId).mBrepGoupId;
        }
        public bool IsBrepGroupCoupledWith(int BrepCouplingId, int StageId = -1)
        {
            return GetCurrentElementData(StageId).IsBrepGroupCoupledWith(BrepCouplingId);
        }
        #endregion
        #region Staging
        public ElementData GetCurrentElementData(int StageId = -1)
        {
            int current_stage_id = StageId;
            if (StageId == -1)
                current_stage_id = CocodriloPlugIn.Instance.GetCurrentStage();
            if (mStageElementData.ContainsKey(current_stage_id))
            {
                return mStageElementData[current_stage_id];
            }
            mStageElementData.Add(current_stage_id, new ElementData());

            return mStageElementData[current_stage_id];
        }
        #endregion
        #region NumericalElement
        public void AddNumericalElement(
            Property ThisProperty,
            ParameterLocation ThisParameterLocation,
            bool Overwrite = true,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).AddNumericalElement(
                ThisProperty,
                ThisParameterLocation,
                Overwrite);
        }
        public abstract void AddNumericalElement(
            Property ThisProperty,
            bool Overwrite = true,
            int StageId = -1);

        public void DeleteNumericalElement(
            Property ThisProperty,
            ParameterLocation ThisParameterLocation,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteNumericalElement(
                ThisProperty,
                ThisParameterLocation);
        }
        public void DeleteNumericalElement(
            Property ThisProperty,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteNumericalElement(
                ThisProperty);
        }
        public void DeleteNumericalElementOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteNumericalElementOfPropertyType(
                ThisPropertyType);
        }

        public void DeleteNumericalElements(
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteNumericalElements();
        }
        public bool HasNumericalElementsOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).HasGeometryElementCurvesOfPropertyType(
                ThisPropertyType);
        }

        public List<NumericalElement> GetNumericalElements(
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).mNumericalElements;
        }
        public List<NumericalElement> GetNumericalElementsOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId)?.GetNumericalElementsOfPropertyType(
                ThisPropertyType);
        }
        public List<NumericalElement> GetNumericalElementsOfPropertyType(
            Type ThisPropertyType,
            ParameterLocationCurve ThisParameterLocationCurve,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetNumericalElementsOfPropertyType(
                ThisPropertyType,
                ThisParameterLocationCurve);
        }
        #endregion
        #region Refinement
        public abstract Refinement.Refinement GetRefinement(int StageId = -1);

        public abstract void ChangeRefinement(
            Refinement.Refinement ThisRefinement,
            int StageId = -1);

        #endregion
        #region KRATOS input
        public bool TryGetKratosPropertyIdsBrepIds(
            ref Dictionary<int, List<Cocodrilo.IO.BrepToParameterLocations>> rPropertyElements,
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
            if (source is UserDataCocodrilo src)
            {
                mStageElementData = src.mStageElementData;
                BrepId = src.BrepId;
            }
        }
        public override bool ShouldWrite
        {
            get
            {
                if ((mStageElementData.Count > 0) || (BrepId != -1))
                    return true;
                return false;
            }
        }

        protected override bool Read(Rhino.FileIO.BinaryArchiveReader archive)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            Rhino.Collections.ArchivableDictionary dict = archive.ReadDictionary();
            if (dict.ContainsKey("BrepId"))
            {
                BrepId = (int)dict["BrepId"];
            }
            if (dict.ContainsKey("ElementDataDict"))
            {
                mStageElementData = new Dictionary<int, ElementData>();
                if (dict.TryGetDictionary("ElementDataDict", out Rhino.Collections.ArchivableDictionary element_data_dict))
                {
                    foreach (var element_data_string in element_data_dict)
                    {
                        string StageElementDataString = (String)element_data_string.Value;
                        var stage_element_data = serializer.Deserialize<ElementData>(StageElementDataString);
                        mStageElementData.Add(Convert.ToInt32(element_data_string.Key), stage_element_data);
                    }
                }
            }
            if (dict.ContainsKey("Couping"))
            {
                string CouplingString = (String)dict["Couping"];
                mCoupling = serializer.Deserialize<Coupling>(CouplingString);
            }
            return true;
        }
        protected override bool Write(Rhino.FileIO.BinaryArchiveWriter archive)
        {
            var serializer = new JavaScriptSerializer();

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

            string Couping = serializer.Serialize(mCoupling);
            dict.Set("Couping", Couping);

            archive.WriteDictionary(dict);

            return true;
        }
        #endregion
    }
}
