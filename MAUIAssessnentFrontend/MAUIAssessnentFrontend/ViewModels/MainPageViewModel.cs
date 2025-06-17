using MAUIAssessnentFrontend.Models;
using MAUIAssessnentFrontend.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUIAssessnentFrontend.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        private readonly IApiService _apiService;
        private bool _isBusy;

        public ObservableCollection<ItemDto> Items { get; set; } = new();
        public ICommand LoadItemsCommand { get; }
        public ICommand RefreshCommand { get; }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public MainPageViewModel()
        {
            _apiService = new ApiService();
            LoadItemsCommand = new Command(async () => await LoadItemsAsync());
            RefreshCommand = new Command(async () => await LoadItemsAsync());
            LoadItemsCommand.Execute(null);
        }

        private async Task LoadItemsAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            var items = await _apiService.GetItemsAsync();
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);
            IsBusy = false;
        }
    }
}
