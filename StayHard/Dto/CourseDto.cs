namespace StayHard.Dto;

public class CourseDto
{
    
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Activation { get; set; }
    public int Semester { get; set; }
    public int Group { get; set; }
    public int[] studentIds { get; set; }    
}