using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;
using System.Collections.ObjectModel;



namespace MAUIAssessmentFrontend.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IItemService _itemService;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(); // Notify the UI
                    FilterItems(); // Call filter every time text changes
                }
            }
        }

        private string _userName;



        private ObservableCollection<ItemResponseDto> _filteredItems;
        public ObservableCollection<ItemResponseDto> FilteredItems
        {
            get => _filteredItems;
            set {
                    _filteredItems = value;
                    OnPropertyChanged();
            }
        }        

        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }

        private string _profileImage;
        public string ProfileImage
        {
            get => _profileImage;
            set { _profileImage = value; OnPropertyChanged(); }
        }



        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        public bool IsNotBusy => !IsBusy;

        public ObservableCollection<ItemResponseDto> Items { get; } = new();

        public ICommand GoToProfileCommand { get; }
        public ICommand LoadItemsCommand { get; }
        public ICommand GoToAddItemCommand { get; }
        public ICommand GoToDetailCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AIBotCommand { get; }

        public MainPageViewModel(IItemService itemService)
        {
            _itemService = itemService;

            UserName = Preferences.Get("userName", "User");
            ProfileImage = Preferences.Get("userImage", "default_image_url");
            GoToProfileCommand = new Command(async () => await GoToProfileAsync());
            LoadItemsCommand = new Command(async () => await LoadItemsAsync());
            GoToAddItemCommand = new Command(async () => await Shell.Current.GoToAsync("AddItemPage"));
            GoToDetailCommand = new Command<int>(async (Id) => await GoToDetailPage(Id));
            AIBotCommand = new Command(async () => await GoToAIBotPage());
            SearchCommand = new Command(() => FilterItems());
            FilteredItems = new ObservableCollection<ItemResponseDto>(Items);
            LoadItemsCommand.Execute(null);
        }


        private async Task GoToProfileAsync()
        {
            await Shell.Current.GoToAsync("ProfilePage");
        }

        private async Task LoadItemsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                Items.Clear();

                var items = await _itemService.GetAllItemsAsync();
                foreach (var item in items)
                {
                    Items.Add(item);

                    Console.Write("Item image is " + item.ItemImageUrl);
                }
            }
            catch (Exception ex)
            {
                // Optional: Handle or log error
            }
            finally
            {
                IsBusy = false;
            }
            FilterItems();
        }


        private void FilterItems()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredItems = new ObservableCollection<ItemResponseDto>(Items);
            }
            else
            {
               
                var lowerSearchText = SearchText.ToLowerInvariant();

                var filtered = Items.Where(item =>
                    !string.IsNullOrEmpty(item.Name) &&
                    item.Name.ToLowerInvariant().Contains(lowerSearchText));

                FilteredItems = new ObservableCollection<ItemResponseDto>(filtered);
            }

        }


        private async void CollectionView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (sender is View view)
            {
                view.Opacity = 0;
                await view.FadeTo(1, 300, Easing.CubicIn);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private async Task GoToDetailPage(int Id)
        {
            Console.WriteLine("Id is" + Id);
            await Shell.Current.GoToAsync($"DetailPage?itemId={Id}");
        }

        private async Task GoToAIBotPage()
        {
            await Shell.Current.GoToAsync("ChatView");
        }
    }
}
