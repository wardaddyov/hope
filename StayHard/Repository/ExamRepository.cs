using Microsoft.EntityFrameworkCore;
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
    

    public ICollection<ExamParticipant> GetExamParticipants(int examId)
    {
        return _context.ExamParticipants
            .Where(ep => ep.ExamId == examId).Include(x=>x.Student).ToList();

    }

    public bool CreateExamParticipants(List<ExamParticipant> examParticipants)
    {
        foreach (var participant in examParticipants)
        {
            _context.ExamParticipants.Add(participant);
        }
        
        return Save();
    }

    public bool RemoveExamParticipant(ExamParticipant examParticipant)
    {
        _context.ExamParticipants.Remove(examParticipant);

        return Save();
    }

    public bool EditExamParticipant(List<ExamParticipant> updatedExamParticipants)
    {
        //_context.ExamParticipants.Remove(examParticipant);
        foreach (var participant in updatedExamParticipants)
        {
            _context.ExamParticipants.Update(participant);
        }
        return Save();
    }

    public bool ParticipantExists(List<ExamParticipant> examParticipants)
    {
        foreach (var participant in examParticipants)
        {
            
        }

        return false;
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

    public ICollection<ExamQuestionFile?> GetExamFiles(int? questionFileId, int? answerFileId)
    {
        var questionFile = _context.ExamQuestionFiles.Where(ef => ef.Id == questionFileId).FirstOrDefault();
        var answerFile = _context.ExamQuestionFiles.Where(ef => ef.Id == answerFileId).FirstOrDefault();

        return new List<ExamQuestionFile?> {questionFile, answerFile };
    }

    public ICollection<ExamQuestionFile> GetExamFiles()
    {
        return _context.ExamQuestionFiles.ToList();
    }

    public ExamQuestionFile GetExamFile(int fileId)
    {
        return _context.ExamQuestionFiles.Where(ef => ef.Id == fileId).FirstOrDefault();
    }

    public bool CreateFile(ExamQuestionFile examQuestionFile)
    {
        _context.Add(examQuestionFile);
        return Save();
    }

    public bool RemoveFile(ExamQuestionFile examQuestionFile)
    {
        _context.Remove(examQuestionFile);
        return Save();
    }

    public bool EditFile(ExamQuestionFile examQuestionFile)
    {
        _context.Update(examQuestionFile);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}