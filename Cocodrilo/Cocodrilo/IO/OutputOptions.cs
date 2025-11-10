namespace Cocodrilo.IO
{
    public class OutputOptions
    {
        /// Integration point informations
        public bool elements { get; set; }
        public bool conditions { get; set; }

        /// Nodal Variables
        public bool displacements { get; set; }

        /// Integration Point Variables
        public bool cauchy_stress { get; set; }
        public bool pk2_stress { get; set; }
        public bool moments { get; set; }
        public bool shear_forces { get; set; }

        /// Material Variables
        public bool damage { get; set; }

        public int precision { get; set; }
        public double output_frequency { get; set; }
        public string output_file_name { get; set; }

        public OutputOptions()
        {
            elements = true;
            conditions = true;

            displacements = true;
            cauchy_stress = true;
            pk2_stress = true;
            moments = true;
            shear_forces = false;

            damage = true;

            output_frequency = 0.1;
            output_file_name = "";
        }
    }
}
