using fima_client_maui7.Api;
using fima_client_maui7.Auth;
using fima_client_maui7.LocalStorage;
using Microsoft.Extensions.Logging;

namespace fima_client_maui7
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // install FIMA specific services:

            builder.Services
                .InstallFimaAuth()
                .InstallFimaApi()
                .InstallFimaLocalStorage();

            // install FIMA pages:

            builder.Services.AddSingleton<GpsPage>();
            builder.Services.AddSingleton<PhotoPage>();
            builder.Services.AddSingleton<DevToolsPage>();
            builder.Services.AddSingleton<SurveyPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
