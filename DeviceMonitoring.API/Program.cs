using DeviceMonitoring.Data.Context;
using DeviceMonitoring.Data.Repositories;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.Business;
using DeviceMonitoring.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.OpenApi.Models;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("Logs/DeviceMonitoring.API", rollingInterval: RollingInterval.Day).CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region SQL DataBase Context

builder.Services.AddDbContext<DeviceMonitoringContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DeviceMonitoring_DBconnectionString")));

#endregion

#region JWT Authentication Configuration

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Jwt:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:Jwt:SecretForKey"]))
    };
});
 builder.Services.AddSwaggerGen(c =>
 {

     // Define the security scheme
     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
     {
         Description = "JWT Authorization header using the Bearer scheme",
         Type = SecuritySchemeType.Http,
         Scheme = "bearer",
         BearerFormat = "JWT",
         In = ParameterLocation.Header,
         Name = "Authorization", 
     });

     // Add the security requirement
     c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { } 
        }
    });
 });
#endregion


#region Inversion Of Control

builder.Services.AddTransient<IRepository<Device>, Repository<Device>>();
builder.Services.AddTransient<IRepository<Measurement>, Repository<Measurement>>();
builder.Services.AddTransient<IRepository<User>, Repository<User>>();

builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<IMeasurementService, MeasurementService>();
builder.Services.AddTransient<IMonitoringService, MonitoringService>();
builder.Services.AddScoped<DeviceMonitoring.Services.IAuthenticationService, DeviceMonitoring.Services.AuthenticationService>();

#endregion

builder.Host.UseSerilog();

var app = builder.Build();

#region Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

#endregion

app.Run();

