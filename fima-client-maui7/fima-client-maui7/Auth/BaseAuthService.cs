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

        public async Task<AuthenticationResult> GetAuthentication(CancellationToken cancellationToken)
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
