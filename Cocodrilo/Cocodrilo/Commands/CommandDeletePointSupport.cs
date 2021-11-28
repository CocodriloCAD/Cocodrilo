using System;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input.Custom;
using TeDaSharp.UserData;

namespace TeDaSharp.Commands
{
#if (KIWI_RELEASE)
#else
    [System.Runtime.InteropServices.Guid("ce5ae254-9f2f-4513-818d-b6df93e6d649")]
    public class CommandDeletePointSupport : Command
    {
        static CommandDeletePointSupport _instance;
        public CommandDeletePointSupport()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandDeletePointSupport command.</summary>
        public static CommandDeletePointSupport Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "TeDA_DeletePointSupport"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // select a surface
            var gs = new GetObject();
            gs.SetCommandPrompt("select surface");
            gs.GeometryFilter = ObjectType.Surface;
            gs.DisablePreSelect();
            gs.SubObjectSelect = true;
            gs.Get();
            if (gs.CommandResult() != Result.Success)
                return Result.Success;
            // get the selected face
            var face = gs.Object(0).Face();
            if (face == null)
                return Result.Success;

            var surface = gs.Object(0);
            //face.ToBrep().UserData
            var ud =
                surface.Brep().Surfaces[surface.Face().FaceIndex].UserData.Find(typeof(UserDataSurface)) as
                    UserDataSurface;
            if (ud == null)
            {
                ud = new UserDataSurface();
                surface.Brep().Surfaces[surface.Face().FaceIndex].UserData.Add(ud);
                RhinoApp.WriteLine("New Userdata Added");
            }

            // pick a point on the surface.  Constrain
            // picking to the face.
            while (true)
            {
                var gp = new GetPoint();
                gp.SetCommandPrompt("select point on surface");
                gp.Constrain(face, false);
                gp.Get();
                if (gp.CommandResult() != Result.Success)
                    break;

                //new BRepElementVerticeSupport(face.)

                double u, v;
                face.ClosestPoint(gp.Point(), out u, out v);


                ud.myElementDataSurface.removeVertexSupport(u, v);

                //var doc = RhinoDoc.ActiveDoc;
                doc.Views.Redraw();

                if (gp.CommandResult() != Result.Success)
                    break;
            }
            return Result.Success;
        }
    }
#endif
}
