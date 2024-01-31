using System.Collections;
using Microsoft.AspNetCore.Components.Web;
using StayHard.Models;

namespace StayHard.Interfaces;

public interface ICourseRepository
{
    ICollection<Course> GetCourses();
    // Because we can have multiple courses with the same name with different semesters
    ICollection<Course> GetCourse(string name);
    // Because we can have multiple courses within the same semester
    ICollection<Course> GetCoursesBySemester(int semester);
    // Because we can have multiple groups within the same semester
    ICollection<Course> GetCourse(string name, int semester);
    ICollection<Student> GetStudentsByCourse(int courseId);
    Course GetCourse(int courseId);
    bool CourseExists(int courseId);
}