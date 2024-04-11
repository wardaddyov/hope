using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StayHard.Dto;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamController: Controller
{
    private readonly IMapper _mapper;
    private readonly IExamRepository _examRepository;
    private readonly ICourseRepository _courseRepository;

    public ExamController(IExamRepository examRepository, ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _examRepository = examRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(type: typeof(IEnumerable<Exam>), statusCode: 200)]
    public IActionResult GetExams()
    {
        var exams = _examRepository.GetExams();

        if (exams.Count == 0)
            return NotFound("No Exams Found");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(exams.Select(e => _mapper.Map<ExamDto>(e)));
    }
    
    [HttpGet("{examId}")]
    [ProducesResponseType(type: typeof(Exam), statusCode: 200)]
    public IActionResult GetExam(int examId)
    {
        var exam = _examRepository.GetExam(examId);

        if (exam == null)
            return NotFound("No Exams Found");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(_mapper.Map<ExamDto>(exam));
    }
    
    [HttpGet("course/{courseId}")]
    [ProducesResponseType(type: typeof(IEnumerable<Exam>), statusCode: 200)]
    public IActionResult GetExams(int courseId)
    {
        var exams = _examRepository.GetExams(courseId);

        if (exams.Count == 0)
            return NotFound("No Exams Found");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(exams.Select(e => _mapper.Map<ExamDto>(e)));
    }
    
    [HttpGet("students/{examId}")]
    [ProducesResponseType(type: typeof(Exam), statusCode: 200)]
    public IActionResult GetExamParticipants(int examId)
    {
        var students = _examRepository.GetExamParticipants(examId);

        if (students.Count == 0)
            return NotFound("No Students Found");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(students.Select(s=> _mapper.Map<StudentDto>(s)));
    }
    
    [HttpPost]
    public IActionResult CreateExam([FromBody] ExamDto examDto)
    {
        if (examDto == null)
            return BadRequest();

        var exam = _examRepository.GetExams()
            .Where(e => e.Name == examDto.Name)
            .Where(e => e.CourseId == examDto.CourseId)
            .FirstOrDefault();

        var courseExists = _courseRepository.CourseExists(examDto.CourseId);

        if (courseExists == false)
        {
            ModelState.AddModelError("", "Course Not found");
            return StatusCode(404, ModelState);
        }

        if (exam != null)
        {
            ModelState.AddModelError("", "Exam Already Exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var examObj = _mapper.Map<Exam>(examDto);
        
        // todo: confirm file exists , error if id not available

        if (!_examRepository.CreateExam(examObj))
        {
            ModelState.AddModelError("", "Something went wrong while saving!");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
    
    [HttpDelete("deleteExam/{examId}")]
    public IActionResult DeleteExam([FromRoute] int examId)
    {
        var exam = _examRepository.GetExam(examId);
        
        if (exam == null)
        {
            return NotFound("Exam Not Found");
        }
        // todo: delete file also
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!_examRepository.RemoveExam(exam))
        {
            return Problem(statusCode: 500, detail: "Something went wrong while deleting!");
        }
        return NoContent();
    }
    
    [HttpPut("{examId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateExam(int examId, [FromBody]ExamDto examDto)
    {
        if (examDto == null)
            return BadRequest(ModelState);

        if (examId != examDto.Id)
            return BadRequest(ModelState);

        if (!_examRepository.ExamExists(examId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var examObj = _mapper.Map<Exam>(examDto);

        if(!_examRepository.EditExam(examObj))
        {
            ModelState.AddModelError("", "Something went wrong updating course");
            return StatusCode(500, ModelState);
        }

        return Ok();
    }
    
    // Exam file controllers
    
    [HttpPost("examFile/create")]
    public IActionResult CreateExamFile([FromBody] ExamFileDto examFileDto)
    {
        if (examFileDto == null)
            return BadRequest();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var examFileObj = _mapper.Map<ExamQuestionFile>(examFileDto);

        if (!_examRepository.CreateFile(examFileObj))
        {
            ModelState.AddModelError("", "Something went wrong while saving!");
            return StatusCode(500, ModelState);
        }

        return Ok(examFileObj.Id);
    }
    
    
    [HttpGet("examFile")]
    [ProducesResponseType(type: typeof(Exam), statusCode: 200)]
    public IActionResult GetExamFiles()
    {
        var exam = _examRepository.GetExamFiles();

        if (exam == null)
            return NotFound("No Exams Found");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var examFiles = _examRepository.GetExamFiles();

        return Ok(examFiles);
    }
    
    [HttpDelete("examFile/{fileId}")]
    public IActionResult DeleteExamFile([FromRoute] int fileId)
    {
        var examFile = _examRepository.GetExamFile(fileId);
        
        if (examFile == null)
        {
            return NotFound("Exam File Not Found");
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!_examRepository.RemoveFile(examFile))
        {
            return Problem(statusCode: 500, detail: "Something went wrong while deleting!");
        }
        return NoContent();
    }
}