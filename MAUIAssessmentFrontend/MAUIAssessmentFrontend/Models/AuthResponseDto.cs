using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAssessmentFrontend.Models
{
    public class AuthResponseDto
    {
        public TokenDataDto Token { get; set; }
    }

    public class TokenDataDto
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public UserDto? UserData { get; set; }
    }

}
