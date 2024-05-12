namespace StayHard.Dto;

public class ExamParticipantDto
{
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public string StudentIdFull { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public float? Score { get; set; }
}