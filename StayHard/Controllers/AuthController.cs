using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StayHard.Dto;
using StayHard.Interfaces;
using StayHard.Repository;

namespace StayHard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IAdminRepository _adminRepository;
    private readonly IAuthRepository _authRepository;

    public AuthController(IAdminRepository adminRepository, IAuthRepository authRepository)
    {
        _adminRepository = adminRepository;
        _authRepository = authRepository;
    }
    
    [HttpPost("login")]
    public IActionResult CreateAdmin([FromBody] AuthDto authDto)
    {
        if (authDto == null)
            return BadRequest();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var admin = _adminRepository.GetAdmin(authDto.Username);

        if (admin == null)
        {
            return Unauthorized("Username Not Found");
        }

        if (!BCrypt.Net.BCrypt.Verify(authDto.Password, admin.HashedPassword))
        {
            return Unauthorized("Password Incorrect");
        }

        var token = _authRepository.CreateAdminToken(admin);
        
        return Ok(token);
    }
}