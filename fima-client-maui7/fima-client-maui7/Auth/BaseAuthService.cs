using Microsoft.Identity.Client;

namespace fima_client_maui7.Auth
{
    public abstract class BaseAuthService : IAuthService
    {
        private readonly IPublicClientApplication _authenticationClient;

        protected BaseAuthService(IPublicClientApplication authenticationClient)
        {
            _authenticationClient = authenticationClient;
        }

        public Task<AuthenticationResult> LogIn(CancellationToken cancellationToken)
        {
            return _authenticationClient
                .AcquireTokenInteractive(Constants.Scopes)
                .ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the authentication token from the cache, the refresh-token flow or the UI authentication flow.
        /// </summary>
        public async Task<AuthenticationResult> GetOrAskAuthentication(CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _authenticationClient.GetAccountsAsync();
                var firstAccount = accounts.FirstOrDefault();
                if (firstAccount is null)
                {
                    return await LogIn(cancellationToken);
                }

                return await _authenticationClient.AcquireTokenSilent(Constants.Scopes, firstAccount)
                    .ExecuteAsync(cancellationToken);
            }
            catch (MsalUiRequiredException)
            {
                return await LogIn(cancellationToken);
            }
        }

        /// <summary>
        /// Gets the authentication token from the cache or the refresh-token flow. Returns null if a new interactive login is needed.
        /// </summary>
        public async Task<AuthenticationResult?> GetAuthenticationSilent(CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _authenticationClient.GetAccountsAsync();
                var firstAccount = accounts.FirstOrDefault();
                if (firstAccount is null)
                {
                    return null;
                }

                return await _authenticationClient.AcquireTokenSilent(Constants.Scopes, firstAccount)
                    .ExecuteAsync(cancellationToken);
            }
            catch (MsalUiRequiredException)
            {
                return null;
            }
        }

        public async Task LogoutAsync(CancellationToken cancellationToken)
        {
            var accounts = await _authenticationClient.GetAccountsAsync();
            foreach (var account in accounts)
            {
                await _authenticationClient.RemoveAsync(account);
            }

            TokenPersistence.Clear();
        }
    }
}
