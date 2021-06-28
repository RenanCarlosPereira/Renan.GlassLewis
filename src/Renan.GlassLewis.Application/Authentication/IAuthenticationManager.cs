namespace Renan.GlassLewis.Application.Authentication
{
    public interface IAuthenticationManager
    {
        AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest);
    }
}