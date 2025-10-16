using CommunityToolkit.Maui;
using easy_journal.Services.Database;
using easy_journal.Services.Quote;
using easy_journal.Servicess.Http;

using easy_journal.ViewModels;
using easy_journal.Views;
using Microsoft.Extensions.Logging;
using easy_journal.Services.Entry;



#if WINDOWS
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;

using SolidColorBrush = Microsoft.UI.Xaml.Media.SolidColorBrush;
using Colors = Microsoft.UI.Colors;
#endif

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

            SetWindowsStyles();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }


        private static void RegisterServices(IServiceCollection services) 
        {
            services.AddSingleton<IHttpService, HttpService>();
            services.AddSingleton<IQuoteService, QuoteService>();
            services.AddSingleton<IEntryService, EntryService>();
            services.AddSingleton<IDatabaseService>(
                new DatabaseService(Path.Combine(
                FileSystem.AppDataDirectory,
                "easyjournal.db3"))
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

        private static void SetWindowsStyles()
        {
            //Removes border for editor
#if WINDOWS
            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("RemoveFocusBackground", (handler, view) =>
            {
                var textBox = handler.PlatformView;

                // Remove borders
                textBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                textBox.BorderBrush = new SolidColorBrush(Colors.Transparent);

                // Set initial background
                textBox.Background = new SolidColorBrush(Colors.Transparent);

                // Remove focus visuals
                textBox.UseSystemFocusVisuals = false;
                textBox.Style = null;

                var resources = new Microsoft.UI.Xaml.ResourceDictionary();

                var transparentBrush = new SolidColorBrush(Colors.Transparent);

                // Background for all states
                resources["TextControlBackground"] = transparentBrush;
                resources["TextControlBackgroundPointerOver"] = transparentBrush;
                resources["TextControlBackgroundFocused"] = transparentBrush;
                resources["TextControlBackgroundDisabled"] = transparentBrush;

                // Border for all states
                resources["TextControlBorderBrush"] = transparentBrush;
                resources["TextControlBorderBrushPointerOver"] = transparentBrush;
                resources["TextControlBorderBrushFocused"] = transparentBrush;
                resources["TextControlBorderBrushDisabled"] = transparentBrush;

                // Foreground color (keep text visible)
                resources["TextControlForeground"] = new SolidColorBrush(
                    Microsoft.UI.ColorHelper.FromArgb(255, 42, 42, 42) // #2A2A2A
                );
                resources["TextControlForegroundPointerOver"] = new SolidColorBrush(
                    Microsoft.UI.ColorHelper.FromArgb(255, 42, 42, 42)
                );
                resources["TextControlForegroundFocused"] = new SolidColorBrush(
                    Microsoft.UI.ColorHelper.FromArgb(255, 42, 42, 42)
                );

                textBox.Resources = resources;
            });
#endif
        }
    }
}
