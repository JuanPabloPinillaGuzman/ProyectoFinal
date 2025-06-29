using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles");
            
            builder.HasKey(dd => new { dd.IdUser, dd.IdRole });

            // Relaciones

            // Muchos DetailsDiagnostic para un ServiceOrder
            builder.HasOne(dd => dd.User)
                   .WithMany(so => so.UserRoles)
                   .HasForeignKey(dd => dd.IdUser);

            // Muchos UserSpessialization para una Spessialization
            builder.HasOne(dd => dd.Role)
                   .WithMany(d => d.UserRoles)
                   .HasForeignKey(dd => dd.IdRole);
        } 
    }
}