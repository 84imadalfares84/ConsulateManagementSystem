using Consulate.Application.Features.Auth.DTOS;
using Consulate.Application.Interfaces;
using Consulate.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Auth.Comands.Register
{
    public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRoleRepository _roleRepository;

        public RegisterCommandHandler(
        
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IRoleRepository roleRepository,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _roleRepository = roleRepository;
         
        }

        public async Task<AuthResponse> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            //  تجزئة كلمة السر
            var hashedPassword = _passwordHasher.Hash(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = hashedPassword,
                UserRoles = new List<UserRole>()

            };
            //  جلب الدور
            string roleName = string.IsNullOrEmpty(request.RoleName) ? "Officer" : request.RoleName;
            var role = await _roleRepository.GetByNameAsync(roleName, cancellationToken);

            if (role != null)
            {
                user.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
            }
           
            await _userRepository.AddAsync(user);

            var accessToKen = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new AuthResponse(
                accessToKen,refreshToken
                
            );
        }
    }
}
