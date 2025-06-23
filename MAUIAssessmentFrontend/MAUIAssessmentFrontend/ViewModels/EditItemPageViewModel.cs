using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;
using Microsoft.Maui.Storage;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
//using Windows.Networking;

namespace MAUIAssessmentFrontend.ViewModels
{
    public class EditItemPageViewModel : INotifyPropertyChanged
    {
        private readonly IItemService _itemService;

        public EditItemPageViewModel(IItemService itemService)
        {
            _itemService = itemService;
            SaveCommand = new Command(async () => await SaveAsync());
            GoBackCommand = new Command(async () => await GoBack());
        }

        private int _id;
        public int Id { get => _id; set { _id = value; OnPropertyChanged(); LoadItemAsync(); } }

        private string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        private string _description;
        public string Description { get => _description; set { _description = value; OnPropertyChanged(); } }

        private double _latitude;
        public double Latitude { get => _latitude; set { _latitude = value; OnPropertyChanged(); } }

        private double _longitude;
        public double Longitude { get => _longitude; set { _longitude = value; OnPropertyChanged(); } }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand GoBackCommand { get; }

        private async Task LoadItemAsync()
        {
            if (Id <= 0) return;
            var item = await _itemService.GetItemByIdAsync(Id);
            if (item == null) return;

            Name = item.Name;
            Description = item.Description;
            Latitude = item.Latitude;
            Longitude = item.Longitude;
        }

        private async Task SaveAsync()
        {
            ErrorMessage = string.Empty;

            // Validations
            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Name is required.";
                return;
            }
            
            if (string.IsNullOrWhiteSpace(Description))
            {
                ErrorMessage = "Description is required";
                return;
            }

            

            
            var dto = new ItemDto
            {
                Name = Name,
                Description = Description,
                Latitude = Latitude,
                Longitude = Longitude
            };

            var ok = await _itemService.UpdateItemAsync(Id, dto);
            if (ok)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Item updated!", "OK");
                await GoBack();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Update failed.", "OK");
            }
        }

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("//MainPage"); // Absolute route to MainPage
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
