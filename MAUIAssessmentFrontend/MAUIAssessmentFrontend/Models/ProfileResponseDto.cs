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
}
