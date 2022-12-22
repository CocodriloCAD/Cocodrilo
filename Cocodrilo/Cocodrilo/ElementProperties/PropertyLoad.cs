using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;

namespace Cocodrilo.ElementProperties
{
    public class PropertyLoad : Property, IEquatable<Property>
    {
        public Load mLoad { get; set; }
        public TimeInterval mTimeInterval { get; set; }

        public PropertyLoad() : base()
        {
        }
        
        public PropertyLoad(
            GeometryType ThisGeometryType,
            Load ThisLoad,
            TimeInterval ThisTimeInterval = new TimeInterval())
            : base(ThisGeometryType)
        {
            mLoad = ThisLoad;
            mTimeInterval = ThisTimeInterval;
        }

        public PropertyLoad(
            PropertyLoad previousPropertyLoad)
            : base(previousPropertyLoad)
        {
            mLoad = previousPropertyLoad.mLoad;
            mTimeInterval = previousPropertyLoad.mTimeInterval;
        }
        public override Property Clone() =>
            new PropertyLoad(this);

        public override string ToString()
        {
            return "load property";
        }

        public override bool Equals(Property ThisProperty)
        {
            if (!(ThisProperty is PropertyLoad))
                return false;
            var load = ThisProperty as PropertyLoad;
            return load.mLoad.Equals(mLoad)
                && load.mTimeInterval.Equals(mTimeInterval)
                && load.mGeometryType == mGeometryType;
        }

        public override List<Dictionary<string, object>> GetKratosProcesses()
        {
            string variable_name = "";
            if (mGeometryType == GeometryType.SurfaceEdge)
                variable_name = "LINE_LOAD";
            else if (mGeometryType == GeometryType.GeometrySurface)
                variable_name = "SURFACE_LOAD";
            else if (mGeometryType == GeometryType.SurfacePoint
                     || mGeometryType == GeometryType.CurvePoint
                     || mGeometryType == GeometryType.Point)
                variable_name = "POINT_LOAD";

            string python_module = "assign_vector_variable_to_conditions_process";

            object value = "";
            if (mLoad.mLoadType == "DEAD")
            {
                if (mGeometryType != GeometryType.SurfacePoint
                     && mGeometryType != GeometryType.CurvePoint)
                {
                    variable_name = "DEAD_LOAD";
                }
                value = mLoad.GetLoadProperyVector();
            }
            else if (mLoad.mLoadType == "PRES" || mLoad.mLoadType == "PRESSURE_LOAD")
            {
                variable_name = "PRESSURE";
                python_module = "assign_scalar_variable_to_conditions_process";
                value = mLoad.mLoadX;
            }
            else if (mLoad.mLoadType == "PRES_FL" || mLoad.mLoadType == "PRESSURE_LOAD_FL")
            {
                variable_name = "PRESSURE_FOLLOWER_LOAD";
                python_module = "assign_scalar_variable_to_conditions_process";
                value = mLoad.mLoadX;
            }
            else if (mLoad.mLoadType == "MOMENT")
            {
                variable_name = "MOMENT";
                value = mLoad.GetLoadProperyVector();
            }
            else if (mLoad.mLoadType == "MOMENT_5P_DIRECTOR")
            {
                variable_name = "MOMENT_LINE_LOAD";
                value = mLoad.GetLoadProperyVector();
            }
            else
            {
                value = mLoad.GetLoadProperyVector();
            }


            var interval = mTimeInterval.GetTimeIntervalVector();
            
            var parameters = new Dictionary<string, object>
            {
                {"mesh_id", 0 },
                {"model_part_name", "IgaModelPart." + GetKratosModelPart() },
                {"variable_name", variable_name },
                {"value", value },
                { "interval", interval }
            };

            return new List<Dictionary<string, object>> {
                new Dictionary<string, object>
                {
                    { "kratos_module", "KratosMultiphysics"},
                    { "python_module", python_module},
                    { "Parameters", parameters }
                }
            };
        }

        public override List<Dictionary<string, object>> GetKratosPhysic(List<int> BrepIds)
        {
            string name = "LoadCondition";

            if (mLoad.mLoadType == "MOMENT_5P_DIRECTOR")
                name = "MomentLoadDirector5pCondition";

            Dictionary<string, object> Parameters = new Dictionary<string, object>
            {
                { "type", "condition" },
                { "name", name },
                { "shape_function_derivatives_order", 2 }
            };

            Dictionary<string, object> property_element = new Dictionary<string, object>
            {
                { "brep_ids", BrepIds},
                { "geometry_type", GeometryTypeString },
                { "iga_model_part", GetKratosModelPart() },
                { "parameters", Parameters }
            };
            return new List<Dictionary<string, object>> { property_element };
        }

        public override string GetKratosModelPart()
        {
            return "Load_" + mPropertyId;
        }

        public override bool HasKratosMaterial()
        {
            return false;
        }
    }
}
