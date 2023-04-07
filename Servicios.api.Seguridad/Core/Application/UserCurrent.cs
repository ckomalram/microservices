using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicios.api.Seguridad.Core.Dto;
using Servicios.api.Seguridad.Core.Entities;
using Servicios.api.Seguridad.Core.Entities.JwtLogic;

namespace Servicios.api.Seguridad.Core.Application;


public class UserCurrent
{
    public class UserCurrentCommand : IRequest<UserDto> { }

    public class UserCurrentHandler : IRequestHandler<UserCurrentCommand, UserDto>
    {
        private readonly UserManager<User> usermanager;
        private readonly IUserSesion usersession;
        private readonly IMapper mapper;
        private readonly IJwtGenerator jwtgenerator;

        public UserCurrentHandler(UserManager<User> _usermanager, IMapper _mapper, IUserSesion _usersession, IJwtGenerator _jwtgenerator)
        {
            usermanager = _usermanager;
            usersession = _usersession;
            jwtgenerator = _jwtgenerator;
            mapper = _mapper;
        }


        public async Task<UserDto> Handle(UserCurrentCommand request, CancellationToken cancellationToken)
        {
            var user = await usermanager.FindByNameAsync(usersession.GetUserSession());

            if (user != null)
            {
                var userRta = mapper.Map<User, UserDto>(user);
                userRta.Token = jwtgenerator.CreateToken(user);

                return userRta;
            }

            throw new Exception("sesion de usuario no valida");

        }
    }
}