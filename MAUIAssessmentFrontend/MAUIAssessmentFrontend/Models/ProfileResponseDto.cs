using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAssessmentFrontend.Models
{
    public class ProfileResponseDto
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public ProfileDto Data { get; set; }
    }
    public class UpdateUserResponseDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? Email { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
    public class UpdateProfileApiResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public UpdateUserResponseDto ProfileData { get; set; }
    }
}
