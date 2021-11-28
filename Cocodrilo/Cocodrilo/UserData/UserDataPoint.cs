using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Cocodrilo.Elements;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Rhino.DocObjects;
using Cocodrilo.ElementProperties;


namespace Cocodrilo.UserData
{
    [Guid("E9F92FF3-F067-46BB-8F93-AD14B5C13654")]
    public class UserDataPoint : Rhino.DocObjects.Custom.UserData
    {
        public int BrepId { get; set; }

        private Coupling mCoupling { get; set; }

        private Dictionary<int, ElementDataPoint> mStageElementData { get; set; }

        public UserDataPoint()
        {
            BrepId = -1;

            mStageElementData = new Dictionary<int, ElementDataPoint>
            {
                { CocodriloPlugIn.Instance.GetCurrentStage(), new ElementDataPoint() }
            };

            mCoupling = new Coupling(0);
        }

        #region ElementData
        public ElementDataPoint GetCurrentElementData(int StageId = -1)
        {
            int current_stage_id = StageId;
            if (StageId == -1)
                current_stage_id = CocodriloPlugIn.Instance.GetCurrentStage();
            if (mStageElementData.ContainsKey(current_stage_id))
            {
                return mStageElementData[current_stage_id];
            }
            mStageElementData.Add(current_stage_id, new ElementDataPoint());

            return mStageElementData[current_stage_id];
        }
        #endregion

        #region Coupling Information
        public bool TryAddTrimIndex(int BrepId, int TrimIndex)
        {
            if (mCoupling.IsCoupledWith(BrepId))
                if (mCoupling.TryAddTrimIndexToBrepId(BrepId, TrimIndex))
                    return true;
            return false;
        }
        public bool TryAddLocalCoordinates(int BrepId, double x, double y, double z)
        {
            if (mCoupling.IsCoupledWith(BrepId))
                if (mCoupling.TryAddTrimIndexToBrepId(BrepId, x, y, z))
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
            return success1 || success2;
        }
        public Coupling GetCoupling()
        {
            return mCoupling;
        }
        #endregion

        #region GeometryElementVertex
        public void AddGeometryElementVertex(
            Property ThisProperty,
            bool Overwrite = true,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).AddGeometryElementVertex(
                ThisProperty,
                Overwrite);
        }

        public void DeleteGeometryElementVertex(
            Property ThisProperty,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteGeometryElementVertex(ThisProperty);
        }

        public void DeleteGeometryElementVertexPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteGeometryElementVertexPropertyType(ThisPropertyType);
        }

        public void DeleteGeometryElementVertices(
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteGeometryElementVertices();
        }

        public List<GeometryElementVertex> GetGeometryElementVertexOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetGeometryElementVertexOfPropertyType(ThisPropertyType);
        }

        public List<GeometryElementVertex> GetGeometryElementVertices(
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetGeometryElementVertices();
        }
        #endregion

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

        #region Read/Write
        public override bool ShouldWrite
        {
            get
            {
                if (mStageElementData.Count > 0)
                    return true;
                if (BrepId != 0)
                    return true;

                return false;
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
                mStageElementData = new Dictionary<int, ElementDataPoint>();
                if (dict.TryGetDictionary("ElementDataDict", out Rhino.Collections.ArchivableDictionary element_data_dict))
                {
                    foreach (var element_data_string in element_data_dict)
                    {
                        string StageElementDataString = (String)element_data_string.Value;
                        var stage_element_data = serializer.Deserialize<ElementDataPoint>(StageElementDataString);
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
