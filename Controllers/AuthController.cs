using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
namespace FirstMVC_1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login()
    {
        string jwt = GenerateToken("vivek", "Admin");
        return Ok(new { token = jwt });
    }

    private string GenerateToken(string username, string role)
    {
        var claims = new[]
        {
         new Claim(ClaimTypes.Name,username),
         new Claim(ClaimTypes.Role, role),
         new Claim("permission", "Users.Read"),
         new Claim("permission", "Users.Create"),
         new Claim("permission", "Users.Update"),
         new Claim("permission", "Users.Delete")
        };


        var secret = _configuration["Jwt:Secret"];


        var key = new SymmetricSecurityKey(
            Convert.FromBase64String(secret)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}