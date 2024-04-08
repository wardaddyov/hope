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

    public ExamController(IExamRepository examRepository, IMapper mapper)
    {
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
    
    [HttpGet("questions/{examId}")]
    [ProducesResponseType(type: typeof(Exam), statusCode: 200)]
    public IActionResult GetQuestionsByExam(int examId)
    {
        var questions = _examRepository.GetExamQuestions(examId);

        if (questions.Count == 0)
            return NotFound("No Questions Found");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(questions.Select(q=> _mapper.Map<QuestionDto>(q)));
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
            .Where(e => e.Date == examDto.Date)
            .Where(e => e.Name == examDto.Name)
            .Where(e => e.CourseId == examDto.CourseId)
            .FirstOrDefault();

        if (exam != null)
        {
            ModelState.AddModelError("", "Exam Already Exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var examObj = _mapper.Map<Exam>(examDto);

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
    
}