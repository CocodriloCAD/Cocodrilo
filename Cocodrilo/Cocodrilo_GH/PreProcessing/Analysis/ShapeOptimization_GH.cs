using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;



namespace Cocodrilo_GH.PreProcessing.Analysis
{
    public class ShapeOptimization_GH : GH_Component
    {
        public ShapeOptimization_GH()
          : base("ShapeOptimization", "Shape Optimization",
              "Shape Optimization",
              "Cocodrilo", "Analyses")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of Analysis", GH_ParamAccess.item, "LinearStaticAnalysis");
            pManager.AddIntegerParameter("Max Optimization Iterations", "MaxOptimizationIterations", "Max processed Optimization Iterations", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Step Size", "StepSize", "Step size of gradient step", GH_ParamAccess.item, 1);
            pManager.AddGenericParameter("Optimization Geometries", "Optimization", "Geometries which are part of the structural optimization.", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Damping Geometries", "Damping", "Geometries which are part of the damping.", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis", "Analysis", "Shape Optimization", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "";
            if (!DA.GetData(0, ref name)) return;

            int max_optimization_iterations = 0;
            if (!DA.GetData(1, ref max_optimization_iterations)) return;

            double step_size = 0;
            if (!DA.GetData(2, ref step_size)) return;

            if (!DA.GetDataTree(3, out GH_Structure<IGH_Goo> optimization_geometries)) return;
            var optimization_geometries_flat = optimization_geometries.FlattenData();

            if (!DA.GetDataTree(4, out GH_Structure<IGH_Goo> damping_geometries)) return;
            var damping_geometries_flat = damping_geometries.FlattenData();

            if (Name.Contains(" "))
            {
                Name = Name.Replace(" ", "");
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed within Name.");
            }

            List<Cocodrilo.ElementProperties.Property> optimization_geometries_properties = new List<Cocodrilo.ElementProperties.Property>();
            foreach (var obj in optimization_geometries_flat)
            {
                bool success = obj.CastTo(out Cocodrilo_GH.PreProcessing.Geometries.Geometries geoms);
                if (!success) continue;
                geoms.breps.ForEach(item => optimization_geometries_properties.Add(item.Value));
                geoms.curves.ForEach(item => optimization_geometries_properties.Add(item.Value));
                geoms.edges.ForEach(item => optimization_geometries_properties.Add(item.Value));
                geoms.points.ForEach(item => optimization_geometries_properties.Add(item.Value));
            }

            List<Cocodrilo.ElementProperties.PropertySupport> damping_geometries_properties = new List<Cocodrilo.ElementProperties.PropertySupport>();
            foreach (var obj in damping_geometries_flat)
            {
                bool success = obj.CastTo(out Cocodrilo_GH.PreProcessing.Geometries.Geometries geoms);
                if (!success) continue;
                var properties = geoms.breps.Where(item => item.Value is Cocodrilo.ElementProperties.PropertySupport).ToList().Select(item => item.Value).ToList();
                properties.AddRange(geoms.curves.Where(item => item.Value is Cocodrilo.ElementProperties.PropertySupport).ToList().Select(item => item.Value).ToList());
                properties.AddRange(geoms.edges.Where(item => item.Value is Cocodrilo.ElementProperties.PropertySupport).ToList().Select(item => item.Value).ToList());
                properties.AddRange(geoms.points.Where(item => item.Value is Cocodrilo.ElementProperties.PropertySupport).ToList().Select(item => item.Value).ToList());

                foreach (Cocodrilo.ElementProperties.PropertySupport property in properties)
                {
                    bool add_property = true;
                    foreach (var damping_property in damping_geometries_properties)
                    {
                        if (damping_property.Equals(property))
                        {
                            add_property = false;
                            break;
                        }
                    }
                    if (add_property)
                        damping_geometries_properties.Add(property);
                }
            }

            DA.SetData(0, new Cocodrilo.Analyses.AnalysisShapeOptimization(
                name, max_optimization_iterations, step_size, optimization_geometries_properties, damping_geometries_properties));
        }
        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.analysis_opt; }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("B4E61B2C-A0AB-4C8B-9888-FCC4C73FAAA1"); }
        }
    }
}