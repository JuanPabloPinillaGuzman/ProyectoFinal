using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class UserSpecializationConfiguration
    {
        public void Configure(EntityTypeBuilder<UserSpecialization> builder)
        {
            builder.ToTable("user_specializations");

            builder.HasKey(dd => new { dd.IdUser, dd.IdSpecialization });

            // Relaciones

            // Muchos UserSpecialization para un ServiceOrder
            builder.HasOne(dd => dd.User)
                   .WithMany(so => so.UserSpecializations)
                   .HasForeignKey(dd => dd.IdUser);

            // Muchos UserSpecialization para una Specialization
            builder.HasOne(dd => dd.Specialization)
                   .WithMany(d => d.UserSpecializations)
                   .HasForeignKey(dd => dd.IdSpecialization);
        } 
    }
}