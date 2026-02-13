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
    // Enter meaningful GUID
    [Guid("ABCDE298-8FAD-4E0B-9A63-242383FB194F")]

    public class UserDataMesh : UserDataCocodrilo
    {
        // Refinement related variables and methods don't make any sense yet. They are just here to ensure the compiling of the project.
        RefinementMesh mRefinement { get; set; }
        public UserDataMesh() : base()
        {
            //mRefinement = new RefinementSurface(1, 1, 0, 0, 0);
            mRefinement = null;
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
                mRefinement = ThisRefinement as RefinementMesh;
            }
            else
            {
                Rhino.RhinoApp.WriteLine("WARNING: Trying to assign a refinement to a surface, which is not of type RefinementSurface.");
            }
        }

        // So far not adapted to Mesh; just copied from surface
        public override void AddNumericalElement(
            Property ThisProperty,
            bool Overwrite = true,
            int StageId = -1)
        {
            var parameter_location_surface = new ParameterLocationSurface(GeometryType.Mesh, -1, -1, -1, -1);

            GetCurrentElementData(StageId).AddNumericalElement(
                ThisProperty,
                parameter_location_surface,
                Overwrite);
        }

        protected override bool Read(Rhino.FileIO.BinaryArchiveReader archive)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            var dict = archive.ReadDictionary();

            //if (dict.ContainsKey("RefinementSurface"))
            //{
            //    mRefinement = (RefinementSurface)dict["RefinementSurface"];
            //}

            return true;
        }

        protected override bool Write(Rhino.FileIO.BinaryArchiveWriter archive)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            var dict = new Rhino.Collections.ArchivableDictionary(1, "Physical");

            //string RefinementSurfaceString = serializer.Serialize((object)mRefinement);
            //dict.Set("RefinementSurface", RefinementSurfaceString);

            //archive.WriteDictionary(dict);

            return true;
        }

    }
}
