using Consulate.Application.Common.Exceptions;
using Consulate.Application.Interfaces;
using MediatR;

namespace Consulate.Application.Features.Auth.Comands.EmailVerification
{
    public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;

        public VerifyEmailHandler(IUserRepository userRepository, ICacheService cacheService)
        {
            _userRepository = userRepository;
           _cacheService = cacheService;
        }

        public async Task<string> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByVerificationTokenAsync(request.Token);

            if (user == null)
                throw new BadRequestException("Invalid verification token.");

            user.IsEmailVerified = true;
            await _cacheService.RemoveAsync($"user:{user.Email}");
            user.EmailVerificationToken = null;

            await _userRepository.SaveChangesAsync();

            return "Email verified successfully";
        }
    }
}
