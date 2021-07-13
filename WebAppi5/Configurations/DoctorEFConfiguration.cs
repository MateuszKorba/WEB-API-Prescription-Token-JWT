using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using WebAppi5.Models;

namespace WebAppi5.Configurations
{
    public class DoctorEFConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctor");
            builder.HasKey(e => e.IdDoctor)
                   .HasName("Doctor_pk");
            builder.Property(e => e.IdDoctor).UseIdentityColumn();
            builder.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(100).IsRequired();

            var doctors = new List<Doctor>();

            doctors.Add(new Doctor
            {
                IdDoctor = 1,
                FirstName = "Nikodem",
                LastName = "Wiśniewski",
                Email = "nwisniewski@gmail.com"
            });
            doctors.Add(new Doctor
            {
                IdDoctor = 2,
                FirstName = "Jan",
                LastName = "Szymański",
                Email = "jszymanski@gmail.com"
            });

            builder.HasData(doctors);
        }
    }
}
