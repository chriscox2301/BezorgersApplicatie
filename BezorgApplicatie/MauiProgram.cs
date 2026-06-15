using BezorgApplicatie.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Java.Util.Jar.Pack200;

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

            return app;
        }
    }
}
