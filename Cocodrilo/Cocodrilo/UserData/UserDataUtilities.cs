using System;
using System.Collections.Generic;
using System.IO;
using Rhino;
using Rhino.Geometry;


namespace Cocodrilo.UserData
{
    public static class UserDataUtilities
    {
        /// <summary>
        /// Obtains the path of the Cocodrilo plugin and creates a new
        /// folder for the current analysis. In this location all
        /// solution files are stored.
        /// </summary>
        /// <returns>Path to the project</returns>
        public static string GetProjectPath(string ProjectName)
        {
            var plugin_path = Rhino.PlugIns.PlugIn.PathFromId(
                new Guid("ce983e9d-72de-4a79-8832-7c374e6e26de"));
            var path_with_slash = plugin_path.Replace("\\", "/");
            var id_of_last_slash = path_with_slash.LastIndexOf("/");
            var path_without_last_slash =  path_with_slash.Substring(0, id_of_last_slash + 1);

            string project_path = path_without_last_slash + ProjectName;
            Directory.CreateDirectory(project_path);
            RhinoApp.WriteLine("Files are stored in: " + project_path);

            return project_path;
        }

        /// <summary>
        /// Obtains the path of the Cocodrilo plugin.
        /// </summary>
        /// <returns>Path to the plugin</returns>
        public static string GetPluginPath()
        {
            var plugin_path = Rhino.PlugIns.PlugIn.PathFromId(
                new Guid("ce983e9d-72de-4a79-8832-7c374e6e26de"));
            var path_with_slash = plugin_path.Replace("\\", "/");
            var id_of_last_slash = path_with_slash.LastIndexOf("/");
            return path_with_slash.Substring(0, id_of_last_slash + 1);
        }

        /// <summary>
        /// This utility adds a list entry to a given index
        /// int the (property_id, List(brep_id)) dictionary.
        /// </summary>
        /// <param name="rPropertyElements">Dictionary which relates
        /// indices belonging to property ids to a list of brep ids.</param>
        /// <param name="Index">First index in dictionary. For property id.</param>
        /// <param name="Entry">Entry which is added to the integer-list
        /// belonging to the index. For brep id, which is added.</param>
        public static void AddEntryToDictionary(
            ref Dictionary<int, List<int>> rPropertyElements,
            int Index,
            int Entry)
        {
            if (rPropertyElements.TryGetValue(Index, out _))
                rPropertyElements[Index].Add(Entry);
            else
            {
                rPropertyElements.Add(Index, new List<int> { Entry });
            }
        }

        public static UserDataSurface GetOrCreateUserDataSurface(
            Surface ThisSurface)
        {
            var ud = ThisSurface.UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
            if (ud == null)
            {
                ud = new UserData.UserDataSurface();
                ThisSurface.UserData.Add(ud);
                RhinoApp.WriteLine("New surface user data added with brep id: " + ud.BrepId);
            }
            return ud;
        }
        public static UserDataCurve GetOrCreateUserDataCurve(
            Curve ThisCurve)
        {
            var ud = ThisCurve.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
            if (ud == null)
            {
                ud = new UserData.UserDataCurve();
                ThisCurve.UserData.Add(ud);
                RhinoApp.WriteLine("New curve user data added with brep id: " + ud.BrepId);
            }
            return ud;
        }
        public static UserDataEdge GetOrCreateUserDataEdge(
            Curve ThisCurve)
        {
            var ud = ThisCurve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;
            if (ud == null)
            {
                ud = new UserData.UserDataEdge();
                ThisCurve.UserData.Add(ud);
                RhinoApp.WriteLine("New edge user data added with brep id: " + ud.BrepId);
            }
            return ud;
        }
        public static UserDataBrep GetOrCreateUserDataBrep(
            Brep ThisBrep)
        {
            var ud = ThisBrep.UserData.Find(typeof(UserDataBrep)) as UserDataBrep;
            if (ud == null)
            {
                ud = new UserData.UserDataBrep();
                ThisBrep.UserData.Add(ud);
                RhinoApp.WriteLine("New brep user data added.");
            }
            return ud;
        }

        public static UserDataPoint GetOrCreateUserDataPoint(
            Point ThisPoint)
        {
            var ud = ThisPoint.UserData.Find(typeof(UserDataPoint)) as UserDataPoint;
            if (ud == null)
            {
                ud = new UserData.UserDataPoint();
                ThisPoint.UserData.Add(ud);
                RhinoApp.WriteLine("New point user data added with brep id: " + ud.BrepId);
            }
            return ud;
        }
    }
}
