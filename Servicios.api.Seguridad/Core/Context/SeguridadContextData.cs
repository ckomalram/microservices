
using Microsoft.AspNetCore.Identity;
using Servicios.api.Seguridad.Core.Entities;

namespace Servicios.api.Seguridad.Core.Context;


public class SeguridadContextData
{
    public static async Task InsertUser(SeguridadContext context,
    UserManager<User> userManager)
    {
        if (!userManager.Users.Any())
        {
            var newUser = new User
            {
                Nombre = "Carlyle",
                Apellido = "Komalram",
                Direccion = "Vacamonte, La hacienda",
                UserName = "ckomalram",
                Email = "ckomalram@cursonet.com"
            };
            await userManager.CreateAsync(newUser, "Pass12345$");
        }
    }
}