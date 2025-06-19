using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAssessmentFrontend.Models
{
    public class ItemResponseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ItemImageUrl { get; set; } // For image URL
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
