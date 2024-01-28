namespace StayHard.Models;

public class Question
{
    public int Id { get; set; }
    public int Number { get; set; }
    public float AvailableScore { get; set; }
    public int ExamId { get; set; }
    public Score Score { get; set; }
    public Exam Exam { get; set; }
}