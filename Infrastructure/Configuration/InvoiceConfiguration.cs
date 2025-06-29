using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class InvoiceConfiguration
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("invoices");
            // Clave primaria
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .IsRequired()
                .HasColumnName("id");

            builder.Property(e => e.IssueDate)
                .HasColumnName("issue_date")
                .HasColumnType("date")
                .HasDefaultValueSql("CURRENT_DATE")
                .ValueGeneratedOnAdd();

            builder.Property(i => i.LaborTotal)
                .HasColumnType("decimal")
                .HasColumnName("labor_total");

            builder.Property(i => i.ReplacementsTotal)
                .HasColumnType("decimal")
                .HasColumnName("replacements_total");

            builder.Property(i => i.TotalAmount)
                .HasColumnType("decimal")
                .HasColumnName("total_amount");

            builder.Property(i => i.IdServiceOrder)
               .HasColumnName("service_order_id");

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
                
            builder.Property(i => i.IdServiceOrder)
                .HasColumnName("service_order_id");

            builder.HasOne(i => i.ServiceOrders)
                .WithOne(s => s.Invoice)
                .HasForeignKey<Invoice>(i => i.IdServiceOrder);
        } 
    }
}