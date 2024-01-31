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

    public CourseController(ICourseRepository courseRepository, IMapper mapper)
    {
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
}