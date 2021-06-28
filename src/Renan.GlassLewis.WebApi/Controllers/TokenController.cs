using Microsoft.AspNetCore.Mvc;
using Renan.GlassLewis.Application.Authentication;

namespace Renan.GlassLewis.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticationManager _service;

        public TokenController(IAuthenticationManager service)
        {
            _service = service;
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