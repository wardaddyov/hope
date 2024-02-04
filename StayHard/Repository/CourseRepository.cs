using Microsoft.EntityFrameworkCore;
using StayHard.Data;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Repository;

public class CourseRepository: ICourseRepository
{
    private readonly DataContext _context;

    public CourseRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<Course> GetCourses()
    {
        return _context.Courses.OrderBy(c => c.Id).ToList();
    }

    public ICollection<Course> GetCourse(string name)
    {
        return _context.Courses.Where(c => c.Name.ToLower() == name.ToLower()).OrderBy(c => c.Id).ToList();
    }

    public ICollection<Course> GetCoursesBySemester(int semester)
    {
        return _context.Courses.Where(c => c.Semester == semester).OrderBy(c => c.Id).ToList();
    }

    public ICollection<Course> GetCourse(string name, int semester)
    {
        return _context.Courses.Where(c => c.Name.ToLower() == name.ToLower() && c.Semester == semester)
            .OrderBy(c => c.Id).ToList();
    }

    public ICollection<Student> GetStudentsByCourse(int courseId)
    {
        return _context.Enrolments.Where(c => c.CourseId == courseId).Select(s => s.Student).ToList();
    }

    public ICollection<Enrolment> GetEnrolments()
    {
        return _context.Enrolments.ToList();
    }

    public Course GetCourse(int courseId)
    {
        return _context.Courses.Where(c => c.Id == courseId).FirstOrDefault();
    }

    public bool CourseExists(int courseId)
    {
        return _context.Courses.Any(c=> c.Id == courseId);
    }

    public bool CreateCourse(Course course)
    {
        _context.Add(course);
        return Save();
    }

    public bool AddStudentToCourse(Enrolment enrolment)
    {
        _context.Add(enrolment);
        return Save();
    }

    public bool DeleteEnrolment(Enrolment enrolment)
    {
        _context.Remove(enrolment);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}