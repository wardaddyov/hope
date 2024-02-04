using StayHard.Models;

namespace StayHard.Interfaces;

public interface IExamRepository
{
    ICollection<Exam> GetExams();
    ICollection<Exam> GetExams(int courseId);
    Exam GetExam(int examId);
    ICollection<Question> GetExamQuestions(int examId);
    ICollection<Student> GetExamParticipants(int examId);
    bool CreateExam(Exam exam);
    bool Save();

}