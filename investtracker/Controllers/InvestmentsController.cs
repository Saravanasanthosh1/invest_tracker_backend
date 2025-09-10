using investtracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace investtracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<InvestmentsController> _logger;
        public InvestmentsController(AppDbContext context, ILogger<InvestmentsController> logger)
        {
            _context = context; 
            _logger = logger; 
        }

        [HttpGet("portfolio")]
        public async Task<IActionResult> GetPortfolio()
        {
            try
            {
                var portfolio = await _context.Portfolio.ToListAsync();
                _logger.LogInformation("Success");
                return Ok(portfolio);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception: {ex.Message}");
                return BadRequest(ex.Message);
            }
            //return Ok();
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthly()
        {
            try
            {
                var monthly = await _context.MonthlyCommitments.ToListAsync();
                _logger.LogInformation("Success");
                return Ok(monthly);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception: {ex.Message}");
                _logger.LogInformation("Failed");
                return BadRequest(ex.Message);
            }

            //return Ok();
        }
    }
}
