using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        var course = _courseRepository.GetCourse(courseDto.Name, courseDto.Semester, courseDto.Group);

        if (course != null)
        {
            return UnprocessableEntity("Course Already Exists");
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var courseObj = _mapper.Map<Course>(courseDto);
        
        return _courseRepository.CreateCourse(courseObj)
            ? Ok(courseObj.Id)
            : Problem(statusCode: 500, detail: "Something went wrong while saving!");
    }
    
    [HttpPost("Enrolments/{studentId}/{courseId}")]
    public IActionResult CreateEnrolment([FromRoute] int studentId, int courseId)
    {
        var student = _studentRepository.GetStudent(studentId);

        var course = _courseRepository.GetCourse(courseId);
        
        if (student == null)
        {
            return NotFound("Student Not Found");
        }   
        if (course == null)
        {
             return NotFound("Course Not Found");
        }

        var enrolment = _courseRepository.GetStudentByCourse(courseId, studentId);
        
        if (enrolment != null)
        {
            return UnprocessableEntity("Student already applied for the course");
        }

        var enrolmentObj = _courseRepository.CreateEnrolmentObject(course, student);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        bool status = _courseRepository.AddStudentToCourse(enrolmentObj);
        if(!status)
            return Problem(statusCode: 500, detail: "Something went wrong while saving!");
        return Ok("Successfully created");
    }
    
    [HttpPost("Enrolments/{courseId}")]
    public IActionResult CreateEnrolments([FromRoute] int courseId, [FromBody] List<int> studentIds)
    {        
        var course = _courseRepository.GetCourse(courseId);
        List<Enrolment> enrolmentObjs = [];
 
        if (course == null)
        {
            return NotFound("Course Not Found");
        }
        
        foreach (var id in studentIds)
        {
            var student = _studentRepository.GetStudent(id);
            
            if (student == null)
            {
                return NotFound("Student Not Found");
            }  
            
            var enrolment = _courseRepository.GetStudentByCourse(courseId, id);
        
            if (enrolment != null)
            {
                return UnprocessableEntity($"Student {enrolment.StudentId} already applied for the course");
            }
            
            var enrolmentObj = _courseRepository.CreateEnrolmentObject(course, student);
            enrolmentObjs.Add(enrolmentObj);
        }

        

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        bool status = _courseRepository.AddStudentsToCourse(enrolmentObjs);
        if(!status)
            return Problem(statusCode: 500, detail: "Something went wrong while saving!");
        return Ok("Successfully created");
    }

    [HttpDelete("Enrolments/{studentId}/{courseId}")]
    public IActionResult DeleteEnrolment([FromRoute] int studentId, int courseId)
    {
        var student = _studentRepository.GetStudent(studentId);
        
        if (student == null)
        {
            return NotFound("Student Not Found");
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var enrolment = _courseRepository.GetStudentByCourse(courseId, studentId);
        
        if (!_courseRepository.DeleteEnrolment(enrolment))
        {
           return Problem(statusCode: 500, detail: "Something went wrong while deleting!");
        }
        return NoContent();
    }
    
    [HttpDelete("deleteCourse/{courseId}")]
    public IActionResult DeleteCourse([FromRoute] int courseId)
    {
        var course = _courseRepository.GetCourse(courseId);
        
        if (course == null)
        {
            return NotFound("Course Not Found");
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!_courseRepository.DeleteCourse(course))
        {
           return Problem(statusCode: 500, detail: "Something went wrong while deleting!");
        }
        return NoContent();
    }
    
    [HttpPut("{courseId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCourse(int courseId, [FromBody]CourseDto course)
    {
        if (course == null)
            return BadRequest(ModelState);

        if (courseId != course.Id)
            return BadRequest(ModelState);

        if (!_courseRepository.CourseExists(courseId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var courseObj = _mapper.Map<Course>(course);

        if(!_courseRepository.UpdateCourse(courseObj))
        {
            ModelState.AddModelError("", "Something went wrong updating course");
            return StatusCode(500, ModelState);
        }

        return Ok();
    }
    
    [HttpPut("Enrolments/{courseId}")]
    public IActionResult UpdateEnrolments([FromRoute] int courseId, [FromBody] List<int> studentIds)
    {        
        var course = _courseRepository.GetCourse(courseId);
        List<Enrolment> enrolmentObjs = [];
 
        if (course == null)
        {
            return NotFound("Course Not Found");
        }
        
        // Create new enrolments
        foreach (var id in studentIds)
        {
            var student = _studentRepository.GetStudent(id);
            
            if (student == null)
            {
                return NotFound("Student Not Found");
            }  
            
            var enrolment = _courseRepository.GetStudentByCourse(courseId, id);
        
            if (enrolment != null)
            {
                enrolmentObjs.Add(enrolment);
                continue;
            }
            
            var enrolmentObj = _courseRepository.CreateEnrolmentObject(course, student);
            enrolmentObjs.Add(enrolmentObj);
        }
        
        // Remove old enrolments
        var oldEnrolments = _courseRepository.GetEnrolments(courseId);
        Console.WriteLine(oldEnrolments.Count);
        if (oldEnrolments.Count != 0)
        {
            bool deleteStatus = _courseRepository.DeleteEnrolments(oldEnrolments.ToList());
        
            if (deleteStatus == false)
                return Problem(statusCode: 500, detail: "Something went wrong while deleting old enrolments!");
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        bool status = _courseRepository.AddStudentsToCourse(enrolmentObjs);
        if(!status)
            return Problem(statusCode: 500, detail: "Something went wrong while saving!");
        return Ok("Successfully updated");
    }

}