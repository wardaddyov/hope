using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StayHard.Dto;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController :Controller
{
    private readonly IMapper _mapper;
    private readonly IQuestionRepository _questionRepository;

    public QuestionController(IQuestionRepository questionRepository, IMapper mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(type: typeof(Question), statusCode: 200)]
    [ProducesResponseType(statusCode: 400)]
    public IActionResult GetQuestion(int id)
    {
        if (!_questionRepository.QuestionExists(id))
            return NotFound();
        var question = _questionRepository.GetQuestion(id);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(_mapper.Map<QuestionDto>(question));
    }
    
    [HttpPost]
    public IActionResult CerateQuestion([FromBody] QuestionDto questionDto)
    {
        if (questionDto == null)
            return BadRequest();

        var question = _questionRepository.GetQuestions()
            .Where(q => q.ExamId == questionDto.ExamId)
            .Where(q => q.Number == questionDto.Number)
            .FirstOrDefault();

        if (question != null)
        {
            ModelState.AddModelError("", "Question Already Exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var questionObj = _mapper.Map<Question>(questionDto);

        if (!_questionRepository.CreateQuestion(questionObj))
        {
            ModelState.AddModelError("", "Something went wrong while saving!");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
}