using MAUIAssessmentFrontend.Services.Interfaces;
using Microsoft.Maui.Storage;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
            GoBackCommand = new Command(async () => await GoBack());
            LoadProfileAsync();
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
        public ICommand GoBackCommand { get; }

        //private async void LoadExistingProfile()
        //{
        //    //FirstName = Preferences.Get("FirstName", "");
        //    //LastName = Preferences.Get("LastName", "");
        //    //Email = Preferences.Get("Email", "");
        //    //PhoneNumber = Preferences.Get("PhoneNumber", "");
        //    //ProfileImage = Preferences.Get("UserImage", "default_profile.png");
        //    var id = Preferences.Get("userId",0);
        //    var profile = await _userService.GetUserByIdAsync(id);
        //    var data = profile.Data;
        //    FirstName = data.FirstName;
        //    LastName = data.LastName;
        //    Email = data.Email;
        //    PhoneNumber = data.PhoneNumber;
        //    ProfileImage = data.ProfileImage;

        //}

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
        public async Task GoBack()
        {
            Shell.Current.GoToAsync("//MainPage");
        }

        public async Task LoadProfileAsync()
        {
            try
            {
                int userId = Preferences.Get("userId", 0);
                if (userId == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "User ID not found.", "OK");
                    return;
                }

                var user = await _userService.GetUserByIdAsync(userId);
                var data = user.Data;

                if (data != null)
                {
                    FirstName = data.FirstName;
                    LastName = data.LastName;
                    Email = data.Email;
                    PhoneNumber = data.PhoneNumber;
                    ProfileImage = data.ProfileImage ?? "default_profile.png";
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }


        private async Task SaveProfileAsync()
        {
            // Validations
            if (string.IsNullOrWhiteSpace(FirstName) || FirstName.Length > 20)
            {
                await App.Current.MainPage.DisplayAlert("Validation Error", "First name is required and must be less than or equal to 20 characters.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(LastName) || LastName.Length > 20)
            {
                await App.Current.MainPage.DisplayAlert("Validation Error", "Last name is required and must be less than or equal to 20 characters.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                await App.Current.MainPage.DisplayAlert("Validation Error", "Please enter a valid email address.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(PhoneNumber) || !PhoneNumber.All(char.IsDigit) || PhoneNumber.Length != 10)
            {
                await App.Current.MainPage.DisplayAlert("Validation Error", "Phone number must be exactly 10 digits and contain only numbers.", "OK");
                return;
            }

            // Proceed to update
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
                await Shell.Current.GoToAsync("ProfilePage");
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
