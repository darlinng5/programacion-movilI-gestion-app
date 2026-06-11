using GestionLibros.Data;
using GestionLibros.ViewModels;
using GestionLibros.Views;
using Microsoft.Extensions.Logging;

namespace GestionLibros
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<AppDatabase>();
            builder.Services.AddTransient<BookViewModel>();
            builder.Services.AddTransient<BookPage>();
            return builder.Build();
        }
    }
}
