namespace MAUIAssessnentFrontend.Views;

public partial class AddItemPage : ContentPage
{
    public AddItemPage()
    {
        InitializeComponent();
        BindingContext = new AddItemViewModel();
    }
}