namespace fima_client_maui7.LocalStorage
{
    internal static class LocalStorageModule
    {
        public static IServiceCollection InstallFimaLocalStorage(this IServiceCollection services)
        {
            services.AddSingleton((sp) =>
            {
                var localStore = new LocalStore();
                // localStore.CreateSchema(false); // uncomment to recreate the DB
                return localStore;
            });
            return services;
        }
    }
}
