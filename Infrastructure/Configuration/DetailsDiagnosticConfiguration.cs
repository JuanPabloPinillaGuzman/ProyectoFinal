using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class DetailsDiagnosticConfiguration : IEntityTypeConfiguration<DetailsDiagnostic>
    {
        public void Configure(EntityTypeBuilder<DetailsDiagnostic> builder)
        {
            builder.ToTable("details_diagnostics");

            // Clave primaria compuesta
            builder.HasKey(dd => new { dd.IdServiceOrder, dd.IdDiagnostic });

            builder.Property(a => a.IdDiagnostic)
                .IsRequired()
                .HasColumnName("id_diagnostic");

            builder.Property(a => a.IdServiceOrder)
                .IsRequired()
                .HasColumnName("id_service_order");

            // Relaciones
            builder.HasOne(dd => dd.ServiceOrder)
                .WithMany(so => so.DetailsDiagnostics)
                .HasForeignKey(dd => dd.IdServiceOrder);

            builder.HasOne(dd => dd.Diagnostic)
                .WithMany(d => d.DetailsDiagnostics)
                .HasForeignKey(dd => dd.IdDiagnostic);
        }
    }
}