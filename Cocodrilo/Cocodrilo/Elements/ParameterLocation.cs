using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.Elements
{
    public abstract class ParameterLocation : IEquatable<ParameterLocation>
    {
        public abstract bool IsOnNodes();

        public abstract bool IsPoint();
        public abstract bool IsEdge();
        public abstract bool IsSurface();
        public abstract bool Equals(ParameterLocation comp);
    }

    public class ParameterLocationSurface : ParameterLocation
    {
        public double mU { get; set; }
        public double mV { get; set; }
        public double mU_Normalized { get; set; }
        public double mV_Normalized { get; set; }

        public ParameterLocationSurface(double U, double V, double U_Normalized, double V_Normalized)
        {
            mU = U;
            mV = V;

            mU_Normalized = U_Normalized;
            mV_Normalized = V_Normalized;
        }
        public override bool Equals(ParameterLocation comp)
        {
            if (!(comp is ParameterLocationSurface))
                return false;
            var par_sur_comp = comp as ParameterLocationSurface;

            return par_sur_comp.mU == mU &&
                par_sur_comp.mV == mV &&
                par_sur_comp.mU_Normalized == mU_Normalized &&
                par_sur_comp.mV_Normalized == mV_Normalized;
        }
        public override bool IsOnNodes()
        {
            double tolerance = 1e-6;
            return (Math.Abs(mU_Normalized - 1) < tolerance
                || Math.Abs(mU_Normalized - 0) < tolerance
                || Math.Abs(mU_Normalized + 1) < tolerance)
                && (Math.Abs(mV_Normalized - 1) < tolerance
                || Math.Abs(mV_Normalized - 0) < tolerance
                || Math.Abs(mV_Normalized + 1) < tolerance);
        }
        public override bool IsPoint()
        {
            return mU_Normalized != -1 && mV_Normalized != -1;
        }

        public override bool IsEdge()
        {
            return ((mU_Normalized != -1 && mV_Normalized == -1)
                || (mU_Normalized == -1 || mV_Normalized != -1));
        }
        public override bool IsSurface()
        {
            return (mU_Normalized == -1 && mV_Normalized == -1);
        }
    }
    public class ParameterLocationCurve : ParameterLocation
    {
        public double mU { get; set; }
        public double mU_Normalized { get; set; }

        public ParameterLocationCurve(double U, double U_Normalized)
        {
            mU = U;

            mU_Normalized = U_Normalized;
        }
        public override bool Equals(ParameterLocation comp)
        {
            if (!(comp is ParameterLocationCurve))
                return false;
            var par_cur_comp = comp as ParameterLocationCurve;

            return par_cur_comp.mU == mU &&
                par_cur_comp.mU_Normalized == mU_Normalized;
        }
        public override bool IsOnNodes()
        {
            double tolerance = 1e-6;
            return (Math.Abs(mU_Normalized - 1) < tolerance
                || Math.Abs(mU_Normalized - 0) < tolerance
                || Math.Abs(mU_Normalized + 1) < tolerance);
        }
        public override bool IsPoint()
        {
            return mU_Normalized != -1;
        }
        public override bool IsEdge()
        {
            return mU_Normalized == -1;
        }
        public override bool IsSurface()
        {
            return false;
        }
    }

}
