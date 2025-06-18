using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utility
{
    public class LoginResponseDto
    {
        public int Status { get; set; } // Represents HTTP status code
        public string Message { get; set; } // Contains the message to describe the response
        public Object Token { get; set; }
        public Object UserData { get; set; }
    }
}
