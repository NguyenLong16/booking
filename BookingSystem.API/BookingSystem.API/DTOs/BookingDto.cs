using System.ComponentModel.DataAnnotations;

namespace BookingSystem.API.DTOs
{
    public class BookingDto
    {
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? Note { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public int ServiceId { get; set; }
    }
}
