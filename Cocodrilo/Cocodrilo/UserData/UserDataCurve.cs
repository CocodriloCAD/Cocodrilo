using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Cocodrilo.Elements;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using Rhino.Geometry;
using Cocodrilo.ElementProperties;
using Cocodrilo.Refinement;

namespace Cocodrilo.UserData
{
    [Guid("EDCDDDB5-051C-4DB8-8B39-AFC21EC6CD08")]
    public class UserDataCurve : UserDataCocodrilo
    {
        RefinementCurve mRefinement;
        public List<double[]> base_vecs { get; set; }

        public UserDataCurve() : base()
        {
            base_vecs = new List<double[]>();
            mRefinement = new RefinementCurve(1, 0, 0);
        }

        public void addBaseVec(double _u, double _nx, double _ny, double _nz)
        {
            double[] base_vec = new double[4] { _u, _nx, _ny, _nz };
            if (base_vecs.Count == 0)
            {
                base_vecs.Add(base_vec);
            }
            else if (base_vecs[base_vecs.Count - 1][0] < _u)
                base_vecs.Add(base_vec);
            else
            {
                for (int i = 0; i < base_vecs.Count - 1; i++)
                {
                    if (base_vecs[i][0] < _u && base_vecs[i + 1][0] >= _u)
                    {
                        base_vecs.Insert(i, base_vec);
                        break;
                    }
                    else if (base_vecs[i + 1][0] == _u)
                    {
                        RhinoApp.WriteLine("U not allowed. Already defined");
                    }
                }
            }
            RhinoApp.WriteLine("New base vector added!");
        }

        public List<double[]> getBaseVecs()
        {
            return base_vecs;
        }
        public override void AddNumericalElement(
            Property ThisProperty,
            bool Overwrite = true,
            int StageId = -1)
        {
            var parameter_location_curve = new ParameterLocationCurve(GeometryType.GeometryCurve, -1, -1);

            GetCurrentElementData(StageId).AddNumericalElement(
                ThisProperty,
                parameter_location_curve,
                Overwrite);
        }
        public override Refinement.Refinement GetRefinement(int StageId = -1)
        {
            return mRefinement;
        }

        public override void ChangeRefinement(
            Refinement.Refinement ThisRefinement,
            int StageId = -1)
        {
            if (ThisRefinement is RefinementCurve)
            {
                mRefinement = ThisRefinement as RefinementCurve;
            }
            else
            {
                Rhino.RhinoApp.WriteLine("WARNING: Trying to assign a refinement to a curve, which is not of type RefinementCurve.");
            }
        }

        protected override bool Read(Rhino.FileIO.BinaryArchiveReader archive)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            var dict = archive.ReadDictionary();

            if (dict.ContainsKey("RefinementCurve"))
            {
                mRefinement = (RefinementCurve)dict["RefinementCurve"];
            }

            return true;
        }
        protected override bool Write(Rhino.FileIO.BinaryArchiveWriter archive)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            var dict = new Rhino.Collections.ArchivableDictionary(1, "Physical");

            string RefinementCurveString = serializer.Serialize((object)mRefinement);
            dict.Set("RefinementCurve", RefinementCurveString);

            archive.WriteDictionary(dict);

            return true;
        }
    }
}
