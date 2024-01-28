namespace StayHard.Models;


public class Score
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public int StudentId { get; set; }
    public float TakenScore { get; set; }

    public Student Student { get; set; }
    public Question Question { get; set; }
}