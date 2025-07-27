using Azure.Core;
using LinkShortner.Api.Models.DataTransferObjects;
using LinkShortner.Api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinkShortner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly LinkShortenerDbDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthController(LinkShortenerDbDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            // Checking if the user already exists
            if(await _dbContext.Users.AnyAsync(user => user.Email == registerRequestDTO.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            var newUser = new User
            {
                FirstName = registerRequestDTO.FirstName,
                Surname = registerRequestDTO.Surname,
                Email = registerRequestDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequestDTO.Password),
                DateRegistered = DateTime.UtcNow
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return Ok(new
            { 
                message = "Registration successfully." 
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == loginRequestDTO.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequestDTO.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            var accessToken = GenerateJwtToken(user);

            return Ok(new
            {
                message = "Login successful.",
                Token = accessToken,
                User = new {
                    user.FirstName,
                    user.Email
                }
            });
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Surname),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("hash-password/{password}")]
        public IActionResult HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);

            return Ok(new
            {
                originalPassword = password,
                bcryptHash = hashedPassword
            });                
        }
    }
}
