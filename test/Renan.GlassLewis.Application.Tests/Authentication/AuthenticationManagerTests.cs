using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Renan.GlassLewis.Application.Authentication;
using Xunit;
using AuthenticationManager = Renan.GlassLewis.Application.Authentication.AuthenticationManager;

namespace Renan.GlassLewis.Application.Tests.Authentication
{
    public class AuthenticationManagerTests
    {
        private readonly IOptions<JwtOptions> _options;
        private readonly Fixture _fixture;

        public AuthenticationManagerTests()
        {
            _fixture = new Fixture();
            _options = Options.Create(_fixture.Create<JwtOptions>());
        }

        private AuthenticationManager CreateManager()
        {
            return new AuthenticationManager(_options);
        }

        [Fact]
        public void Authenticate_WhenRequestIsNull_ResponseIsNull()
        {
            // Arrange
            var manager = CreateManager();

            // Act
            var result = manager.Authenticate(null);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Authenticate_UserNotWhiteListed_ResponseIsNull()
        {
            // Arrange
            var manager = CreateManager();

            var authenticationRequest = _fixture.Create<AuthenticationRequest>();
            // Act
            var result = manager.Authenticate(authenticationRequest);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Authenticate_UserNotWhiteListed_ResponseToken()
        {
            // Arrange
            var manager = CreateManager();

            var authenticationRequest = new AuthenticationRequest { Username = "GlassLewis", Password = "123" };
            // Act
            var result = manager.Authenticate(authenticationRequest);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrWhiteSpace();
        }
    }
}