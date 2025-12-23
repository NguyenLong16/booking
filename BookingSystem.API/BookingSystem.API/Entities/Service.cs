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

        // --- SỬA ĐỔI PHẦN THỜI GIAN ---
        public int Duration { get; set; } // Vẫn giữ số lượng (Ví dụ: 30, 2, 4...)

        [MaxLength(20)]
        public string DurationUnit { get; set; } = "Phút"; // Đơn vị: "Phút", "Giờ", "Ngày", "Đêm"

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        // --- THÊM PHẦN LIÊN KẾT CATEGORY ---
        public int? CategoryId { get; set; } // Cho phép null nếu dịch vụ chưa phân loại (tùy bạn)

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}
