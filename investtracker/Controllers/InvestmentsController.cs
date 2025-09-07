using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestmentsController : ControllerBase
    {
        [HttpGet("portfolio")]
        public IActionResult GetPortfolio()
        {
            var portfolio = new[]
            {
                new { name = "Saravana Mutual Funds", value = 40 },
                new { name = "Saravana Chits", value = 20 },
                new { name = "Saravana FDs", value = 15 },
                new { name = "Saravana Gold", value = 15 },
                new { name = "Saravana Loans", value = 10 }
            };

            return Ok(portfolio);
        }

        [HttpGet("monthly")]
        public IActionResult GetMonthly()
        {
            var monthly = new[]
            {
                new { month = "Jan", amount = 400 },
                new { month = "Feb", amount = 300 },
                new { month = "Mar", amount = 500 }
            };

            return Ok(monthly);
        }
    }
}
