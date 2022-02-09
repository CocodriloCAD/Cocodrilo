using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;

namespace Cocodrilo.ElementProperties
{
    public struct ShellProperties : IEquatable<ShellProperties>
    {
        public double mThickness { get; set; }
        public bool mCoupleRotations { get; set; }
        public string mType { get; set; }

        public ShellProperties(
            double Thickness,
            bool CoupleRotations,
            string Type = "Shell3pElement")
        {
            mThickness = Thickness;
            mCoupleRotations = CoupleRotations;
            mType = Type;
        }

        public bool Equals(ShellProperties comp)
        {
            return comp.mThickness == mThickness &&
                comp.mCoupleRotations == mCoupleRotations &&
                comp.mType == mType;
        }
    }

    public class PropertyShell : Property
    {
        /// <summary>
        /// This Id is used to connect all shell formulations with a coupling information.
        /// Needed if discontinuities within a patch can occur.
        /// </summary>
        public PropertyCoupling mPropertyCoupling { get; set; }
        public ShellProperties mShellProperties { get; set; }

        public PropertyShell() : base()
        {
        }

        public PropertyShell(
            int mMaterialId,
            ShellProperties ThisShellProperties) 
            : base(
                GeometryType.GeometrySurface,
                mMaterialId)
        {
            mShellProperties = ThisShellProperties;

            var this_support = new Support(
                false,
                false,
                false,
                SupportRotation:true);

            mPropertyCoupling = new PropertyCoupling(
                GeometryType.SurfaceEdgeSurfaceEdge,
                this_support);

            CocodriloPlugIn.Instance.AddProperty(
                mPropertyCoupling);
        }

        public override string ToString()
        {
            return "shell property";
        }

        public override bool Equals(Property ThisProperty)
        {
            if (!(ThisProperty is PropertyShell))
                return false;
            var shell = ThisProperty as PropertyShell;
            return shell.mMaterialId == mMaterialId &&
                    shell.mShellProperties.Equals(mShellProperties);
        }

        public override List<Dictionary<string, object>> GetKratosPhysic(List<int> BrepIds)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>
            {
                { "type", "element"},
                { "name", mShellProperties.mType },
                { "shape_function_derivatives_order", 3}
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

        public override List<Dictionary<string, object>> GetKratosProcesses(List<int> BrepIds)
        {
            if (mShellProperties.mType == "Shell5pElement")
            {
                Dictionary<string, object> linear_solver_settings = new Dictionary<string, object>
                {
                    {"solver_type", "skyline_lu_factorization"}
                };

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"brep_ids", BrepIds},
                    {"model_part_name", "IgaModelPart." + GetKratosModelPart() },
                    {"linear_solver_settings", linear_solver_settings }
                };

                return new List<Dictionary<string, object>> {
                    new Dictionary<string, object>
                    {
                        { "kratos_module", "IgaApplication"},
                        { "python_module", "set_directors_process"},
                        { "Parameters", parameters }
                    }};
            }
            return new List<Dictionary<string, object>> { };
        }

        public override Dictionary<string, object> GetKratosVariables()
        {
            return new Dictionary<string, object>
            {
                { "THICKNESS", mShellProperties.mThickness}
            };
        }

        public override bool RotationDofs()
        {
            if (mShellProperties.mType == "Shell5pHierarchicElement")
                return true;
            else
                return false;
        }

        public override string GetAdditionalDofs()
        {
            if (mShellProperties.mType == "Shell5pElement")
                return "DIRECTORINC";
            else
                return "";
        }

        public override string GetAdditionalDofReactions()
        {
            if (mShellProperties.mType == "Shell5pElement")
                return "MOMENTDIRECTORINC";
            else
                return "";
        }

        public override string GetKratosModelPart()
        {
            if (mIsFormFinding)
                return "FormFinding_" + mPropertyId;
            return "StructuralAnalysis_" + mPropertyId;
        }

        public override int GetMaterialId()
        {
            return mMaterialId;
        }

        public override List<string> GetKratosOutputValuesIntegrationPoints(
            Cocodrilo.IO.OutputOptions ThisOutputOptions)
        {
            var variable_list = new List<string> { };
            if (ThisOutputOptions.cauchy_stress) {
                variable_list.Add("CAUCHY_STRESS");
            }
            if (ThisOutputOptions.pk2_stress)
            {
                variable_list.Add("PK2_STRESS");
            }
            if (ThisOutputOptions.moments)
            {
                variable_list.Add("INTERNAL_MOMENT_XX");
                variable_list.Add("INTERNAL_MOMENT_YY");
                variable_list.Add("INTERNAL_MOMENT_XY");
            }
            return variable_list;
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
                    { "output_file_name", AnalysisName + "_kratos_shell_" + mPropertyId + "_integrationdomain.json" },
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
            Analyses.Analysis ThisAnalysis,
            string ModelPartName)
        {
            var integration_point_results = new List<string> { };
            if (ThisOutputOptions.cauchy_stress)
                integration_point_results.Add("CAUCHY_STRESS");
            if (ThisOutputOptions.pk2_stress)
                integration_point_results.Add("PK2_STRESS");
            if (ThisOutputOptions.moments)
                integration_point_results.Add("INTERNAL_MOMENT");

            integration_point_results.AddRange(
                CocodriloPlugIn.Instance.GetMaterial(mMaterialId).GetKratosOutputValuesIntegrationPoints(ThisOutputOptions));

            string[] nodal_results = (ThisOutputOptions.displacements)
                ? new string[] { "DISPLACEMENT" }
                : new string[] { };

            var output_process_parameters = new Dictionary<string, object>
            {
                { "nodal_results", nodal_results },
                { "integration_point_results", integration_point_results},
                { "output_file_name", ThisAnalysis.Name + "_kratos_shell_" + mPropertyId + ".post.res"},
                { "model_part_name", ModelPartName + "." + GetKratosModelPart() },
                { "file_label", "step" },
                { "output_control_type", "time" },
                { "output_frequency", CocodriloPlugIn.Instance.OutputOptions.output_frequency }
            };
            if (ThisAnalysis.GetType() == typeof(Analyses.AnalysisFormfinding))
            {
                output_process_parameters.Add("is_formfinding", true);
            }
            return new Dictionary<string, object>
                {
                    { "kratos_module", "IgaApplication"},
                    { "python_module", "iga_output_process"},
                    { "Parameters", output_process_parameters}
                };
        }
    }
}
