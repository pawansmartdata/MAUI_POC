using MAUIAssessmentFrontend.Views;

namespace MAUIAssessmentFrontend
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Optional: Register routes for non-Shell navigation
            Routing.RegisterRoute(nameof(LoginPage), typeof(Views.LoginPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(Views.SignUpPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(Views.MainPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(Views.ProfilePage));
            Routing.RegisterRoute(nameof(AddItemPage), typeof(Views.AddItemPage));
            Routing.RegisterRoute(nameof(DetailPage), typeof(Views.DetailPage));
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            Routing.RegisterRoute(nameof(EditItemPage), typeof(Views.EditItemPage));

        }
    }
}
