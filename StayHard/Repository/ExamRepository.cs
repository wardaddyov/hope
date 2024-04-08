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

    public bool ExamExists(int examId)
    {
        return _context.Exams.Any(e=> e.Id == examId);
    }

    public bool CreateExam(Exam exam)
    {
        _context.Add(exam);
        return Save();
    }

    public bool EditExam(Exam exam)
    {
        _context.Update(exam);
        return Save();
    }

    public bool RemoveExam(Exam exam)
    {
        _context.Remove(exam);
        return Save();
    }

    public ICollection<ExamFile?> GetExamFiles(int questionFileId, int answerFileId)
    {
        var questionFile = _context.ExamFiles.Where(ef => ef.Id == questionFileId).FirstOrDefault();
        var answerFile = _context.ExamFiles.Where(ef => ef.Id == answerFileId).FirstOrDefault();

        return new List<ExamFile?> {questionFile, answerFile };
    }

    public bool CreateFile(ExamFile examFile)
    {
        _context.Add(examFile);
        return Save();
    }

    public bool RemoveFile(ExamFile examFile)
    {
        _context.Remove(examFile);
        return Save();
    }

    public bool EditFile(ExamFile examFile)
    {
        _context.Update(examFile);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}