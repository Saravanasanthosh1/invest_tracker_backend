using investtracker.Data;
using investtracker.Helpers;
using investtracker.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace investtracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, IConfiguration config, ILogger<AuthController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
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
            _logger.LogInformation("Login request: " + request.UserId);
            var user = await _context.AuthUsers.FirstOrDefaultAsync(u => u.UserId == request.UserId);
            if (user == null) return Unauthorized("Invalid credentials");

            // Hash password with stored salt
            using var sha = SHA256.Create();
            var hashedInput = Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(request.Password + user.Salt)));

            if (hashedInput != user.PasswordHash)
                return Unauthorized("Invalid credentials");

            // ✅ Generate JWT token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
