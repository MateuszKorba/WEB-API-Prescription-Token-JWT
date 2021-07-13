using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using WebAppi5.Models;

namespace WebAppi5.Configurations
{
    public class PrescriptionEFConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.ToTable("Prescription");
            builder.HasKey(e => e.IdPrescription)
                   .HasName("Prescription_pk");
            builder.Property(e => e.IdPrescription).UseIdentityColumn();
            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.DueDate).IsRequired();
            builder.HasOne(e => e.IdPatientNavigation) //dla kluczy obcych : strona 1
                   .WithMany(e => e.Prescriptions) // strona wiele
                   .HasForeignKey(f => f.IdPatient)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("Prescription_Patient");
            builder.HasOne(e => e.IdDoctorNavigation)
                   .WithMany(e => e.Prescriptions)
                   .HasForeignKey(f => f.IdDoctor)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("Prescription_Doctor");

            var prescriptions = new List<Prescription>();

            prescriptions.Add(new Prescription
            {
                IdPrescription = 1,
                Date = "2021-03-12",
                DueDate = "2021-03-22",
                IdPatient = 1,
                IdDoctor = 1
            });
            prescriptions.Add(new Prescription
            {
                IdPrescription = 2,
                Date = "2021-04-15",
                DueDate = "2021-04-25",
                IdPatient = 2,
                IdDoctor = 2
            });

            builder.HasData(prescriptions);
        }
    }
}