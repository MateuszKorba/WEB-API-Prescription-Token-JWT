using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppi5.Configurations;

namespace WebAppi5.Models
{
    public class MyContext : DbContext
    {
        public MyContext(){}

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<Medicament> Medicaments { get; set; }
        public virtual DbSet<PrescriptionMedicament> PrescriptionMediciments { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DoctorEFConfiguration());
            modelBuilder.ApplyConfiguration(new PatientEFConfiguration());
            modelBuilder.ApplyConfiguration(new PrescriptionEFConfiguration());
            modelBuilder.ApplyConfiguration(new MedicamentEFConfiguration());
            modelBuilder.ApplyConfiguration(new PrescriptionMedicamentEFConfiguration());
            modelBuilder.ApplyConfiguration(new UserEFConfiguration());
        }
    }
}
