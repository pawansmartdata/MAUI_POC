using MAUIAssessmentFrontend.ViewModels;

namespace MAUIAssessmentFrontend.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
