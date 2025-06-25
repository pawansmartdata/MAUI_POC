using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUIAssessmentFrontend.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _email;
        private bool _isPassword = true;
        public string EyeIcon => IsPassword ? "show.png" : "hide.png";
        public ICommand TogglePasswordVisibilityCommand { get; }
        public bool IsPassword
        {
            get => _isPassword;
            set
            {
                if (_isPassword != value)
                {
                    _isPassword = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(EyeIcon)); // Also notify icon change
                }
            }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToSignupCommand { get; }

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () => await LoginAsync());
            NavigateToSignupCommand = new Command(async () => await Shell.Current.GoToAsync("SignUpPage"));
            TogglePasswordVisibilityCommand = new Command(() =>
            {
                IsPassword = !IsPassword;
                OnPropertyChanged(nameof(EyeIcon));
            });
        }

        private async Task LoginAsync()
        {
            ErrorMessage = string.Empty;

            if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ErrorMessage = "Please enter a valid email address.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both email and password.";
                return;
            }

            var loginDto = new LoginDto
            {
                UserName = this.Email,
                Password = this.Password
            };

            var result = await _authService.LoginAsync(loginDto);

            if (result?.Token?.Status == 200)
            {
                var user = result.Token.UserData;

                // store JWT or user info as needed
                Preferences.Set("userId", user.Id);
                Preferences.Set("userName", user.FirstName);
                Preferences.Set("userImage", user.ProfileImagePath);
                Preferences.Set("jwtToken", result.Token.Token);
                Preferences.Set("email", result.Token.UserData.Email);

                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                ErrorMessage = result?.Token?.Message ?? "Login failed.";
            }
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
