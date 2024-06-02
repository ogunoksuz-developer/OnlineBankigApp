using Banking.App.Data;
using Banking.App.Data.Entities;
using Banking.App.Helpers;
using Banking.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace Banking.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(UserLogin userDto)
        {
            var hashedPassword = PasswordHasher.HashPassword(userDto.Password);

           var user =  new User { Username = userDto.Username, PasswordHash = hashedPassword,Role= "Admin" };

            _context.Users.Add(user);

            _context.SaveChanges();

            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        public IActionResult Login(UserLogin userDto)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == userDto.Username);

            if (user == null || !PasswordHasher.VerifyPassword(userDto.Password, user.PasswordHash))
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            var token = JwtTokenGenerator.GenerateToken(user.Username);
            return Ok(new { Token = token });
        }
    }
}
