using Backend.Entities;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("rate")]
    [Authorize]
    public class RateController : ControllerBase
    {
        private readonly IGenericRepository<Rate> _repo;
        public RateController(IGenericRepository<Rate> repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetCurrent()
        {
            var last = (await _repo.GetAllAsync())
                .OrderByDescending(r => r.UpdatedAt)
                .FirstOrDefault();
            return last is null ? NotFound() : Ok(last);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Rate dto)
        {
            dto.UpdatedAt = DateTime.UtcNow;
            await _repo.AddAsync(dto);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
    }
}
