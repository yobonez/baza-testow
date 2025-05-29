using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TestyLogic.Models;
using TestyMAUI.Configuration;
using TestyMAUI.ViewModel;

namespace TestyMAUI
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

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<QuestionsCreatorPage>();
            builder.Services.AddSingleton<QuestionsCreatorViewModel>();
            builder.Services.AddSingleton<TestsCreatorPage>();
            builder.Services.AddSingleton<TestsCreatorViewModel>();
            builder.Services.AddSingleton<TestSelectorPage>();
            builder.Services.AddSingleton<TestSelectorViewModel>();
            builder.Services.AddTransient<SearchPage>();
            builder.Services.AddTransient<SearchViewModel>();

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("TestyMAUI.appsettings.json");

            if (stream is not null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                SQLServer connectionOptions = config.GetRequiredSection("SQL Server").Get<SQLServer>();

                builder.Configuration.AddConfiguration(config);
                builder.Services.AddDbContext<TestyDBContext>(options => options.UseSqlServer(
                    $"Server={connectionOptions.Server};" +
                    $"database={connectionOptions.Database};" +
                    $"User ID={connectionOptions.User};" +
                    $"password={connectionOptions.Password};" +
                    $"TrustServerCertificate=true"
                ));
            }

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
