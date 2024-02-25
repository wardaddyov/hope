using System.Collections;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using StayHard.Controllers;
using StayHard.Interfaces;
using StayHard.Models;

namespace Stayhard.Tests.Controllers;

public class CourseControllerTests
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;
    private readonly IStudentRepository _studentRepository;
    private readonly CourseController _controller;

    public CourseControllerTests()
    {
        _courseRepository = A.Fake<ICourseRepository>();
        _studentRepository = A.Fake<IStudentRepository>();
        _mapper = A.Fake<IMapper>();
        _controller = new CourseController(_courseRepository, _studentRepository, _mapper);
    }
    
    [Fact]
    public void CourseController_GetCourses_ReturnNotFound()
    {
        // Arrange
        var courses = new List<Course>(); // Create an empty list to pass it while mocking repository call
        A.CallTo(() => _courseRepository.GetCourses()).Returns(courses);
        
        // Act
        var result = _controller.GetCourses();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundObjectResult));
    }
    
    [Fact]
    public void CourseController_GetCourses_ReturnOk()
    {
        // Arrange
        var courses = new List<Course>();
        var course = A.Fake<Course>();
        courses.Add(course);
        
        A.CallTo(() => _courseRepository.GetCourses()).Returns(courses);
        
        // Act
        var result = _controller.GetCourses();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));
    }
}