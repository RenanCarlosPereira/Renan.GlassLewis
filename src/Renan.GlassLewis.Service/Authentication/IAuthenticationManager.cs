namespace Renan.GlassLewis.Service.Authentication
{
    public interface IAuthenticationManager
    {
        AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest);
    }
}