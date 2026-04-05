using ApiNexusERP.Mappings;
using ApiNexusERP.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NugetModelsNexusERP.Data;
using NugetModelsNexusERP.Helpers;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connectionString = builder.Configuration.GetConnectionString("NexusConnection");
builder.Services.AddDbContext<NexusContext>(options => options.UseSqlServer(connectionString));

//INYECCIONES PARA EL HELPER
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HelperSessionContextAccessor>();

//REPOSITORIOS
builder.Services.AddTransient<RepositoryDepartamentos>();
builder.Services.AddTransient<RepositoryClientes>();
builder.Services.AddTransient<RepositoryEmpresas>();

//MAPPINGS
builder.Services.AddAutoMapper(typeof(NexusProfile));

//TEMPORAL
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Esta línea mágica corta los bucles infinitos de raíz
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseAuthorization();



// HACK TEMPORAL PARA PRUEBAS (Borrar cuando hagamos el Login JWT)
app.Use(async (context, next) =>
{
    // Simulamos que el usuario de la Empresa con ID 1 está logueado
    var claims = new List<System.Security.Claims.Claim>
    {
        new System.Security.Claims.Claim("EmpresaId", "1"),
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "Usuario Pruebas")
    };
    var identity = new System.Security.Claims.ClaimsIdentity(claims, "TestAuth");
    context.User = new System.Security.Claims.ClaimsPrincipal(identity);

    await next();
});


app.MapControllers();

app.Run();
