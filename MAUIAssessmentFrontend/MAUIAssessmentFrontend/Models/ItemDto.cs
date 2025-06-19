using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAssessmentFrontend.Models
{
    public class ItemDto
    {
        public int Id { get; set; } // or Guid
        public string Name { get; set; }
        public string Image { get; set; } // image URL
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}
