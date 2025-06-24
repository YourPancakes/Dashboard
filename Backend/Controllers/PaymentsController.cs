using Backend.Entities;
using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("payments")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IGenericRepository<Payment> _repo;
        public PaymentsController(IGenericRepository<Payment> repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? take)
        {
            var list = await _repo.Query()
                .Include(p => p.Client)
                .OrderByDescending(p => p.Timestamp)
                .Take(take ?? 5)
                .Select(p => new PaymentDto(
                    p.Id, p.ClientId, p.AmountT, p.Timestamp,
                    new ClientDto(p.Client.Id, p.Client.Name, p.Client.Email, p.Client.BalanceT)
                ))
                .ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _repo.Query()
                .Include(p => p.Client)
                .Where(p => p.Id == id)
                .Select(p => new PaymentDto(
                    p.Id, p.ClientId, p.AmountT, p.Timestamp,
                    new ClientDto(p.Client.Id, p.Client.Name, p.Client.Email, p.Client.BalanceT)
                ))
                .FirstOrDefaultAsync();
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p is null) return NotFound();
            _repo.Remove(p);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
        {
            var entity = new Payment
            {
                ClientId = dto.ClientId,
                AmountT = dto.AmountT,
                Timestamp = dto.Timestamp
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            var saved = await _repo.Query()
                .Include(p => p.Client)
                .Where(p => p.Id == entity.Id)
                .Select(p => new PaymentDto(
                    p.Id,
                    p.ClientId,
                    p.AmountT,
                    p.Timestamp,
                    new ClientDto(p.Client.Id, p.Client.Name, p.Client.Email, p.Client.BalanceT)
                ))
                .FirstAsync();

            return CreatedAtAction(nameof(GetById), new { id = saved.Id }, saved);
        }
    }
}
