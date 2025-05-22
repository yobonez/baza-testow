using Microsoft.Extensions.Logging;
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

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
