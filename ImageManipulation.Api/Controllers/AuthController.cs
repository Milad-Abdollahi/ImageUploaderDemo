using ImageManipulation.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ImageManipulation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Validate if the user exists in DB
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.UserName && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate a JWT token
            var tokenString = GenerateJwtToken(user);
            return Ok(new { Token = tokenString });
        }

        private string GenerateJwtToken(User user)
        {
            var secretKey = _configuration["Jwt:Key"] ?? "InsecureKey123"; // fallback if config is missing
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            // Create security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username ?? string.Empty),
                new Claim("userId", user.Id.ToString())
            };

            // Create token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.Now.AddHours(1), // token valid for 1 hour
                signingCredentials: creds,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // DTO for login credentials
    public class LoginRequest
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
