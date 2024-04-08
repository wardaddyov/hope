using System.ComponentModel.DataAnnotations.Schema;

namespace StayHard.Models;

public class Exam
{
    public enum ExamType
    {
        Mixed,
        Descriptive,
        Test
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    
    public bool IsExercise { get; set; }
    public bool IsOpenBook { get; set; }
    public bool FileAccessible { get; set; }
    
    public int AvailableScore { get; set; }
    public ExamType Type { get; set; }
    public string Description { get; set; }
    public int? QuestionFileId { get; set; }
    public int? AnswerFileId { get; set; }
    public int CourseId { get; set; }

    public Course Course { get; set; }
    public ExamFile QuestionFile { get; set; }
    public ExamFile AnswerFile { get; set; }
    public ICollection<ExamParticipant> ExamParticipants { get; set; }
}