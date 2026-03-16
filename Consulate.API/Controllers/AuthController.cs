using Consulate.Application.Features.Auth.Comands.Login;
using Consulate.Application.Features.Auth.Comands.Register;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost("register")]//الريجيستر كنترولر بيستقبل كوماند من نوع ريجيستر كوماند(يحوي ايميل وباسوورد) و بيرجع اوكي مع الريزالت اللي هو الاكسس توكن والاكسباير ات
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
