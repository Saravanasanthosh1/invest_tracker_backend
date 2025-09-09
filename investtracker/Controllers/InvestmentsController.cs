using investtracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace investtracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public InvestmentsController(AppDbContext context) => _context = context;

        [HttpGet("portfolio")]
        public async Task<IActionResult> GetPortfolio()
        {
            var portfolio = await _context.Portfolio.ToListAsync();
             return Ok(portfolio);
            //return Ok();
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthly()
        {
            var monthly = await _context.MonthlyCommitments.ToListAsync();
            return Ok(monthly);
            //return Ok();
        }
    }
}
