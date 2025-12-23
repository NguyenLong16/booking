using System.ComponentModel.DataAnnotations;

namespace BookingSystem.API.DTOs
{
    public class ServiceDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        
        // --- SỬA ĐỔI ---
        public int Duration { get; set; }
        public string DurationUnit { get; set; } = "Phút"; // Mặc định là Phút
        public int? CategoryId { get; set; } // Chọn danh mục
        // ----------------

        public bool IsActive { get; set; }
        
        // Dùng cho trường hợp update, nếu không chọn ảnh mới thì giữ ảnh cũ
        public IFormFile? ImageUrl { get; set; }

    }
}
