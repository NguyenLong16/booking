using BookingSystem.API.Data;
using BookingSystem.API.DTOs;
using BookingSystem.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ServiceController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _context.Services.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetServiceById(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if(service == null)
            {
                return NotFound("Dịch vụ không tồn tại");
            }

            return service;
        }

        [HttpPost]
        public async Task<ActionResult<Service>> createService([FromForm] ServiceDto request)
        {
            var service = new Service
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Duration = request.Duration,
                IsActive = request.IsActive,
            };

            if(request.ImageUrl != null)
            {
                service.ImageUrl = await SaveImage(request.ImageUrl);
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return Ok(new {message= "Tạo dịch vụ thành công", data = service});
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Service>> updateService(int id, [FromForm] ServiceDto request)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound("Dịch vụ không tồn tại");

            service.Name = request.Name;
            service.Description = request.Description;
            service.Price = request.Price;
            service.Duration = request.Duration;
            service.IsActive = request.IsActive;

            if(request.ImageUrl !=null)
            {
                service.ImageUrl = await SaveImage(request.ImageUrl);
            }

            await _context.SaveChangesAsync();
            return Ok(new {message="Cập nhật dịch vụ thành công", data = service});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound("Dịch vụ không tồn tại");
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string originalFileName = Path.GetFileName(imageFile.FileName);
            string uniqueFileName = $"{Guid.NewGuid()}_{originalFileName}";

            string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
            
            string filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return $"{baseUrl}/uploads/{uniqueFileName}";
        }
    }
}
