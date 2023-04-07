using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicios.api.Seguridad.Core.Context;
using Servicios.api.Seguridad.Core.Dto;
using Servicios.api.Seguridad.Core.Entities;

// Esta clase es una logica de negocio CQRS
namespace Servicios.api.Seguridad.Core.Application;

public class Register
{
    public class UserRegisterCommand : IRequest<UserDto>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Password { get; set; }

    }

    public class UserRegisterValidation : AbstractValidator<UserRegisterCommand>
    {
        public UserRegisterValidation()
        {
            RuleFor(x => x.Nombre).NotEmpty();
            RuleFor(x => x.Apellido).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Direccion).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    // Clase para manejar la respuesta con el dto
    public class UserRegisterHandler : IRequestHandler<UserRegisterCommand, UserDto>
    {
        private readonly SeguridadContext context;
        private readonly UserManager<User> usermanager;
        private readonly IMapper mapper;

        public UserRegisterHandler(SeguridadContext segcontext, UserManager<User> manager, IMapper _mapper)
        {
            context = segcontext;
            usermanager = manager;
            mapper = _mapper;
        }
        public async Task<UserDto> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var existe = context.Users.Where(p => p.Email == request.Email).Any();

            if (existe)
            {
                throw new Exception("El email del usuario ya existe en la base de datos");
            }

            existe = context.Users.Where(p => p.UserName == request.Username).Any();

            if (existe)
            {
                throw new Exception("El nombre de usuario ya existe en la base de datos");
            }

            var newUser = new User
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email,
                UserName = request.Username,
                Direccion = request.Direccion
            };

            var rta = await usermanager.CreateAsync(newUser, request.Password);
            Console.WriteLine(rta);
            if (rta.Succeeded)
            {
                var userDto = mapper.Map<User, UserDto>(newUser);
                return userDto;
            }

            throw new Exception("No se pudo registrar el usuario");


        }
    }

}