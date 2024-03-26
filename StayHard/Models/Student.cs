namespace StayHard.Models;

public class Student
{
    public int Id { get; set; }
    public string StudentID { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public int EntryYear { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }

    public ICollection<Score> Scores { get; set; }
    public ICollection<ExamParticipant> ExamParitcipants { get; set; }
    public ICollection<Enrolment> Enrolments { get; set; }
    
}