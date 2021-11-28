using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Cocodrilo.UserData;
using System.Collections.Generic;

namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("245C8C8E-7E26-47B4-8CC3-82C4CA5035BD")]
    public class CommandGetProperties : Command
    {
        static CommandGetProperties _instance;
        public CommandGetProperties()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandDeleteAll command.</summary>
        public static CommandGetProperties Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_GetProperties"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            ObjRef[] objrefGeo = null;
            var rcGeo = RhinoGet.GetMultipleObjects("Select Geometry", false, ObjectType.None, out objrefGeo);
            if (rcGeo != Result.Success)
                return Result.Success;

            int default_mode = 0;
            //var rcMode = RhinoGet.GetInteger("Show property (0) or show all type of properties (1):",true,ref default_mode, 0,1);

            if(default_mode == 0)
            {
                int prop_ID = 0;    // ID of the first found property in UserData. 0 = no property found
                List<string> output_txt = new List<string>();
                if (objrefGeo.Length == 0)
                {
                    return Result.Success;
                }
                else if (objrefGeo.Length == 1)
                {
                    var brep = objrefGeo[0].Brep();
                    if (brep != null)
                    {
                        foreach (var srf in brep.Surfaces)
                        {
                            var ud_srf = srf.UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                            if (ud_srf != null)
                            {
                                //prop_ID = ud_srf.myElementDataSurface.element.propertyID;
                                break;
                            }
                            else
                            {
                                RhinoApp.WriteLine("Geometry has no Property");
                            }
                        }
                    }
                    else
                    {
                        var crv = objrefGeo[0].Curve();
                        if (crv != null)
                        {
                            var ud_crv = crv.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                            if (ud_crv != null)
                            {
                                //prop_ID = ud_crv.myElementDataCurve.element.propertyID;
                            }
                            else
                            {
                                RhinoApp.WriteLine("Geometry has no Property");
                            }
                        }
                        else
                        {
                            RhinoApp.WriteLine("Geometry has no Property");
                        }
                    }
                    foreach (var prop in Cocodrilo.CocodriloPlugIn.Instance.Properties)
                    {
                        if (prop_ID == prop.mPropertyId)
                        {
                            //output_txt = prop.getCaratProperty();
                            break;
                        }
                    }
                    foreach (var line in output_txt)
                    {
                        RhinoApp.WriteLine(line);
                    }
                }
                else
                {
                    bool is_same_prop = true;
                    bool contains_srf = false;
                    bool has_geo_wo_ud = false;
                    foreach (var geo in objrefGeo)
                    {
                        var brep = geo.Brep();
                        if (brep != null)
                        {
                            foreach (var srf in brep.Surfaces)
                            {
                                var ud_srf = srf.UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                                if (ud_srf != null)
                                {
                                    contains_srf = true;
                                    //if (prop_ID == 0)
                                    //{
                                    //    prop_ID = ud_srf.myElementDataSurface.element.propertyID;
                                    //}
                                    //else if (prop_ID == ud_srf.myElementDataSurface.element.propertyID)
                                    //{

                                    //}
                                    //else
                                    //{
                                    //    RhinoApp.WriteLine("Properties of selected Geometries are not the same!");
                                    //    is_same_prop = false;
                                    //    break;
                                    //}
                                }
                                else
                                {
                                    has_geo_wo_ud = true;
                                }
                            }
                        }
                        if (!is_same_prop)
                            break;

                        var crv = geo.Curve();
                        if(crv != null)
                        {
                            if (contains_srf)
                            {
                                RhinoApp.WriteLine("Properties of selected Geometries are not the same!");
                                is_same_prop = false;
                                break;
                            }
                            var ud_crv = crv.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                            if (ud_crv != null)
                            {
                                //if (prop_ID == 0)
                                //{
                                //    prop_ID = ud_crv.myElementDataCurve.element.propertyID;
                                //}
                                //else if (prop_ID == ud_crv.myElementDataCurve.element.propertyID)
                                //{

                                //}
                                //else
                                //{
                                //    RhinoApp.WriteLine("Properties of selected Geometries are not the same!");
                                //    is_same_prop = false;
                                //    break;
                                //}
                            }
                            else
                            {
                                has_geo_wo_ud = true;
                            }
                        }

                    }
                    if (is_same_prop && prop_ID!=0)
                    {
                        foreach (var prop in Cocodrilo.CocodriloPlugIn.Instance.Properties)
                        {
                            if (prop_ID == prop.mPropertyId)
                            {
                                //output_txt = prop.getCaratProperty();
                                break;
                            }
                        }
                        if(has_geo_wo_ud)
                        {
                            output_txt.Add("=================================================");
                            output_txt.Add("Warning: There is also Geometry without Property!");
                        }
                    }
                    else if(prop_ID == 0)
                    {
                        output_txt.Add("Geometry has no Property!");
                    }
                    foreach (var line in output_txt)
                    {
                        RhinoApp.WriteLine(line);
                    }
                }
            }
            else if (default_mode == 1)
            {
                RhinoApp.WriteLine("Showing all Properties is not yet implemented! Use mode 0");
            }
            else
            {
                RhinoApp.WriteLine("Integer was not valid!");
            }

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}
