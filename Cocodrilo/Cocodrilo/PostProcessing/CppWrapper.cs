
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Rhino;
using Rhino.Geometry;
using Cocodrilo.UserData;

class CppWrapper
{

    [DllImport(@"Z:\TeDa\TeDa\Cocodrilo\x64\Debug\CppWrapper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern int CageEdit_cpp(Guid[] guid_list, int n_guids, string directory, int p_u, int p_v, int p_w, int cp_u, int cp_v, int cp_w, double scaling);


    public static int CageEdit( List<Guid> guid_list, string directory, int p_u, int p_v, int p_w, int cp_u, int cp_v, int cp_w, double scaling)
    {  
        var guid_array = guid_list.ToArray();
        var n_guids = guid_list.Count;
        int a = CageEdit_cpp(guid_array, n_guids, directory, p_u, p_v, p_w, cp_u, cp_v, cp_w, scaling); // directory);

        
        List<Guid> id_ = new List<Guid>();
        foreach (var objc in RhinoDoc.ActiveDoc.Objects.GetObjectList(Rhino.DocObjects.ObjectType.MorphControl))
        { 
            RhinoDoc.ActiveDoc.Objects.GripUpdate(objc, true);
            id_.Add(objc.Id);
            objc.GripsOn = false;
        }

        foreach (var id in id_)
        {
            RhinoDoc.ActiveDoc.Objects.Delete(id, true);
        }
        RhinoDoc.ActiveDoc.Views.Redraw();

        return 5;
    }
};