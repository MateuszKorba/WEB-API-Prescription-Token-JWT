using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppi5.Models
{
    public class Prescription
    {
        public Prescription()
        {
            PrescriptionMediciments = new HashSet<PrescriptionMedicament>();
        }
        public int IdPrescription { get; set; }
        public string Date { get; set; }
        public string DueDate { get; set; }
        public int IdPatient { get; set; }
        public int IdDoctor { get; set; }

        public virtual Patient IdPatientNavigation { get; set; }
        public virtual Doctor IdDoctorNavigation { get; set; }
        public virtual ICollection<PrescriptionMedicament> PrescriptionMediciments { get; set; }
    }
}
