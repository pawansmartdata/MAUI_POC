using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAUIAssessmentFrontend.Models
{
    public class ProfileDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [JsonPropertyName("profileImagePath")]
        public string ProfileImage { get; set; }
    }
}
