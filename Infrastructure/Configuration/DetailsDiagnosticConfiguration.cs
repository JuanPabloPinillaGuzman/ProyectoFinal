using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class DetailsDiagnosticConfiguration
    {
        public void Configure(EntityTypeBuilder<DetailsDiagnostic> builder)
        {
            builder.ToTable("details_diagnostics");
            
            builder.HasKey(dd => new { dd.IdServiceOrder, dd.IdDiagnostic });

            // Relaciones

            // Muchos DetailsDiagnostic para un ServiceOrder
            builder.HasOne(dd => dd.ServiceOrder)
                   .WithMany(so => so.DetailsDiagnostics)
                   .HasForeignKey(dd => dd.IdServiceOrder);

            // Muchos UserSpessialization para una Spessialization
            builder.HasOne(dd => dd.Diagnostic)
                   .WithMany(d => d.DetailsDiagnostics)
                   .HasForeignKey(dd => dd.IdDiagnostic);
        } 
    }
}