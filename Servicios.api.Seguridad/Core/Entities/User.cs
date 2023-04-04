using Microsoft.AspNetCore.Identity;

namespace Servicios.api.Seguridad.Core.Entities;

public class User : IdentityUser
{

    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Direccion { get; set; }

}