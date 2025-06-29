using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
    {
        public void Configure(EntityTypeBuilder<ServiceOrder> builder)
        {
            builder.ToTable("service_orders");

            // Clave primaria
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .IsRequired()
                .HasColumnName("id");

            builder.Property(a => a.IdUser)
                .IsRequired()
                .HasColumnName("id_user");

            builder.HasOne(dd => dd.User)
                .WithMany(so => so.ServiceOrders)
                .HasForeignKey(dd => dd.IdUser);

            builder.Property(a => a.IdVehicle)
                .IsRequired()
                .HasColumnName("id_vehicle");

            builder.HasOne(dd => dd.Vehicle)
                .WithMany(so => so.ServiceOrders)
                .HasForeignKey(dd => dd.IdVehicle);

            builder.Property(a => a.IdServiceType)
                .IsRequired()
                .HasColumnName("id_service_type");

            builder.HasOne(dd => dd.ServiceType)
                   .WithMany(so => so.ServiceOrders)
                   .HasForeignKey(dd => dd.IdServiceType);

            builder.Property(a => a.IdState)
                .IsRequired()
                .HasColumnName("id_state");

            builder.HasOne(dd => dd.State)
                   .WithMany(so => so.ServiceOrders)
                   .HasForeignKey(dd => dd.IdState);

            builder.Property(c => c.EntryDate)
                .IsRequired()
                .HasColumnType("date")
                .HasColumnName("entry_date");

            builder.Property(c => c.ExitDate)
                .IsRequired()
                .HasColumnType("date")
                .HasColumnName("exit_date");

            builder.Property(a => a.ClientMessage)
                .HasColumnName("client_message");

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