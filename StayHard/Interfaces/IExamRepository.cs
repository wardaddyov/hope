using StayHard.Models;

namespace StayHard.Interfaces;

public interface IExamRepository
{
    ICollection<Exam> GetExams();
    ICollection<Exam> GetExams(int courseId);
    Exam GetExam(int examId);
    
    ICollection<ExamParticipant> GetExamParticipants(int examId);
    bool CreateExamParticipants(List<ExamParticipant> examParticipants);
    bool RemoveExamParticipant(ExamParticipant examParticipant);
    bool EditExamParticipant(List<ExamParticipant> updatedExamParticipants);
    bool ParticipantExists(List<ExamParticipant> examParticipants);
    bool ExamExists(int examId);
    bool CreateExam(Exam exam);
    bool EditExam(Exam exam);
    bool RemoveExam(Exam exam);

    ICollection<ExamQuestionFile?> GetExamFiles(int? questionFileId, int? answerFileId);
    ICollection<ExamQuestionFile> GetExamFiles();
    ExamQuestionFile GetExamFile(int fileId);
    bool CreateFile(ExamQuestionFile examQuestionFile);
    bool RemoveFile(ExamQuestionFile examQuestionFile);
    bool EditFile(ExamQuestionFile examQuestionFile);
    bool Save();
}