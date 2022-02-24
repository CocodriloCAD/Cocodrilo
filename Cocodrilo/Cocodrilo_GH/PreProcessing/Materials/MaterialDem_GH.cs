using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Cocodrilo.Materials;

namespace Cocodrilo_GH.PreProcessing.Materials
{
    public class MaterialDem_GH : GH_Component
    {
        public MaterialDem_GH()
          : base("MaterialDem_GH", "Nickname",
              "Description",
              "Cocodrilo", "DEM")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Material Name", "Name", "Material name", GH_ParamAccess.item, "DemMaterial");
            pManager.AddGenericParameter("Material Relation", "Mat", "Material relations", GH_ParamAccess.list);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Mat", "Material", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "";
            if (!DA.GetData(0, ref name)) return;

            var material = new MaterialDem(name);

            List<MaterialDem> material_relations = new List<MaterialDem>();
            DA.GetDataList(1, material_relations);

            foreach (var material_relation in material_relations)
            {
                material.AddMaterialRelation(material_relation);
            }

            Cocodrilo.CocodriloPlugIn.Instance.AddMaterial(material);

            DA.SetData(0, material);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.dem_spheres_mat; }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("FF53353D-5EB5-4821-BD8E-2592C65AF71B"); }
        }
    }
}