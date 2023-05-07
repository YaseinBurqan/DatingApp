using System.Security.Cryptography;
using System.Text;
using API.Interfaces;
using API.Models;
using API.Models.Data;
using API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserTokenDTOsModel>> Register(
            [FromBody] UserRegistrationDTOsModel model
        )
        {
            if (await UserExists(model.Username))
                return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();
            var user = new User
            {
                Username = model.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserTokenDTOsModel
            {
                Username = model.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDTOsModel>> Login(
            [FromBody] UserLoginDTOsModel model
        )
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == model.Username);
            if (user == null)
                return Unauthorized("Invalid username or password");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var passwordHashed = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

            for (int i = 0; i < user.PasswordHash.Length; i++)
            {
                if (passwordHashed[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid username or password");
            }

            return new UserTokenDTOsModel
            {
                Username = model.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.Username == username.ToLower());
        }
    }
}
