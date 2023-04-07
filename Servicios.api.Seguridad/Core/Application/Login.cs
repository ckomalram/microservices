using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicios.api.Seguridad.Core.Context;
using Servicios.api.Seguridad.Core.Dto;
using Servicios.api.Seguridad.Core.Entities;
using Servicios.api.Seguridad.Core.Entities.JwtLogic;

namespace Servicios.api.Seguridad.Core.Application;

public class Login
{
    public class UserLoginCommand : IRequest<UserDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class UserLoginValidation : AbstractValidator<UserLoginCommand>
    {
        public UserLoginValidation()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    // Clase para manejar la respuesta con el dto
    public class UserLoginHandler : IRequestHandler<UserLoginCommand, UserDto>
    {
        private readonly SeguridadContext context;
        private readonly UserManager<User> usermanager;
        private readonly IMapper mapper;
        private readonly IJwtGenerator jwtGenerator;
        private readonly SignInManager<User> signinmanager;

        public UserLoginHandler(SeguridadContext segcontext, UserManager<User> manager,
         IMapper _mapper, IJwtGenerator _jwtGenerator, SignInManager<User> _signinmanager)
        {
            context = segcontext;
            usermanager = manager;
            mapper = _mapper;
            jwtGenerator = _jwtGenerator;
            signinmanager = _signinmanager;
        }

        public async Task<UserDto> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await usermanager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new Exception("El usuario no exise...");
            }

            // Validar login
            var rta = await signinmanager.CheckPasswordSignInAsync(user, request.Password, false);

            if (rta.Succeeded)
            {
                var userRta = mapper.Map<User, UserDto>(user);

                userRta.Token = jwtGenerator.CreateToken(user);

                return userRta;
            }


            throw new Exception("Login fallido...");


        }
    }

}