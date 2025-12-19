using System.ComponentModel.DataAnnotations;

namespace BookingSystem.API.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;    
        [Required]
        public string Email { get; set; } = string.Empty ;
        public string PasswordHash { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
