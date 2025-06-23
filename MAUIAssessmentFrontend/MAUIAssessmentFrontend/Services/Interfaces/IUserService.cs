using MAUIAssessmentFrontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAssessmentFrontend.Services.Interfaces
{
    public interface IUserService
    {
        Task<ProfileResponseDto?> GetUserByIdAsync(int id);
        Task<bool> UpdateProfileAsync(int userId, string firstName, string lastName, string email, string phoneNumber, string imageFile);
    }
}
