using Backend.Entities;
using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("clients")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IGenericRepository<Client> _repo;
        public ClientsController(IGenericRepository<Client> repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.Query()
                .Include(c => c.Payments)
                .Select(c => new ClientWithPaymentsDto(
                    c.Id, c.Name, c.Email, c.BalanceT,
                    c.Payments.Select(p => new PaymentIdDto(p.Id)).ToList()
                ))
                .ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _repo.Query()
                .Include(c => c.Payments)
                .Where(c => c.Id == id)
                .Select(c => new ClientWithPaymentsDto(
                    c.Id, c.Name, c.Email, c.BalanceT,
                    c.Payments.Select(p => new PaymentIdDto(p.Id)).ToList()
                ))
                .FirstOrDefaultAsync();
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientUpsertDto dto)
        {
            var c = new Client { Name = dto.Name, Email = dto.Email, BalanceT = dto.BalanceT };
            await _repo.AddAsync(c);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientUpsertDto dto)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c is null) return NotFound();
            c.Name = dto.Name;
            c.Email = dto.Email;
            c.BalanceT = dto.BalanceT;
            _repo.Update(c);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c is null) return NotFound();
            _repo.Remove(c);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id:int}/payments")]
        public async Task<IActionResult> GetPayments(int id)
        {
            var history = await _repo.Query()
                .Where(c => c.Id == id)
                .SelectMany(c => c.Payments)
                .OrderByDescending(p => p.Timestamp)
                .Select(p => new PaymentDto(
                    p.Id, p.ClientId, p.AmountT, p.Timestamp,
                    new ClientDto(p.Client.Id, p.Client.Name, p.Client.Email, p.Client.BalanceT)
                ))
                .ToListAsync();
            return history.Any() ? Ok(history) : NotFound();
        }
    }
}
