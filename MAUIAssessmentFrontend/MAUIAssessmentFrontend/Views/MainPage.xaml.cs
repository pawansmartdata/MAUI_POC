using MAUIAssessmentFrontend.ViewModels;

namespace MAUIAssessmentFrontend.Views;
public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // This ensures data is refreshed every time the page appears
        if (_viewModel.LoadItemsCommand.CanExecute(null))
        {
            _viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
