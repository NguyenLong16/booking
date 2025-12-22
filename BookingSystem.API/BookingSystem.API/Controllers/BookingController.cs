using BookingSystem.API.Data;
using BookingSystem.API.DTOs;
using BookingSystem.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto request)
        {
            // Validate: Không cho đặt lịch quá khứ
            if (request.BookingDate < DateTime.Now)
            {
                return BadRequest("Không thể đặt lịch trong quá khứ.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var booking = new Booking
            {
                CustomerName = request.CustomerName,
                PhoneNumber = request.PhoneNumber,
                Note = request.Note,
                BookingDate = request.BookingDate,
                ServiceId = request.ServiceId,
                Status = 0, // Mới tạo
                UserId = userId
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đặt lịch thành công! Chúng tôi sẽ liên hệ sớm." });
        }

        // GET: api/Booking/history
        [HttpGet("history")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Booking>>> GetHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Bạn chưa đăng nhập");
            }

            var bookings = await _context.Bookings
                .Include(b => b.Service)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return Ok(bookings);
        }
    }
}
