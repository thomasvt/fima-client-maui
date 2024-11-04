using Microsoft.Identity.Client;

namespace fima_client_maui7.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Starts the interactive UI flow to authenticate a user.
        /// </summary>
        Task<AuthenticationResult> LogIn(CancellationToken cancellationToken);
        /// <summary>
        /// Gets the authentication token from the cache, the refresh-token flow or the UI authentication flow.
        /// </summary>
        Task<AuthenticationResult> GetAuthentication(CancellationToken cancellationToken);
        Task LogoutAsync(CancellationToken cancellationToken);
    }
}
