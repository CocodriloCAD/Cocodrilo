using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocodrilo.ElementProperties
{
    public class Property : IEquatable<Property>
    {
        public GeometryType mGeometryType { get; set; }
        public string GeometryTypeString
        {
            get => mGeometryType.ToString("G");
        }
        public int mPropertyId { get; set; }
        public int mMaterialId { get; set; }
        public bool mIsFormFinding { get; set; }

        protected Property()
        {
        }

        protected Property(
            GeometryType ThisGeometryType,
            int MaterialId = -1,
            bool IsFormFinding = false)
        {
            mGeometryType = ThisGeometryType;
            mMaterialId = MaterialId;
            mIsFormFinding = IsFormFinding;
        }

        public virtual bool Equals(Property ThisProperty)
        {
            return false;
        }

        public virtual List<Dictionary<string, object>> GetKratosProcesses()
        {
            return new List<Dictionary<string, object>> {};
        }

        public virtual List<Dictionary<string, object>> GetKratosProcesses(List<int> BrepIds)
        {
            return new List<Dictionary<string, object>> { };
        }

        /// <summary>
        /// Returns the physical properties which are necessary for each respective problem
        /// e.g. element formulations, boundary condition formulations, ...
        /// </summary>
        /// <param name="BrepIds">List of the brep ids which are related to this property.</param>
        /// <returns>Physics block</returns>
        public virtual List<Dictionary<string, object>> GetKratosPhysic(List<int> BrepIds)
        {
            return new List<Dictionary<string, object>> { };
        }

        /// <summary>
        /// Returns the name of the respective Kratos Model Part. Default name is "IgaModelPart".
        /// </summary>
        /// <returns></returns>
        public virtual string GetKratosModelPart()
        {
            return "IgaModelPart";
        }

        public virtual Dictionary<string, object> GetKratosVariables()
        {
            return new Dictionary<string, object>();
        }

        public virtual bool RotationDofs()
        {
            return false;
        }

        public virtual string GetAdditionalDofs()
        {
            return "";
        }
        public virtual string GetAdditionalDofReactions()
        {
            return "";
        }
        public virtual int GetMaterialId()
        {
            return mMaterialId;
        }

        public virtual bool HasKratosMaterial()
        {
            return true;
        }
        public virtual List<string> GetKratosOutputValuesNodes(Cocodrilo.IO.OutputOptions ThisOutputOptions)
            => new List<string> { };
        public virtual List<string> GetKratosOutputValuesIntegrationPoints(Cocodrilo.IO.OutputOptions ThisOutputOptions)
            => new List<string> { };
        public virtual Dictionary<string, object> GetKratosOutputIntegrationDomainProcess(
            Cocodrilo.IO.OutputOptions ThisOutputOptions,
            string AnalysisName,
            string ModelPartName)
        {
            return new Dictionary<string, object>();
        }
        public virtual Dictionary<string, object> GetKratosOutputProcess(Cocodrilo.IO.OutputOptions ThisOutputOptions,
            string AnalysisName,
            string ModelPartName)
        {
            return new Dictionary<string, object>();
        }
    }
}
