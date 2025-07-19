

// Conexión a la base de datos SANJUAN
using Microsoft.EntityFrameworkCore;
using INTEGRACION_SANJUAN.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 

// Solo API REST (sin vistas)
builder.Services.AddControllers();
builder.Services.AddCors();


// Autorización (necesaria si usas UseAuthorization)
builder.Services.AddAuthorization();

// Swagger y documentación
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Antes de app.UseAuthorization();

app.UseCors(builder =>
{
    builder.WithOrigins("http://127.0.0.1:5500")
           .AllowAnyHeader()
           .AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();

// Rutas tipo API REST
app.MapControllers();

app.Run();


