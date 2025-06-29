using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class ReplacementConfiguration
    {
        public void Configure(EntityTypeBuilder<Replacement> builder)
        {
            builder.ToTable("replacements");

            // Clave primaria
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .IsRequired()
                .HasColumnName("id");

            builder.Property(a => a.Description)
                .IsRequired()
                .HasColumnName("description");

            builder.Property(a => a.Code)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("code");

            builder.Property(a => a.StockQuantity)
                .IsRequired()
                .HasColumnName("stock_quantity");

            builder.Property(a => a.MinimumStock)
                .IsRequired()
                .HasColumnName("minimum_stock");

            builder.Property(a => a.Category)
                .IsRequired()
                .HasColumnName("category");

            builder.Property(i => i.UnitPrice)
                .HasColumnType("decimal")
                .HasColumnName("unit_price");
                
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