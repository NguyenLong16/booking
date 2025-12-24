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
        public async Task<IActionResult> GetRevenueStats()
        {
            var startDate = DateTime.Now.AddDays(-30);

            // 1. Lấy dữ liệu thô
            var bookings = await _context.Bookings
                .Include(b => b.Service)
                .ThenInclude(s => s.Category) // Quan trọng: Phải join Category
                .Where(b => b.Status == 2 && b.BookingDate >= startDate)
                .ToListAsync();

            // 2. Xử lý dữ liệu cho biểu đồ cột chồng (Stacked Bar Chart)
            // Cấu trúc mong muốn: { date: "24/12", "Spa": 500000, "Cắt tóc": 200000 }
            var chartData = bookings
                .GroupBy(b => b.BookingDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => {
                    // Tạo dictionary động để chứa tên các category làm key
                    var dataPoint = new Dictionary<string, object>();
                    dataPoint["date"] = g.Key.ToString("dd/MM");

                    // Tính tổng tiền cho từng category trong ngày đó
                    var categoriesInDay = g.GroupBy(b => b.Service.Category?.Name ?? "Khác");
                    foreach (var catGroup in categoriesInDay)
                    {
                        dataPoint[catGroup.Key] = catGroup.Sum(b => b.Service.Price);
                    }

                    return dataPoint;
                })
                .ToList();

            // 3. Tính số liệu tổng quan (Giữ nguyên)
            var totalRevenueAllTime = await _context.Bookings.Where(b => b.Status == 2).SumAsync(b => b.Service.Price);
            var totalOrdersCompleted = await _context.Bookings.CountAsync(b => b.Status == 2);

            return Ok(new
            {
                ChartData = chartData, // Dữ liệu dạng động
                Summary = new
                {
                    TotalRevenue = totalRevenueAllTime,
                    TotalOrders = totalOrdersCompleted
                }
            });
        }

        // GET: api/Stats/category
        [HttpGet("category")]
        public async Task<IActionResult> GetCategoryStats()
        {
            // Thống kê số lượng đơn hàng hoàn thành (Status = 2) theo từng danh mục
            var stats = await _context.Bookings
                .Include(b => b.Service)
                .ThenInclude(s => s.Category) // Join sâu vào Category
                .Where(b => b.Status == 2) // Chỉ tính đơn hoàn thành
                .GroupBy(b => b.Service.Category.Name) // Nhóm theo tên danh mục
                .Select(g => new
                {
                    Name = g.Key ?? "Chưa phân loại", // Tên danh mục
                    Value = g.Count() // Số lượng đơn
                })
                .ToListAsync();

            return Ok(stats);
        }
    }
}
