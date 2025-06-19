using MAUIAssessmentFrontend.ViewModels;

namespace MAUIAssessmentFrontend.Views
{
    public partial class AddItemPage : ContentPage
    {
        public AddItemPage(AddItemViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
