using Microsoft.EntityFrameworkCore;
using StayHard.Data;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Repository;

public class StudentRepository: IStudentRepository
{
    private readonly DataContext _context;

    public StudentRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Student> GetStudents()
    {
        return _context.Students.OrderBy(s => s.Id).ToList();
    }

    public Student GetStudent(string studentId)
    {
        return _context.Students.Where(s => s.StudentID == studentId).FirstOrDefault();
    }
    
    public Student GetStudent(int studentId)
    {
        return _context.Students.Where(s => s.Id == studentId).FirstOrDefault();
    }

    public ICollection<Student> GetStudent(string firstName, string lastName)
    {
        return _context.Students
            .Where(s => s.Firstname.ToLower() == firstName.ToLower() || s.Lastname.ToLower() == lastName.ToLower())
            .ToList();
    }

    public bool StudentExists(string studentId)
    {
        return _context.Students.Any(s => s.StudentID == studentId);
    }

    public bool StudentExists(int id)
    {
        return _context.Students.Any(s => s.Id == id);
    }

    public bool CreateStudent(Student student)
    {
        _context.Add(student);
        return Save();
    }
    
    public bool UpdateStudent(Student student)
    {
        _context.Update(student);
        return Save();
    }
    
    public bool DeleteStudent(Student student)
    {
        _context.Remove(student);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}