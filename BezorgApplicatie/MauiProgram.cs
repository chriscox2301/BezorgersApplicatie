using BezorgApplicatie.Data;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BezorgApplicatie
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

            builder.Services.AddTransient<Views.PakketLijstPage>();
            builder.Services.AddTransient<Views.ProbleemMeldenPage>();

            builder.Services.AddDbContext<DataContext>(
                options =>
                {
                    var dbPath = Path.Combine(FileSystem.AppDataDirectory, "MatrixIncBezorger.db");
                    options.UseSqlite($"Data Source={dbPath}");
                }
                );

#if DEBUG
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
