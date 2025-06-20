using MAUIAssessmentFrontend.Services.Interfaces;
using Microsoft.Maui.Storage;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MAUIAssessmentFrontend.ViewModels
{
    public class EditProfileViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;

        public EditProfileViewModel(IUserService userService)
        {
            _userService = userService;
            PickImageCommand = new Command(async () => await PickImageAsync());
            SaveCommand = new Command(async () => await SaveProfileAsync());
            LoadExistingProfile();
        }

        private string _firstName, _lastName, _email, _phoneNumber, _profileImage;
        private FileResult _selectedImage;

        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string PhoneNumber { get => _phoneNumber; set { _phoneNumber = value; OnPropertyChanged(); } }
        public string ProfileImage { get => _profileImage; set { _profileImage = value; OnPropertyChanged(); } }

        public ICommand PickImageCommand { get; }
        public ICommand SaveCommand { get; }

        private void LoadExistingProfile()
        {
            FirstName = Preferences.Get("FirstName", "");
            LastName = Preferences.Get("LastName", "");
            Email = Preferences.Get("Email", "");
            PhoneNumber = Preferences.Get("PhoneNumber", "");
            ProfileImage = Preferences.Get("UserImage", "default_profile.png");
        }

        private async Task PickImageAsync()
        {
            _selectedImage = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Choose a profile image",
                FileTypes = FilePickerFileType.Images
            });

            if (_selectedImage != null)
                ProfileImage = _selectedImage.FullPath;
        }

        private async Task SaveProfileAsync()
        {
            int userId = Preferences.Get("userId", 0);
            if (userId == 0)
            {
                await App.Current.MainPage.DisplayAlert("Error", "User ID not found.", "OK");
                return;
            }

            var success = await _userService.UpdateProfileAsync(userId, FirstName, LastName, Email, PhoneNumber, _selectedImage);
            if (success)
            {
                Preferences.Set("FirstName", FirstName);
                Preferences.Set("LastName", LastName);
                Preferences.Set("Email", Email);
                Preferences.Set("PhoneNumber", PhoneNumber);
                if (!string.IsNullOrEmpty(ProfileImage))
                    Preferences.Set("UserImage", ProfileImage);

                await App.Current.MainPage.DisplayAlert("Success", "Profile updated!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Failed to update profile.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
