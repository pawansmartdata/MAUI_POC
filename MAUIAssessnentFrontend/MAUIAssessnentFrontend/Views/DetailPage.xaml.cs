using System.Net.NetworkInformation;
using System.Windows.Input;
using MAUIAssessnentFrontend.Models;
using MAUIAssessnentFrontend.Services;
using MAUIAssessnentFrontend.ViewModels;
using Microsoft.Maui.Controls.Maps;

namespace MAUIAssessnentFrontend.Views;

public partial class DetailPage : ContentPage
{
    private readonly DetailPageViewModel _vm;

    public DetailPage(int itemId)
    {
        InitializeComponent();
        _vm = new DetailPageViewModel(itemId);
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadItemAsync();

        // Show map location
        var position = new Location(_vm.Item.Latitude, _vm.Item.Longitude);
        var mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1));
        ItemMap.MoveToRegion(mapSpan);
        ItemMap.Pins.Clear();
        ItemMap.Pins.Add(new Pin
        {
            Label = _vm.Item.Name,
            Address = _vm.Item.Description,
            Location = position,
            Type = PinType.Place
        });
    }
}