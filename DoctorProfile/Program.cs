using DoctorProfile.Configurations;
using DoctorProfile.Repositories.Interfaces;
using DoctorProfile.Repositories;
using DoctorProfile.Services.Interfaces;
using DoctorProfile.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using DoctorProfile.Data;
using Microsoft.EntityFrameworkCore;
using DoctorProfile.Authorization.DoctorInfo;
using DoctorProfile.Authorization.DoctorTimetable;

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

builder.Services.Configure<DoctorConfig>(configuration);

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


builder.Services.AddMemoryCache();

// Add services to the container.


builder.Services.AddTransient<IAuthorizationHandler, DoctorTimetableAuthorizationHandler>();
builder.Services.AddTransient<IAuthorizationHandler, DoctorInfoAuthorizationHandler>();

builder.Services.AddTransient<IDoctorTimetableRepository, DoctorTimetableRepository>();
builder.Services.AddTransient<IDoctorInfoRepository, DoctorInfoRepository>();

builder.Services.AddTransient<IDoctorTimetableService, DoctorTimetableService>();
builder.Services.AddTransient<IDoctorInfoService, DoctorInfoService>();



builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = false,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"])),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();

    options.AddPolicy("CanCreateDoctorInfo", policy =>
        policy.Requirements.Add(new DoctorInfoOperationRequirement(DoctorProfile.Authorization.OperationType.Create)));
    options.AddPolicy("CanUpdateDoctorInfo", policy =>
        policy.Requirements.Add(new DoctorInfoOperationRequirement(DoctorProfile.Authorization.OperationType.Update)));
    options.AddPolicy("CanDeleteDoctorInfo", policy =>
        policy.Requirements.Add(new DoctorInfoOperationRequirement(DoctorProfile.Authorization.OperationType.Delete)));

    options.AddPolicy("CanCreateDoctorTimetable", policy =>
        policy.Requirements.Add(new DoctorTimetableOperationRequirement(DoctorProfile.Authorization.OperationType.Create)));
    options.AddPolicy("CanUpdateDoctorTimetable", policy =>
        policy.Requirements.Add(new DoctorTimetableOperationRequirement(DoctorProfile.Authorization.OperationType.Update)));
    options.AddPolicy("CanDeleteDoctorTimetable", policy =>
        policy.Requirements.Add(new DoctorTimetableOperationRequirement(DoctorProfile.Authorization.OperationType.Delete)));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add JWT Authentication support to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Указываем, что для использования Swagger требуется аутентификация
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
