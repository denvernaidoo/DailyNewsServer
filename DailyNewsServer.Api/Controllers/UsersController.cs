using BCrypt.Net;
using DailyNewsServer.Core.Interfaces;
using DailyNewsServer.Core.Models;
using DailyNewsServer.Core.Models.Config;
using DailyNewsServer.Core.Models.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DailyNewsServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private AppSettings _appSettings;
        private IUserService _userService;

        public UsersController(IOptions<AppSettings> appSettings, IUserService userService)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            var user = _userService.GetUserByEmailAddress(login.Username);
            if (user == null)
            {
                return Unauthorized();
            }
            bool verified = _userService.Verify(login.Password,user.PasswordHash);
            if (!verified)
            {
                return Unauthorized();
            }

            var response = new LoginResponse
            {
                UserId = user.UserId,
                Username = user.EmailAddress,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Role = user.Role.Description,
                Token = GenerateToken(user)
            };

            return new OkObjectResult(response);
        }

        private string GenerateToken(User user)
        {
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role.Description)
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

            var Token = new JwtSecurityToken(
                "https://csharpapi.com",
                "https://csharpapi.com",
                Claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
