using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;

namespace Cocodrilo.ElementProperties
{
    public struct MembraneProperties : IEquatable<MembraneProperties>
    {
        public double mThickness { get; set; }
        public double[] mDirection1 { get; set; }
        public double[] mDirection2 { get; set; }
        public double mPrestress1 { get; set; }
        public double mPrestress2 { get; set; }

        public MembraneProperties(
            double Thickness,
            double[] Direction1,
            double[] Direction2,
            double Prestress1,
            double Prestress2)
        {
            mThickness = Thickness;
            mDirection1 = Direction1;
            mDirection2 = Direction2;
            mPrestress1 = Prestress1;
            mPrestress2 = Prestress2;
        }

        public override string ToString()
        {
            return "membrane property";
        }

        public bool Equals(MembraneProperties comp)
        {

            return comp.mThickness == mThickness &&
                comp.mPrestress1 == mPrestress1 &&
                comp.mPrestress2 == mPrestress2;
        }
    }

    public class PropertyMembrane : Property, IEquatable<Property>
    {
        public MembraneProperties mMembraneProperties { get; set; }

        public PropertyMembrane() : base()
        {
        }

        public PropertyMembrane(
            int MaterialId,
            bool IsFormFinding,
            MembraneProperties ThisMembraneProperties) 
            : base(GeometryType.GeometrySurface, MaterialId, IsFormFinding)
        {
            mMembraneProperties = ThisMembraneProperties;
        }
        public PropertyMembrane(
            PropertyMembrane previousPropertyMembrane)
            : base(previousPropertyMembrane)
        {
            mMembraneProperties = previousPropertyMembrane.mMembraneProperties;
        }
        public override Property Clone() =>
            new PropertyMembrane(this);
        public override List<Dictionary<string, object>> GetKratosPhysic(List<int> BrepIds)
        {
            string[] variables = new[] { "INTEGRATION_WEIGHT" };

            Dictionary<string, object> Parameters = new Dictionary<string, object>
            {
                { "type", "element"},
                { "name", "IgaMembraneElement"},
                { "properties_id", mMaterialId},
                { "shape_function_derivatives_order", 2},
                { "variables", variables}
            };

            Dictionary<string, object> property_element = new Dictionary<string, object>
            {
                {"brep_ids", BrepIds},
                {"geometry_type", GeometryTypeString},
                {"iga_model_part", GetKratosModelPart() },
                {"parameters", Parameters}
            };
            return new List<Dictionary<string, object>> { property_element };
        }

        public override Dictionary<string, object> GetKratosVariables()
        {
            return new Dictionary<string, object>
            {
                { "THICKNESS", mMembraneProperties.mThickness},
                { "PRESTRESS", new double[]{
                    mMembraneProperties.mPrestress1,
                    mMembraneProperties.mPrestress2,
                    0.0}}
            };
        }

        public override string GetKratosModelPart()
        {
            if (mIsFormFinding)
                return "FormFinding_" + mPropertyId;
            return "StructuralAnalysis_" + mPropertyId;
        }

        public override Dictionary<string, object> GetKratosOutputIntegrationDomainProcess(
            Cocodrilo.IO.OutputOptions ThisOutputOptions,
            string AnalysisName,
            string ModelPartName)
        {
            if (ThisOutputOptions.elements)
            {
                var output_integration_domain_process_parameters = new Dictionary<string, object>
            {
                { "output_file_name", AnalysisName + "_kratos_membrane_" + mPropertyId + "_integrationdomain.json" },
                { "model_part_name", ModelPartName + "." + GetKratosModelPart() },
                { "output_geometry_elements", ThisOutputOptions.elements },
                { "output_geometry_conditions", false }
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
            var integration_point_results = new List<string> { };
            if (ThisOutputOptions.cauchy_stress)
                integration_point_results.Add("CAUCHY_STRESS");
            if (ThisOutputOptions.pk2_stress)
                integration_point_results.Add("PK2_STRESS");
            string[] nodal_results = (ThisOutputOptions.displacements)
                ? new string[] { "DISPLACEMENT" }
                : new string[] { };

            var output_process_parameters = new Dictionary<string, object>
            {
                { "nodal_results", nodal_results },
                { "integration_point_results", integration_point_results},
                { "output_file_name", Analysis.Name + "_kratos_membrane_" + mPropertyId + ".post.res"},
                { "model_part_name", ModelPartName + "." + GetKratosModelPart() },
                { "file_label", "step" },
                { "output_control_type", "time" },
                { "output_frequency", CocodriloPlugIn.Instance.OutputOptions.output_frequency }
            };

            if (mIsFormFinding)
                output_process_parameters.Add("form_finding", true);
            
            return new Dictionary<string, object>
                {
                    { "kratos_module", "IgaApplication"},
                    { "python_module", "iga_output_process"},
                    { "Parameters", output_process_parameters}
                };
        }

        public override bool Equals(Property ThisProperty)
        {
            if (!(ThisProperty is PropertyMembrane))
                return false;
            var membrane = ThisProperty as PropertyMembrane;
            return membrane.mMaterialId == mMaterialId &&
                membrane.mIsFormFinding == mIsFormFinding &&
                membrane.mMembraneProperties.Equals(mMembraneProperties);
        }
    }
}
