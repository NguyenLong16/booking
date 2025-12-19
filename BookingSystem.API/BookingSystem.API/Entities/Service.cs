using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.API.Entities
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int Duration { get; set; } //Thời gian thực hiện 
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set;  }
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
