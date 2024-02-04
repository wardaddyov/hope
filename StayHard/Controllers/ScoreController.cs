using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StayHard.Dto;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScoreController : Controller
{
    private readonly IMapper _mapper;
    private readonly IScoreRepository _scoreRepository;

    public ScoreController(IScoreRepository scoreRepository, IMapper mapper)
    {
        _scoreRepository = scoreRepository;
        _mapper = mapper;
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(type: typeof(Question), statusCode: 200)]
    [ProducesResponseType(statusCode: 400)]
    public IActionResult GetScore(int id)
    {
        if (!_scoreRepository.ScoreExists(id))
            return NotFound();
        var score = _scoreRepository.GetScore(id);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(_mapper.Map<ScoreDto>(score));
    }
    
    [HttpPost]
    public IActionResult CreateScore([FromBody] ScorePostDto scorePostDto)
    {
        if (scorePostDto == null)
            return BadRequest();

        var score = _scoreRepository.GetScores()
            .Where(s => s.QuestionId == scorePostDto.QuestionId)
            .Where(s => s.StudentId == scorePostDto.StudentId)
            .FirstOrDefault();

        if (score != null)
        {
            ModelState.AddModelError("", "Question Already Exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var scoreObj = _mapper.Map<Score>(scorePostDto);

        if (!_scoreRepository.CreateScore(scoreObj))
        {
            ModelState.AddModelError("", "Something went wrong while saving!");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
}