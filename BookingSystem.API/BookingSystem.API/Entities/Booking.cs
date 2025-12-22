using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.API.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Note { get; set; }

        public DateTime BookingDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Status { get; set; } = 0;

        // FK Service
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        // FK User
        public int UserId { get; set; }   
        public User User { get; set; } = null!;
    }
}
