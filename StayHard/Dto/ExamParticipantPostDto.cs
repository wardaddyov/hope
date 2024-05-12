namespace StayHard.Dto;

public class ExamParticipantPostDto
{
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public float? Score { get; set; }
}