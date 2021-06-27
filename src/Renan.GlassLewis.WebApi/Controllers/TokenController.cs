using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Renan.GlassLewis.Service.Authentication;

namespace Renan.GlassLewis.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IAuthenticationManager _service;

        public TokenController(ILogger<TokenController> logger, IAuthenticationManager service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<AuthenticationResponse> Authenticate([FromBody] AuthenticationRequest request)
        {
            var authenticationResponse = _service.Authenticate(request);

            if (authenticationResponse == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(authenticationResponse);
        }
    }
}