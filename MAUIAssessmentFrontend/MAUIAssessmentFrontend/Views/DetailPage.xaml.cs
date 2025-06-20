using MAUIAssessmentFrontend.ViewModels;
using Microsoft.Maui.Controls;

namespace MAUIAssessmentFrontend.Views;

[QueryProperty(nameof(Id), "itemId")]
public partial class DetailPage : ContentPage, IQueryAttributable
{
    private readonly DetailPageViewModel _viewModel;

    public DetailPage(DetailPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }


    //public async void ApplyQueryAttributes(IDictionary<string, object> query)
    //{
    //    if (query.TryGetValue("itemId", out var idValue) && idValue is string idStr && int.TryParse(idStr, out var itemId))
    //    {
    //        await _viewModel.InitializeAsync(itemId);
    //    }
    //}

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("itemId", out var idValue) && idValue is string idStr && int.TryParse(idStr, out var itemId))
        {
            Console.WriteLine($"[DEBUG] Navigated with itemId = {itemId}"); // ? Helpful
            await _viewModel.InitializeAsync(itemId);
        }
    }

}
