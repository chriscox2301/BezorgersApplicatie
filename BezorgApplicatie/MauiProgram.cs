using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using ZXing.Net.Maui.Controls;

namespace BezorgApplicatie
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseBarcodeReader();

            builder.Services.AddDbContext<DataContext>(
                options =>
                {
                    var dbPath = Path.Combine(FileSystem.AppDataDirectory, "MatrixIncBezorger.db");
                    options.UseSqlite($"Data Source={dbPath}");
                }
                );

            // Registreer de pagina's
            builder.Services.AddSingleton<BezorgApplicatie.Views.PakkettenInscannen>();

#if DEBUG
            
            //builder.Services.AddTransient<Views.PackageListPage>();
            builder.Services.AddTransient<Views.ProblemPage>();
            builder.Logging.AddDebug();
#endif
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
               
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                context.Database.EnsureCreated();

                DataContextInitializer.Initialize(context);
            }

            return app;
        }
    }
}
