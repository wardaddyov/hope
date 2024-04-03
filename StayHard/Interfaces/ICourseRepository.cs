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
    Course GetCourse(string name, int semester, int group);
    ICollection<Student> GetStudentsByCourse(int courseId);
    Enrolment GetStudentByCourse(int courseId, int studentId);
    ICollection<Enrolment> GetEnrolments(int courseId);
    Course GetCourse(int courseId);
    bool CourseExists(int courseId);
    bool CreateCourse(Course course);
    bool UpdateCourse(Course course);
    bool UpdateEnrolments(List<Enrolment> enrolments);
    Enrolment CreateEnrolmentObject(Course course, Student student);
    bool AddStudentsToCourse(List<Enrolment> enrolments);
    bool AddStudentToCourse(Enrolment enrolment);
    bool DeleteEnrolment(Enrolment enrolment);
    bool DeleteEnrolments(List<Enrolment> enrolments);
    bool DeleteCourse(Course course);

    bool Save();
}