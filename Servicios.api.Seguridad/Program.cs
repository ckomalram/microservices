using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Servicios.api.Seguridad.Core.Context;
using Servicios.api.Seguridad.Core.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar EF
builder.Services.AddSqlServer<SeguridadContext>(builder.Configuration.GetConnectionString("cnSeguridadDb"));
// Agregando identity core
builder.Services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<SeguridadContext>()
                .AddSignInManager<SignInManager<User>>();
// Agregando system clock para cuando se crea un nuevo usuario.
// Si el objeto existe, simplemente toma la hora, si no existe, lo toma y lo crea.
builder.Services.TryAddSingleton<ISystemClock, SystemClock>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
