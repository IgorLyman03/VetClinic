using Appointment.Clients;
using Appointment.Clients.Interfaces;
using Appointment.Configurations;
using Appointment.Data;
using Appointment.Repositories;
using Appointment.Repositories.Interfaces;
using Appointment.Services;
using Appointment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.SetIsOriginAllowed(_ => true)
                   .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.Configure<AppointmentConfig>(configuration);

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
// Add services to the container.

builder.Services.AddHttpClient();

builder.Services.AddTransient<IDoctorClient, DoctorClient>();

builder.Services.AddHttpClient<IVetAidClient, VetAidClient>();

builder.Services.AddTransient<IBookingRepository, BookingRepository>();
builder.Services.AddTransient<IBookingService, BookingService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new List<string>()
        }
    });
});

builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));


var app = builder.Build();

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = "swagger";
    options.DisplayRequestDuration();
    options.EnableDeepLinking();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

CreateDbIfNotExists(app);

app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

void CreateDbIfNotExists(IHost host)
{
    using var scope = host.Services.CreateScope();

    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        DbInitializer.Initialize(context).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }

}