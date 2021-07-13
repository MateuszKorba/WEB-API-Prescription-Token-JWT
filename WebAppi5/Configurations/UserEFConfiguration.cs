using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppi5.Models;

namespace WebAppi5.Configurations
{
    public class UserEFConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.IdUser)
                   .HasName("User_pk");
            builder.Property(e => e.IdUser).UseIdentityColumn();
            builder.Property(e => e.Login).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Password).HasMaxLength(200).IsRequired();
            builder.Property(e => e.RefreshedToken).HasMaxLength(200);
            builder.Property(e => e.Salt).HasMaxLength(200).IsRequired();
        }
    }
}
