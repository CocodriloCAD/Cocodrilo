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
        public bool mOutputReactions { get; set; }

        /// <summary>
        /// mActiveSupport needed for ShapeOptimization and is required
        /// to put damping, but not enforce the structural system.
        /// </summary>
        public bool mActiveSupport { get; set; }

        public PropertySupport() : base() { }

        public PropertySupport(
            GeometryType ThisGeometryType,
            Support ThisSupport,
            TimeInterval ThisTimeInterval,
            bool OutputReactions = false,
            bool ActiveSupport = true
            ) : base(ThisGeometryType)
        {
            mSupport = ThisSupport;
            mTimeInterval = ThisTimeInterval;
            mOutputReactions = OutputReactions;
            mActiveSupport = ActiveSupport;
        }

        public PropertySupport(
            PropertySupport previousPropertySupport)
            : base(previousPropertySupport)
        {
            mSupport = previousPropertySupport.mSupport;
            mTimeInterval = previousPropertySupport.mTimeInterval;
            mOutputReactions = previousPropertySupport.mOutputReactions;
            mActiveSupport = previousPropertySupport.mActiveSupport;
        }
        public override Property Clone() =>
            new PropertySupport(this);

        public override string ToString()
        {
            return "support property";
        }

        public Dictionary<string, object> GetKratosProcessReaction()
        {
            var condition_variable_list = new List<string>();
            if (mSupport.mIsSupportStrong == true)
                condition_variable_list.Add("REACTION");
            else if (mSupport.mSupportType == "SupportPenaltyCondition")
                condition_variable_list.Add("PENALTY_REACTION_FORCE");
            else if (mSupport.mSupportType == "SupportLagrangeCondition")
                condition_variable_list.Add("VECTOR_LAGRANGE_MULTIPLIER");

            var parameters = new Dictionary<string, object>
                {
                    {"output_file_name", mSupport.mSupportType + "_Reactions" },
                    {"model_part_name", "IgaModelPart." + GetKratosModelPart() + "_Output" },
                    {"condition_variable_list", condition_variable_list }
                };

            return new Dictionary<string, object>
                    {
                        { "kratos_module", "IgaApplication"},
                        { "python_module", "get_reaction_process"},
                        { "Parameters", parameters }
                    };
        }
        public Dictionary<string, object> GetKratosOptimizationDamping()
        {
            return new Dictionary<string, object> {
                { "sub_model_part_name", GetKratosModelPart() },
                { "damp_X", mSupport.mSupportX },
                { "damp_Y", mSupport.mSupportY },
                { "damp_Z", mSupport.mSupportZ },
                { "damping_function_type", "cosine" },
                { "damping_radius", 2 }
            };
        }
        public override List<Dictionary<string, object>> GetKratosProcesses()
        {
            var interval = mTimeInterval.GetTimeIntervalVector();
            var supports = mSupport.GetSupportVector();
            var list = new List<Dictionary<string, object>>();

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

                list.Add( new Dictionary<string, object>
                    {
                        { "kratos_module", "KratosMultiphysics"},
                        { "python_module", "assign_vector_variable_to_conditions_process"},
                        { "Parameters", parameters }
                    });

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

                list.Add( new Dictionary<string, object>
                    {
                        { "kratos_module", "KratosMultiphysics"},
                        { "python_module", "assign_vector_variable_process"},
                        { "Parameters", parameters }
                    });

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
            }

            if (mOutputReactions)
                list.Add(GetKratosProcessReaction());

            return list;
        }

        public List<Dictionary<string, object>> GetKratosPhysicReaction(List<IO.BrepToParameterLocations> BrepToParameterLocations)
        {
            var list = new List<Dictionary<string, object>>();
            if (mSupport.mIsSupportStrong)
            {
                foreach (var brep_to_parameter_location in BrepToParameterLocations)
                {
                    foreach (var parameter_location in brep_to_parameter_location.ParameterLocations)
                    {
                        Dictionary<string, object> Parameters = new Dictionary<string, object>
                        {
                            { "type", "condition"},
                            { "quadrature_method", "GRID"},
                            { "number_of_integration_points_per_span", 5},
                            { "name", "OutputCondition"},
                            { "shape_function_derivatives_order", 2},
                            { "local_parameters", parameter_location.GetNormalizedParameters()},
                        };
                        list.Add(new Dictionary<string, object>
                        {
                            {"brep_id", brep_to_parameter_location.BrepId},
                            {"geometry_type", "SurcaceIsoCurve" },
                            {"iga_model_part", GetKratosModelPart() + "_Reaction" },
                            {"parameters", Parameters}
                        });
                    }
                }
            }
            else
            {
                List<int> BrepIds = BrepToParameterLocations.Select(item => item.BrepId).ToList();
                Dictionary<string, object> Parameters = new Dictionary<string, object>
                {
                    { "type", "condition"},
                    { "quadrature_method", "GRID"},
                    { "number_of_integration_points_per_span", 5},
                    { "name", "OutputCondition"},
                    { "shape_function_derivatives_order", 2},
                };
                list.Add(new Dictionary<string, object>
                {
                    {"brep_ids", BrepIds},
                    {"geometry_type", GeometryTypeString },
                    {"iga_model_part", GetKratosModelPart() + "_Reaction" },
                    {"parameters", Parameters}
                });
            }
            return list;
        }
        public override List<Dictionary<string, object>> GetKratosPhysic(List<IO.BrepToParameterLocations> BrepToParameterLocations)
        {
            var list = new List<Dictionary<string, object>>();
            if (mSupport.mIsSupportStrong  && mSupport.mSupportType != "DirectorInc5pShellSupport")
            {
                foreach (var brep_property in BrepToParameterLocations)
                {
                    foreach (var parameter_loaction in brep_property.ParameterLocations)
                    {
                        Dictionary<string, object> Parameters = new Dictionary<string, object> { { "local_parameters", parameter_loaction.GetNormalizedParameters() } };

                        string geometry_type = (parameter_loaction.mGeometryType == GeometryType.CurvePoint)
                            ? "GeometryCurveNodes"
                            : "GeometrySurfaceNodes";

                        list.Add(new Dictionary<string, object>
                            {
                                {"brep_id", brep_property.BrepId },
                                {"geometry_type", geometry_type },
                                {"iga_model_part", GetKratosModelPart() },
                                {"parameters", Parameters}
                            });

                        if (mSupport.mSupportRotation)
                        {
                            string variation_type = "GeometrySurfaceVariationNodes";
                            if (mGeometryType == GeometryType.GeometryCurve)
                                variation_type = "GeometryCurveVariationNodes";
                            list.Add(new Dictionary<string, object>()
                                {
                                    {"brep_id", brep_property.BrepId },
                                    {"geometry_type", variation_type},
                                    {"iga_model_part", GetKratosModelPart() + "_Rotational" },
                                    {"parameters", Parameters}
                                });
                        }
                    }
                }
            }
            else
            {
                List<int> BrepIds = BrepToParameterLocations.Select(item => item.BrepId).ToList();
                Dictionary<string, object> Parameters = new Dictionary<string, object>
                {
                    { "type", "condition"},
                    { "name", mSupport.mSupportType},
                    { "shape_function_derivatives_order", 2},
                };
                list.Add( new Dictionary<string, object>
                {
                    {"brep_ids", BrepIds},
                    {"geometry_type", GeometryTypeString },
                    {"iga_model_part", GetKratosModelPart() },
                    {"parameters", Parameters}
                } );
            }
            if (mOutputReactions)
                list.AddRange(GetKratosPhysicReaction(BrepToParameterLocations));
            return list;
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
            Analyses.Analysis Analysis,
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
                    { "output_file_name", Analysis.Name + "_kratos_support_" + mPropertyId + ".post.res"},
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
                    support.mMaterialId == mMaterialId &&
                    support.mOutputReactions == mOutputReactions &&
                    support.mIsFormFinding == mIsFormFinding;
            }
        }
    }
}
