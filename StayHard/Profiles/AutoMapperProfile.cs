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
        //CreateMap<Score, ScoreDto>().IncludeMembers(s => s.Question);
        CreateMap<Admin, AdminGetDto>();
        CreateMap<ExamQuestionFile, ExamFileDto>();
            /*.ForMember(x => x.Number, opt => opt.MapFrom(src => src.Question.Number));*/
        
        // DTO to Model
        CreateMap<StudentDto, Student>();
        CreateMap<CourseDto, Course>();
        CreateMap<ExamDto, Exam>();
        CreateMap<ExamFileDto, ExamQuestionFile>();
    }
}