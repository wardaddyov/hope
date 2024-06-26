using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StayHard.Dto;
using StayHard.Interfaces;
using StayHard.Models;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation("get all the students")]
    [HttpGet]
    [ProducesResponseType(type: typeof(IEnumerable<Student>), statusCode: 200)]
    public IActionResult? GetStudents()
    {
        var students = _studentRepository.GetStudents();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(students.Select(s => _mapper.Map<StudentDto>(s)));
    }

    [SwaggerOperation("get students based on actual student id")]
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

    [SwaggerOperation("get student based on firstname and lastname")]
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

    [SwaggerOperation("create a new student")]
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
    
    [SwaggerOperation("update an existing student")]
    [HttpPut("{studentId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateStudent(int studentId, [FromBody]StudentDto updatedStudent)
    {
        if (updatedStudent == null)
            return BadRequest(ModelState);

        if (studentId != updatedStudent.Id)
            return BadRequest(ModelState);

        if (!_studentRepository.StudentExists(studentId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var studentObj = _mapper.Map<Student>(updatedStudent);
        studentObj.Username = studentObj.StudentID;
        studentObj.Password = "1234";

        if(!_studentRepository.UpdateStudent(studentObj))
        {
            ModelState.AddModelError("", "Something went wrong updating category");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    
    [SwaggerOperation("delete an exising student")]
    [HttpDelete("{studentId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteStudent(int studentId)
    {
        if(!_studentRepository.StudentExists(studentId))
        {
            return NotFound();
        }

        var student = _studentRepository.GetStudent(studentId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if(!_studentRepository.DeleteStudent(student))
        {
            ModelState.AddModelError("", "Something went wrong deleting student");
        }

        return NoContent();
    }
}