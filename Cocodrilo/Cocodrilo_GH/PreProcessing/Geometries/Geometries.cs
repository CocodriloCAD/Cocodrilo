using System;
using System.Collections.Generic;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.Geometries
{
    public class Geometries// : Grasshopper.Kernel.Types.IGH_Goo
    {
        public List<KeyValuePair<Brep, Cocodrilo.ElementProperties.Property>> breps = new List<KeyValuePair<Brep, Cocodrilo.ElementProperties.Property>>();
        public List<KeyValuePair<Curve, Cocodrilo.ElementProperties.Property>> curves = new List<KeyValuePair<Curve, Cocodrilo.ElementProperties.Property>>();
        public List<KeyValuePair<Curve, Cocodrilo.ElementProperties.Property>> edges = new List<KeyValuePair<Curve, Cocodrilo.ElementProperties.Property>>();
        public List<KeyValuePair<Point, Cocodrilo.ElementProperties.Property>> points = new List<KeyValuePair<Point, Cocodrilo.ElementProperties.Property>>();

        //public Cocodrilo.Refinement.RefinementSurface RefinementSurface = new Cocodrilo.Refinement.RefinementSurface();
        public Geometries()
        {
        }

        //public bool IsValid => true;

        //public string IsValidWhyNot => "";

        //public string TypeName => "Geometries Structure.";

        //public string TypeDescription => "";

        //public bool CastFrom(object source)
        //{
        //    return source.GetType() == typeof(Geometries);
        //}

        //public bool CastTo<T>(out T target)
        //{
        //    target = (T)(object)this;
        //    return true;
        //}

        //public IGH_Goo Duplicate()
        //{
        //    throw new NotImplementedException();
        //}

        //public IGH_GooProxy EmitProxy()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Read(GH_IReader reader)
        //{
        //    return true;
        //}

        //public object ScriptVariable()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Write(GH_IWriter writer)
        //{
        //    return true;
        //}
    }
}