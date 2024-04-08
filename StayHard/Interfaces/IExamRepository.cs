using StayHard.Models;

namespace StayHard.Interfaces;

public interface IExamRepository
{
    ICollection<Exam> GetExams();
    ICollection<Exam> GetExams(int courseId);
    Exam GetExam(int examId);
    
    ICollection<Question> GetExamQuestions(int examId);
    ICollection<Student> GetExamParticipants(int examId);
    bool ExamExists(int examId);
    bool CreateExam(Exam exam);
    bool EditExam(Exam exam);
    bool RemoveExam(Exam exam);

    ICollection<ExamFile?> GetExamFiles(int questionFileId, int answerFileId);
    bool CreateFile(ExamFile examFile);
    bool RemoveFile(ExamFile examFile);
    bool EditFile(ExamFile examFile);
    bool Save();

}