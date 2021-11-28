using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.DEM
{
    public struct SolutionVariable<T>
    {
        public SortedDictionary<int, T> solution_data;
    }

    public struct Sphere
    {
        public Sphere(int id, int material_id, double radius)
        {
            this.id = id;
            this.material_id = material_id;
            this.radius = radius;
        }
        public int id;
        public int material_id;
        public double radius;
    }

    public struct SolutionStep
    {
        public SolutionStep(SortedDictionary<int, Vector3d> coordinates, SortedDictionary<int, Sphere> elements)
        {
            this.coordinates = coordinates;
            this.elements = elements;
        }
        public SortedDictionary<int, Vector3d> coordinates;
        public SortedDictionary<int, Sphere> elements;
    }
    class AnalysisDEM
    {
        SortedDictionary<double, SolutionStep> solution_steps;
        public AnalysisDEM(string solution_file)
        {
            solution_steps = new SortedDictionary<double, SolutionStep>();

            ReadSolutions(solution_file);
        }

        public void PrintAll()
        {
            foreach (var solution_step in solution_steps)
            {
                PrintResults(solution_step.Key);
            }
        }

        public void PrintResults(double solution_step)
        {
            var solution = solution_steps[solution_step];
            foreach(var element in solution.elements)
            {
                Vector3d coords = solution.coordinates[element.Value.id];
                Rhino.Geometry.Point3d point = new Rhino.Geometry.Point3d(coords);
                double radius = element.Value.radius;
                var sphere = RhinoDoc.ActiveDoc.Objects.AddSphere(new Rhino.Geometry.Sphere(point, radius));
                RhinoDoc.ActiveDoc.Views.Redraw();
                Rhino.Display.RhinoView View = RhinoDoc.ActiveDoc.Views.ActiveView;
                var bitmap = View.CaptureToBitmap();
                bitmap.Save(@"C:\Users\TobiasTeschemacher\Documents\FiguresRhino\bitmapfile" + solution_step*1000 +  ".bmp");
                System.Threading.Thread.Sleep(100);
                RhinoDoc.ActiveDoc.Objects.Delete(sphere, true);
            }
        }

        private void ReadSolutions(string solution_file)
        {
            FileStream file_stream;
            StreamReader file_reader;

            //create file stream object
            using (file_stream = new FileStream(solution_file, FileMode.Open, FileAccess.Read))
            using (file_reader = new StreamReader(file_stream))
            {
                int index = solution_file.LastIndexOf('\\');
                string path = solution_file.Substring(0, index + 1);

                string content = file_reader.ReadLine();
                if (content == "Multiple")
                    while (!file_reader.EndOfStream)
                    {
                        content = file_reader.ReadLine();
                        int index_solution_step = content.LastIndexOf('_');
                        string solution_step_post = content.Substring(index_solution_step + 1);
                        int index_solution_step_post = solution_step_post.IndexOf(".post");
                        double solution_step = double.Parse(solution_step_post.Substring(0, index_solution_step_post));
                        RhinoApp.WriteLine("Solution step: " + solution_step);

                        if (content.Contains(".msh"))
                        {
                            solution_steps.Add(solution_step, ReadMesh(path + content));
                        }
                        content = file_reader.ReadLine();
                        if (content.Contains(".res"))
                        {
                            //ReadResults(path + content);
                        }
                    }
            }
        }
        private SolutionStep ReadMesh(string mesh_file)
        {
            FileStream file_stream;
            StreamReader file_reader;

            //create file stream object
            file_stream = new FileStream(mesh_file, FileMode.Open, FileAccess.Read);
            file_reader = new StreamReader(file_stream);

            var coordinates = new SortedDictionary<int, Vector3d>();
            var elements = new SortedDictionary<int, Sphere>();

            string content = file_reader.ReadLine();
            while (!file_reader.EndOfStream)
            {
                if (content == "Coordinates")
                {
                    content = file_reader.ReadLine();
                    while (content != "End Coordinates")
                    {
                        string[] tokens = content.Split();
                        int id = int.Parse(tokens[0]);
                        Vector3d coords = new Vector3d();
                        coords[0] = double.Parse(tokens[1]);
                        coords[1] = double.Parse(tokens[2]);
                        coords[2] = double.Parse(tokens[3]);

                        coordinates.Add(id, coords);

                        content = file_reader.ReadLine();
                    }
                }
                if (content == "Elements")
                {
                    content = file_reader.ReadLine();
                    while (content != "End Elements")
                    {
                        string[] tokens = content.Split();
                        int id = int.Parse(tokens[0]);
                        int node_id = int.Parse(tokens[1]);
                        double radius = double.Parse(tokens[2]);
                        int material_id = int.Parse(tokens[3]);
                        Sphere element = new Sphere(node_id, material_id, radius);

                        elements.Add(id, element);

                        content = file_reader.ReadLine();
                    }
                }
                content = file_reader.ReadLine();
            }

            return new SolutionStep(coordinates, elements);
        }
        private void ReadResults(string result_file)
        {
            FileStream file_stream;
            StreamReader file_reader;

            //create file stream object
            file_stream = new FileStream(result_file, FileMode.Open, FileAccess.Read);
            file_reader = new StreamReader(file_stream);

            string content = file_reader.ReadLine();
            while (!file_reader.EndOfStream)
            {
                string[] tokens = content.Split();
                if(tokens[0] == "Result")
                {
                    string variable_name = tokens[1];
                    double time_step = double.Parse(tokens[3]);
                    if (tokens[4] == "Scalar")
                    {
                        SortedDictionary<int, double> solution_data = new SortedDictionary<int, double>();
                        content = file_reader.ReadLine();
                        if (content == "Values")
                        {
                            while (content != "End Values")
                            {
                                content = file_reader.ReadLine();
                                string[] tokens2 = content.Split();
                                int id = int.Parse(tokens2[0]);
                                double variable = double.Parse(tokens2[1]);
                                solution_data.Add(id, variable);
                            }
                        }

                        SolutionVariable<double> solution = new SolutionVariable<double>();
                        solution.solution_data = solution_data;
                    }
                    else if (tokens[4] == "Vector")
                    {
                        content = file_reader.ReadLine();
                        if (content == "Values")
                        {
                            while (content != "End Values")
                            {
                                content = file_reader.ReadLine();
                                string[] tokens2 = content.Split();
                                int id = int.Parse(tokens2[0]);
                                Vector3d vector_variable = new Vector3d();
                                vector_variable[0] = double.Parse(tokens2[1]);
                                vector_variable[1] = double.Parse(tokens2[2]);
                                vector_variable[2] = double.Parse(tokens2[3]);
                            }
                        }
                    }
                }
            }
        }
    }
}
