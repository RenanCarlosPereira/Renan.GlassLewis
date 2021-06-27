using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Renan.GlassLewis.Service.Authentication
{
    internal class AuthenticationManager : IAuthenticationManager
    {
        private readonly List<AuthenticationRequest> _users = new List<AuthenticationRequest>
        {
            new AuthenticationRequest { Id = 1, Username = "GlassLewis", Password = "123" }
        };

        private readonly JwtOptions _jwtOptions;

        public AuthenticationManager(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest)
        {
            var user = _users.SingleOrDefault(x => x.Username == authenticationRequest.Username && x.Password == authenticationRequest.Password);

            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),

                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var authenticationResponse = new AuthenticationResponse { Token = tokenHandler.WriteToken(token) };

            return authenticationResponse;
        }

        public IEnumerable<AuthenticationRequest> GetAll()
        {
            // return users without passwords
            return _users.Select(x =>
            {
                x.Password = null;
                return x;
            });
        }
    }
}