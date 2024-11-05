using Microsoft.Identity.Client;

namespace fima_client_maui7.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Starts the interactive UI flow to authenticate a user.
        /// </summary>
        Task<AuthenticationResult> LogIn(CancellationToken cancellationToken);
        
        Task<AuthenticationResult> GetOrAskAuthentication(CancellationToken cancellationToken);
        Task<AuthenticationResult> GetAuthenticationSilent(CancellationToken cancellationToken);
        Task LogoutAsync(CancellationToken cancellationToken);
    }
}
