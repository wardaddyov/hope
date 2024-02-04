using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StayHard.Dto;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController: Controller
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;
    private readonly IStudentRepository _studentRepository;

    public CourseController(ICourseRepository courseRepository, IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(type: typeof(IEnumerable<Course>), statusCode: 200)]
    public IActionResult GetCourses()
    {
        var courses = _courseRepository.GetCourses();

        if (courses.Count == 0)
            return NotFound("No Courses Found");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(courses.Select(c => _mapper.Map<CourseDto>(c)));
    }
    
    [HttpGet("course/{courseId}")]
    public IActionResult GetCourse(int courseId)
    {
        if (!_courseRepository.CourseExists(courseId))
            return NotFound("Course Not Found");
        
        var course = _courseRepository.GetCourse(courseId);

        if (course == null)
            return NotFound("No Students Found");
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(_mapper.Map<CourseDto>(course));
    }
    
    [HttpGet("{courseName}")]
    [ProducesResponseType(type: typeof(IEnumerable<Course>), statusCode: 200)]
    public IActionResult GetCoursesByName(string courseName)
    {
        var courses = _courseRepository.GetCourse(courseName);

        if (courses.Count == 0)
            return NotFound("No Courses Found");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(courses.Select(c => _mapper.Map<CourseDto>(c)));
    }
    
    [HttpGet("courses/{semester}")]
    [ProducesResponseType(type: typeof(IEnumerable<Course>), statusCode: 200)]
    public IActionResult GetCoursesBySemester(int semester)
    {
        var courses = _courseRepository.GetCoursesBySemester(semester);

        if (courses.Count == 0)
            return NotFound("No Courses Found");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(courses.Select(c => _mapper.Map<CourseDto>(c)));
    }
    
    [HttpGet("{courseName}/{semester}")]
    [ProducesResponseType(type: typeof(IEnumerable<Course>), statusCode: 200)]
    public IActionResult GetCoursesByNameAndSemester(string courseName, int semester)
    {
        var courses = _courseRepository.GetCourse(courseName, semester);

        if (courses.Count == 0)
            return NotFound("No Courses Found");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(courses.Select(c => _mapper.Map<CourseDto>(c)));
    }

    [HttpGet("student/{courseId}")]
    public IActionResult GetStudentsByCourseId(int courseId)
    {
        if (!_courseRepository.CourseExists(courseId))
            return NotFound("Course Not Found");
        
        var students = _courseRepository.GetStudentsByCourse(courseId);

        if (students.Count == 0)
            return NotFound("No Students Found");
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(students.Select(s => _mapper.Map<StudentDto>(s)));
    }
    
    [HttpPost]
    public IActionResult CreateCourse([FromBody] CourseDto courseDto)
    {
        if (courseDto == null)
            return BadRequest();

        var course = _courseRepository.GetCourses()
            .Where(c => c.Name == courseDto.Name)
            .Where(c => c.Semester == courseDto.Semester)
            .Where(c => c.Group == courseDto.Group)
            .FirstOrDefault();

        if (course != null)
        {
            ModelState.AddModelError("", "Course Already Exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var courseObj = _mapper.Map<Course>(courseDto);


        if (!_courseRepository.CreateCourse(courseObj))
        {
            ModelState.AddModelError("", "Something went wrong while saving!");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
    
    [HttpPost("Enrolments/{studentId}/{courseId}")]
    public IActionResult CreateStudent([FromRoute] int studentId, int courseId)
    {
        var student = _studentRepository.GetStudents()
            .Where(s => s.Id == studentId)
            .FirstOrDefault();

        var course = _courseRepository.GetCourses()
            .Where(c => c.Id == courseId)
            .FirstOrDefault();
        

        
        if (student == null)
        {
            ModelState.AddModelError("", "Student Not Found");
            return StatusCode(404, ModelState);
        }   
        if (course == null)
        {
            ModelState.AddModelError("", "Course Not Found");
            return StatusCode(404, ModelState);
        }
        
        var enrolment = _courseRepository.GetStudentsByCourse(courseId)
            .Where(s => s.Id == studentId)
            .FirstOrDefault();
        
        if (enrolment != null)
        {
            ModelState.AddModelError("", "Student already applied for the course");
            return StatusCode(404, ModelState);
        }
        
        Enrolment enrolmentObj = new Enrolment();
        enrolmentObj.StudentId = studentId;
        enrolmentObj.CourseId = courseId;
        enrolmentObj.Course = course;
        enrolmentObj.Student = student;

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!_courseRepository.AddStudentToCourse(enrolmentObj))
        {
            ModelState.AddModelError("", "Something went wrong while saving!");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpDelete("Enrolments/{studentId}/{courseId}")]
    public IActionResult DeleteEnrolment([FromRoute] int studentId, int courseId)
    {
        var student = _courseRepository.GetStudentsByCourse(courseId)
            .Where(s => s.Id == studentId)
            .FirstOrDefault();
        
        if (student == null)
        {
            ModelState.AddModelError("", "Student Not Found");
            return StatusCode(404, ModelState);
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var enrolment = _courseRepository.GetEnrolments()
            .Where(e => e.CourseId == courseId)
            .Where(e => studentId == studentId)
            .FirstOrDefault();
        
        if (!_courseRepository.DeleteEnrolment(enrolment))
        {
            ModelState.AddModelError("", "Something went wrong while saving!");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}