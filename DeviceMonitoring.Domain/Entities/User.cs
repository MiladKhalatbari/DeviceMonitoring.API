using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitoring.Domain.Entities;
public record User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public User(int id, string userName, string firstName, string lastName, string password)
    {
        Id = id;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Password = password;

    }
}
public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.UserName).HasMaxLength(256).IsRequired();
        builder.Property(p => p.Password).HasMaxLength(256).IsRequired(); ;
        builder.Property(p => p.FirstName).HasMaxLength(256);
        builder.Property(p => p.FirstName).HasMaxLength(256);
    }
}