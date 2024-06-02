using Asp.Versioning;
using Banking.App.Data;
using Banking.App.Data.Entities;
using Banking.App.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking.App.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]

    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] Account account)
        {
            account.CreatedDate = DateTime.UtcNow;
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAccountBalance), new { id = account.Id }, account);
        }

        [HttpPost("{id}/deposit")]
        public async Task<IActionResult> DepositMoney(int id, [FromBody] decimal amount)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            account.Balance += amount;
            await _context.SaveChangesAsync();
            return Ok(account);
        }

        [HttpPost("{id}/withdraw")]
        public async Task<IActionResult> WithdrawMoney(int id, [FromBody] decimal amount)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            if (account.Balance < amount)
            {
                return BadRequest("Insufficient funds");
            }
            account.Balance -= amount;
            await _context.SaveChangesAsync();
            return Ok(account);
        }

        [HttpGet("{id}/balance")]
        public async Task<IActionResult> GetAccountBalance(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account.Balance);
        }
    }

}
