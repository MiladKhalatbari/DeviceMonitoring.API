﻿// <auto-generated />
using System;
using DeviceMonitoring.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeviceMonitoring.Data.Migrations
{
    [DbContext(typeof(DeviceMonitoringContext))]
    [Migration("20230820163301_First-Mig-Build-Database")]
    partial class FirstMigBuildDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DeviceMonitoring.Domain.Entities.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("InstallationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ModelNumber")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Devices");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            InstallationDate = new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsActive = true,
                            Location = "Location A",
                            Manufacturer = "Manufacturer X",
                            ModelNumber = "Model 123",
                            Name = "Device 1"
                        },
                        new
                        {
                            Id = 2,
                            InstallationDate = new DateTime(2021, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsActive = true,
                            Location = "Location B",
                            Manufacturer = "Manufacturer Y",
                            ModelNumber = "Model 456",
                            Name = "Device 2"
                        },
                        new
                        {
                            Id = 3,
                            InstallationDate = new DateTime(2020, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsActive = false,
                            Location = "Location C",
                            Manufacturer = "Manufacturer Z",
                            ModelNumber = "Model 789",
                            Name = "Device 3"
                        },
                        new
                        {
                            Id = 4,
                            InstallationDate = new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsActive = true,
                            Location = "Location D",
                            Manufacturer = "Manufacturer X",
                            ModelNumber = "Model 234",
                            Name = "Device 4"
                        },
                        new
                        {
                            Id = 5,
                            InstallationDate = new DateTime(2023, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsActive = true,
                            Location = "Location E",
                            Manufacturer = "Manufacturer Z",
                            ModelNumber = "Model 567",
                            Name = "Device 5"
                        });
                });

            modelBuilder.Entity("DeviceMonitoring.Domain.Entities.Measurement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("Measurements");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedOn = new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 1,
                            Value = 10.5
                        },
                        new
                        {
                            Id = 2,
                            CreatedOn = new DateTime(2023, 8, 21, 1, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 2,
                            Value = 15.199999999999999
                        },
                        new
                        {
                            Id = 3,
                            CreatedOn = new DateTime(2023, 8, 20, 6, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 3,
                            Value = 9.8000000000000007
                        },
                        new
                        {
                            Id = 4,
                            CreatedOn = new DateTime(2023, 8, 20, 8, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 4,
                            Value = 12.699999999999999
                        },
                        new
                        {
                            Id = 5,
                            CreatedOn = new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 5,
                            Value = 11.300000000000001
                        },
                        new
                        {
                            Id = 6,
                            CreatedOn = new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 1,
                            Value = 14.9
                        },
                        new
                        {
                            Id = 7,
                            CreatedOn = new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 2,
                            Value = 8.5999999999999996
                        },
                        new
                        {
                            Id = 8,
                            CreatedOn = new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 3,
                            Value = 13.1
                        },
                        new
                        {
                            Id = 9,
                            CreatedOn = new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 4,
                            Value = 10.0
                        },
                        new
                        {
                            Id = 10,
                            CreatedOn = new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 5,
                            Value = 16.399999999999999
                        },
                        new
                        {
                            Id = 11,
                            CreatedOn = new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 1,
                            Value = 9.1999999999999993
                        },
                        new
                        {
                            Id = 12,
                            CreatedOn = new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 2,
                            Value = 11.699999999999999
                        },
                        new
                        {
                            Id = 13,
                            CreatedOn = new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 3,
                            Value = 14.1
                        },
                        new
                        {
                            Id = 14,
                            CreatedOn = new DateTime(2023, 8, 20, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 4,
                            Value = 7.7999999999999998
                        },
                        new
                        {
                            Id = 15,
                            CreatedOn = new DateTime(2023, 8, 20, 20, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 5,
                            Value = 10.300000000000001
                        },
                        new
                        {
                            Id = 16,
                            CreatedOn = new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 1,
                            Value = 12.9
                        },
                        new
                        {
                            Id = 17,
                            CreatedOn = new DateTime(2023, 8, 20, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 2,
                            Value = 15.6
                        },
                        new
                        {
                            Id = 18,
                            CreatedOn = new DateTime(2023, 8, 21, 6, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 3,
                            Value = 8.0999999999999996
                        },
                        new
                        {
                            Id = 19,
                            CreatedOn = new DateTime(2023, 8, 21, 11, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 4,
                            Value = 11.5
                        },
                        new
                        {
                            Id = 20,
                            CreatedOn = new DateTime(2023, 8, 20, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 5,
                            Value = 13.699999999999999
                        },
                        new
                        {
                            Id = 21,
                            CreatedOn = new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 2,
                            Value = 15.199999999999999
                        },
                        new
                        {
                            Id = 22,
                            CreatedOn = new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 3,
                            Value = 9.8000000000000007
                        },
                        new
                        {
                            Id = 23,
                            CreatedOn = new DateTime(2023, 8, 21, 6, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 4,
                            Value = 12.699999999999999
                        },
                        new
                        {
                            Id = 24,
                            CreatedOn = new DateTime(2023, 8, 21, 4, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 5,
                            Value = 11.300000000000001
                        },
                        new
                        {
                            Id = 25,
                            CreatedOn = new DateTime(2023, 8, 21, 2, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 1,
                            Value = 14.9
                        },
                        new
                        {
                            Id = 26,
                            CreatedOn = new DateTime(2023, 8, 20, 8, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 2,
                            Value = 8.5999999999999996
                        },
                        new
                        {
                            Id = 27,
                            CreatedOn = new DateTime(2023, 8, 20, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 3,
                            Value = 13.1
                        },
                        new
                        {
                            Id = 28,
                            CreatedOn = new DateTime(2023, 8, 20, 6, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 4,
                            Value = 10.0
                        },
                        new
                        {
                            Id = 29,
                            CreatedOn = new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 5,
                            Value = 16.399999999999999
                        },
                        new
                        {
                            Id = 30,
                            CreatedOn = new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 3,
                            Value = 16.399999999999999
                        },
                        new
                        {
                            Id = 31,
                            CreatedOn = new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 2,
                            Value = 18.699999999999999
                        },
                        new
                        {
                            Id = 32,
                            CreatedOn = new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 2,
                            Value = 22.300000000000001
                        },
                        new
                        {
                            Id = 33,
                            CreatedOn = new DateTime(2023, 8, 20, 2, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 2,
                            Value = 14.800000000000001
                        },
                        new
                        {
                            Id = 34,
                            CreatedOn = new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 3,
                            Value = 9.5
                        },
                        new
                        {
                            Id = 35,
                            CreatedOn = new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 3,
                            Value = 12.199999999999999
                        },
                        new
                        {
                            Id = 36,
                            CreatedOn = new DateTime(2023, 8, 20, 18, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 3,
                            Value = 15.9
                        },
                        new
                        {
                            Id = 37,
                            CreatedOn = new DateTime(2023, 8, 20, 8, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 4,
                            Value = 7.2999999999999998
                        },
                        new
                        {
                            Id = 38,
                            CreatedOn = new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 4,
                            Value = 11.1
                        },
                        new
                        {
                            Id = 39,
                            CreatedOn = new DateTime(2023, 8, 20, 3, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 4,
                            Value = 16.5
                        },
                        new
                        {
                            Id = 40,
                            CreatedOn = new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified),
                            DeviceId = 5,
                            Value = 8.8000000000000007
                        });
                });

            modelBuilder.Entity("DeviceMonitoring.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstName = "Milad",
                            LastName = "Khalatbari",
                            Password = "123456",
                            UserName = "Admin"
                        });
                });

            modelBuilder.Entity("DeviceMonitoring.Domain.Entities.Measurement", b =>
                {
                    b.HasOne("DeviceMonitoring.Domain.Entities.Device", "Device")
                        .WithMany("Measurements")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("DeviceMonitoring.Domain.Entities.Device", b =>
                {
                    b.Navigation("Measurements");
                });
#pragma warning restore 612, 618
        }
    }
}
