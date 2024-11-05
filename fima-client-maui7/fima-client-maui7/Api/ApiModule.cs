namespace fima_client_maui7.Api
{
    internal static class ApiModule
    {
        public static IServiceCollection InstallFimaApi(this IServiceCollection services)
        {
            services.AddSingleton(new HttpClient());
            services.AddTransient<ApiClient>();
            return services;
        }
    }
}
