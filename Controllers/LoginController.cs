using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDoApp.Models;
using ToDoApp.Repositories;
using ToDoApp.Utilities;

namespace ToDoApp.Controllers;

[Route("api/[controller]")]
[ApiController]

public class LoginController : ControllerBase
{
     private IConfiguration _config;

     private IUsersRepository _users;

        public LoginController(IConfiguration config, IUsersRepository users)
        {
            _config = config;
            _users = users;
        }

        [AllowAnonymous]
        [HttpPost]

        public async Task<IActionResult> Login([FromBody] Users users)
        {
            var user = await Authenticate(users);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

    private string Generate(Users users)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, users.UserName),
                //new Claim(ClaimTypes.Email, users.Email),
                new Claim(ClaimTypes.SerialNumber, users.UserId.ToString()),
                
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private  async Task<Users> Authenticate(Users users)
    {
        var currentUser = await _users.GetByUserName(users.UserName);

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
    }
}