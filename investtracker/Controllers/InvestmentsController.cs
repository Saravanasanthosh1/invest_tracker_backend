using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestmentsController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetInvestments()
        {
            var data = new[] {
                new { Id = 1, Type = "Mutual Fund", Amount = 1000 },
                new { Id = 2, Type = "FD", Amount = 500 }
            };
            return Ok(data);
        }
    }
}
