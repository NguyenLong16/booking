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

            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

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
            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var bookings = await _context.Bookings
                .Include(b => b.Service)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpGet("admin/all")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
        {
            var booking = await _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return Ok(booking);
        }

        [HttpPut("admin/{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] int newStatus)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if(booking == null)
            {
                return NotFound("Không tìm thấy đơn hàng");
            }

            if(newStatus < 0 || newStatus > 3)
            {
                return BadRequest("Trạng thái không hợp lệ");
            }

            booking.Status = newStatus;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật trạng thái thành công" });
        }
    }
}
