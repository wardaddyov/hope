using System.ComponentModel.DataAnnotations;

namespace StayHard.Models;

public class ExamParticipant
{
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public float? Score { get; set; }
    public Student Student { get; set; }
    public Exam Exam { get; set; }
}