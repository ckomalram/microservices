using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Servicios.api.Seguridad.Core.Application;
using Servicios.api.Seguridad.Core.Context;
using Servicios.api.Seguridad.Core.Entities;
using Servicios.api.Seguridad.Core.Entities.JwtLogic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Register>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//## Agregar EF
builder.Services.AddSqlServer<SeguridadContext>(builder.Configuration.GetConnectionString("cnSeguridadDb"));
//## Agregando identity core
builder.Services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<SeguridadContext>()
                .AddSignInManager<SignInManager<User>>();
//## Agregando system clock para cuando se crea un nuevo usuario.
// Si el objeto existe, simplemente toma la hora, si no existe, lo toma y lo crea.
builder.Services.TryAddSingleton<ISystemClock, SystemClock>();

//Agregar Media TR para uso de clase Register en Controladores
// No es necesario agregar para otra clase applicacion
builder.Services.AddMediatR(typeof(Register.UserRegisterCommand).Assembly);

// agregando auto Mapper
// No es necesario agregar para otra clase applicacion
builder.Services.AddAutoMapper(typeof(Register.UserRegisterHandler));

//Agregar uso de TOken
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
//Agregando session del usuario
builder.Services.AddScoped<IUserSesion, UserSesion>();

//configuracion para autenticación
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RrF1XwA6ke5nApomZfCzrflviFtkxgqj"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // options.RequireHttpsMetadata = false;
        // options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false, // Verificar el dominio de donde se genera el token
            ValidateAudience = false, // true ciertos ip puedan acceder a mi aplicacion
        };
    });

var app = builder.Build();

//##  logica ALCANCE DE SERVICIOS
/**
Este tipo de código suele ser utilizado para realizar 
operaciones que deben ejecutarse una sola vez 
al inicio de la aplicación, como agregar un usuario de 
administrador o inicializar una base de datos. 
Al utilizar un alcance de servicio, se garantiza 
que los servicios que se están utilizando sean eliminados de
 la memoria una vez que la operación se ha completado, 
 lo que ayuda a optimizar el rendimiento de la aplicación.
*/
// Llamar al contextdata seguridad para probar el user manager
using (var context = app.Services.CreateScope())
{
    var services = context.ServiceProvider;

    try
    {
        var usermanager = services.GetRequiredService<UserManager<User>>();
        var contextEf = services.GetRequiredService<SeguridadContext>();

        SeguridadContextData.InsertUser(contextEf, usermanager).Wait();

    }
    catch (Exception e)
    {
        var logging = services.GetRequiredService<ILogger<Program>>();
        logging.LogError(e, "Error al registrar usuario");
    }
}

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
