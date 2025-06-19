using MAUIAssessmentFrontend.ViewModels;

namespace MAUIAssessmentFrontend.Views;
public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
