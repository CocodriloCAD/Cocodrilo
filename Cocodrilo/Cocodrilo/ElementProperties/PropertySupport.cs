using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.ElementProperties
{
    public class PropertySupport : Property
    {
        public Support mSupport { get; set; }
        public TimeInterval mTimeInterval { get; set; }
        public int mKnotLocationU { get; set; }
        public int mKnotLocationV { get; set; }

        public PropertySupport() : base() { }

        public PropertySupport(
            GeometryType ThisGeometryType,
            Support ThisSupport,
            TimeInterval ThisTimeInterval,
            int KnotLocationU = -1,
            int KnotLocationV = -1
            ) : base(ThisGeometryType)
        {
            mSupport = ThisSupport;
            mTimeInterval = ThisTimeInterval;

            mKnotLocationU = KnotLocationU;
            mKnotLocationV = KnotLocationV;
        }

        public override string ToString()
        {
            return "support property";
        }

        public override List<Dictionary<string, object>> GetKratosProcesses()
        {
            var interval = mTimeInterval.GetTimeIntervalVector();
            var supports = mSupport.GetSupportVector();

            if (!mSupport.mIsSupportStrong)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"mesh_id", 0 },
                    {"model_part_name", "IgaModelPart." + GetKratosModelPart() },
                    {"variable_name", "DISPLACEMENT" },
                    {"value", supports },
                    { "interval", interval }
                };

                var list = new List<Dictionary<string, object>>{ new Dictionary<string, object>
                    {
                        { "kratos_module", "KratosMultiphysics"},
                        { "python_module", "assign_vector_variable_to_conditions_process"},
                        { "Parameters", parameters }
                    }
                };

                if (mSupport.mSupportRotation)
                {
                    var parameters_rotation = new Dictionary<string, object>
                    {
                        {"mesh_id", 0 },
                        {"model_part_name", "IgaModelPart." + GetKratosModelPart()  },
                        {"variable_name", "ROTATION" },
                        {"value", supports },
                        { "interval", interval }
                    };

                    list.Add(
                        new Dictionary<string, object>
                        {
                            { "kratos_module", "KratosMultiphysics"},
                            { "python_module", "assign_vector_variable_to_conditions_process"},
                            { "Parameters", parameters_rotation }
                        }
                    );
                }
                
                if (mSupport.mSupportType == "SupportNitscheCondition")
                {
                    var eigen_system_settings = new Dictionary<string, object>
                    {
                        {"solver_type", "feast" }
                    };

                    var parameters_nitsche = new Dictionary<string, object>
                    {
                        {"model_part_name", "IgaModelPart." + GetKratosModelPart() },
                        { "eigen_system_settings", eigen_system_settings }
                    };

                    list.Add(
                        new Dictionary<string, object>
                        {
                            { "kratos_module", "IgaApplication"},
                            { "python_module", "nitsche_stabilization_process"},
                            { "Parameters", parameters_nitsche },
                            { "number_of_conditions", 1 }
                        }
                    );
                }

                return list;
            }
            else
            {
                var parameters = new Dictionary<string, object>
                {
                    {"mesh_id", 0 },
                    {"model_part_name", "IgaModelPart." + GetKratosModelPart() },
                    {"variable_name", "DISPLACEMENT" },
                    {"value", supports },
                    { "interval", interval }
                };

                var list = new List<Dictionary<string, object>>{ new Dictionary<string, object>
                    {
                        { "kratos_module", "KratosMultiphysics"},
                        { "python_module", "assign_vector_variable_process"},
                        { "Parameters", parameters }
                    }
                };

                if (mSupport.mSupportRotation)
                {
                    string model_part_name = "IgaModelPart." + GetKratosModelPart() + "_Rotational";
                    string variable_name = "DISPLACEMENT";

                    if (mSupport.mSupportType == "DirectorInc5pShellSupport")
                    {
                        model_part_name = "IgaModelPart." + GetKratosModelPart();
                        variable_name = "DIRECTORINC";
                    }


                    var parameters_rotation = new Dictionary<string, object>
                    {
                        {"mesh_id", 0 },
                        {"model_part_name", model_part_name },
                        {"variable_name", variable_name },
                        {"value", supports },
                        { "interval", interval }
                    };

                    list.Add(
                        new Dictionary<string, object>
                        {
                            { "kratos_module", "KratosMultiphysics"},
                            { "python_module", "assign_vector_variable_process"},
                            { "Parameters", parameters_rotation }
                        }
                    );
                }
                return list;
            }
        }

        public override List<Dictionary<string, object>> GetKratosPhysic(List<int> BrepIds)
        {
            if (mSupport.mIsSupportStrong)
            {
                Dictionary<string, object> Parameters = (mGeometryType == GeometryType.CurvePoint)
                    ? new Dictionary<string, object> { { "local_parameters", new int[] { mKnotLocationU } } }
                    : new Dictionary<string, object> { { "local_parameters", new int[] { mKnotLocationU, mKnotLocationV } } };

                string geometry_type = (mGeometryType == GeometryType.CurvePoint)
                    ? "GeometryCurveNodes"
                    : "GeometrySurfaceNodes";

                var KratosPhysicList = new List<Dictionary<string, object>> { new Dictionary<string, object>
                {
                    {"brep_ids", BrepIds},
                    {"geometry_type", geometry_type },
                    {"iga_model_part", GetKratosModelPart() },
                    {"parameters", Parameters}
                } };

                if (mSupport.mSupportRotation)
                {
                    string variation_type = "GeometrySurfaceVariationNodes";
                    if (mGeometryType == GeometryType.GeometryCurve)
                        variation_type = "GeometryCurveVariationNodes";
                    KratosPhysicList.Add(new Dictionary<string, object>()
                    {
                        {"brep_ids", BrepIds},
                        {"geometry_type", variation_type},
                        {"iga_model_part", GetKratosModelPart() + "_Rotational" },
                        {"parameters", Parameters}
                    });
                }

                return KratosPhysicList;
            }
            else
            {
                Dictionary<string, object> Parameters = new Dictionary<string, object>
                {
                    { "type", "condition"},
                    { "name", mSupport.mSupportType},
                    { "shape_function_derivatives_order", 2},
                };

                return new List<Dictionary<string, object>> { new Dictionary<string, object>
                {
                    {"brep_ids", BrepIds},
                    {"geometry_type", GeometryTypeString },
                    {"iga_model_part", GetKratosModelPart() },
                    {"parameters", Parameters}
                } };
            }
        }
        public override string GetAdditionalDofs()
        {
            if (mSupport.mSupportType == "SupportLagrangeCondition")
                return "VECTOR_LAGRANGE_MULTIPLIER";
            else
                return "";
        }

        public override string GetAdditionalDofReactions()
        {
            if (mSupport.mSupportType == "SupportLagrangeCondition")
                return "VECTOR_LAGRANGE_MULTIPLIER_REACTION";
            else
                return "";
        }
        public override string GetKratosModelPart()
        {
            return "Support_" + mPropertyId;
        }

        public override Dictionary<string, object> GetKratosVariables()
        {
            if (!mSupport.mIsSupportStrong && mSupport.mSupportType == "SupportPenaltyCondition")
            {
                return new Dictionary<string, object>
                {
                    { "PENALTY_FACTOR", CocodriloPlugIn.Instance.GlobPenaltyFactor + 0.1}
                };
            }
            else
            {
                return new Dictionary<string, object> { };
            }
        }

        public override bool HasKratosMaterial()
        {
            return !mSupport.mIsSupportStrong && mSupport.mSupportType == "SupportPenaltyCondition";
        }

        public override Dictionary<string, object> GetKratosOutputIntegrationDomainProcess(
            Cocodrilo.IO.OutputOptions ThisOutputOptions,
            string AnalysisName,
            string ModelPartName)
        {
            if (ThisOutputOptions.conditions)
            {
                var output_integration_domain_process_parameters = new Dictionary<string, object>
                {
                    { "output_file_name", AnalysisName + "_kratos_support_" + mPropertyId + "_integrationdomain.json" },
                    { "model_part_name", ModelPartName + "." + GetKratosModelPart() },
                    { "output_geometry_elements", false },
                    { "output_geometry_conditions", ThisOutputOptions.conditions }
                };

                return new Dictionary<string, object>
                {
                    { "kratos_module", "IgaApplication"},
                    { "python_module", "output_quadrature_domain_process"},
                    { "Parameters", output_integration_domain_process_parameters}
                };
            }
            else
            {
                return new Dictionary<string, object>();
            }
        }
        public override Dictionary<string, object> GetKratosOutputProcess(
            Cocodrilo.IO.OutputOptions ThisOutputOptions,
            string AnalysisName,
            string ModelPartName)
        {
            if (ThisOutputOptions.conditions)
            {
                var integration_point_results = new List<string> { };
                if (mSupport.mIsSupportStrong)
                    integration_point_results.Add("REACTION");
                string[] nodal_results = new string[] { };

                var output_process_parameters = new Dictionary<string, object>
                {
                    { "nodal_results", nodal_results },
                    { "integration_point_results", integration_point_results},
                    { "output_file_name", AnalysisName + "_kratos_support_" + mPropertyId + ".post.res"},
                    { "model_part_name", ModelPartName + "." + GetKratosModelPart() },
                    { "file_label", "step" },
                    { "output_control_type", "time" },
                    { "output_frequency", CocodriloPlugIn.Instance.OutputOptions.output_frequency }
                };
                return new Dictionary<string, object>
                {
                    { "kratos_module", "IgaApplication"},
                    { "python_module", "iga_output_process"},
                    { "Parameters", output_process_parameters}
                };
            }
            else
            {
                return new Dictionary<string, object>();
            }
        }

        public override bool Equals(Property ThisProperty)
        {
            if (!(ThisProperty is PropertySupport))
                return false;
        
            if (mSupport.mSupportType == "SupportNitscheCondition")
            {
                return false;
            }
            else
            {    
                var support = ThisProperty as PropertySupport;
                return support.mSupport.Equals(mSupport) &&
                    support.mTimeInterval.Equals(mTimeInterval) &&
                    support.mGeometryType == mGeometryType &&
                    support.mKnotLocationU == mKnotLocationU &&
                    support.mKnotLocationV == mKnotLocationV &&
                    support.mMaterialId == mMaterialId &&
                    support.mIsFormFinding == mIsFormFinding;
            }
        }
    }
}
