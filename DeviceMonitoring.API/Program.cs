using DeviceMonitoring.API.ExceptionHandling;
using DeviceMonitoring.Data;
using DeviceMonitoring.Data.Context;
using DeviceMonitoring.Services;
using DeviceMonitoring.Services.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/DeviceMonitoring.API-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    var connectionString = builder.Configuration
        .GetConnectionString("DeviceMonitoring")
        ?? throw new InvalidOperationException(
            "Connection string 'DeviceMonitoring' was not found.");

    var jwtOptions = builder.Configuration
        .GetRequiredSection(JwtOptions.SECTION_NAME)
        .Get<JwtOptions>()
        ?? throw new InvalidOperationException(
            $"Configuration section '{JwtOptions.SECTION_NAME}' was not found.");

    builder.Services.Configure<JwtOptions>(
        builder.Configuration.GetSection(JwtOptions.SECTION_NAME));

    builder.Services.AddControllers(options =>
    {
        options.ReturnHttpNotAcceptable = true;
    })
    .AddXmlDataContractSerializerFormatters();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Enter a JWT Bearer token.",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization"
        });

        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("Bearer", document),
                []
            }
        });
    });

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.SecretForKey)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

    builder.Services.AddAuthorization();
    builder.Services.AddDataServices(connectionString);
    builder.Services.AddApplicationServices(builder.Configuration);

    builder.Services.AddProblemDetails(options =>
    {
        options.CustomizeProblemDetails = context =>
        {
            context.ProblemDetails.Extensions["traceId"] =
                context.HttpContext.TraceIdentifier;
        };
    });
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<DeviceMonitoringContext>();
        await context.Database.MigrateAsync();
    }

    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception exception)
{
    Log.Fatal(exception, "Application startup failed.");
}
finally
{
    Log.CloseAndFlush();
}