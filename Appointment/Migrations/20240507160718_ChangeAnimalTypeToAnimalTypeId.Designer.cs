﻿// <auto-generated />
using System;
using Appointment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Appointment.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240507160718_ChangeAnimalTypeToAnimalTypeId")]
    partial class ChangeAnimalTypeToAnimalTypeId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Appointment.Data.Entities.Booking", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int?>("AnimalTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("AppointmentDateTime")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("AppointmentStatus")
                        .HasColumnType("integer");

                    b.Property<string>("BookingNode")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("ClientEmail")
                        .HasColumnType("text");

                    b.Property<int?>("DoctorId")
                        .HasColumnType("integer");

                    b.Property<int?>("PetId")
                        .HasColumnType("integer");

                    b.Property<int?>("VetAidId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
