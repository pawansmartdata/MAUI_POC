using MAUIAssessnentFrontend.Models;
using MAUIAssessnentFrontend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUIAssessnentFrontend.ViewModels
{
    public class DetailPageViewModel : BindableObject
    {
        private readonly IApiService _apiService;
        private readonly int _itemId;

        public ItemDto Item { get; set; } = new();

        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public DetailPageViewModel(int itemId)
        {
            _itemId = itemId;
            _apiService = new ApiService();

            UpdateCommand = new Command(async () => await UpdateItemAsync());
            DeleteCommand = new Command(async () => await DeleteItemAsync());
        }

        public async Task LoadItemAsync()
        {
            var result = await _apiService.GetItemAsync(_itemId);
            if (result != null)
                Item = result;

            OnPropertyChanged(nameof(Item));
        }

        private async Task UpdateItemAsync()
        {
            var success = await _apiService.UpdateItemAsync(_itemId, Item);
            if (success)
                await Shell.Current.DisplayAlert("Success", "Item updated", "OK");
        }

        private async Task DeleteItemAsync()
        {
            var confirm = await Shell.Current.DisplayAlert("Confirm", "Delete this item?", "Yes", "No");
            if (!confirm) return;

            var success = await _apiService.DeleteItemAsync(_itemId);
            if (success)
            {
                await Shell.Current.DisplayAlert("Deleted", "Item removed", "OK");
                await Shell.Current.Navigation.PopAsync();
            }
        }
    }
}
