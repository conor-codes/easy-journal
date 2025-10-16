using CommunityToolkit.Maui;
using easy_journal.Services.Database;
using easy_journal.Services.Quote;
using easy_journal.Servicess.Http;

using easy_journal.ViewModels;
using easy_journal.Views;
using Microsoft.Extensions.Logging;

namespace easy_journal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiCommunityToolkit()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //Register Views
            RegisterViews(builder.Services);
            //Register ViewModels
            RegisterViewModels(builder.Services);
            //Register Services
            RegisterServices(builder.Services);
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void RegisterServices(IServiceCollection services) 
        {
            // HTTP Services
            services.AddSingleton<IHttpService, HttpService>();

            // API Services
            services.AddSingleton<IQuoteService, QuoteService>();

            // Data Services
            var dbPath = Path.Combine(
                FileSystem.AppDataDirectory,
                "easyjournal.db3"
            );

            services.AddSingleton<IDatabaseService>(
                new DatabaseService(dbPath)
            );
        }

        private static void RegisterViews(IServiceCollection services)
        {
            services.AddTransient<EntryPage>();
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            services.AddTransient<EntryPageViewModel>();
        }
    }
}
