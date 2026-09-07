using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data;
using LatihanEFCore.DTO.Responses.Services;
using LatihanEFCore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Text;
using LatihanEFCore.DTOs;
using LatihanEFCore.Validator;
using LatihanEFCore.Data.Seeders;
using LatihanEFCore.Services;
using System.Security.Claims;
using LatihanEFCore.Repository;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;

        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

var jwtSection = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSection["Key"]
    ?? throw new InvalidOperationException(
        "JWT Key tidak ditemukan.");

var jwtIssuer = jwtSection["Issuer"]
    ?? throw new InvalidOperationException(
        "JWT Issuer tidak ditemukan.");

var jwtAudience = jwtSection["Audience"]
    ?? throw new InvalidOperationException(
        "JWT Audience tidak ditemukan.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),

                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),

                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IDbinitializer, Dbinitializer>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// add FluentValidation
builder.Services.AddScoped<IValidator<CreateStudentDTO>, CreateStudentValidator>();
builder.Services.AddScoped<IValidator<UpdateStudentDTO>, UpdateStudentValidator>();
builder.Services.AddScoped<IValidator<RegisterDTO>, RegisterValidator>();
builder.Services.AddScoped<IValidator<LoginDTO>, LoginValidator>();


// Mengaktifkan API berbasis Controller.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "Masukkan token JWT dengan format: Bearer {token}"
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbinitializer>();
    await dbInitializer.Initialized();
    await IdentityUserSeeder.SeedAsync(serviceProvider);

}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Ok(new
{
    message = "Latihan EF Core API berjalan"
}));

app.UseAuthentication();
app.UseAuthorization();


// Menghubungkan route dari seluruh class Controller.
app.MapControllers();

app.Run();
