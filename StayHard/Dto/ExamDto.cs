namespace StayHard.Dto;

public class ExamDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    
    public bool IsExercise { get; set; }
    public bool IsOpenBook { get; set; }
    public bool FileAccessible { get; set; }
    
    public int AvailableScore { get; set; }
    public int Type { get; set; }
    public string Description { get; set; }
    public int? QuestionFileId { get; set; }
    public int? AnswerFileId { get; set; }
    public int CourseId { get; set; }
}