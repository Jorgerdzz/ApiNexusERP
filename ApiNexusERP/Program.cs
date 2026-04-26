using ApiNexusERP.Helpers;
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

//CREAMOS UNA INSTANCIA DE NUESTRO HELPER
HelperActionOAuthService helper = new HelperActionOAuthService(builder.Configuration);
//ESTA INSTANCIA SOLAMENTE DEBEMOS CREARLA UNA VEZ
builder.Services.AddSingleton<HelperActionOAuthService>(helper);
//HABILITAMOS LA SEGURIDAD DENTRO DE PROGRAM
builder.Services.AddAuthentication(helper.GetAuthenticationSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());

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
builder.Services.AddTransient<RepositoryAuth>();
builder.Services.AddTransient<RepositoryEmpleados>();
builder.Services.AddTransient<RepositoryNominas>();
builder.Services.AddTransient<RepositoryContabilidad>();
builder.Services.AddTransient<RepositoryFacturacion>();
builder.Services.AddTransient<RepositoryEstadisticas>();

//MAPPINGS
builder.Services.AddAutoMapper(typeof(NexusProfile));

builder.Services.AddControllers();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
