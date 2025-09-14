using investtracker.Data;
using investtracker.Helpers;
using investtracker.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace investtracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Models.LoginRequest request)
        {
            if (await _context.AuthUsers.AnyAsync(u => u.UserId == request.UserId))
                return BadRequest("User already exists.");

            var salt = PasswordHelper.GenerateSalt();
            var hash = PasswordHelper.ComputeHash(request.Password, salt);

            var user = new AuthUser
            {
                UserId = request.UserId,
                PasswordHash = hash,
                Salt = salt
            };

            _context.AuthUsers.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.LoginRequest request)
        {
            var user = await _context.AuthUsers.FirstOrDefaultAsync(u => u.UserId == request.UserId);
            if (user == null) return Unauthorized("Invalid user.");

            var hash = PasswordHelper.ComputeHash(request.Password, user.Salt);
            if (hash != user.PasswordHash) return Unauthorized("Invalid password.");

            // For now, return success (later you can generate JWT)
            return Ok("Login successful");
        }
    }
}
