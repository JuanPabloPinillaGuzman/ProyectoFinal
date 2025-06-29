using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Configuration
{
     public class AuditoryConfiguration : IEntityTypeConfiguration<Auditory>
     {
          public void Configure(EntityTypeBuilder<Auditory> builder)
          {
               builder.ToTable("auditories");

               // Clave primaria
               builder.HasKey(a => a.Id);
               builder.Property(di => di.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired()
                   .HasColumnName("id");

               // Propiedades

               builder.Property(a => a.EntityName)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("entity_name");

               builder.Property(a => a.Date)
                    .HasColumnName("date")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

               builder.Property(a => a.ChangeType)
                    .IsRequired()
                    .HasColumnName("change_type");

               builder.Property(a => a.ChangedBy)
                    .IsRequired()
                    .HasColumnName("user");

               builder.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("date")
                    .HasDefaultValueSql("CURRENT_DATE")
                    .ValueGeneratedOnAdd();

               builder.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("date")
                    .HasDefaultValueSql("CURRENT_DATE")
                    .ValueGeneratedOnAddOrUpdate();
          }
     }
}