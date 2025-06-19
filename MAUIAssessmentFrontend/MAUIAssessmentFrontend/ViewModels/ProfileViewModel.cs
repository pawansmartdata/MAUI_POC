using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Storage;

namespace MAUIAssessmentFrontend.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }

        public ICommand LogoutCommand { get; }

        public ProfileViewModel()
        {
            UserName = Preferences.Get("UserName", "User");
            Email = Preferences.Get("UserEmail", "user@example.com");
            ProfileImage = Preferences.Get("UserImage", "default_profile_image_url");

            LogoutCommand = new Command(Logout);
        }

        private async void Logout()
        {
            Preferences.Clear();
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
