using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MAUIAssessmentFrontend.Services.Interfaces;
using MAUIAssessmentFrontend.Views;
using Microsoft.Maui.Storage;

namespace MAUIAssessmentFrontend.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;

        public ICommand LogoutCommand { get; }

        public ICommand EditProfileCommand { get; }
        public ICommand GoBackCommand { get; }

        public ProfileViewModel(IUserService userService)
        {
            _userService = userService;
            LogoutCommand = new Command(async () => await LogoutAsync());
            GoBackCommand = new Command(async () => await GoBack());
            EditProfileCommand = new Command(async () => await Shell.Current.GoToAsync("EditProfilePage"));
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UserName)); // Trigger update for combined property
                }
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UserName)); // Trigger update for combined property
                }
            }
        }

        public string UserName => $"{FirstName} {LastName}".Trim();

        //private string _userName;
        //public string UserName
        //{
        //    get => _userName;
        //    set { _userName = value; OnPropertyChanged(); }
        //}

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _profileImage;
        public string ProfileImage
        {
            get => _profileImage;
            set { _profileImage = value; OnPropertyChanged(); }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
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
                    Console.WriteLine("Profile image"+data.ProfileImage);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task LogoutAsync()
        {
            Preferences.Clear();
            await Toast.Make("Logout Successful", ToastDuration.Short).Show();
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
