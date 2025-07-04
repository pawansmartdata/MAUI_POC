using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MAUIAssessmentFrontend.Services.Interfaces;
using MAUIAssessmentFrontend.Utility;
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
        private string _selectedImage;

        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string PhoneNumber { get => _phoneNumber; set { _phoneNumber = value; OnPropertyChanged(); } }
        public string ProfileImage { get => _profileImage; set { _profileImage = value; OnPropertyChanged(); } }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(); // Or use INotifyPropertyChanged/Fody/MVVM Toolkit
            }
        }

        public ICommand PickImageCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand GoBackCommand { get; }

        public async Task PickImageAsync()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Choose a profile image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                _selectedImage = result.FullPath; // Just store path as string
                ProfileImage = _selectedImage;
            }
        }
        public async Task GoBack()
        {
            Shell.Current.GoToAsync("//MainPage");
        }

        public async Task LoadProfileAsync()
        {
            await LoaderHelper.RunWithLoader(async()=>
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
            }, isBusy => IsBusy = isBusy, "Proifile Updating...");
        }


        private async Task SaveProfileAsync()
        {
            ErrorMessage = string.Empty;
            // Validations
            if (string.IsNullOrWhiteSpace(FirstName) || FirstName.Length > 20)
            {
                ErrorMessage = "First name is required and must be less than or equal to 20 characters.";
                return;
            }

            if (string.IsNullOrWhiteSpace(LastName) || LastName.Length > 20)
            {
                ErrorMessage = "Last name is required and must be less than or equal to 20 characters.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ErrorMessage = "Please enter a valid email address.";
                return;
            }

            if (string.IsNullOrWhiteSpace(PhoneNumber) || !PhoneNumber.All(char.IsDigit) || PhoneNumber.Length != 10)
            {
                ErrorMessage = "Phone number must be exactly 10 digits and contain only numbers.";
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
            if (success.Status==200)
            {
                Preferences.Set("userName", success.FirstName);
                Preferences.Set("LastName", success.LastName);
                Preferences.Set("Email", success.Email);
                Preferences.Set("PhoneNumber", success.PhoneNumber);
                if (!string.IsNullOrEmpty(ProfileImage))
                    Preferences.Set("userImage", success.ProfileImagePath);

                //await App.Current.MainPage.DisplayAlert("Success", "Profile updated!", "OK");
                await Toast.Make("Profile Updated!!", ToastDuration.Short).Show();
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
