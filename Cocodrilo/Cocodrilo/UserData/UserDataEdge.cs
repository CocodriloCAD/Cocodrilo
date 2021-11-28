using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Cocodrilo.Elements;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using Cocodrilo.ElementProperties;
using Cocodrilo.Refinement;

namespace Cocodrilo.UserData
{
    [Guid("E9F92FF3-F067-46BB-8F93-AD14B5C13554")]
    public class UserDataEdge : Rhino.DocObjects.Custom.UserData
    {
        public int BrepId { get; set; }

        public bool IsActive => GetCurrentElementData().mIsActive;

        private Coupling mCoupling { get; set; }

        private Dictionary<int, ElementDataEdge> mStageElementData { get; set; }

        public UserDataEdge()
        {
            BrepId = -1;

            mStageElementData = new Dictionary<int, ElementDataEdge>
            {
                { CocodriloPlugIn.Instance.GetCurrentStage(), new ElementDataEdge() }
            };

            mCoupling = new Coupling(0);
        }


        #region Coupling Functions
        public bool TryAddEmbeddedEdgeTrimIndex(int BrepId, int TrimIndex)
        {
            if (mCoupling.IsCoupledWith(BrepId))
                if (mCoupling.TryAddTrimIndexToBrepId(BrepId, TrimIndex))
                    return true;
            return false;
        }
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
        public ElementDataEdge GetCurrentElementData(int StageId = -1)
        {
            int current_stage_id = StageId;
            if (StageId == -1)
                current_stage_id = CocodriloPlugIn.Instance.GetCurrentStage();
            if (mStageElementData.ContainsKey(current_stage_id))
            {
                return mStageElementData[current_stage_id];
            }
            mStageElementData.Add(current_stage_id, new ElementDataEdge());

            return mStageElementData[current_stage_id];
        }
        #endregion
        #region BrepElementEdge
        public void AddBrepElementEdge(
            Property ThisProperty,
            bool Overwrite = true,
            int StageId = -1)
        {

            GetCurrentElementData(StageId).AddBrepElementEdge(
                ThisProperty,
                Overwrite);
        }

        public void DeleteBrepElementEdge(
            Property ThisProperty,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteBrepElementEdge(ThisProperty);
        }

        public void DeleteBrepElementEdgePropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteBrepElementEdgePropertyType(ThisPropertyType);
        }

        public void DeleteBrepElementEdges(
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteBrepElementEdges();
        }

        public List<BrepElementEdge> GetBrepElementEdgeOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetBrepElementEdgeOfPropertyType(ThisPropertyType);
        }
        public List<BrepElementEdge> GetBrepElementEdges(int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetBrepElementEdges();
        }
        #endregion
        #region Refinement
        public RefinementEdge GetRefinement(int StageId = -1)
        {
            return GetCurrentElementData(StageId).mRefinementEdge;
        }

        public void ChangeRefinement(
            RefinementEdge ThisRefinementEdge,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).mRefinementEdge = ThisRefinementEdge;
        }
        #endregion

        public List<int> GetPropertyIds(
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetPropertyIds();
        }

        #region KRATOS input
        public bool TryGetKratosPropertyIdsBrepIds(
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
            if (source is UserDataEdge src)
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
                mStageElementData = new Dictionary<int, ElementDataEdge>();
                if (dict.TryGetDictionary("ElementDataDict", out Rhino.Collections.ArchivableDictionary element_data_dict))
                {
                    foreach (var element_data_string in element_data_dict)
                    {
                        string StageElementDataString = (String)element_data_string.Value;
                        var stage_element_data = serializer.Deserialize<ElementDataEdge>(StageElementDataString);
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
