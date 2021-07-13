using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppi5.Models
{
    public class PrescriptionMedicament
    {
        public int IdMedicament { get; set; }
        public int IdPrescripion { get; set; }
        public int Dose { get; set; }
        public string Details { get; set; }
        public virtual Medicament IdMedicamentNavigation { get; set; }
        public virtual Prescription IdPrescripionNavigation { get; set; }
    }
}
