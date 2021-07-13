using System.Collections.Generic;

namespace WebAppi5.Models.DTO.Responses
{
    public class PrescriptionResponseDto
    {
        public string Date { get; set; }
        public string DueDate { get; set; }
        public PatientResponseDto Patient { get; set; }
        public DoctorResponseDto Doctor { get; set; }
        public IEnumerable<MedicamentResponseDto> Medicaments { get; set; }
    }
}
