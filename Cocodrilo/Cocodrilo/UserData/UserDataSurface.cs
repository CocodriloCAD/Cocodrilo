using System;
using System.Linq;
using System.Collections.Generic;
using Cocodrilo.Elements;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using Cocodrilo.ElementProperties;
using Cocodrilo.Refinement;

namespace Cocodrilo.UserData
{
    [Guid("9F5AF298-8FAD-4E0B-9A63-242383FB194F")]
    public class UserDataSurface : UserDataCocodrilo
    {
        RefinementSurface mRefinement { get; set; }

        public UserDataSurface() : base()
        {
            mRefinement = new RefinementSurface(1,1,0,0,0);
        }

        public override void AddNumericalElement(
            Property ThisProperty,
            bool Overwrite = true,
            int StageId = -1)
        {
            var parameter_location_surface = new ParameterLocationSurface(GeometryType.GeometrySurface, -1, -1, -1, -1);

            GetCurrentElementData(StageId).AddNumericalElement(
                ThisProperty,
                parameter_location_surface,
                Overwrite);
        }

        public bool CheckRotationalContinuity(
            int StageId = -1)
        {
            var shell_elements = GetCurrentElementData(StageId)
                                     .GetNumericalElementsOfPropertyType(typeof(PropertyShell));
            foreach (var shell in shell_elements)
                if (shell.GetProperty<PropertyShell>().mShellProperties.mCoupleRotations)
                    return true;
            return false;
        }

        public override Refinement.Refinement GetRefinement(int StageId = -1)
        {
            return mRefinement;
        }

        public override void ChangeRefinement(
            Refinement.Refinement ThisRefinement,
            int StageId = -1)
        {
            if (ThisRefinement is RefinementSurface)
            {
                mRefinement = ThisRefinement as RefinementSurface;
            }
            else
            {
                Rhino.RhinoApp.WriteLine("WARNING: Trying to assign a refinement to a surface, which is not of type RefinementSurface.");
            }
        }
        protected override void OnDuplicate(Rhino.DocObjects.Custom.UserData source)
        {
            if (source is UserDataSurface src)
            {
                base.OnDuplicate(src);
                mRefinement = src.mRefinement;
            }
        }
        //    #region Staging
        //    public ElementDataSurface GetCurrentElementData(int StageId = -1)
        //    {
        //        int current_stage_id = StageId;
        //        if (StageId == -1)
        //            current_stage_id = CocodriloPlugIn.Instance.GetCurrentStage();
        //        if (mStageElementData.ContainsKey(current_stage_id))
        //        {
        //            return mStageElementData[current_stage_id];
        //        }
        //        else
        //        {
        //            mStageElementData.Add(current_stage_id, new ElementDataSurface());
        //            return mStageElementData[current_stage_id];
        //        }
        //    }
        //    #endregion
        //    #region BrepCoupling
        //    public int GetBrepGroupCouplingId(int StageId = -1)
        //    {
        //        return GetCurrentElementData(StageId).mBrepGoupId;
        //    }
        //    public void AddBrepGroupCouplingIds(List<int> BrepCouplingIds, int StageId = -1)
        //    {
        //        GetCurrentElementData(StageId).AddBrepGroupCouplingIds(BrepCouplingIds);
        //    }
        //    public void ChangeBrepGroupCouplingIds(List<int> BrepCouplingIds, int StageId = -1)
        //    {
        //        GetCurrentElementData(StageId).mBrepGroupCouplingIds = BrepCouplingIds;
        //    }
        //    public int GetThisBrepGroupCouplingId(int StageId = -1)
        //    {
        //        return GetCurrentElementData(StageId).mBrepGoupId;
        //    }
        //    public int ChangeThisBrepGroupCouplingId(int BrepGroupId, int StageId = -1)
        //    {
        //        return GetCurrentElementData(StageId).mBrepGoupId = BrepGroupId;
        //    }
        //    public bool IsBrepGroupCoupledWith(int BrepCouplingId, int StageId = -1)
        //    {
        //        return GetCurrentElementData(StageId).IsBrepGroupCoupledWith(BrepCouplingId);
        //    }
        //    #endregion
        //    //#region BrepElementSurface
        //    //public void AddBrepElementSurfaceVertex(
        //    //    Property ThisProperty,
        //    //    ParameterLocationSurface ThisParameterLocationSurface,
        //    //    bool Overwrite = true,
        //    //    int StageId = -1)
        //    //{
        //    //    GetCurrentElementData(StageId).AddBrepElementSurfaceVertex(
        //    //        ThisProperty,
        //    //        ThisParameterLocationSurface,
        //    //        Overwrite);
        //    //}

        //    //public void DeleteBrepElementSurfaceVertex(
        //    //    Property ThisProperty,
        //    //    ParameterLocationSurface ThisParameterLocationSurface,
        //    //    int StageId = -1)
        //    //{
        //    //    GetCurrentElementData(StageId).DeleteBrepElementSurfaceVertex(
        //    //        ThisProperty,
        //    //        ThisParameterLocationSurface);
        //    //}

        //    //public void DeleteBrepElementSurfaceVertexPropertyType(
        //    //    Property ThisProperty,
        //    //    ParameterLocationSurface ThisParameterLocationSurface,
        //    //    int StageId = -1)
        //    //{
        //    //    GetCurrentElementData(StageId).DeleteBrepElementSurfaceVertexPropertyType(
        //    //        ThisProperty,
        //    //        ThisParameterLocationSurface);
        //    //}
        //    //public void DeleteBrepElementSurfaceVertices(
        //    //    int StageId = -1)
        //    //{
        //    //    GetCurrentElementData(StageId).DeleteBrepElementSurfaceVertices();
        //    //}
        //    //public List<NumericalElementPoint> GetBrepElementSurfaceVerticesOfPropertyType(
        //    //    Type ThisPropertyType,
        //    //    ParameterLocationSurface ThisParameterLocationSurface,
        //    //    int StageId = -1)
        //    //{
        //    //    return GetCurrentElementData(StageId).GetBrepElementSurfaceVerticesOfPropertyType(
        //    //        ThisPropertyType,
        //    //        ThisParameterLocationSurface);
        //    //}
        //    //public List<NumericalElementPoint> GetBrepElementSurfaceVerticesOfPropertyType(
        //    //    Type ThisPropertyType,
        //    //    int StageId = -1)
        //    //{
        //    //    return GetCurrentElementData(StageId).GetBrepElementSurfaceVerticesOfPropertyType(
        //    //        ThisPropertyType);
        //    //}
        //    //public List<NumericalElementPoint> GetBrepElementSurfaceVertices(
        //    //    int StageId = -1)
        //    //{
        //    //    return GetCurrentElementData(StageId).mBrepElementSurfaceVertices;
        //    //}
        //    //#endregion
        //    #region GeometryElementSurface
        //    public void AddNumericalElement(
        //        Property ThisProperty,
        //        ParameterLocationSurface ThisParameterLocationSurface,
        //        bool Overwrite = true,
        //        int StageId = -1)
        //    {
        //        GetCurrentElementData(StageId).AddNumericalElement(
        //            ThisProperty,
        //            ThisParameterLocationSurface,
        //            Overwrite);
        //    }
        //    public override void AddNumericalElement(
        //        Property ThisProperty,
        //        bool Overwrite = true,
        //        int StageId = -1)
        //    {
        //        var parameter_location_surface = new ParameterLocationSurface(GeometryType.GeometrySurface, -1, -1, -1, -1);

        //        GetCurrentElementData(StageId).AddNumericalElement(
        //            ThisProperty,
        //            parameter_location_surface,
        //            Overwrite);
        //    }
        //    public void DeleteNumericalElement(
        //        Property ThisProperty,
        //        ParameterLocationSurface ThisParameterLocation,
        //        int StageId = -1)
        //    {
        //        GetCurrentElementData(StageId).DeleteNumericalElement(
        //            ThisProperty,
        //            ThisParameterLocation);
        //    }

        //    public void DeleteNumericalElementOfPropertyType(
        //        Type ThisPropertyType,
        //        int StageId = -1)
        //    {
        //        GetCurrentElementData(StageId).DeleteNumericalElementOfPropertyType(
        //            ThisPropertyType);
        //    }

        //    public void DeleteNumericalElement(
        //        int StageId = -1)
        //    {
        //        GetCurrentElementData(StageId).DeleteNumericalElements();
        //    }

        //    public List<NumericalElement> GetNumericalElementsOfPropertyType(
        //        Type ThisPropertyType,
        //        ParameterLocationCurve ThisParameterLocationCurve,
        //        int StageId = -1)
        //    {
        //        return GetCurrentElementData(StageId).GetNumericalElementsOfPropertyType(
        //            ThisPropertyType,
        //            ThisParameterLocationCurve);
        //    }
        //    public bool HasNumericalElementsOfPropertyType(
        //        Type ThisPropertyType,
        //        int StageId = -1)
        //    {
        //        return GetCurrentElementData(StageId).HasGeometryElementCurvesOfPropertyType(
        //            ThisPropertyType);
        //    }
        //    public List<NumericalElement> GetNumericalElementsOfPropertyType(
        //        Type ThisPropertyType,
        //        int StageId = -1)
        //    {
        //        return GetCurrentElementData(StageId).GetNumericalElementsOfPropertyType(
        //            ThisPropertyType);
        //    }

        //    public List<NumericalElement> GetNumericalElements(
        //        int StageId = -1)
        //    {
        //        return GetCurrentElementData(StageId).mNumericalElements;
        //    }

        //    public List<NumericalElement> GetGeometryElementSurfacesElementFormulation(int StageId = -1)
        //    {
        //        return GetCurrentElementData(StageId).mNumericalElements.Where(item =>
        //                (item.GetPropertyType() == typeof(PropertyMembrane)) ||
        //                (item.GetPropertyType() == typeof(PropertyShell))).ToList();
        //    }
        //    public List<NumericalElement> GetGeometryElementSurfacesNonElementFormulation(int StageId = -1)
        //    {
        //        return GetCurrentElementData(StageId).mNumericalElements.Where(item =>
        //                (item.GetPropertyType() != typeof(PropertyMembrane)) &&
        //                (item.GetPropertyType() != typeof(PropertyShell))).ToList();
        //    }
        //    #endregion
        //    #region Special Functions
        //    public bool CheckRotationalContinuity(
        //        int StageId = -1)
        //    {
        //        var shell_elements = GetCurrentElementData(StageId)
        //                                 .GetNumericalElementsOfPropertyType(typeof(PropertyShell));
        //        foreach (var shell in shell_elements)
        //            if (shell.GetProperty<PropertyShell>().mShellProperties.mCoupleRotations)
        //                return true;
        //        return false;
        //    }
        //    #endregion
        //    #region KRATOS input
        //    public bool TryGetKratosPropertyIdsBrepIds(
        //        ref Dictionary<int, List<int>> rPropertyElements,
        //        int StageId = -1)
        //    {
        //        if (IsActive == false)
        //            return false;
        //        return GetCurrentElementData(StageId).TryAddPropertyIdsBrepIds(
        //            ref rPropertyElements,
        //            BrepId);
        //    }
        //    #endregion
        //    #region Rhino methods
        //    protected override void OnDuplicate(Rhino.DocObjects.Custom.UserData source)
        //    {
        //        if (source is UserDataSurface src)
        //        {
        //            mStageElementData = src.mStageElementData;
        //            BrepId = src.BrepId;
        //        }
        //    }
        //    public override bool ShouldWrite
        //    {
        //        get
        //        {
        //            if ((mStageElementData.Count > 0) || (BrepId != -1))
        //                return true;
        //            return false;
        //        }
        //    }

        protected override bool Read(Rhino.FileIO.BinaryArchiveReader archive)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            var dict = archive.ReadDictionary();

            if (dict.ContainsKey("RefinementSurface"))
            {
                mRefinement = (RefinementSurface)dict["RefinementSurface"];
            }

            return true;
        }
        protected override bool Write(Rhino.FileIO.BinaryArchiveWriter archive)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            var dict = new Rhino.Collections.ArchivableDictionary(1, "Physical");

            string RefinementSurfaceString = serializer.Serialize((object)mRefinement);
            dict.Set("RefinementSurface", RefinementSurfaceString);

            archive.WriteDictionary(dict);

            return true;
        }
        //    #endregion
        //}
    }
}
