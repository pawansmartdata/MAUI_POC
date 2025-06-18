using MAUIAssessmentFrontend.Services.Interfaces;
using MAUIAssessmentFrontend.Services;
using Microsoft.Extensions.Logging;
using MAUIAssessmentFrontend.Views;
using MAUIAssessmentFrontend.ViewModels;


namespace MAUIAssessmentFrontend
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

            builder.Services.AddHttpClient<IAuthService, AuthService>(client=>
            {
                client.BaseAddress = new Uri("https://71d7-49-248-148-242.ngrok-free.app/");
            });
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IItemService, ItemService>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
