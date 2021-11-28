using Rhino.Geometry;
using System;

namespace Cocodrilo
{
    class EventWatcher
    {
        /// <summary>
        /// Called once a object is modified or changed and ensures that the correct user data is kept.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void onReplaceObject(Object sender, Rhino.DocObjects.RhinoReplaceObjectEventArgs e)
        {
            var old_brep = (e.OldRhinoObject.Geometry as Brep);
            var new_brep = (e.NewRhinoObject.Geometry as Brep);

            if (new_brep == null) return;
            if (old_brep == null) return;
            if (!new_brep.HasBrepForm) return;
            if (!old_brep.HasBrepForm) return;
            if (!old_brep.IsValid) return;
            if (!new_brep.IsValid) return;

            if (new_brep.Curves2D.Count == old_brep.Curves2D.Count)
            {
                for (int i = 0; i < old_brep.Curves2D.Count; i++)
                {
                    var ud = old_brep.Curves2D[i].UserData.Find(typeof(UserData.UserDataEdge)) as UserData.UserDataEdge;
                    var udnew = new_brep.Curves2D[i].UserData.Find(typeof(UserData.UserDataEdge)) as UserData.UserDataEdge;
                    if (udnew != null) continue;
                    if (ud != null)
                    {
                        new_brep.Curves2D[i].UserData.Add(ud);
                    }

                }
            }
            if (new_brep.Surfaces.Count == old_brep.Surfaces.Count)
            {
                for (int i = 0; i < old_brep.Surfaces.Count; i++)
                {
                    var ud = old_brep.Surfaces[i].UserData.Find(typeof(UserData.UserDataSurface)) as UserData.UserDataSurface;
                    var udnew = new_brep.Surfaces[i].UserData.Find(typeof(UserData.UserDataSurface)) as UserData.UserDataSurface;
                    if (udnew != null) continue;
                    if (ud != null)
                    {
                        new_brep.Surfaces[i].UserData.Add(ud);
                    }
                }
            }
        }

    }
}
