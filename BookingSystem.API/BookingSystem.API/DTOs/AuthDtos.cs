using System.ComponentModel.DataAnnotations;

namespace BookingSystem.API.DTOs
{
    public class AuthDtos
    {
        public class LoginDto
        {
            [Required]
            public string Identifier { get; set; } //có thể dăng nhập bằng username hoặc email
            [Required]
            public string Password { get; set; }
        }

        public class RegisterDto
        {
            [Required]
            public string Username { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
            public string Fullname { get; set; }
           
        }

        public class AuthResponseDto
        {
            public string Token { get; set; }
            public string Role { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string AvatarUrl { get; set; }
        }
    }
}
