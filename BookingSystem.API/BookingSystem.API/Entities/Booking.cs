using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.API.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty; // Tên khách

        [Required]
        public string PhoneNumber { get; set; } = string.Empty; // SĐT để liên hệ

        public string? Note { get; set; } // Ghi chú thêm

        public DateTime BookingDate { get; set; } // Ngày giờ khách muốn làm

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int Status { get; set; } = 0; // 0: Mới, 1: Đã xác nhận, 2: Hoàn thành, 3: Hủy

        // Khóa ngoại liên kết với Service
        public int ServiceId { get; set; }
        public string? UserId { get; set; }

        [ForeignKey("ServiceId")]
        public Service? Service { get; set; }
    }
}
