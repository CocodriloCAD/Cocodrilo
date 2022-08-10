using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.ElementProperties
{
    public struct ConnectorProperties : IEquatable<ConnectorProperties>
    {
        public double mStiffnessNormal { get; set; }
        public double mStiffnessTangent1 { get; set; }
        public double mStiffnessTangent2 { get; set; }
        public double mStiffnessMoment1 { get; set; }
        public double mStiffnessMoment2 { get; set; }

        public ConnectorProperties(
            double StiffnessNormal,
            double StiffnessTangent1,
            double StiffnessTangent2,
            double StiffnessMoment1,
            double StiffnessMoment2)
        {
            mStiffnessNormal = StiffnessNormal;
            mStiffnessTangent1 = StiffnessTangent1;
            mStiffnessTangent2 = StiffnessTangent2;
            mStiffnessMoment1 = StiffnessMoment1;
            mStiffnessMoment2 = StiffnessMoment2;
        }

        public bool Equals(ConnectorProperties comp)
        {
            return comp.mStiffnessNormal == mStiffnessNormal &&
                   comp.mStiffnessTangent1 == mStiffnessTangent1 &&
                   comp.mStiffnessTangent2 == mStiffnessTangent2 &&
                   comp.mStiffnessMoment1 == mStiffnessMoment1 &&
                   comp.mStiffnessMoment2 == mStiffnessMoment2;
        }
    }

    public class PropertyConnector : Property
    {
        ConnectorProperties mConnectorProperties;
        public TimeInterval mTimeInterval { get; set; }
        public PropertyConnector(
            GeometryType ThisGeometryType,
            ConnectorProperties ThisConnectorProperties,
            TimeInterval ThisTimeInterval = new TimeInterval())
            : base(ThisGeometryType)
        {
            mConnectorProperties = ThisConnectorProperties;
        }
        public override bool Equals(Property ThisProperty)
        {
            if (!(ThisProperty is PropertyConnector))
                return false;

            var connector = ThisProperty as PropertyConnector;
            return connector.mConnectorProperties.Equals(mConnectorProperties) &&
                connector.mTimeInterval.Equals(mTimeInterval) &&
                connector.mMaterialId == mMaterialId;
        }
        public override List<Dictionary<string, object>> GetKratosPhysic(List<int> BrepIds)
        {
            var variables = new System.Collections.ArrayList();

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
            return new Dictionary<string, object>
            {
                { "PENALTY_FACTOR_NORMAL", mConnectorProperties.mStiffnessNormal},
                { "PENALTY_FACTOR_TANGENT_1", mConnectorProperties.mStiffnessTangent1},
                { "PENALTY_FACTOR_TANGENT_2", mConnectorProperties.mStiffnessTangent2},
                { "PENALTY_FACTOR_MOMENT_1", mConnectorProperties.mStiffnessMoment1},
                { "PENALTY_FACTOR_MOMENT_2", mConnectorProperties.mStiffnessMoment2}
            };
        }


        public override string GetKratosModelPart()
        {
            return "Connector_" + mPropertyId;
        }

        public string GetCouplingType()
        {
            return "ConnectorPenaltyCondition";
        }

        public string[] GetKratosOutputNodalResults()
        {
            return new string[] { };
        }

        public string[] GetKratoIntegrationPointResults()
        {
            return new string[] { "PENALTY_FACTOR_NORMAL" };
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
                    { "output_file_name", AnalysisName + "_kratos_connector_" + mPropertyId + "_integrationdomain.json" },
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
                var output_process_parameters = new Dictionary<string, object>
                {
                    { "nodal_results", GetKratosOutputNodalResults() },
                    { "integration_point_results", GetKratoIntegrationPointResults()},
                    { "output_file_name", Analysis.Name + "_kratos_connector_" + mPropertyId + ".post.res"},
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