using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class OrderDetailsConfiguration
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.ToTable("order_details");

            // Clave primaria
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .IsRequired()
                .HasColumnName("id");

            builder.Property(a => a.IdOrder)
                .IsRequired()
                .HasColumnName("id_order");

            builder.HasOne(dd => dd.ServiceOrder)
                   .WithMany(so => so.OrderDetails)
                   .HasForeignKey(dd => dd.IdOrder);

            builder.Property(a => a.IdReplacement)
                .IsRequired()
                .HasColumnName("id_replacement");

            builder.HasOne(dd => dd.Replacement)
                   .WithMany(so => so.OrderDetails)
                   .HasForeignKey(dd => dd.IdReplacement);

            builder.Property(a => a.Quantity)
                .IsRequired()
                .HasColumnName("quantity");

            builder.Property(i => i.TotalCost)
                .HasColumnType("decimal")
                .HasColumnName("total_cost");

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