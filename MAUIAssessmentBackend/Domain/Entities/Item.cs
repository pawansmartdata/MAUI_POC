using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }                        // Primary key
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ItemImageUrl { get; set; }= string.Empty;
        public double Latitude { get; set; }  
        public double Longitude { get; set; }

        public bool IsDeleted { get; set; }
    }
}
