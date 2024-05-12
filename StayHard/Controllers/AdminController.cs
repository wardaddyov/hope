using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayHard.Dto;
using StayHard.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace StayHard.Controllers;

[ApiController]

[Route("api/[controller]")]
public class AdminController : Controller
{
    private readonly IMapper _mapper;
    private readonly IAdminRepository _adminRepository;

    public AdminController(IMapper mapper, IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
        _mapper = mapper;
    }
    
    [Authorize(Roles = "3")]
    [SwaggerOperation("Get all the available admins")]
    [HttpGet]
    public IActionResult GetAdmins()
    {
        var admins = _adminRepository.GetAdmins();

        if (admins.Count == 0)
            return NotFound("No Admins Found");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(admins.Select(a => _mapper.Map<AdminGetDto>(a)));
    }
    
    [SwaggerOperation("create new admins")]
    [HttpPost]
    public IActionResult CreateAdmin([FromBody] AdminCreateDto adminCreateDto)
    {
        if (adminCreateDto == null)
            return BadRequest();

        var adminExists = _adminRepository.AdminExists(adminCreateDto.Username);

        if (adminExists)
        {
            return UnprocessableEntity("Course Already Exists");
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var adminObj = _adminRepository.CreateAdminObject(adminCreateDto);
        
        return _adminRepository.CreateAdmin(adminObj)
            ? Ok("Successfully created")
            : Problem(statusCode: 500, detail: "Something went wrong while saving!");
    }
    
    [SwaggerOperation("Delete an admin with id")]
    [HttpDelete("admins/delete/{adminId}")]
    public IActionResult DeleteAdmin([FromRoute] int adminId)
    {
        var adminExists = _adminRepository.AdminExists(adminId);
        
        if (!adminExists)
        {
            return NotFound("Student Not Found");
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var admin = _adminRepository.GetAdmin(adminId);
        
        if (!_adminRepository.DeleteAdmin(admin))
        {
            return Problem(statusCode: 500, detail: "Something went wrong while deleting!");
        }
        return NoContent();
    }
}