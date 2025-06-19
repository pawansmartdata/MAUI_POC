using App.Core.Dtos;
using App.Core.Entity;
using App.Core.Interfaces.IRepository;
using App.Core.Interfaces.IServices;
using Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService:IUserService
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;
        public UserService(IUserRepository userRepository, IImageService imageService)
        {
            _userRepository = userRepository;
            _imageService = imageService;
        }
        public async Task<Object> GetUserByIdAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("Invalid user ID.");

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new KeyNotFoundException("User not found.");

                return new ResponseDto
                {
                    Status = 200,
                    Message = "Get User Information successfully",
                    Data = user
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching user: {ex.Message}");
            }
        }
        public async Task<bool> UpdateUserAsync(int userId, UpdateUserDto updateUserDto, string webRootPath)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted)
                throw new Exception("User not found.");

            // Optional profile image upload
            if (updateUserDto.ProfilePicture != null)
            {
                var result = await _imageService.Upload(updateUserDto.ProfilePicture, webRootPath);
                if (result is ResponseDto uploadResponse && uploadResponse.Status == 200)
                {
                    user.ProfileImagePath = uploadResponse.Data?.ToString() ?? user.ProfileImagePath;
                }
                // Optional: You can log the error if upload fails instead of throwing
            }

            // Update user fields if provided
            user.FirstName = updateUserDto.FirstName ?? user.FirstName;
            user.LastName = updateUserDto.LastName ?? user.LastName;
            user.Email = updateUserDto.Email ?? user.Email;
            user.PhoneNumber = updateUserDto.PhoneNumber ?? user.PhoneNumber;

            await _userRepository.UpdateAsync(user);
            return true;
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted)
                throw new Exception("User not found.");

            user.IsDeleted = true;
            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}

