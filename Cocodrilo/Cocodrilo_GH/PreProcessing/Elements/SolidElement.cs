using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Cocodrilo.Materials;
using Cocodrilo.ElementProperties;


namespace Cocodrilo_GH.PreProcessing.Elements
{
    public class SolidElement_GH : GH_Component
    {

        public SolidElement_GH()
          : base("Solid", "Solid", "Solid Element", "Cocodrilo", "Elements")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Bodymesh", "Mesh", "Meshed Geometry of Body-Element", GH_ParamAccess.list);
            pManager.AddGenericParameter("Material", "Mat", "Material of Element", GH_ParamAccess.item);
            pManager.AddNumberParameter("Thickness", "Thickness", "Thickness of shell element", GH_ParamAccess.item, 1.0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometries", "Geo", "Geometries Enhanced with Element Formulations", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> mesh_list = new List<Mesh>();
            DA.GetDataList(0, mesh_list);

            Material material = null;
            if (!DA.GetData(1, ref material)) return;

            double thickness = 0;
            if (DA.GetData(2, ref thickness)) return;

            SolidElementProperties this_properties = new SolidElementProperties(thickness);

            Property this_property = new PropertySolidElement(material.Id, this_properties);

            var geometries = new Cocodrilo_GH.PreProcessing.Geometries.Geometries();
            foreach (var mesh in mesh_list)
            {
                geometries.meshes.Add(new KeyValuePair<Mesh, Property>(mesh, this_property));
            }

            DA.SetData(0, geometries);
        }
        // Enter meaningful Guid!!! 
        public override Guid ComponentGuid
        {
            get { return new Guid("A108FC46-10F7-40F8-8EF5-17C7F6ACC332"); }
        }
    }

}
