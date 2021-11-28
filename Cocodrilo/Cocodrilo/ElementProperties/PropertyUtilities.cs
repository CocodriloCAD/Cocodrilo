using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.FileIO;

namespace Cocodrilo.ElementProperties
{
    public struct TimeInterval : IEquatable<TimeInterval>
    {
        string mStartTime { get; set; }
        string mEndTime { get; set; }

        public TimeInterval(string StartTime = "0.0", string EndTime = "End")
        {
            mStartTime = StartTime;
            mEndTime = EndTime;
        }

        public object[] GetTimeIntervalVector()
        {
            var interval = new object[] { mStartTime, mEndTime };
            if (mStartTime == null)
                interval[0] = 0.0;
            else
            {
                double start_time_double;
                if (Double.TryParse(mStartTime, out start_time_double))
                {
                    interval[0] = start_time_double;
                }
            }
            if (mEndTime == null)
                interval[1] = "End";
            else
            {
                double end_time_double;
                if (Double.TryParse(mEndTime, out end_time_double))
                {
                    interval[1] = end_time_double;
                }
            }

            return interval;
        }

        public bool Equals(TimeInterval comp)
        {
            return mStartTime == comp.mStartTime &&
                mEndTime == comp.mEndTime;
        }
    }

    public struct Support : IEquatable<Support>
    {
        public bool mSupportX { get; set; }
        public bool mSupportY { get; set; }
        public bool mSupportZ { get; set; }

        public string mDisplacementX { get; set; }
        public double DisplacementX => TryParseStringToDouble(mDisplacementX);
        public string mDisplacementY { get; set; }
        public double DisplacementY => TryParseStringToDouble(mDisplacementY);
        public string mDisplacementZ { get; set; }
        public double DisplacementZ => TryParseStringToDouble(mDisplacementZ);

        public bool mSupportRotation { get; set; }
        public bool mSupportRotationTorsion { get; set; }

        public bool mIsSupportStrong { get; set; }
        public string mSupportType { get; set; }

        public Support(
            bool SupportX,
            bool SupportY,
            bool SupportZ,
            string DisplacementX = "0.0",
            string DisplacementY = "0.0",
            string DisplacementZ = "0.0",
            bool SupportRotation = false,
            bool SupportRotationTorsion = false,
            bool IsSupportStrong = false,
            string SupportType = "Penalty")
        {
            mSupportX = SupportX;
            mSupportY = SupportY;
            mSupportZ = SupportZ;

            mDisplacementX = DisplacementX;
            mDisplacementY = DisplacementY;
            mDisplacementZ = DisplacementZ;

            mSupportRotation = SupportRotation;
            mSupportRotationTorsion = SupportRotationTorsion;

            mIsSupportStrong = IsSupportStrong;
            mSupportType = SupportType;
        }

        public Support(
            Support CopySupport)
        {
            mSupportX = CopySupport.mSupportX;
            mSupportY = CopySupport.mSupportY;
            mSupportZ = CopySupport.mSupportZ;

            mDisplacementX = CopySupport.mDisplacementX;
            mDisplacementY = CopySupport.mDisplacementY;
            mDisplacementZ = CopySupport.mDisplacementZ;

            mSupportRotation = CopySupport.mSupportRotation;
            mSupportRotationTorsion = CopySupport.mSupportRotationTorsion;

            mIsSupportStrong = CopySupport.mIsSupportStrong;
            mSupportType = CopySupport.mSupportType;
        }

        private double TryParseStringToDouble(string value)
        {
            if (Double.TryParse(value, out double factor))
                return factor;
            return 0;
        }

        public object[] GetSupportVector()
        {
            var directions = new object[] { mDisplacementX, mDisplacementY, mDisplacementZ };

            double displacement_x_double;
            if (Double.TryParse(mDisplacementX, out displacement_x_double))
            {
                directions[0] = displacement_x_double;
            }
            double displacement_y_double;
            if (Double.TryParse(mDisplacementY, out displacement_y_double))
            {
                directions[1] = displacement_y_double;
            }
            double displacement_z_double;
            if (Double.TryParse(mDisplacementZ, out displacement_z_double))
            {
                directions[2] = displacement_z_double;
            }

            if (!mSupportX)
                directions[0] = null;
            if (!mSupportY)
                directions[1] = null;
            if (!mSupportZ)
                directions[2] = null;

            return directions;
    }

        public bool Equals(Support comp)
        {
            return comp.mSupportX == mSupportX &&
                   comp.mSupportY == mSupportY &&
                   comp.mSupportZ == mSupportZ &&
                   comp.mDisplacementX == mDisplacementX &&
                   comp.mDisplacementY == mDisplacementY &&
                   comp.mDisplacementZ == mDisplacementZ &&
                   comp.mSupportRotation == mSupportRotation &&
                   comp.mSupportRotationTorsion == mSupportRotationTorsion &&
                   comp.mIsSupportStrong == mIsSupportStrong &&
                   comp.mSupportType == mSupportType;
        }
    }

    public struct ParameterLocationSurface : IEquatable<ParameterLocationSurface>
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
        public bool Equals(ParameterLocationSurface comp)
        {
            return comp.mU == mU &&
                comp.mV == mV &&
                comp.mU_Normalized == mU_Normalized &&
                comp.mV_Normalized == mV_Normalized;
        }
        public bool IsOnNodes()
        {
            double tolerance = 1e-6;
            return (Math.Abs(mU_Normalized - 1) < tolerance
                || Math.Abs(mU_Normalized - 0) < tolerance
                || Math.Abs(mU_Normalized + 1) < tolerance)
                && (Math.Abs(mV_Normalized - 1) < tolerance
                || Math.Abs(mV_Normalized - 0) < tolerance
                || Math.Abs(mV_Normalized + 1) < tolerance);
        }
        public bool IsPoint()
        {
            return mU_Normalized != -1 && mV_Normalized != -1;
        }
        public bool IsCornerPoint(double tolerance = 0.001)
        {
            return ((Math.Abs(mU_Normalized) < tolerance || Math.Abs(mU_Normalized - 1) < tolerance)
                && (Math.Abs(mV_Normalized) < tolerance || Math.Abs(mV_Normalized - 1) < tolerance));
        }

        public bool IsEdge()
        {
            return ((mU_Normalized != -1 && mV_Normalized == -1) || (mU_Normalized == -1 || mV_Normalized != -1));
        }
        public bool IsSurface()
        {
            return (mU_Normalized == -1 && mV_Normalized == -1);
        }
    }
    public struct ParameterLocationCurve : IEquatable<ParameterLocationCurve>
    {
        public double mU { get; set; }
        public double mU_Normalized { get; set; }

        public ParameterLocationCurve(double U, double U_Normalized)
        {
            mU = U;

            mU_Normalized = U_Normalized;
        }
        public bool Equals(ParameterLocationCurve comp)
        {
            return comp.mU == mU &&
                comp.mU_Normalized == mU_Normalized;
        }
        public bool IsOnNodes()
        {
            double tolerance = 1e-6;
            return (Math.Abs(mU_Normalized - 1) < tolerance
                || Math.Abs(mU_Normalized - 0) < tolerance
                || Math.Abs(mU_Normalized + 1) < tolerance);
        }
        public bool IsPoint()
        {
            return mU_Normalized != -1;
        }
        public bool IsEdge()
        {
            return mU_Normalized == -1;
        }
    }



    public struct Load : IEquatable<Load>
    {
        public string mLoadX { get; set; }
        public double LoadX => TryParseStringToDouble(mLoadX);
        public string mLoadY { get; set; }
        public double LoadY => TryParseStringToDouble(mLoadY);
        public string mLoadZ { get; set; }
        public double LoadZ => TryParseStringToDouble(mLoadZ);
        public string mFactor { get; set; }
        public double Factor => TryParseStringToDouble(mFactor);
        public string mLoadType { get; set; }

        public Load(
            string LoadX,
            string LoadY,
            string LoadZ,
            string Factor, 
            string LoadType)
        {
            mLoadX = LoadX;
            mLoadY = LoadY;
            mLoadZ = LoadZ;

            mFactor = Factor;

            mLoadType = LoadType;
        }

        public Load(
            GeometryType ThisGeometryType,
            double LoadX,
            double LoadY,
            double LoadZ,
            double Factor,
            string LoadType)
        {
            mLoadX = LoadX.ToString();
            mLoadY = LoadY.ToString();
            mLoadZ = LoadZ.ToString();

            mFactor = Factor.ToString();

            mLoadType = LoadType;
        }

        private double TryParseStringToDouble(string value)
        {
            if (Double.TryParse(value, out double factor))
                return factor;
            else return 1;
        }

        public Vector3d GetLoadVector()
        {
            var load_vector = new Vector3d(0.0, 0.0, 0.0);

            if (!Double.TryParse(mFactor, out double factor))
                factor = 1;

            if (Double.TryParse(mLoadX, out double load_x_double))
                load_vector[0] = load_x_double;
            else
                load_vector[0] = 1;

            if (Double.TryParse(mLoadY, out double load_y_double))
                load_vector[1] = load_y_double;
            else
                load_vector[1] = 1;
            
            if (Double.TryParse(mLoadZ, out double load_z_double))
                load_vector[2] = load_z_double;
            else
                load_vector[2] = 1;

            return load_vector * factor;
        }

        public object[] GetLoadProperyVector()
        {
            var load = new object[] { mLoadX, mLoadY, mLoadZ };

            double load_x_double;
            if (Double.TryParse(mLoadX, out load_x_double))
            {
                load[0] = load_x_double;
            }
            double load_y_double;
            if (Double.TryParse(mLoadY, out load_y_double))
            {
                load[1] = load_y_double;
            }
            double load_z_double;
            if (Double.TryParse(mLoadZ, out load_z_double))
            {
                load[2] = load_z_double;
            }

            return load;
        }


        public bool Equals(Load comp)
        {
            return comp.mLoadX == mLoadX &&
                comp.mLoadY == mLoadY &&
                comp.mLoadZ == mLoadZ &&
                comp.mFactor == mFactor &&
                comp.mLoadType == mLoadType;
        }
    }

    public struct Coupling : IEquatable<Coupling>
    {
        public List<Topology> mTopologies { get; set; }

        public Coupling(int NumberOfTopologies)
        {
            if (NumberOfTopologies != 0)
                RhinoApp.WriteLine("ERROR: NumberOfTopologies != 0");
            mTopologies = new List<Topology>();
        }

        public Coupling(int MasterBrepId, int SlaveBrepId)
        {
            mTopologies = new List<Topology>();
            mTopologies.Add(new Topology(MasterBrepId));
            mTopologies.Add(new Topology(SlaveBrepId));
        }
        public Coupling(int BrepId, int TrimIndex, int NumberOfTopologies)
        {
            if (NumberOfTopologies != 1)
                RhinoApp.WriteLine("ERROR: NumberOfTopologies != 1");
            mTopologies = new List<Topology>();
            mTopologies.Add(new Topology(BrepId, TrimIndex));
        }
        public bool IsCoupledWith(int BrepId)
        {
            foreach (var topology in mTopologies)
            {
                if (topology.mBrepId == BrepId)
                    return true;
            }
            return false;
        }

        public bool TryAddCoupling(int BrepId)
        {
            if (!IsCoupledWith(BrepId))
            {
                mTopologies.Add(new Topology(BrepId));
                return true;
            }
            return false;
        }
        public bool TryAddTrimIndexToBrepId(int BrepId, int TrimIndex)
        {
            for (int i = 0; i< mTopologies.Count; i++)
            {
                if (BrepId == mTopologies[i].mBrepId)
                {
                    var topology = mTopologies[i];
                    topology.mTrimIndex = TrimIndex;
                    mTopologies[i] = topology;
                    return true;
                }
            }
            return false;
        }
        public bool TryAddTrimIndexToBrepId(int BrepId, double x, double y, double z)
        {
            for (int i = 0; i < mTopologies.Count; i++)
            {
                if (BrepId == mTopologies[i].mBrepId)
                {
                    mTopologies[i] = new TopologyLocalPoint(BrepId, x, y, z);
                    return true;
                }
            }
            return false;
        }
        public bool Equals(Coupling comp)
        {
            return mTopologies == comp.mTopologies;
        }
    }

    public class Topology : IEquatable<Topology>
    {
        public int mBrepId { get; set; }
        public int mTrimIndex { get; set; }

        public Topology(int BrepId, int TrimIndex = -1)
        {
            mBrepId = BrepId;
            mTrimIndex = TrimIndex;
        }

        public void SetTrimIndex(int TrimIndex)
        {
            mTrimIndex = TrimIndex;
        }


        public Dictionary<string, object> GetInputDictionary()
        {
            return new Dictionary<string, object>()
                {
                    {"brep_id", mBrepId},
                    {"trim_index", mTrimIndex}
                };
        }

        public bool Equals(Topology comp)
        {
            return mBrepId == comp.mBrepId &&
                   mTrimIndex == comp.mTrimIndex;
        }
    }


    public class TopologyLocalPoint : Topology
    {
        public double m_u { get; set; }
        public double m_v { get; set; }
        public double m_w { get; set; }

        public TopologyLocalPoint(int BrepId, double u, double v, double w)
            : base(BrepId, -1)
        {
            m_u = u;
            m_v = v;
            m_w = w;
        }

        new public Dictionary<string, object> GetInputDictionary()
        {
            return new Dictionary<string, object>()
                {
                    {"brep_id", mBrepId},
                    {"local_coordinates", new double[] { m_u, m_v, m_w}}
                };
        }

        public bool Equals(TopologyLocalPoint comp)
        {
            return mBrepId == comp.mBrepId &&
                m_u == comp.m_u &&
                m_v == comp.m_v &&
                m_w == comp.m_w &&
                mTrimIndex == comp.mTrimIndex;
        }
    }
}
