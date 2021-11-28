using System;
using System.Collections.Generic;

namespace Cocodrilo.ElementProperties
{
    class PropertySurfaceLoadLoop : Property, IEquatable<Property>
    {
        public double loadX { get; set; }
        public double loadY { get; set; }
        public double loadZ { get; set; }
        public double factor { get; set; }
        public string description { get; set; }

        public PropertySurfaceLoadLoop() : base()
        {
        }

        public PropertySurfaceLoadLoop(double loadX = 0.0, double loadY = 0.0, double loadZ = 0.0,
            double factor = 1.0, string description = "")
            : base(GeometryType.CurvePoint)
        {
            this.factor = (loadX == 0.0 && loadY == 0.0 && loadZ == 0.0)
                ? factor
                : Math.Sqrt(loadX*loadX + loadY*loadY + loadZ*loadZ);

            this.loadX = loadX/this.factor;
            this.loadY = loadY/this.factor;
            this.loadZ = loadZ/this.factor;

            this.description = description;
        }
        public override bool Equals(Property ThisProperty)
        {
            var surface_load_loop  = ThisProperty as PropertySurfaceLoadLoop;
            return surface_load_loop.loadX == loadX &&
                    surface_load_loop.loadY == loadY &&
                    surface_load_loop.loadZ == loadZ &&
                    surface_load_loop.factor == factor &&
                    surface_load_loop.description == description;
        }

        public override List<Dictionary<string, object>> GetKratosProcesses()
        {
            //not adapted for loop kratos

            switch (description)
            {
                case "DEAD":
                    var loads = new double[] { loadX, loadY, loadZ };
                    var interval = new object[] { 0.0, "End" };
                    var parameters = new Dictionary<string, object>
                    {
                        {"mesh_id", 0 },
                        {"model_part_name", GetKratosModelPart() },
                        {"variable_name", "SURFACE_LOAD" },
                        {"modulus", factor },
                        {"direction", loads },
                        { "interval", interval }
                    };

                    return new List<Dictionary<string, object>> {new Dictionary<string, object>
                    {
                        { "python_module", "assign_vector_by_direction_to_condition_process"},
                        { "Parameters", parameters }
                    }
                    };

                case "PRES":
                    var interval2 = new object[] { 0.0, "End" };
                    var parameters2 = new Dictionary<string, object>
                    {
                        {"mesh_id", 0 },
                        {"model_part_name", GetKratosModelPart() },
                        {"variable_name", "PRESSURE" },
                        {"value", factor },
                        { "interval", interval2 }
                    };

                    return new List<Dictionary<string, object>> {new Dictionary<string, object>
                    {
                        { "python_module", "assign_scalar_variable_to_conditions_process"},
                        { "Parameters", parameters2 }
                    }
                    };
            }

            return new List<Dictionary<string, object>> { };
        }

        public override string GetKratosModelPart()
        {
            return "Load_" + mPropertyId;
        }
    }
}
