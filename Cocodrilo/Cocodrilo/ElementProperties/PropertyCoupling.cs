using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.ElementProperties
{
    public class PropertyCoupling : Property, IEquatable<Property>
    {
        public Support mSupport { get; set; }
        public TimeInterval mTimeInterval { get; set; }


        public PropertyCoupling() : base()
        {
        }

        public PropertyCoupling(
            GeometryType ThisGeometryType,
            Support ThisSupport,
            TimeInterval ThisTimeInterval = new TimeInterval()
        ) : base(ThisGeometryType)
        {
            mSupport = ThisSupport;
            mTimeInterval = ThisTimeInterval;
        }

        public override bool Equals(Property ThisProperty)
        {
            if (!(ThisProperty is PropertyCoupling))
                return false;
                
            if (GetCouplingType() == CouplingType.CouplingNitscheCondition)
            {
                return false;
            }
            else
            {
                var coupling = ThisProperty as PropertyCoupling;
                return coupling.mSupport.Equals(mSupport) &&
                   coupling.mTimeInterval.Equals(mTimeInterval) &&
                   coupling.mMaterialId == mMaterialId;
            }
        }

        public override List<Dictionary<string, object>> GetKratosProcesses()
        {
            var interval = mTimeInterval.GetTimeIntervalVector();
            var displacements = mSupport.GetSupportVector();

            var parameters_displacements = new Dictionary<string, object>
            {
                {"mesh_id", 0},
                {"model_part_name", "IgaModelPart." + GetKratosModelPart()},
                {"variable_name", "DISPLACEMENT"},
                {"value", displacements},
                {"interval", interval}
            };

            var processes_list = new List<Dictionary<string, object>>
            {
            };

            if (GetCouplingType() == CouplingType.CouplingPenaltyCondition)
            {
                processes_list.Add(new Dictionary<string, object>
                {
                    { "kratos_module", "IgaApplication"},
                    { "python_module", "assign_vector_variable_and_constraints_to_conditions_process"},
                    { "Parameters", parameters_displacements}
                });
            }

            if (mSupport.mSupportRotation)
            {
                var rotations = new object[] { 0.0, 0.0, 0.0 };
                var parameters_rotations = new Dictionary<string, object>
                {
                    {"mesh_id", 0},
                    {"model_part_name", "IgaModelPart." + GetKratosModelPart()},
                    {"variable_name", "ROTATION"},
                    {"value", rotations},
                    {"interval", interval}
                };
                processes_list.Add(new Dictionary<string, object>
                {
                    { "kratos_module", "IgaApplication"},
                    { "python_module", "assign_vector_variable_and_constraints_to_conditions_process"},
                    { "Parameters", parameters_rotations}
                });
            }

            if (GetCouplingType() == CouplingType.CouplingNitscheCondition)
            {
                var eigen_system_settings = new Dictionary<string, object>
                {
                    {"solver_type", "feast" }
                };

                var parameters_nitsche = new Dictionary<string, object>
                {
                    {"model_part_condition_name", "IgaModelPart." + GetKratosModelPart() },
                    { "eigen_system_settings", eigen_system_settings }
                };

                processes_list.Add(
                    new Dictionary<string, object>
                    {
                        { "kratos_module", "IgaApplication"},
                        { "python_module", "nitsche_stabilization_process"},
                        { "Parameters", parameters_nitsche },
                        { "number_of_conditions", 1 }
                    }
                );
            }

            return processes_list;
        }

        public override List<Dictionary<string, object>> GetKratosPhysic(List<int> BrepIds)
        {
            var variables = new ArrayList();

            string condition_name = GetCouplingType().ToString();

            Dictionary<string, object> Parameters = new Dictionary<string, object>
            {
                {"type", "condition"},
                {"name", condition_name},
                {"shape_function_derivatives_order", 2},
                {"variables", variables}
            };

            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"brep_ids", BrepIds},
                    {"geometry_type", GeometryTypeString},
                    {"iga_model_part", GetKratosModelPart()},
                    {"parameters", Parameters}
                }
            };
        }

        public override Dictionary<string, object> GetKratosVariables()
        {
            if (GetCouplingType() == CouplingType.CouplingPenaltyCondition)
            {
                return new Dictionary<string, object>
                {
                    { "PENALTY_FACTOR", CocodriloPlugIn.Instance.GlobPenaltyFactor + 0.001}
                };
            }
            else
            {
                return new Dictionary<string, object> { };
            }
        }

        public override string GetAdditionalDofs()
        {
            if (GetCouplingType() == CouplingType.CouplingLagrangeCondition)
                return "VECTOR_LAGRANGE_MULTIPLIER";
            else
                return "";
        }

        public override string GetAdditionalDofReactions()
        {
            if (GetCouplingType() == CouplingType.CouplingLagrangeCondition)
                return "VECTOR_LAGRANGE_MULTIPLIER_REACTION";
            else
                return "";
        }

        public override string GetKratosModelPart()
        {
            return "Coupling_" + mPropertyId;
        }

        public CouplingType GetCouplingType()
        {
            return CocodriloPlugIn.Instance.GlobalCouplingMethod;
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
                    { "output_file_name", AnalysisName + "_kratos_coupling_" + mPropertyId + "_integrationdomain.json" },
                    { "model_part_name", ModelPartName + "." + GetKratosModelPart() },
                    { "output_geometry_elements", false },
                    { "output_geometry_conditions", false },
                    { "output_coupling_geometry_conditions", ThisOutputOptions.conditions }
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
                string[] nodal_results = new string[] { };

                var output_process_parameters = new Dictionary<string, object>
                {
                    { "nodal_results", nodal_results },
                    { "integration_point_results", integration_point_results},
                    { "output_file_name", Analysis.Name + "_kratos_coupling_" + mPropertyId + ".post.res"},
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
    }
}
