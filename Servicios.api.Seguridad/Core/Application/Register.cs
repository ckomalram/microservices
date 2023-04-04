using MediatR;
using Servicios.api.Seguridad.Core.Dto;

namespace Servicios.api.Seguridad.Core.Application;

public class Register
{
    public class UserRegisterCommand : IRequest<UserDto>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }

}