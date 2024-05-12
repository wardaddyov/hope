using Microsoft.AspNetCore.Mvc;
using StayHard.Interfaces;
using StayHard.Models;
using Swashbuckle.AspNetCore.Annotations;

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
    
    [SwaggerOperation("used to confirm connection to the database")]
    [HttpGet("status")]
    public IActionResult GetDatabaseConnectionStatus()
    {
        if (!_optionsRepository.GetDatabaseConnectionStatus())
        {
            return StatusCode(500, "Database connection failed");
        }

        return Ok("Database connection successful");
    }
    
    [SwaggerOperation("used to apply migration to the database")]
    [HttpGet("migrate")]
    public IActionResult Migrate()
    {
        if (!_optionsRepository.Migrate())
        {
            return StatusCode(500, "Database Migration failed");
        }

        return Ok("Database Migration successful");
    }

    [SwaggerOperation("used to seed data into the database for testing purposes")]
    [HttpGet("seed")]
    public IActionResult Seed()
    {
        try
        {
            _optionsRepository.Seed();
            return Ok("Seed Successful");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Database Migration failed");
        }
    }
    
}