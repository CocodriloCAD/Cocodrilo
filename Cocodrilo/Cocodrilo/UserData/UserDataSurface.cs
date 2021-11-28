using System;
using System.Collections.Generic;
using Cocodrilo.Elements;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using Cocodrilo.ElementProperties;
using Cocodrilo.Refinement;

namespace Cocodrilo.UserData
{
    [Guid("9F5AF298-8FAD-4E0B-9A63-242383FB194F")]
    public class UserDataSurface : Rhino.DocObjects.Custom.UserData
    {
        public int BrepId { get; set; }
        public bool IsActive => GetCurrentElementData().mIsActive;
        private Dictionary<int, ElementDataSurface> mStageElementData { get; set; }

        public UserDataSurface()
        {
            BrepId = -1;

            mStageElementData = new Dictionary<int, ElementDataSurface>
            {
                { CocodriloPlugIn.Instance.GetCurrentStage(), new ElementDataSurface() }
            };
        }

        #region Staging
        public ElementDataSurface GetCurrentElementData(int StageId = -1)
        {
            int current_stage_id = StageId;
            if (StageId == -1)
                current_stage_id = CocodriloPlugIn.Instance.GetCurrentStage();
            if (mStageElementData.ContainsKey(current_stage_id))
            {
                return mStageElementData[current_stage_id];
            }
            else
            {
                mStageElementData.Add(current_stage_id, new ElementDataSurface());
                return mStageElementData[current_stage_id];
            }
        }
        #endregion
        #region BrepCoupling
        public int GetBrepGroupCouplingId(int StageId = -1)
        {
            return GetCurrentElementData(StageId).mBrepGoupId;
        }
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
        #region BrepElementSurface
        public void AddBrepElementSurfaceVertex(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface,
            bool Overwrite = true,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).AddBrepElementSurfaceVertex(
                ThisProperty,
                ThisParameterLocationSurface,
                Overwrite);
        }

        public void DeleteBrepElementSurfaceVertex(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteBrepElementSurfaceVertex(
                ThisProperty,
                ThisParameterLocationSurface);
        }

        public void DeleteBrepElementSurfaceVertexPropertyType(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteBrepElementSurfaceVertexPropertyType(
                ThisProperty,
                ThisParameterLocationSurface);
        }
        public void DeleteBrepElementSurfaceVertices(
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteBrepElementSurfaceVertices();
        }
        public List<BrepElementSurfaceVertex> GetBrepElementSurfaceVerticesOfPropertyType(
            Type ThisPropertyType,
            ParameterLocationSurface ThisParameterLocationSurface,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetBrepElementSurfaceVerticesOfPropertyType(
                ThisPropertyType,
                ThisParameterLocationSurface);
        }
        public List<BrepElementSurfaceVertex> GetBrepElementSurfaceVerticesOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetBrepElementSurfaceVerticesOfPropertyType(
                ThisPropertyType);
        }
        public List<BrepElementSurfaceVertex> GetBrepElementSurfaceVertices(
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).mBrepElementSurfaceVertices;
        }
        #endregion
        #region GeometryElementSurface
        public void AddGeometryElementSurface(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface,
            bool Overwrite = true,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).AddGeometryElementSurface(
                ThisProperty,
                ThisParameterLocationSurface,
                Overwrite);
        }
        public void AddGeometryElementSurface(
            Property ThisProperty,
            bool Overwrite = true,
            int StageId = -1)
        {
            var parameter_location_surface =
                new ParameterLocationSurface(-1, -1, -1, -1);

            GetCurrentElementData(StageId).AddGeometryElementSurface(
                ThisProperty,
                parameter_location_surface,
                Overwrite);
        }
        public void DeleteGeometryElementSurface(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteGeometryElementSurface(
                ThisProperty,
                ThisParameterLocationSurface);
        }

        public void DeleteGeometryElementSurfaceOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteGeometryElementSurfacePropertyType(
                ThisPropertyType);
        }

        public void DeleteGeometryElementSurfaces(
            int StageId = -1)
        {
            GetCurrentElementData(StageId).DeleteGeometryElementSurfaces();
        }

        public List<GeometryElementSurface> GetGeometryElementSurfacesOfPropertyType(
            Type ThisPropertyType,
            ParameterLocationSurface ThisParameterLocationSurface,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetGeometryElementSurfacesOfPropertyType(
                ThisPropertyType,
                ThisParameterLocationSurface);
        }
        public List<GeometryElementSurface> GetGeometryElementSurfacesOfPropertyType(
            Type ThisPropertyType,
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).GetGeometryElementSurfacesOfPropertyType(
                ThisPropertyType);
        }

        public List<GeometryElementSurface> GetGeometryElementSurfaces(
            int StageId = -1)
        {
            return GetCurrentElementData(StageId).mGeometryElementSurfaces;
        }

        public int GetOneElementId(int StageId = -1)
        {
            foreach (var geometry_element_surface in GetCurrentElementData(StageId).mGeometryElementSurfaces)
            {
                if (geometry_element_surface.GetPropertyType() == typeof(PropertyMembrane) ||
                    geometry_element_surface.GetPropertyType() == typeof(PropertyShell))
                    return geometry_element_surface.mElementId;
            }

            return -1;
        }

        public List<GeometryElementSurface> GetGeometryElementSurfacesElementFormulation(int StageId = -1)
        {
            var list = new List<GeometryElementSurface>();
            foreach (var geometry_element_surface in GetCurrentElementData(StageId).mGeometryElementSurfaces)
            {
                if (geometry_element_surface.GetPropertyType() == typeof(PropertyMembrane) ||
                    geometry_element_surface.GetPropertyType() == typeof(PropertyShell))
                    list.Add(geometry_element_surface);
            }

            return list;
        }
        public List<GeometryElementSurface> GetGeometryElementSurfacesNonElementFormulation(int StageId = -1)
        {
            var list = new List<GeometryElementSurface>();
            foreach (var geometry_element_surface in GetCurrentElementData(StageId).mGeometryElementSurfaces)
            {
                if (geometry_element_surface.GetPropertyType() != typeof(PropertyMembrane) &&
                    geometry_element_surface.GetPropertyType() != typeof(PropertyShell))
                    list.Add(geometry_element_surface);
            }

            return list;
        }
        #endregion
        #region Special Functions
        public bool CheckRotationalContinuity(
            int StageId = -1)
        {
            var shell_elements = GetCurrentElementData(StageId)
                                     .GetGeometryElementSurfacesOfPropertyType(typeof(PropertyShell));
            foreach (var shell in shell_elements)
                if (shell.GetProperty<PropertyShell>().mShellProperties.mCoupleRotations)
                    return true;
            return false;
        }
        #endregion
        #region Carat
        public List<int> GetElementProperties(int Stage = -1)
        {
            var property_id_list = new List<int>();

            var element_data = GetCurrentElementData();

            foreach (var membrane in element_data.GetGeometryElementSurfacesOfPropertyType(typeof(PropertyMembrane)))
            {
                property_id_list.Add(membrane.GetPropertyId());
            }
            foreach (var shell3p in element_data.GetGeometryElementSurfacesOfPropertyType(typeof(PropertyShell)))
            {
                property_id_list.Add(shell3p.GetPropertyId());
            }

            return property_id_list;
        }
        #endregion
        #region Refinement
        public RefinementSurface GetRefinement(int Stage = -1)
        {
            return GetCurrentElementData().mRefinementSurface;
        }
        public void ChangeRefinement(RefinementSurface ThisRefinementSurface, int Stage = -1)
        {
            GetCurrentElementData().mRefinementSurface = ThisRefinementSurface;
        }
        #endregion
        #region KRATOS input
        public bool TryGetKratosPropertyIdsBrepIds(
            ref Dictionary<int, List<int>> rPropertyElements,
            int StageId = -1)
        {
            if (IsActive == false)
                return false;
            return GetCurrentElementData(StageId).TryAddPropertyIdsBrepIds(
                ref rPropertyElements,
                BrepId);
        }
        #endregion
        #region Rhino methods
        protected override void OnDuplicate(Rhino.DocObjects.Custom.UserData source)
        {
            if (source is UserDataSurface src)
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
            var dict = archive.ReadDictionary();

            if (dict.ContainsKey("BrepId"))
            {
                BrepId = (int) dict["BrepId"];
            }
            if (dict.ContainsKey("ElementDataDict"))
            {
                mStageElementData = new Dictionary<int, ElementDataSurface>();
                if (dict.TryGetDictionary("ElementDataDict", out Rhino.Collections.ArchivableDictionary element_data_dict))
                {
                    foreach (var element_data_string in element_data_dict)
                    {
                        string StageElementDataString = (String)element_data_string.Value;
                        var stage_element_data = serializer.Deserialize<ElementDataSurface>(StageElementDataString);
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
