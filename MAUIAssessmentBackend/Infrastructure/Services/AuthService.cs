using App.Core.Dtos;
using App.Core.Entity;
using App.Core.Interfaces.IRepository;
using App.Core.Interfaces.IServices;
using Domain.Utility;
using Microsoft.AspNetCore.Http;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IImageService imageService,IEmailService emailService,IEmailTemplateService emailTemplateService,IJwtService jwtService)
        {
            _userRepository = userRepository;
            _imageService = imageService;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _jwtService = jwtService;
        }

        public async Task RegisterAsync(User request, IFormFile? imageFile, string webRootPath)
        {
            try
            {
                // Check for existing user
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    throw new Exception("Email already exists.");
                }

                // Handle image upload
                string profileImageUrl = GetDefaultImageUrl(); // default first

                if (imageFile != null)
                {
                    var uploadResult = await _imageService.Upload(imageFile, webRootPath);
                    if (uploadResult is ResponseDto response && response.Status == 200 && !string.IsNullOrEmpty(response.Data?.ToString()))
                    {
                        profileImageUrl = response.Data.ToString()!;
                    }
                }

                // Final user entity to save
                var newUser = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash), // Make sure to hash in production
                    ProfileImagePath = profileImageUrl
                };
                string htmlContent = await _emailTemplateService.GenerateRegistrationEmail(newUser.FirstName+" "+newUser.LastName, "MyRestaurantApp", newUser.Email, request.PasswordHash);
               // await _emailService.SendEmailAsync(user.Email, "Welcome to MyRestaurantApp!", htmlContent);

                await _userRepository.AddAsync(newUser);
                await _emailService.SendEmailAsync(newUser.Email, newUser.FirstName+" "+newUser.LastName, "Welcome to our platform", htmlContent);
            }
            catch (Exception ex)
            {
                throw new Exception($"Registration failed: {ex.Message}");
            }
        }

        public async Task<Object> LoginAsync(LoginUserDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                {
                    throw new ArgumentException("Username or Password cannot be empty.");
                }

                var user = await _userRepository.GetByEmailAsync(request.UserName);
                if (user == null)
                {
                    throw new UnauthorizedAccessException("Invalid email or password.");
                }

                // ⚠️ This is for demo; in production, use a proper password hashing check
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Invalid email or password.");
                }

                string token = await _jwtService.Authenticate(user.Id, user.Email, user.FirstName, user.LastName);
                return new LoginResponseDto
                {
                    Status = 200,
                    Message = "User Login SuccessFully",
                    UserData = user,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Login failed: {ex.Message}");
            }
        }
    

        private string GetDefaultImageUrl()
        {
            return "https://img.freepik.com/free-psd/3d-icon-social-media-app_23-2150049569.jpg?t=st=1750222786~exp=1750226386~hmac=00a906dfbd364099333be77c2b7466ff0776c65f5e7061b84ea8674ee1eec314&w=900"; // Replace with your actual domain and path
        }
    }
}
