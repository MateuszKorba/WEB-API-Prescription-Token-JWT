using System.Collections.Generic;

namespace WebAppi5.Models
{
    public class Medicament
    {
        public Medicament()
        {
            PrescriptionMediciments = new HashSet<PrescriptionMedicament>();
        }
        public int IdMedicament { get; set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public virtual ICollection<PrescriptionMedicament> PrescriptionMediciments { get; set; }

    }
}
