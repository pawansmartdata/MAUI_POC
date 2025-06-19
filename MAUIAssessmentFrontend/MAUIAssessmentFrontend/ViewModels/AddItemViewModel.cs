using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MAUIAssessmentFrontend.Services.Interfaces;
using Microsoft.Maui.Storage;
using MAUIAssessmentFrontend.Models;

namespace MAUIAssessmentFrontend.ViewModels
{
    public class AddItemViewModel : INotifyPropertyChanged
    {
        private readonly IItemService _itemService;

        public AddItemViewModel(IItemService itemService)
        {
            _itemService = itemService;
            PickImageCommand = new Command(async () => await PickImageAsync());
            SubmitCommand = new Command(async () => await SubmitAsync());
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set { _latitude = value; OnPropertyChanged(); }
        }

        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set { _longitude = value; OnPropertyChanged(); }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set { _imagePath = value; OnPropertyChanged(); }
        }

        public ICommand PickImageCommand { get; }
        public ICommand SubmitCommand { get; }

        private async Task PickImageAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    var tempPath = Path.Combine(FileSystem.CacheDirectory, result.FileName);

                    using var fileStream = File.Create(tempPath);
                    await stream.CopyToAsync(fileStream);

                    ImagePath = tempPath;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Image error: {ex.Message}", "OK");
            }
        }

        private async Task SubmitAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await ShowValidationError("Name is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                await ShowValidationError("Description is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Latitude) || !double.TryParse(Latitude, out double lat))
            {
                await ShowValidationError("Latitude must be a valid number.");
                return;
            }

            if (lat < -90 || lat > 90)
            {
                await ShowValidationError("Latitude must be between -90 and 90.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Longitude) || !double.TryParse(Longitude, out double lng))
            {
                await ShowValidationError("Longitude must be a valid number.");
                return;
            }

            if (lng < -180 || lng > 180)
            {
                await ShowValidationError("Longitude must be between -180 and 180.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ImagePath))
            {
                await ShowValidationError("Please select an image.");
                return;
            }

            var item = new ItemDto
            {
                Name = Name.Trim(),
                Description = Description.Trim(),
                Latitude = lat,
                Longitude = lng,
                Image = ImagePath
            };

            var success = await _itemService.AddItemAsync(item);
            if (success)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Item added!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Failed to add item.", "OK");
            }
        }

        private Task ShowValidationError(string message)
        {
            return App.Current.MainPage.DisplayAlert("Validation Error", message, "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
