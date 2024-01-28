namespace StayHard.Models;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Activation { get; set; }
    public int Semester { get; set; }
    public int Group { get; set; }
    public ICollection<Exam> Exams { get; set; }
    public ICollection<Enrolment> Enrolments { get; set; }
}