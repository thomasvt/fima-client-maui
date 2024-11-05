using Microsoft.Identity.Client;

namespace fima_client_maui7.Auth
{
    internal static class AuthModule
    {
        public static IServiceCollection InstallFimaAuth(this IServiceCollection services)
        {
            services.AddSingleton(BuildAuthService());
            return services;
        }

        public static IAuthService BuildAuthService()
        {
            var clientApplicationBuilder = PublicClientApplicationBuilder.Create(Constants.ClientId)
                    .WithAuthority(AzureCloudInstance.AzurePublic, Constants.TenantId)
#if ANDROID
			        .WithParentActivityOrWindow(() => Platform.CurrentActivity)
#endif

#if WINDOWS
                    .WithRedirectUri("http://localhost/")
#else
                    .WithRedirectUri($"msal{Constants.ClientId}://auth")
#endif
                ;

            var clientApplication = clientApplicationBuilder.Build();
            TokenPersistence.ConfigureOn(clientApplication.UserTokenCache);

            return new AuthService(clientApplication);
        }
    }
}
