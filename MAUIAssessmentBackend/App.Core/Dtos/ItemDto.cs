using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Dtos
{
    public class ItemDto
    {
       
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? ItemImage { get; set; } // For image upload
        public string? ItemImageUrl { get; set; } // For image URL
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
