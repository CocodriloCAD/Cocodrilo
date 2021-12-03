using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;using Rhino.Input.Custom;

namespace Cocodrilo.ElementProperties
{
    public struct CheckProperties : IEquatable<CheckProperties>
    {
        public bool mDISP_X { get; set; }
        public bool mDISP_Y { get; set; }
        public bool mDISP_Z { get; set; }
        public bool mLAGRANGE_MP { get; set; }

        public CheckProperties(
            bool DISP_X, bool DISP_Y, bool DISP_Z, bool LAGRANGE_MP)
        {
            mDISP_X = DISP_X;
            mDISP_Y = DISP_Y;
            mDISP_Z = DISP_Z;
            mLAGRANGE_MP = LAGRANGE_MP;
        }
        

        public bool Equals(CheckProperties comp)
        {
            return comp.mDISP_X == mDISP_X &&
                   comp.mDISP_Y == mDISP_Y &&
                   comp.mDISP_Z == mDISP_Z &&
                   comp.mLAGRANGE_MP == mLAGRANGE_MP;
        }
    }



    public class PropertyCheck : Property
    {
        //private CheckProperties mCheckProperties { get; set; }
        CheckProperties mCheckProperties { get; set; }
        public TimeInterval mTimeInterval { get; set; }
        bool mOnNode { get; set;}

        public PropertyCheck() : base()
        {
        }
        public PropertyCheck(
                GeometryType ThisGeometryType,
                CheckProperties ThisCheckProperties,
                bool OnNode,
                TimeInterval ThisTimeInterval)
            : base(ThisGeometryType)
        {
            mCheckProperties = ThisCheckProperties;
            mTimeInterval = ThisTimeInterval;

            mOnNode = OnNode;
        }

        public override bool Equals(Property OtherProperty)
        {
            var other_property = OtherProperty as PropertyCheck;

            return other_property.mMaterialId == mMaterialId &&
                   other_property.mGeometryType == mGeometryType &&
                   other_property.mCheckProperties.Equals(mCheckProperties) &&
                   other_property.mOnNode == mOnNode;
        }

        public override List<Dictionary<string, object>> GetKratosPhysic(List<IO.BrepToParameterLocations> ThisBrepToParameterLocations)
        {
            var KratosPhysicList = new List<Dictionary<string, object>>();
            foreach (var brep_parameter_location_combination in ThisBrepToParameterLocations)
            {
                foreach (var parameter_location in brep_parameter_location_combination.ParameterLocations)
                {
                    if (parameter_location.IsOnNodes())
                    {
                        Dictionary<string, object> Parameters = new Dictionary<string, object>
                        {
                            { "local_parameters", parameter_location.GetNormalizedParameters() }
                        };

                            string geometry_type = "GeometrySurfaceNodes";
                            if (mGeometryType == GeometryType.GeometryCurve)
                                geometry_type = "GeometryCurveNodes";

                            KratosPhysicList.Add(new Dictionary<string, object>
                        {
                            {"brep_id", brep_parameter_location_combination.BrepId },
                            {"geometry_type", geometry_type },
                            {"iga_model_part", GetKratosModelPart() },
                            {"parameters", Parameters}
                        });
                    }
                    else
                    {
                        string name = "OutputCondition";

                        Dictionary<string, object> Parameters = new Dictionary<string, object>
                        {
                            { "type", "condition"},
                            { "name", name},
                            { "shape_function_derivatives_order", 2}
                        };

                        Dictionary<string, object> property_element = new Dictionary<string, object>
                        {
                            {"brep_ids", brep_parameter_location_combination.BrepId},
                            {"geometry_type", GeometryTypeString },
                            {"iga_model_part", GetKratosModelPart() },
                            {"parameters", Parameters}
                        };
                        KratosPhysicList.Add(property_element);
                    }
                }
            }
            return KratosPhysicList;
        }

        public override List<Dictionary<string, object>> GetKratosProcesses()
        {
            Dictionary<string, object> output_process_parameters = new Dictionary<string, object>
            {
                { "output_file_name", GetKratosModelPart() + ".txt"},
                { "model_part_name", "IgaModelPart." + GetKratosModelPart() },
                { "file_label", "step" },
                { "output_control_type", "time" },
                { "output_frequency", 0.1 }
            };


                List<string> results = new List<string>();
                if (mCheckProperties.mDISP_X && mCheckProperties.mDISP_Y && mCheckProperties.mDISP_Z)
                results.Add( "DISPLACEMENT" );
                else
                {
                    if (mCheckProperties.mDISP_X)
                    results.Add("DISPLACEMENT_X");
                    if (mCheckProperties.mDISP_Y)
                    results.Add("DISPLACEMENT_Y");
                    if (mCheckProperties.mDISP_Z)
                    results.Add("DISPLACEMENT_Z");
                }
                if(mCheckProperties.mLAGRANGE_MP)
                results.Add("VECTOR_LAGRANGE_MULTIPLIER");
            if (mOnNode)
            {
                output_process_parameters.Add("nodal_results", results);
                output_process_parameters.Add("integration_point_results", new ArrayList());
            }
            else
            {
                output_process_parameters.Add("nodal_results", new ArrayList());
                output_process_parameters.Add("integration_point_results", results);
            }


            return new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        {"kratos_module", "IgaApplication"},
                        {"python_module", "iga_output_process"},
                        {"Parameters", output_process_parameters}
                    }
                };
        }

        public override string GetKratosModelPart()
        {
            return "Output_" + mPropertyId;
        }
        public override bool HasKratosMaterial()
        {
            return false;
        }
        public override int GetMaterialId()
        {
            return mMaterialId;
        }
    }
}
