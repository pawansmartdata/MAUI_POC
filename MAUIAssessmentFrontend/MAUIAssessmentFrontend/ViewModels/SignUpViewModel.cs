using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MAUIAssessmentFrontend.ViewModels
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;

        public event PropertyChangedEventHandler PropertyChanged;

        public SignUpViewModel(IAuthService authService)
        {
            _authService = authService;
            RegisterCommand = new Command(async () => await RegisterAsync());
            NavigateToLoginCommand = new Command(async () => await NavigateToLogin());
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        private async Task NavigateToLogin()
        {
            //await Shell.Current.GoToAsync("LoginPage")
        }

        private async Task RegisterAsync()
        {
            ErrorMessage = string.Empty;

            // Validations
            if (string.IsNullOrWhiteSpace(FirstName) )
            {
                ErrorMessage = "First name is required.";
                return;
            }
            if (string.IsNullOrWhiteSpace(LastName) )
            {
                ErrorMessage = "Last name is required.";
                return;
            }
            if (LastName.Length > 20)
            {
                ErrorMessage = "Last name should ne be more than 20 characters.";
                return;
            }
            if ( FirstName.Length > 20)
            {
                ErrorMessage = "First name should ne be more than 20 characters.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ErrorMessage = "Enter a valid email.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
            {
                ErrorMessage = "Password must be at least 6 characters.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            var registerDto = new RegisterDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Password = Password
            };

            var result = await _authService.RegisterAsync(registerDto);

            if (result == true)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                ErrorMessage = "Registration failed. Try again.";
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
