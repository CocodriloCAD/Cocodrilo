using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rhino;
using Rhino.Geometry;
using System.IO;
using System.Web.Script.Serialization;
using Cocodrilo.ElementProperties;
using Cocodrilo.UserData;
using Cocodrilo.Analyses;
using Rhino.ApplicationSettings;
using Rhino.Render.ChangeQueue;

namespace Cocodrilo.IO
{

    using Dict = Dictionary<string, object>;
    using DictList = List<Dictionary<string, object>>;
    using IdDict = Dictionary<int, List<int>>;
    using PropertyIdDict = Dictionary<int, List<BrepToParameterLocations>>;

    public class OutputKratosModuleOptimization
    {
        static public void WriteOptimizationFiles(
            List<int> MaterialIds,
            string ProjectPath)
        {
            //GET GEOMETRY JSON INPUT TEXT
            try
            {
                System.IO.File.WriteAllLines(ProjectPath + "/" + "optimization_materials.json",
                    new List<string> { GetOptimizationMaterials(MaterialIds) });
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine("Optimization materials output not possible.");
                RhinoApp.WriteLine(ex.ToString());
            }

            OutputPythonScripts.WriteKratosOptimization(ProjectPath + "/" + "kratos_main_iga.py", MaterialIds);
        }

        static public string GetOptimizationMaterials(List<int> MaterialIds)
        {
            var property_dict_list = new DictList();
            foreach (var material_id in MaterialIds)
            {
                var property_dict = new Dict
                    {
                        {"model_part_name", "IgaModelPart"},
                        {"properties_id", material_id},
                    };

                var material_dict = new Dict { };
                var variables = new Dict { };
                if (material_id >= 0)
                {
                    var material = CocodriloPlugIn.Instance.GetMaterial(material_id);
                    var material_variables = material.GetKratosVariables();
                    foreach (var material_variable in material_variables)
                        variables.Add(material_variable.Key, material_variable.Value);

                    material_dict.Add("name", material.Name);
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
