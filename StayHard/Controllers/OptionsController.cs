using Microsoft.AspNetCore.Mvc;
using StayHard.Interfaces;

namespace StayHard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OptionsController : Controller
{
    private readonly IOptionsRepository _optionsRepository;

    public OptionsController(IOptionsRepository optionsRepository)
    {
        _optionsRepository = optionsRepository;
    }
    
    [HttpGet("status")]
    public IActionResult GetDatabaseConnectionStatus()
    {
        if (!_optionsRepository.GetDatabaseConnectionStatus())
        {
            return StatusCode(500, "Database connection failed");
        }

        return Ok("Database connection successful");
    }
    
    [HttpGet("migrate")]
    public IActionResult Migrate()
    {
        if (!_optionsRepository.Migrate())
        {
            return StatusCode(500, "Database Migration failed");
        }

        return Ok("Database Migration successful");
    }
    
}