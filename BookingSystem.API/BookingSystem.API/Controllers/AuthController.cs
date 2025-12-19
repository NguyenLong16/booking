using BookingSystem.API.Data;
using BookingSystem.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BookingSystem.API.DTOs.AuthDtos;

namespace BookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto request)
        {
            if(await _context.Users.AnyAsync(u => u.Username == request.Username)){
                return BadRequest("Username đã tồn tại");
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email đã được sử dụng.");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                Fullname = request.Fullname,
                Role = "User",
                AvatarUrl = "https://api.dicebear.com/7.x/initials/svg?seed=Long",
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công" });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Username == request.Identifier || u.Email == request.Identifier);

            if(user == null)
            {
                return Unauthorized("Tài khoản không tồn tại");
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Mật khẩu không đúng");
            }

            string token = CreateToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Role = user.Role,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl
            });
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
