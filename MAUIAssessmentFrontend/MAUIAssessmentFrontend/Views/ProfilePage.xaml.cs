using MAUIAssessmentFrontend.ViewModels;

namespace MAUIAssessmentFrontend.Views;

public partial class ProfilePage : ContentPage
{
    private ProfileViewModel ViewModel => BindingContext as ProfileViewModel;

    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (ViewModel != null)
            await ViewModel.LoadProfileAsync();
            
    }
}
