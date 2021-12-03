using Cocodrilo.UserData;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Cocodrilo.IO
{
    using Dict = Dictionary<string, object>;
    using DictList = List<Dictionary<string, object>>;
    using IdDict = Dictionary<int, List<int>>;
    using PropertyIdDict = Dictionary<int, List<BrepToParameterLocations>>;
    public class OutputKratosFEM
    {
        public OutputKratosFEM()
        {
        }
        public void StartAnalysis(List<Mesh> MeshList)
        {
            string project_path = UserDataUtilities.GetProjectPath("DEM");

            StartAnalysis(project_path, MeshList);
        }

        public void StartAnalysis(string project_path, List<Mesh> MeshList)
        {
            try
            {
                PropertyIdDict property_id_dictionary = new PropertyIdDict();

                System.IO.File.WriteAllLines(project_path + "/" + "KlausFEM.mdpa",
                    new List<string> { GetFemMdpaFile(MeshList, ref property_id_dictionary) });
                System.IO.File.WriteAllLines(project_path + "/" + "Materials.json",
                    new List<string> { "" });
                System.IO.File.WriteAllLines(project_path + "/" + "ProjectParamaters.json",
                    new List<string> { "" });
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine("Json output not possible.");
                RhinoApp.WriteLine(ex.ToString());
            }
        }

        private string GetFemMdpaFile(List<Mesh> MeshList, ref PropertyIdDict PropertyIdDictionary)
        {
            string mdpa_file;

            mdpa_file = "Begin ModelPartData\n"
                        + "//  VARIABLE_NAME value\n"
                        + "End ModelPartData\n\n";

            for (int i = 2; i < MeshList.Count + 2; i++)
            {
                mdpa_file += "Begin Properties " + i.ToString() + "\n End Properties \n";
            }
            mdpa_file += "\n\n";

            string node_string = "Begin Nodes\n";
            string element_string = "Begin Conditions RigidFace3D3N\n";
            int id_node_counter = 1;
            int id_element_counter = 1;

            int sub_model_part_counter = 1;

            string sub_model_part_string = "";

            for (int m = 0; m < MeshList.Count; m++)
            {

                sub_model_part_string += "Begin SubModelPart " + sub_model_part_counter + " // GUI DEM-FEM-Wall - DEM-FEM-Wall - group identifier: Parts_membran_oben\n"
                    + "  Begin SubModelPartData // DEM-FEM-Wall. Group name: Parts_membran_oben\n"
                    + "    IS_GHOST false\n"
                    + "    IDENTIFIER Parts_membran_oben\n"
                    + "    FORCE_INTEGRATION_GROUP 0\n"
                    + "  End SubModelPartData\n"
                    + "  Begin SubModelPartNodes\n";

                var mesh = MeshList[m];

                for (int i = 0; i < mesh.Vertices.Count; i++)
                {
                    node_string += "    " + (id_node_counter + i).ToString() + " " + mesh.Vertices[i].X + " " + mesh.Vertices[i].Y + " " + mesh.Vertices[i].Z + "\n";
                    sub_model_part_string += "     " + (id_node_counter + i).ToString() + "\n";
                }
                sub_model_part_string += "End SubModelPartNodes\n";
                sub_model_part_string += "Begin SubModelPartConditions\n";
                foreach (var face in mesh.Faces)
                {
                    element_string += "    " + id_element_counter + "  " + (m + 2).ToString() + "  " + (face.A + id_node_counter) + "  " + (face.B + id_node_counter) + "  " + (face.C + id_node_counter) + "\n";
                    sub_model_part_string += "     " + id_element_counter.ToString() + "\n";
                    id_element_counter++;
                }

                sub_model_part_string += "End SubModelPartConditions\n";
                sub_model_part_string += "End SubModelPart\n\n";

                id_node_counter += mesh.Vertices.Count;
                sub_model_part_counter++;
            }
            node_string += "End Nodes\n\n";
            element_string += "End Elements\n\n";

            mdpa_file += node_string;
            mdpa_file += element_string;
            mdpa_file += sub_model_part_string;

            return mdpa_file;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ElementConditionDictionary"></param>
        /// <returns></returns>
        public string GetMaterials(PropertyIdDict ElementConditionDictionary)
        {
            var property_dict_list = new DictList();
            foreach (var property_id in ElementConditionDictionary.Keys)
            {
                var this_property = CocodriloPlugIn.Instance.GetProperty(property_id, out bool success);
                if (!success)
                {
                    RhinoApp.WriteLine("InputJSON::GetMaterials: Property with Id: " + property_id + " does not exist.");
                    continue;
                }

                //some properties do not need materials and properties
                if (!this_property.HasKratosMaterial())
                    continue;

                var variables = this_property.GetKratosVariables();

                var property_dict = new Dict
                    {
                        {"model_part_name", "IgaModelPart." + this_property.GetKratosModelPart()},
                        {"properties_id", this_property.mPropertyId},
                    };

                var material_dict = new Dict { };

                int material_id = this_property.GetMaterialId();
                if (material_id >= 0)
                {
                    var material = CocodriloPlugIn.Instance.GetMaterial(material_id);
                    var material_variables = material.GetKratosVariables();
                    foreach (var material_variable in material_variables)
                        variables.Add(material_variable.Key, material_variable.Value);

                    material_dict.Add("name", material.Name);
                    material_dict.Add("material_id", material.Id);
                    material_dict.Add("constitutive_law", material.GetKratosConstitutiveLaw());

                    if (material.HasKratosSubProperties())
                    {
                        property_dict.Add("sub_properties", material.GetKratosSubProperties());
                    }
                }

                material_dict.Add("Variables", variables);
                material_dict.Add("Tables", new Dict { });

                property_dict.Add("Material", material_dict);

                property_dict_list.Add(property_dict);
            }

            var dict = new Dict
            {
                {"properties", property_dict_list }
            };
            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize((object)dict);

            return json;
        }
    }

}
