using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cocodrilo.Analyses
{
    public class AnalysisShapeOptimization : Analysis
    {
        public int mMaxOptimizationIterations { get; set; }
        public double mStepSize { get; set; }

        List<ElementProperties.PropertySupport> mDampingRegions { get; set; }
        List<ElementProperties.Property> mOptimizationParts { get; set; }

        public AnalysisShapeOptimization() { }

        public AnalysisShapeOptimization(
            string Name,
            int MaxOptimizationIterations,
            double StepSize,
            List<ElementProperties.Property> OptimizationParts,
            List<ElementProperties.PropertySupport> DampingRegions
            )
        {
            this.Name = Name;
            mMaxOptimizationIterations = MaxOptimizationIterations;
            mStepSize = StepSize;

            mDampingRegions = DampingRegions;
            mOptimizationParts = OptimizationParts;
        }

        public void WriteOptimizationParameters(
            System.Collections.Generic.List<Dictionary<string, object>> Modelers,
            string ModelPartName,
            string ProjectPath)
        {
            List<Dictionary<string, object>> damping_regions = new List<Dictionary<string, object>>();
            foreach (var damping_region in mDampingRegions)
            {
                damping_regions.Add(damping_region.GetKratosOptimizationDamping());
            }

            Dictionary<string, object> model_settings = new Dictionary<string, object>
            {
                { "domain_size", 3 },
                { "model_part_name", ModelPartName },
                { "model_import_settings", new Dictionary<string, object> { { "input_type", "use_input_model_part" } } },
                { "design_surface_sub_model_part_name", mOptimizationParts[0].GetKratosModelPart() },
                { "damping", new Dictionary<string, object> { { "damping_regions", damping_regions } } },
                { "modelers", Modelers }
            };

            List<Dictionary<string, object>> objectives = new List<Dictionary<string, object>> {
                new Dictionary<string, object>
                {
                    { "identifier", "strain_energy_main" },
                    { "type", "minimization" },
                    { "analyzer", "kratos" },
                    { "response_settings", new Dictionary<string, object> {
                        { "response_type", "strain_energy" },
                        { "primal_settings", "ProjectParameters.json" },
                        { "gradient_mode", "semi_analytic" },
                        { "step_size", 1e-5 }
                    } },
                    { "project_gradient_on_surface_normals", false }
                }
            };

            Dictionary<string, object> design_variables = new Dictionary<string, object>
            {
                { "type", "vertex_morphing" },
                { "filter", new Dictionary<string, object> {
                    { "filter_function_type", "linear" },
                    { "filter_radius", 0.1 },
                    { "max_nodes_in_filter_radius", 10000 },
                    { "matrix_free_filtering", true }
                } }
            };

            Dictionary<string, object> optimization_algorithm = new Dictionary<string, object>
            {
                { "name", "steepest_descent" },
                { "max_iterations", mMaxOptimizationIterations },
                { "relative_tolerance", -1e-0 },
                { "line_search", new Dictionary<string, object> {
                    { "line_search_type", "manual_stepping" },
                    { "normalize_search_direction", true },
                    { "step_size", mStepSize }
                } }
            };

            Dictionary<string, object> optimization_settings = new Dictionary<string, object>
            {
                { "optimization_settings", new Dictionary<string, object>
                    {
                        { "model_settings", model_settings },
                        { "objectives", objectives },
                        { "constraints", new ArrayList() },
                        { "design_variables", design_variables },
                        { "optimization_algorithm", optimization_algorithm },
                        { "output", new Dictionary<string, object> {
                            { "output_format", new Dictionary<string, object> { { "name", "gid" } } } }
                        }
                    }
                }
            };

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string optimization_settings_string = serializer.Serialize((object)optimization_settings);

            System.IO.File.WriteAllLines(ProjectPath + "/optimization_parameters.json",
                new List<string> { optimization_settings_string });
        }
    }
}