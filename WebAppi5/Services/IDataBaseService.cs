using System.Threading.Tasks;
using System.Web.Mvc;
using WebAppi5.Models;
using WebAppi5.Models.DTO.Requests;
using WebAppi5.Models.DTO.Responses;

namespace WebAppi5.Services
{
    public interface IDataBaseService
    {
        public Task<DoctorResponseDto> GetDoctor(int IdDoctor);
        public Task<HttpStatusCodeResult> AddDoctor(DoctorRequestDto doctor);
        public Task<HttpStatusCodeResult> UpdateDoctor(DoctorRequestDto doctor, int IdDoctor);
        public Task<HttpStatusCodeResult> DeleteDoctor(int IdDoctor);
        public Task<PrescriptionResponseDto> GetPrescription(int IdPrescription);

        //metody sprawdzajace
        public Task<HttpStatusCodeResult> IsDoctorExists(int IdDoctor);
        public Task<HttpStatusCodeResult> IsPrescriptionExists(int IdPrescription);
    }
}
