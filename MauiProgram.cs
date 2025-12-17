using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using life_Book.Data.Utils;
using life_Book.Data.Services;

namespace life_Book
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
                });

            builder.Services.AddMauiBlazorWebView();

            // Configure SQLite Database
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "lifebook.db");
            builder.Services.AddDbContext<DatabaseSchema>(options =>
                options.UseSqlite($"Filename={dbPath}"));

            // Register Database Connection Service
            builder.Services.AddScoped<DatabaseConnectionService>();
  
            // Register User Service
            builder.Services.AddScoped<UserService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
