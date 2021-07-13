using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppi5.Models;

namespace WebAppi5.Configurations
{
    public class MedicamentEFConfiguration : IEntityTypeConfiguration<Medicament>
    {
        public void Configure(EntityTypeBuilder<Medicament> builder)
        {
            builder.ToTable("Medicament");
            builder.HasKey(e => e.IdMedicament)
                      .HasName("Medicament_pk");
            builder.Property(e => e.IdMedicament).UseIdentityColumn();
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Type).HasMaxLength(100).IsRequired();

            var medicaments = new List<Medicament>();
            medicaments.Add(new Medicament
            {
                IdMedicament = 1,
                Name = "Starazolin",
                Description = "cos tam",
                Type = "lek"
            });
            medicaments.Add(new Medicament
            {
                IdMedicament = 2,
                Name = "Starazolin2",
                Description = "cos tam",
                Type = "lek"
            });

            builder.HasData(medicaments);
        }

    }
}
