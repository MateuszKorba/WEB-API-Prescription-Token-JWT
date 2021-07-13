using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using WebAppi5.Models;

namespace WebAppi5.Configurations
{
    public class PatientEFConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patient");
            builder.HasKey(e => e.IdPatient)
                   .HasName("Patient_pk");
            builder.Property(e => e.IdPatient).UseIdentityColumn();
            builder.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Birthdate).IsRequired();

            var patients = new List<Patient>();
            patients.Add(new Patient
            {
                IdPatient = 1,
                FirstName = "Oliwia",
                LastName = "Domagała",
                Birthdate = "1977-03-05"
            });
            patients.Add(new Patient
            {
                IdPatient = 2,
                FirstName = "Mariusz",
                LastName = "Zieliński",
                Birthdate = "1980-07-20"
            });

            builder.HasData(patients);
        }
    }
}
