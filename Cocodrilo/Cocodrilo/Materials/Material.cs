using System.Collections.Generic;

namespace Cocodrilo.Materials
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Material()
        {
        }
        protected Material(string Name)
        {
            this.Id = -1;
            this.Name = Name;
        }

        public virtual void SetId(int Id)
        {
            this.Id = Id;
        }
        public virtual int GetLastId() => Id;

        public virtual Dictionary<string, object> GetKratosVariables()
            => new Dictionary<string, object> { };

        public virtual Dictionary<string, object> GetKratosConstitutiveLaw()
            => new Dictionary<string, object> { };
        public virtual bool HasKratosSubProperties()
            => false;
        public virtual List<Dictionary<string, object>> GetKratosSubProperties()
            => new List<Dictionary<string, object>> { };
        public virtual List<string> GetKratosOutputValuesNodes(Cocodrilo.IO.OutputOptions ThisOutputOptions)
            => new List<string> { };
        public virtual List<string> GetKratosOutputValuesIntegrationPoints(Cocodrilo.IO.OutputOptions ThisOutputOptions)
            => new List<string> { };

        public virtual bool Equals(Material comparison)
        {
            return (comparison.Id == Id &&
                    comparison.Name == Name);
        }

    }
}
