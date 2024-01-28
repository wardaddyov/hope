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
    
    [HttpGet("question/{examId}")]
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
}