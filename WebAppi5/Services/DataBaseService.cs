using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAppi5.Models;
using WebAppi5.Models.DTO.Requests;
using WebAppi5.Models.DTO.Responses;

namespace WebAppi5.Services
{
    public class DataBaseService : IDataBaseService
    {
        private readonly MyContext _myContext;

        public DataBaseService(MyContext context)
        {
            _myContext = context;
        }

        public async Task<DoctorResponseDto> GetDoctor(int IdDoctor)
        {
            var doctor = await _myContext.Doctors.Select(x => new DoctorResponseDto
            {
                IdDoctor = x.IdDoctor,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email
            }).Where(e => e.IdDoctor == IdDoctor).FirstAsync();

            return doctor;
        }

        public async Task<HttpStatusCodeResult> AddDoctor(DoctorRequestDto doctor)
        {
            var addDoctor = await _myContext.Doctors.AddAsync(new Doctor
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email,
            });
            await _myContext.SaveChangesAsync();
            return new HttpStatusCodeResult(200, "Dodano nowego Doctora");
        }

        public async Task<HttpStatusCodeResult> UpdateDoctor(DoctorRequestDto doctor, int IdDoctor)
        {
            var doctorExist = await _myContext.Doctors.Where(x => x.IdDoctor == IdDoctor).CountAsync();
            if (doctorExist > 0)
            {
                var d = await _myContext.Doctors.Where(x => x.IdDoctor == IdDoctor).FirstAsync();
                _myContext.Doctors.Remove(d);
                await _myContext.Doctors.AddAsync(new Doctor
                {
                    IdDoctor = IdDoctor,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Email = doctor.Email,
                });
                await _myContext.SaveChangesAsync();
                return new HttpStatusCodeResult(200, "Zaaktualizowano dane Doktora");
            }
            else
            {
                return new HttpStatusCodeResult(404, "Brak Doctora o podanym id w bazie");
            }
        }

        public async Task<HttpStatusCodeResult> DeleteDoctor(int IdDoctor)
        {
            var doctorExist = await _myContext.Doctors.Where(x => x.IdDoctor == IdDoctor).CountAsync();
            if (doctorExist > 0)
            {
                var doctor = await _myContext.Doctors.Where(x => x.IdDoctor == IdDoctor).FirstAsync();
                var prescriptionRekords = await _myContext.Prescriptions.Where(x => x.IdDoctor == IdDoctor).ToListAsync();

                foreach(var prescription in prescriptionRekords)
                {
                    var prescriptionMedicamentRekords = await _myContext.PrescriptionMediciments.Where(x => x.IdPrescripion == prescription.IdPrescription).ToListAsync();
                    foreach (var prescriptionM in prescriptionMedicamentRekords)
                    {
                        _myContext.PrescriptionMediciments.Remove(prescriptionM);
                    }
                    _myContext.Prescriptions.Remove(prescription);
                }
                _myContext.Doctors.Remove(doctor);
                await _myContext.SaveChangesAsync();
                return new HttpStatusCodeResult(200, "Usunieto Doktora");
            }
            else
            {
                return new HttpStatusCodeResult(404, "Brak Doctora o podanym id w bazie");
            }
        }

        public async Task<PrescriptionResponseDto> GetPrescription(int IdPrescription)
        {
            var prescription = await _myContext.Prescriptions.Where(x => x.IdPrescription == IdPrescription).Select(x => new PrescriptionResponseDto
            {
                Date = x.Date,
                DueDate = x.DueDate,
                Patient = new PatientResponseDto
                {
                    FirstName = x.IdPatientNavigation.FirstName,
                    LastName = x.IdPatientNavigation.LastName,
                    Birthdate = x.IdPatientNavigation.Birthdate
                },
                Doctor = new DoctorResponseDto
                {
                    IdDoctor = x.IdDoctorNavigation.IdDoctor,
                    FirstName = x.IdDoctorNavigation.FirstName,
                    LastName = x.IdDoctorNavigation.LastName,
                    Email = x.IdDoctorNavigation.Email
                },
                Medicaments = x.PrescriptionMediciments.Select(x => new MedicamentResponseDto {
                    Name = x.IdMedicamentNavigation.Name,
                    Description = x.IdMedicamentNavigation.Description,
                    Type = x.IdMedicamentNavigation.Type
                }).ToList()
            }).FirstAsync();

            return prescription;
        }

        public async Task<HttpStatusCodeResult> IsDoctorExists(int IdDoctor)
        {
            var doctorExist = await _myContext.Doctors.Where(x => x.IdDoctor == IdDoctor).CountAsync();
            if(doctorExist > 0)
            {
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(404, "Brak Doctora o podanym id w bazie");
            }
        }

        public async Task<HttpStatusCodeResult> IsPrescriptionExists(int IdPrescription)
        {
            var prescriptionExist = await _myContext.Prescriptions.Where(x => x.IdPrescription == IdPrescription).CountAsync();
            if (prescriptionExist > 0)
            {
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(404, "Brak Recepty o podanym id w bazie");
            }
        }
    }
}
