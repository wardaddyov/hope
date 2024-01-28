using AutoMapper;
using StayHard.Dto;
using StayHard.Models;

namespace StayHard.Profiles;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        // Model to DTO
        CreateMap<Student, StudentDto>();
        CreateMap<Course, CourseDto>();
        CreateMap<Exam, ExamDto>();
        CreateMap<Question, QuestionDto>();
        
        // DTO to Model
        CreateMap<StudentDto, Student>();
    }
}