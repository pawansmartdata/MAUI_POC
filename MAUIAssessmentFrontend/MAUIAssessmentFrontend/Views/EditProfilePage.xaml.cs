using MAUIAssessmentFrontend.ViewModels;

namespace MAUIAssessmentFrontend.Views;

public partial class EditProfilePage : ContentPage
{
    private EditProfileViewModel ViewModel => BindingContext as EditProfileViewModel;

    public EditProfilePage(EditProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (ViewModel != null)
        {
            await ViewModel.LoadProfileAsync();
        }
    }
}
