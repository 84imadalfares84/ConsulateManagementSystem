using Consulate.Application.Features.Auth.DTOS;
using Consulate.Application.Interfaces;
using MediatR;

namespace Consulate.Application.Features.Auth.Comands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password");
            }

            var token = _jwtService.GenerateAccessToken(user);


            return new AuthResponse(
                token,
                DateTime.UtcNow.AddMinutes(30)
            );
        }
    }
}
