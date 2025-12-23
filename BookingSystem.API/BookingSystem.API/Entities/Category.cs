using System.ComponentModel.DataAnnotations;

namespace BookingSystem.API.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // Ví dụ: "Chăm sóc da", "Tour Hạ Long", "Massage"

        public string? Description { get; set; }

        public string? ImageUrl { get; set; } // Ảnh đại diện cho danh mục

        // Navigation Property: Một danh mục chứa nhiều dịch vụ
        //public ICollection<Service> Services { get; set; }
    }
}
