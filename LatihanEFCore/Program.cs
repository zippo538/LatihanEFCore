using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' tidak ditemukan.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IDbinitializer, Dbinitializer>();

// Mengaktifkan API berbasis Controller.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbinitializer>();
    await dbInitializer.Initialized();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Ok(new
{
    message = "Latihan EF Core API berjalan"
}));

// Menghubungkan route dari seluruh class Controller.
app.MapControllers();

app.Run();
