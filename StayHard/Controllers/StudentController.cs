using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StayHard.Dto;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : Controller
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public StudentController(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(type: typeof(IEnumerable<Student>), statusCode: 200)]
    public IActionResult? GetStudents()
    {
        var students = _studentRepository.GetStudents();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(students.Select(s => _mapper.Map<StudentDto>(s)));
    }

    [HttpGet("{studentId}")]
    [ProducesResponseType(type: typeof(Student), statusCode: 200)]
    [ProducesResponseType(statusCode: 400)]
    public IActionResult GetStudentById(string studentId)
    {
        if (!_studentRepository.StudentExists(studentId))
            return NotFound();
        var student = _studentRepository.GetStudent(studentId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(_mapper.Map<StudentDto>(student));
    }

    [HttpGet("{firstName}/{lastName}")]
    [ProducesResponseType(type: typeof(Student), statusCode: 200)]
    [ProducesResponseType(statusCode: 400)]
    public IActionResult GetStudentByName(string firstName, string lastName)
    {
        var students = _studentRepository.GetStudent(firstName, lastName);
        if (students.Count == 0)
            return NotFound();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(students.Select(s => _mapper.Map<StudentDto>(s)));
    }

    [HttpPost]
    public IActionResult CreateStudent([FromBody] StudentDto studentDto)
    {
        if (studentDto == null)
            return BadRequest();

        var student = _studentRepository.GetStudents()
            .Where(s => s.StudentID == studentDto.StudentID)
            .FirstOrDefault();

        if (student != null)
        {
            ModelState.AddModelError("", "Student Already Exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var studentObj = _mapper.Map<Student>(studentDto);
        studentObj.Username = studentObj.StudentID;
        studentObj.Password = "1234";

        if (!_studentRepository.CreateStudent(studentObj))
        {
            ModelState.AddModelError("", "Something went wrong while saving!");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
}