using MAUIAssessmentFrontend.ViewModels;
using Microsoft.Maui.Controls;

namespace MAUIAssessmentFrontend.Views;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class EditItemPage : ContentPage, IQueryAttributable
{
    private readonly EditItemPageViewModel _viewModel;

    public EditItemPage(EditItemPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    public int ItemId
    {
        set => _viewModel.Id = value; // Sets Id on VM, which triggers LoadItemAsync()
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // No-op: QueryProperty already set ItemId for us
    }
}
