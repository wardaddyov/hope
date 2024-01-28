using StayHard.Data;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Repository;

public class ExamRepository: IExamRepository
{
    private readonly DataContext _context;

    public ExamRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<Exam> GetExams()
    {
        return _context.Exams.OrderBy(e => e.Date).ToList();
    }

    public ICollection<Exam> GetExams(int courseId)
    {
        return _context.Exams.Where(e => e.CourseId == courseId).ToList();
    }

    public Exam GetExam(int examId)
    {
        return _context.Exams.Where(e => e.Id == examId).FirstOrDefault();
    }

    public ICollection<Question> GetExamQuestions(int examId)
    {
        return _context.Questions.Where(q => q.ExamId == examId).ToList();
    }

    public ICollection<Student> GetExamParticipants(int examId)
    {
        return _context.ExamParticipants
            .Where(ep => ep.ExamId == examId)
            .Select(s => s.Student)
            .OrderBy(s => s.StudentID).ToList();
    }
}