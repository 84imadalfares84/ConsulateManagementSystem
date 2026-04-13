using Consulate.Application.Features.Auth.Comands.EmailVerification;
using Consulate.Application.Features.Auth.Comands.Login;
using Consulate.Application.Features.Auth.Comands.Refresh;
using Consulate.Application.Features.Auth.Comands.Register;
using Consulate.Application.Features.Auth.Comands.RevokeAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Consulate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]//اللوغ ان كنترولر بيستقبل كوماند من نوع لوق ان كوماند(يحوي ايميل وباسوورد) و بيرجع اوكي مع الريزالت اللي هو الاكسس توكن والاكسباير ات
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost("register")]//الريجيستر كنترولر بيستقبل كوماند من نوع ريجيستر كوماند(يحوي ايميل وباسوورد) و بيرجع اوكي مع الريزالت اللي هو الاكسس توكن والاكسباير ات
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//استخراج اليوزر اي دي من الكلايمز اللي موجود في التوكن وهذا افضل امنيا

            if (!Guid.TryParse(userId, out var id))
                return Unauthorized();

            await _mediator.Send(new RevokeAllToKensCommand(id));

            return Ok("Logged out from all devices");
        }
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var result = await _mediator.Send(new VerifyEmailCommand(token));
            return Ok(result);
        }
    }
}
