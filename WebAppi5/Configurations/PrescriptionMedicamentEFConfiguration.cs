using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using WebAppi5.Models;

namespace WebAppi5.Configurations
{
    public class PrescriptionMedicamentEFConfiguration : IEntityTypeConfiguration<PrescriptionMedicament>
    {
        public void Configure(EntityTypeBuilder<PrescriptionMedicament> builder)
        {
            builder.ToTable("Prescription_Medicament");
            builder.HasKey(e => new { e.IdMedicament, e.IdPrescripion }).HasName("PrescriptionMedicament_Medicament_pk_Prescripion_pk");
            builder.Property(e => e.Details).HasMaxLength(100).IsRequired();
            builder.HasOne(e => e.IdMedicamentNavigation)
                   .WithMany(e => e.PrescriptionMediciments)
                   .HasForeignKey(f => f.IdMedicament)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("PrescriptionMedicament_Medicament");
            builder.HasOne(e => e.IdPrescripionNavigation)
                   .WithMany(e => e.PrescriptionMediciments)
                   .HasForeignKey(f => f.IdPrescripion)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("PrescriptionMedicament_Prescription");

            var prescriptionMedicaments = new List<PrescriptionMedicament>();

            prescriptionMedicaments.Add(new PrescriptionMedicament
            {
                IdMedicament = 1,
                IdPrescripion = 1,
                Dose = 2,
                Details = "cos tam"
            });
            prescriptionMedicaments.Add(new PrescriptionMedicament
            {
                IdMedicament = 2,
                IdPrescripion = 2,
                Dose = 4,
                Details = "cos tam"
            });

            builder.HasData(prescriptionMedicaments);
        }
    }
}
