using Consulate.Application.Common.Exceptions;
using Consulate.Application.Interfaces;
using MediatR;

namespace Consulate.Application.Features.Auth.Comands.EmailVerification
{
    public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public VerifyEmailHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByVerificationTokenAsync(request.Token);

            if (user == null)
                throw new BadRequestException("Invalid verification token.");

            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;

            await _userRepository.SaveChangesAsync();

            return "Email verified successfully";
        }
    }
}
