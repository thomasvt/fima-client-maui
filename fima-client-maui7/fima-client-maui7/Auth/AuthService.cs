using Microsoft.Identity.Client;

namespace fima_client_maui7.Auth
{
    public class AuthService : BaseAuthService
    {
        public AuthService(IPublicClientApplication publicClientApplication) : base(publicClientApplication)
        {
        }
    }
}
