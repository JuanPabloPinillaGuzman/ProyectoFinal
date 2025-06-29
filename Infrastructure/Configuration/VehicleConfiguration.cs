using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class VehicleConfiguration
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("replacements");

            // Clave primaria
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .IsRequired()
                .HasColumnName("id");

            builder.Property(a => a.IdClient)
                .IsRequired()
                .HasColumnName("id_client");

            builder.HasOne(dd => dd.Client)
                .WithMany(so => so.Vehicles)
                .HasForeignKey(dd => dd.IdClient);

            builder.Property(a => a.Brand)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("brand");

            builder.Property(a => a.Model)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("model");

            builder.Property(a => a.Year)
                .IsRequired()
                .HasColumnName("year");

            builder.Property(a => a.SerialNumberVIN)
                .IsRequired()
                .HasColumnName("serial_vin");

            builder.Property(a => a.Mileage)
                .IsRequired()
                .HasColumnName("mileage");


        } 
    }
}