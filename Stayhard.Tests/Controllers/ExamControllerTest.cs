using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using StayHard.Controllers;
using StayHard.Dto;
using StayHard.Interfaces;
using StayHard.Models;

namespace Stayhard.Tests.Controllers;

public class ExamControllerTest
{
    
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;
    private readonly IExamRepository _examRepository;
    private readonly ExamController _controller;

    public ExamControllerTest()
    {
        _examRepository = A.Fake<IExamRepository>();
        _courseRepository = A.Fake<ICourseRepository>();
        _mapper = A.Fake<IMapper>();
        _controller = new ExamController(_examRepository, _courseRepository, _mapper);
    }
    [Fact]
    public void ExamController_GetExamParticipants_ReturnsOK()
    {
        //Arrange
        Exam exam = A.Fake<Exam>();
        ExamParticipant examParticipant = A.Fake<ExamParticipant>();
        
        A.CallTo(() => _examRepository.GetExamParticipants(exam.Id)).Returns([examParticipant]);
        A.CallTo(() => _examRepository.ExamExists(exam.Id)).Returns(true);
        
        //Act
        var result = _controller.GetExamParticipants(exam.Id);
        
        //Assert
        result.Should().BeOfType(typeof(OkObjectResult));
    }

    [Fact]
    public void GetExamParticipants_OnNoStudents_ReturnNotFound()
    {
        //Arrange
        var exam = A.Fake<Exam>();
        var examParticipants = new List<ExamParticipant> {};
        A.CallTo(() => _examRepository.ExamExists(exam.Id)).Returns(true);
        A.CallTo(() => _examRepository.GetExamParticipants(exam.Id)).Returns(examParticipants);
        
        //Act
        var result = _controller.GetExamParticipants(exam.Id) as NotFoundObjectResult;
        
        //Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
        result.Value.Should().Be("No Students Found");
    }

    [Fact]
    public void GetExamParticipants_OnExamNotFound_ReturnNotFound()
    {
        //Arrange
        var exam = A.Fake<Exam>();
        A.CallTo(() => _examRepository.ExamExists(exam.Id)).Returns(false);
        
        //Act
        var result = _controller.GetExamParticipants(exam.Id) as NotFoundObjectResult;
        
        //Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
        result.Value.Should().Be($"No Exam with id {exam.Id} found");

    }

    [Fact]
    public void UpdateExamParticipants_OnExamNotFound_ReturnNotFound()
    {
        //Arrange
        var exam = A.Fake<Exam>();
        var participantsDto = A.Fake<List<ExamParticipantPostDto>>();
        A.CallTo(() => _examRepository.ExamExists(exam.Id)).Returns(false);
        
        //Act
        var result = _controller.UpdateExamParticipants(participantsDto,exam.Id);
        
        //Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
    }

    [Fact]
    public void UpdateExamParticipants_OnSuccess_ReturnOk()
    {
        //Arrange
        var exam = A.Fake<Exam>();
        var participantsDto = A.Fake<List<ExamParticipantPostDto>>();
        var participants = participantsDto.Select(p => _mapper.Map<ExamParticipant>(p)).ToList();
        A.CallTo(() => _examRepository.ExamExists(exam.Id)).Returns(true);
        A.CallTo(() => _examRepository.EditExamParticipant(participants)).Returns(true);
        
        //Act
        var result = _controller.UpdateExamParticipants(participantsDto, exam.Id);
        
        //Assert
        result.Should().BeOfType(typeof(OkResult));
    }

    [Fact]
    public void UpdateExamParticipants_OnSavedFailure_ReturnStatusCode500()
    {
        //Arrange
        var exam = A.Fake<Exam>();
        var participantsDto = A.Fake<List<ExamParticipantPostDto>>();
        var participants = participantsDto.Select(p => _mapper.Map<ExamParticipant>(p)).ToList();
        A.CallTo(() => _examRepository.ExamExists(exam.Id)).Returns(true);
        A.CallTo(() => _examRepository.EditExamParticipant(participants)).Returns(false);
        
        //Act
        var result = _controller.UpdateExamParticipants(participantsDto, exam.Id) as ObjectResult;
        
        //Assert
        result.StatusCode.Should().Be(500);
    }

    [Fact]
    public void UpdateExamParticipants_OnParticipantNotFound_ReturnNotFound()
    {
        //Arrange
        var exam = A.Fake<Exam>();
        var participantsDto = A.Fake<List<ExamParticipantPostDto>>();
        var participants = participantsDto.Select(p => _mapper.Map<ExamParticipant>(p)).ToList();
    }
}