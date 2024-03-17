using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly IConfiguration _configuration;

    public AuthRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string CreateAdminToken(Admin admin)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.Name, admin.Username),
            new Claim(ClaimTypes.Role, admin.AccessLevel.ToString())
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddSeconds(30),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}