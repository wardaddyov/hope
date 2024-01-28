namespace StayHard.Models;

public class Exam
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public int CourseId { get; set; }

    public Course Course { get; set; }
    public ICollection<Question> Questions { get; set; }
    public ICollection<ExamParticipant> ExamParticipants { get; set; }
}