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
        CreateMap<Score, ScoreDto>().IncludeMembers(s => s.Question);
        CreateMap<Question, ScoreDto>();
        CreateMap<Admin, AdminGetDto>();
            /*.ForMember(x => x.Number, opt => opt.MapFrom(src => src.Question.Number));*/
        
        // DTO to Model
        CreateMap<StudentDto, Student>();
        CreateMap<CourseDto, Course>();
        CreateMap<ExamDto, Exam>();
        CreateMap<QuestionDto, Question>();
        CreateMap<ScorePostDto, Score>();
    }
}