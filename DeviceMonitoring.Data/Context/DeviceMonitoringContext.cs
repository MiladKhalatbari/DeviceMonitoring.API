using DeviceMonitoring.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DeviceMonitoring.Data.Context;

public class DeviceMonitoringContext : DbContext
{
    public DeviceMonitoringContext(DbContextOptions<DeviceMonitoringContext> options) : base(options)
    { }


    public DbSet<Device> Devices { get; set; }

    public DbSet<Measurement> Measurements { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new DeviceConfig());
        modelBuilder.ApplyConfiguration(new UserConfig());

        #region Seed Data Users
        modelBuilder.Entity<User>().HasData(
           new User(1,"Admin","Milad","Khalatbari","123456"));
        #endregion

        #region  Seed Data Devices
        modelBuilder.Entity<Device>().HasData(
            new Device
            {
                Id = 1,
                Name = "Device 1",
                Location = "Location A",
                Manufacturer = "Manufacturer X",
                ModelNumber = "Model 123",
                InstallationDate = new DateTime(2023, 1, 15),
                IsActive = true
            },
            new Device
            {
                Id = 2,
                Name = "Device 2",
                Location = "Location B",
                Manufacturer = "Manufacturer Y",
                ModelNumber = "Model 456",
                InstallationDate = new DateTime(2021, 5, 20),
                IsActive = true
            },
            new Device
            {
                Id = 3,
                Name = "Device 3",
                Location = "Location C",
                Manufacturer = "Manufacturer Z",
                ModelNumber = "Model 789",
                InstallationDate = new DateTime(2020, 10, 10),
                IsActive = false
            },
            new Device
            {
                Id = 4,
                Name = "Device 4",
                Location = "Location D",
                Manufacturer = "Manufacturer X",
                ModelNumber = "Model 234",
                InstallationDate = new DateTime(2023, 3, 1),
                IsActive = true
            },
            new Device
            {
                Id = 5,
                Name = "Device 5",
                Location = "Location E",
                Manufacturer = "Manufacturer Z",
                ModelNumber = "Model 567",
                InstallationDate = new DateTime(2023, 7, 8),
                IsActive = true
            });
        #endregion

        #region Seed Data Measurments
        modelBuilder.Entity<Measurement>().HasData(
             new Measurement
             {
                 Id = 1,
                 Value = 10.5,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(5),
                 DeviceId = 1
             },
             new Measurement
             {
                 Id = 2,
                 Value = 15.2,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(25),
                 DeviceId = 2
             },
             new Measurement
             {
                 Id = 3,
                 Value = 9.8,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(6),
                 DeviceId = 3
             },
             new Measurement
             {
                 Id = 4,
                 Value = 12.7,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(8),
                 DeviceId = 4
             },
             new Measurement
             {
                 Id = 5,
                 Value = 11.3,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(19),
                 DeviceId = 5
             },
             new Measurement
             {
                 Id = 6,
                 Value = 14.9,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(5),
                 DeviceId = 1
             },
             new Measurement
             {
                 Id = 7,
                 Value = 8.6,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(4),
                 DeviceId = 2
             },
             new Measurement
             {
                 Id = 8,
                 Value = 13.1,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(5),
                 DeviceId = 3
             },
             new Measurement
             {
                 Id = 9,
                 Value = 10.0,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(4),
                 DeviceId = 4
             },
             new Measurement
             {
                 Id = 10,
                 Value = 16.4,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(5),
                 DeviceId = 5
             },
             new Measurement
             {
                 Id = 11,
                 Value = 9.2,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(4),
                 DeviceId = 1
             },
             new Measurement
             {
                 Id = 12,
                 Value = 11.7,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(19),
                 DeviceId = 2
             },
             new Measurement
             {
                 Id = 13,
                 Value = 14.1,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(19),
                 DeviceId = 3
             },
             new Measurement
             {
                 Id = 14,
                 Value = 7.8,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(9),
                 DeviceId = 4
             },
             new Measurement
             {
                 Id = 15,
                 Value = 10.3,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(20),
                 DeviceId = 5
             },
             new Measurement
             {
                 Id = 16,
                 Value = 12.9,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(19),
                 DeviceId = 1
             },
             new Measurement
             {
                 Id = 17,
                 Value = 15.6,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(9),
                 DeviceId = 2
             },
             new Measurement
             {
                 Id = 18,
                 Value = 8.1,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(30),
                 DeviceId = 3
             },
             new Measurement
             {
                 Id = 19,
                 Value = 11.5,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(35),
                 DeviceId = 4
             },
             new Measurement
             {
                 Id = 20,
                 Value = 13.7,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(9),
                 DeviceId = 5
             },
             new Measurement
             {
                 Id = 21,
                 Value = 15.2,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(5),
                 DeviceId = 2
             },
             new Measurement
             {
                 Id = 22,
                 Value = 9.8,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(19),
                 DeviceId = 3
             },
             new Measurement
             {
                 Id = 23,
                 Value = 12.7,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(30),
                 DeviceId = 4
             },
             new Measurement
             {
                 Id = 24,
                 Value = 11.3,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(28),
                 DeviceId = 5
             },
             new Measurement
             {
                 Id = 25,
                 Value = 14.9,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(26),
                 DeviceId = 1
             },
             new Measurement
             {
                 Id = 26,
                 Value = 8.6,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(8),
                 DeviceId = 2
             },
             new Measurement
             {
                 Id = 27,
                 Value = 13.1,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(9),
                 DeviceId = 3
             },
             new Measurement
             {
                 Id = 28,
                 Value = 10.0,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(6),
                 DeviceId = 4
             },
             new Measurement
             {
                 Id = 29,
                 Value = 16.4,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(4),
                 DeviceId = 5
             },
             new Measurement
             {
                 Id = 30,
                 Value = 16.4,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(19),
                 DeviceId = 3
             },
             new Measurement
             {
                 Id = 31,
                 Value = 18.7,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(4),
                 DeviceId = 2
             },
             new Measurement
             {
                 Id = 32,
                 Value = 22.3,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(5),
                 DeviceId = 2
             },
             new Measurement
             {
                 Id = 33,
                 Value = 14.8,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(2),
                 DeviceId = 2
             },
             new Measurement
             {
                 Id = 34,
                 Value = 9.5,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(19),
                 DeviceId = 3
             },
             new Measurement
             {
                 Id = 35,
                 Value = 12.2,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(5),
                 DeviceId = 3
             },
             new Measurement
             {
                 Id = 36,
                 Value = 15.9,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(18),
                 DeviceId = 3
             },
             new Measurement
             {
                 Id = 37,
                 Value = 7.3,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(8),
                 DeviceId = 4
             },
             new Measurement
             {
                 Id = 38,
                 Value = 11.1,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(5),
                 DeviceId = 4
             },
             new Measurement
             {
                 Id = 39,
                 Value = 16.5,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(3),
                 DeviceId = 4
             },
             new Measurement
             {
                 Id = 40,
                 Value = 8.8,
                 CreatedOn = new DateTime(2023, 8, 20).AddHours(4),
                 DeviceId = 5
             });
        #endregion
    }
}
