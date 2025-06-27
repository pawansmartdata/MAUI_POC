using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;
using MAUIAssessmentFrontend.Utility;
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

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        private bool _isPasswordHidden = true;

        public string PasswordEyeIcon => IsPasswordHidden ? "show.png" : "hide.png";
        public string ConfirmPasswordEyeIcon => IsConfirmPasswordHidden ? "show.png" : "hide.png";

        public ICommand TogglePasswordVisibilityCommand => new Command(() => IsPasswordHidden = !IsPasswordHidden);
        public ICommand ToggleConfirmPasswordVisibilityCommand => new Command(() => IsConfirmPasswordHidden = !IsConfirmPasswordHidden);
        public bool IsPasswordHidden
        {
            get => _isPasswordHidden;
            set
            {
                _isPasswordHidden = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PasswordEyeIcon));
            }
        }

        private bool _isConfirmPasswordHidden = true;
        public bool IsConfirmPasswordHidden
        {
            get => _isConfirmPasswordHidden;
            set
            {
                _isConfirmPasswordHidden = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ConfirmPasswordEyeIcon));
            }
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

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        private async Task NavigateToLogin()
        {
            //await Shell.Current.GoToAsync("LoginPage")
        }

        private async Task RegisterAsync()
        {
            await LoaderHelper.RunWithLoader(async() =>
            {


                ErrorMessage = string.Empty;

                // Validations
                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    ErrorMessage = "First name is required.";
                    return;
                }
                if (string.IsNullOrWhiteSpace(LastName))
                {
                    ErrorMessage = "Last name is required.";
                    return;
                }
                if (LastName.Length > 20)
                {
                    ErrorMessage = "Last name should ne be more than 20 characters.";
                    return;
                }
                if (FirstName.Length > 20)
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
            }, isBusy => IsBusy = isBusy, "Creating Account...");
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
