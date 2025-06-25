using MAUIAssessmentFrontend.Services.Interfaces;
using MAUIAssessmentFrontend.Services;
using Microsoft.Extensions.Logging;
using MAUIAssessmentFrontend.Views;
using MAUIAssessmentFrontend.ViewModels;
using CommunityToolkit.Maui;


namespace MAUIAssessmentFrontend
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

           // var uri = new Uri("https://7245-49-248-148-242.ngrok-free.app/");
           // var uri = new Uri("https://5c86-49-248-148-242.ngrok-free.app/");

         
            builder.Services.AddHttpClient<IAuthService, AuthService>(client=>
            {
                client.BaseAddress = uri;
            });
            builder.Services.AddHttpClient<IItemService, ItemService>(client =>
            {
                client.BaseAddress = uri;
            });
            builder.Services.AddHttpClient<IUserService, UserService>(client =>
            {
                client.BaseAddress = uri;
            });

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<AddItemPage>();
            builder.Services.AddTransient<AddItemViewModel>();
            builder.Services.AddTransient<EditProfilePage>();
            builder.Services.AddTransient<EditProfileViewModel>();
            //builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddTransient<DetailPage>();
            builder.Services.AddTransient<DetailPageViewModel>();
            builder.Services.AddTransient<EditItemPageViewModel>();
            builder.Services.AddTransient<EditItemPage>(); 
            // builder.Services.AddSingleton<IUserService, UserService>();
            //builder.Services.AddSingleton<IItemService, ItemService>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
