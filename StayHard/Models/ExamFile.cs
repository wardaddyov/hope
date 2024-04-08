namespace StayHard.Models;

public class ExamFile
{
    public int Id { get; set; }
    public byte[] File { get; set; }
    public Exam Exam { get; set; }
    // Becuase we have two refrences in the exam model, we should have two here also
    public Exam Exam2 { get; set; }
}