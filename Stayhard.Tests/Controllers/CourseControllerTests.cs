using System.Collections;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using StayHard.Controllers;
using StayHard.Dto;
using StayHard.Interfaces;
using StayHard.Models;

namespace Stayhard.Tests.Controllers;

public class CourseControllerTests
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;
    private readonly IStudentRepository _studentRepository;
    private readonly CourseController _controller;
    private readonly List<Course> _emptyCourseList;
    private readonly List<Course> _seededCourseList;


    public CourseControllerTests()
    {
        _courseRepository = A.Fake<ICourseRepository>();
        _studentRepository = A.Fake<IStudentRepository>();
        _mapper = A.Fake<IMapper>();
        _controller = new CourseController(_courseRepository, _studentRepository, _mapper);

        _emptyCourseList = new List<Course>();
        _seededCourseList = new List<Course>() { A.Fake<Course>() };
    }

    [Fact]
    public void CourseController_GetCourses_ReturnNotFound()
    {
        // Arrange
        A.CallTo(() => _courseRepository.GetCourses()).Returns(_emptyCourseList);

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
        A.CallTo(() => _courseRepository.GetCourses()).Returns(_seededCourseList);

        // Act
        var result = _controller.GetCourses();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));
    }

    [Fact]
    // Course with given id does not exist
    public void CourseController_GetCourse_ReturnNotFound()
    {
        // Arrange
        const bool courseExists = false;
        A.CallTo(() => _courseRepository.CourseExists(A.Fake<Course>().Id)).Returns(courseExists);

        // Act
        var result = _controller.GetCourse(A.Fake<Course>().Id);

        // Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
    }

    [Fact]
    public void CourseController_GetCourse_ReturnOk()
    {
        // Arrange
        const bool courseExists = true;
        A.CallTo(() => _courseRepository.CourseExists(A.Fake<Course>().Id)).Returns(courseExists);

        // Act
        var result = _controller.GetCourse(A.Fake<Course>().Id);

        // Assert
        result.Should().BeOfType(typeof(OkObjectResult));
    }

    [Fact]
    // Course with given name does not exist
    public void CourseController_GetCoursesByName_ReturnNotFound()
    {
        // Arrange
        A.CallTo(() => _courseRepository.GetCourses()).Returns(_emptyCourseList);

        // Act
        var result = _controller.GetCoursesByName("FakeName");

        // Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
    }

    [Fact]
    public void CourseController_GetCoursesByName_ReturnOk()
    {
        // Arrange
        var course = A.Fake<Course>();
        A.CallTo(() => _courseRepository.GetCourse(course.Name)).Returns(_seededCourseList);

        // Act
        var result = _controller.GetCoursesByName(course.Name);

        // Assert
        result.Should().BeOfType(typeof(OkObjectResult));
    }

    [Fact]
    // No courses exist in the current semester
    public void CourseController_GetCoursesBySemester_ReturnNotFound()
    {
        // Arrange
        A.CallTo(() => _courseRepository.GetCoursesBySemester(1234)).Returns(_emptyCourseList);

        // Act
        var result = _controller.GetCoursesBySemester(1234);

        // Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
    }

    [Fact]
    public void CourseController_GetCoursesBySemester_ReturnOk()
    {
        // Arrange
        A.CallTo(() => _courseRepository.GetCoursesBySemester(1234)).Returns(_seededCourseList);

        // Act
        var result = _controller.GetCoursesBySemester(1234);

        // Assert
        result.Should().BeOfType(typeof(OkObjectResult));
    }

    [Fact]
    // No courses exist in the current semester with this name
    public void CourseController_GetCoursesByNameAndSemester_ReturnNotFound()
    {
        // Arrange
        var course = A.Fake<Course>();
        A.CallTo(() => _courseRepository.GetCourse(course.Name, 1234)).Returns(_emptyCourseList);

        // Act
        var result = _controller.GetCoursesByNameAndSemester(course.Name, 1234);

        // Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
    }

    [Fact]
    public void CourseController_GetCoursesByNameAndSemester_ReturnOk()
    {
        // Arrange
        var course = A.Fake<Course>();
        A.CallTo(() => _courseRepository.GetCourse(course.Name, 1234)).Returns(_seededCourseList);

        // Act
        var result = _controller.GetCoursesByNameAndSemester(course.Name, 1234);

        // Assert
        result.Should().BeOfType(typeof(OkObjectResult));
    }

    [Fact]
    // No courses found with this id
    public void CourseController_GetStudentsByCourseId_ReturnCourseNotFound()
    {
        // Arrange
        const bool courseExists = false;
        A.CallTo(() => _courseRepository.CourseExists(1234)).Returns(courseExists);
        // Act
        var result = _controller.GetStudentsByCourseId(1234);

        // Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
    }

    [Fact]
    // No students found in this course
    public void CourseController_GetStudentsByCourseId_ReturnStudentsNotFound()
    {
        // Arrange
        const bool courseExists = true;
        var course = A.Fake<Course>();
        var students = new List<Student>();

        A.CallTo(() => _courseRepository.CourseExists(course.Id)).Returns(courseExists);
        A.CallTo(() => _courseRepository.GetStudentsByCourse(course.Id)).Returns(students);

        // Act
        var result = _controller.GetStudentsByCourseId(course.Id);

        // Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
    }

    [Fact]
    // No students found in this course
    public void CourseController_GetStudentsByCourseId_ReturnOk()
    {
        // Arrange
        const bool courseExists = true;
        var course = A.Fake<Course>();
        var students = new List<Student>() { A.Fake<Student>() };

        A.CallTo(() => _courseRepository.CourseExists(course.Id)).Returns(courseExists);
        A.CallTo(() => _courseRepository.GetStudentsByCourse(course.Id)).Returns(students);

        // Act
        var result = _controller.GetStudentsByCourseId(course.Id);

        // Assert
        result.Should().BeOfType(typeof(OkObjectResult));
    }

    [Fact]
    // Course Already exists
    public void CourseController_CreateCourse_ReturnUnprocessableEntity()
    {
        //Arrange
        var courseDto = A.Fake<CourseDto>();
        var course = A.Fake<Course>();
        
        A.CallTo(() => _courseRepository.GetCourse(courseDto.Name, courseDto.Semester, courseDto.Group)).Returns(course);
        
        //Act
        var result = _controller.CreateCourse(courseDto);
        
        //Assert
        result.Should().BeOfType(typeof(UnprocessableEntityObjectResult));
    }
    
    [Fact]
    // Course couldn't be created
    public void CourseController_CreateCourse_ReturnProblem()
    {
        //Arrange
        var courseDto = A.Fake<CourseDto>();
        
        A.CallTo(() => _courseRepository.GetCourse(courseDto.Name, courseDto.Semester, courseDto.Group)).Returns(null);
        A.CallTo(() => _courseRepository.CreateCourse(A.Fake<Course>())).Returns(false);
        
        //Act
        var result = _controller.CreateCourse(courseDto);
        
        //Assert
        result.Should().BeOfType(typeof(ObjectResult));
    }
    
    [Fact]
    // Ok
    public void CourseController_CreateCourse_ReturnOk()
    {
        //Arrange
        var courseDto = A.Fake<CourseDto>();
        var courseObj = A.Fake<Course>();
        
        A.CallTo(() => _courseRepository.GetCourse(courseDto.Name, courseDto.Semester, courseDto.Group)).Returns(null);
        A.CallTo(() => _mapper.Map<Course>(courseDto)).Returns(courseObj);
        A.CallTo(() => _courseRepository.CreateCourse(courseObj)).Returns(true);

        
        //Act
        var result = _controller.CreateCourse(courseDto);
        
        //Assert
        result.Should().BeOfType(typeof(OkObjectResult));
    }
    
    [Fact]
    // No students found
    public void CourseController_CreateStudent_ReturnStudentNotFound()
    {
        //Arrange
        const int studentId = 0;
        const int courseId = 0;
        A.CallTo(() => _studentRepository.GetStudent(studentId)).Returns(null);
        
        //Act
        var result = _controller.CreateStudent(studentId, courseId);
        
        //Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
    }
    
    [Fact]
    // No course found
    public void CourseController_CreateStudent_ReturnCourseNotFound()
    {
        //Arrange
        const int studentId = 0;
        const int courseId = 0;
        A.CallTo(() => _courseRepository.GetCourse(courseId)).Returns(null);
        
        //Act
        var result = _controller.CreateStudent(studentId, courseId);
        
        //Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
    }
    
    [Fact]
    // Student Already Enrolled
    public void CourseController_CreateStudent_ReturnUnprocessableEntity()
    {
        //Arrange
        const int studentId = 0;
        const int courseId = 0;
        var enrolment = A.Fake<Enrolment>();
        A.CallTo(() => _courseRepository.GetStudentByCourse(courseId, studentId)).Returns(enrolment);
        
        //Act
        var result = _controller.CreateStudent(studentId, courseId);
        
        //Assert
        result.Should().BeOfType(typeof(UnprocessableEntityObjectResult));
    }

    [Fact]
    // Couldn't be created
    public void CourseController_CreateStudent_ReturnObjectResult()
    {
        //Arrange
        var student = A.Fake<Student>();
        var course = A.Fake<Course>();
        var enrolment = A.Fake<Enrolment>();
        var enrolmentObj = A.Fake<Enrolment>();
        
        
        A.CallTo(() => _courseRepository.GetStudentByCourse(course.Id, student.Id)).Returns(null);
        A.CallTo(() => _courseRepository.CreateEnrolment(course, student)).Returns(enrolmentObj);
        //A.CallTo(() => _courseRepository.AddStudentToCourse(enrolmentObj)).Returns(false);
        
        //Act
        var result = _controller.CreateStudent(student.Id, course.Id);
        
        //Assert
        result.Should().BeOfType(typeof(ObjectResult));
        
        
    }
    
    /* The method is actually functional and works as expected in production But the unit test is not working, I don't know why */
    /******  Wooooooooorked finally ******/
    [Fact]
    public void CourseController_CreateStudent_ReturnOk()
    {
        //Arrange
        var student = A.Fake<Student>();
        var course = A.Fake<Course>();
        var enrolmentObj = new Enrolment()
        {
            Course = course,
            CourseId = course.Id,
            Student = student,
            StudentId = student.Id
        };

        A.CallTo(() => _studentRepository.GetStudent(student.Id)).Returns(student);
        A.CallTo(() => _courseRepository.GetCourse(course.Id)).Returns(course);
        A.CallTo(() => _courseRepository.GetStudentByCourse(course.Id, student.Id)).Returns(null);
        A.CallTo(() => _courseRepository.CreateEnrolment(course, student)).Returns(enrolmentObj);
        A.CallTo(() => _courseRepository.AddStudentToCourse(enrolmentObj)).Returns(true);
        
        //Act
        var result = _controller.CreateStudent(student.Id, course.Id);
        
        //Assert
        result.Should().BeOfType(typeof(OkObjectResult));
    }
    
    [Fact]
    public void CourseController_DeleteEnrolment_ReturnNoContent()
    {
        // Arrange
        var student = A.Fake<Student>();
        var course = A.Fake<Course>();
        var enrolment = new Enrolment()
        {
            Student = student,
            Course = course,
            StudentId = student.Id,
            CourseId = course.Id
        };
        
        A.CallTo(() => _studentRepository.GetStudent(enrolment.StudentId)).Returns(enrolment.Student);
        A.CallTo(() => _courseRepository.GetStudentByCourse(enrolment.CourseId, enrolment.StudentId))
            .Returns(enrolment);
        A.CallTo(() => _courseRepository.DeleteEnrolment(enrolment)).Returns(true);
        // Act
        var result = _controller.DeleteEnrolment(enrolment.StudentId, enrolment.CourseId);
        // Assert
        result.Should().BeOfType(typeof(NoContentResult));
    }
    
    [Fact]
    // No student found
    public void CourseController_DeleteEnrolment_ReturnNoStudentFound()
    {
        // Arrange
        var student = A.Fake<Student>();
        var course = A.Fake<Course>();
        var enrolment = new Enrolment()
        {
            Student = student,
            Course = course,
            StudentId = student.Id,
            CourseId = course.Id
        };
        
        A.CallTo(() => _studentRepository.GetStudent(enrolment.StudentId)).Returns(null);
        A.CallTo(() => _courseRepository.GetStudentByCourse(enrolment.CourseId, enrolment.StudentId))
            .Returns(enrolment);
        A.CallTo(() => _courseRepository.DeleteEnrolment(enrolment)).Returns(true);
        // Act
        var result = _controller.DeleteEnrolment(enrolment.StudentId, enrolment.CourseId);
        // Assert
        result.Should().BeOfType(typeof(NotFoundObjectResult));
    }
    
    [Fact]
    // Couldn't delete
    public void CourseController_DeleteEnrolment_ReturnObjectResult()
    {
        // Arrange
        var student = A.Fake<Student>();
        var course = A.Fake<Course>();
        var enrolment = new Enrolment()
        {
            Student = student,
            Course = course,
            StudentId = student.Id,
            CourseId = course.Id
        };
        
        A.CallTo(() => _studentRepository.GetStudent(enrolment.StudentId)).Returns(student);
        A.CallTo(() => _courseRepository.GetStudentByCourse(enrolment.CourseId, enrolment.StudentId))
            .Returns(enrolment);
        A.CallTo(() => _courseRepository.DeleteEnrolment(enrolment)).Returns(false);
        // Act
        var result = _controller.DeleteEnrolment(enrolment.StudentId, enrolment.CourseId);
        // Assert
        result.Should().BeOfType(typeof(ObjectResult));
    }
}