using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using TestyLogic.Models;
using TestyMAUI.Configuration;
using TestyMAUI.ViewModel;

namespace TestyMAUI.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDb(this IServiceCollection services, ConfigurationManager configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("TestyMAUI.appsettings.json");

        if (stream is not null)
        {
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            SQLServer connectionOptions = config.GetRequiredSection("SQL Server").Get<SQLServer>();

            configuration.AddConfiguration(config);
            services.AddDbContext<TestyDBContext>(
                options => options.UseSqlServer(
                    $"Server={connectionOptions.Server};" +
                    $"database={connectionOptions.Database};" +
                    $"User ID={connectionOptions.User};" +
                    $"password={connectionOptions.Password};" +
                    $"TrustServerCertificate=true"), 
                contextLifetime: ServiceLifetime.Transient,
                optionsLifetime: ServiceLifetime.Singleton);
        }

        return services;
    }
    public static IServiceCollection AddPages(this IServiceCollection services) 
    {
        services.AddSingleton<MainPage>();

        services.AddSingleton<QuestionsCreatorPage>();
        services.AddSingleton<TestsCreatorPage>();
        services.AddSingleton<TestSelectorPage>();
        services.AddSingleton<SearchPage>();
        services.AddSingleton<TestPage>();

        return services;
    }
    public static IServiceCollection AddViewModels(this IServiceCollection services) {
        services.AddSingleton<MainViewModel>();

        services.AddSingleton<QuestionsCreatorViewModel>();
        services.AddSingleton<TestsCreatorViewModel>();
        services.AddSingleton<TestSelectorViewModel>();
        services.AddSingleton<SearchViewModel>();
        services.AddSingleton<TestViewModel>();

        return services;
    }
    public static IServiceCollection AddViewModelHelpers(this IServiceCollection services) 
    {
        services.AddSingleton<ViewModelLoader>();

        return services;
    }
}
