using StayHard.Models;

namespace StayHard.Interfaces;

public interface IStudentRepository
{
    ICollection<Student> GetStudents();
    Student GetStudent(string studentId);
    Student GetStudent(int studentId);
    // We use ICollection here because name is not a unique parameter and our search can return multiple students
    ICollection<Student> GetStudent(string firstName, string lastName);
    ICollection<Score> GetStudentScoresByExam(int id, int examId);
    bool StudentExists(string studentId);
    bool CreateStudent(Student student);
    bool Save();
}