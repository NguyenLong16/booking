using BookingSystem.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StatsController (AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("revenue")]
        // [Authorize(Roles = "Admin")] // Bật lại khi bạn đã có phân quyền
        public async Task<IActionResult> GetRevenueStats()
        {
            // 1. Lấy danh sách các đơn hàng ĐÃ HOÀN THÀNH (Status = 2)
            // Lấy dữ liệu của 30 ngày gần nhất
            var startDate = DateTime.Now.AddDays(-30);

            var bookings = await _context.Bookings
                .Include(b => b.Service)
                .Where(b => b.Status == 2 && b.BookingDate >= startDate)
                .ToListAsync(); // Tải về bộ nhớ để xử lý GroupBy cho dễ (tránh lỗi EF Core translation)

            // 2. Nhóm theo ngày và tính tổng tiền
            var revenueByDate = bookings
                .GroupBy(b => b.BookingDate.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("dd/MM"), // Format ngày để hiển thị trục hoành
                    TotalRevenue = g.Sum(b => b.Service?.Price ?? 0), // Tổng tiền
                    Count = g.Count() // Số lượng đơn
                })
                .OrderBy(x => x.Date)
                .ToList();

            // 3. Tính các số liệu tổng quan (Card ở trên cùng)
            var totalRevenueAllTime = await _context.Bookings
                .Where(b => b.Status == 2)
                .SumAsync(b => b.Service.Price);

            var totalOrdersCompleted = await _context.Bookings
                .CountAsync(b => b.Status == 2);

            return Ok(new
            {
                ChartData = revenueByDate,
                Summary = new
                {
                    TotalRevenue = totalRevenueAllTime,
                    TotalOrders = totalOrdersCompleted
                }
            });
        }
    }
}
