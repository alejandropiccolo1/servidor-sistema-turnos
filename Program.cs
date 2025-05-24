using Microsoft.EntityFrameworkCore;
using ReservasBackend.Data;
using ReservasBackend.Services; // Agregá esto si ahí está UserService
using Microsoft.OpenApi.Models;
using ReservasBackend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Agregar ApplicationDbContext con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios propios (como el UserService)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Reservas API", Version = "v1" });
});

builder.Services.AddControllers(); // Necesario para los controladores

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Activar los controladores

app.Run();
