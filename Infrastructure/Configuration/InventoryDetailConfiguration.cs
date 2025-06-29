using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class InventoryDetailConfiguration
    {
        public void Configure(EntityTypeBuilder<InventoryDetail> builder)
        {
            builder.ToTable("inventory_details");

            // Clave primaria
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .IsRequired()
                .HasColumnName("id");

            builder.Property(a => a.IdOrder)
                .IsRequired()
                .HasColumnName("id_order");

            builder.Property(a => a.IdInventory)
                .IsRequired()
                .HasColumnName("id_inventory");

            builder.Property(a => a.Quantity)
                .IsRequired()
                .HasColumnName("quantity");

            builder.HasOne(dd => dd.ServiceOrder)
                .WithMany(so => so.InventoryDetails)
                .HasForeignKey(dd => dd.IdOrder);

            builder.HasOne(dd => dd.Inventory)
                   .WithMany(so => so.InventoryDetails)
                   .HasForeignKey(dd => dd.IdInventory);

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