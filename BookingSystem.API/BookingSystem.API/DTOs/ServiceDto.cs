using System.ComponentModel.DataAnnotations;

namespace BookingSystem.API.DTOs
{
    public class ServiceDto
    {
        [Required(ErrorMessage ="Tên dịch vụ là bắt buộc")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, double.MaxValue, ErrorMessage ="Giá trị phải lớn hơn 0")]
        public decimal Price { get; set; }
        [Range(1, int.MaxValue, ErrorMessage ="Thời gian phải lớn hơn 1 phút")]
        public int Duration { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
