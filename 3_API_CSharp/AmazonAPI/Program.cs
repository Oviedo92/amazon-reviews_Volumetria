using Microsoft.EntityFrameworkCore;
using AmazonAPI.Data;


var builder = WebApplication.CreateBuilder(args);

// -- CONECTAR LA BASE DE DATOS A SQL SERVER --
builder.Services.AddDbContext<AmazonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AmazonDB")));


// 1. CONFIGURACIÓN DE CORS (Para que el Frontend o otras páginas puedan consultar la API sin bloqueos)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()    // Permite cualquier origen (luego puedes limitarlo a tu IP)
              .AllowAnyMethod()    // Permite GET, POST, PUT, DELETE
              .AllowAnyHeader();   // Permite cualquier tipo de header
    });
});

// 2. REGISTRO DE CONTROLADORES (Los endpoints)
builder.Services.AddControllers();

// 3. CONFIGURACIÓN DE SWAGGER (La página web de pruebas)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -- NOTA PARA VALLEDUPAR: AQUÍ CONECTAREMOS LA BASE DE DATOS EN EL SIGUIENTE PASO --

var app = builder.Build();

// ================= PIPELINE DE PETICIONES HTTP =================

// 4. MODO DESARROLLO (Activa Swagger solo si estamos probando en local)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 5. SEGURIDAD Y REDIRECCIÓN HTTPS
app.UseHttpsRedirection();

// 6. ACTIVAR CORS (Aplica la política que creamos arriba)
app.UseCors("PermitirTodo");

app.UseAuthorization();

// 7. MAPEO DE LOS CONTROLADORES
app.MapControllers();

// 8. ¡ARRANCA EL MOTOR!
app.Run();