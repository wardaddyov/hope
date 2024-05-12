using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StayHard.Data;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Repository;

public class OptionsRepository : IOptionsRepository
{
    private readonly DataContext _context;

    public OptionsRepository(DataContext context)
    {
        _context = context;
    }
    public bool GetDatabaseConnectionStatus()
    {
        return _context.Database.CanConnect();
    }

    public bool Migrate()
    {
        try
        {
            _context.Database.Migrate();
        }
        catch(SqlException e)
        {
            Trace.WriteLine(e.Message);
            return false;
        }
        return true;
    }

    public void Seed()
    {
        _context.Courses.AddRange(
            new Course() {Name = "مباحث ویژه ۲", Activation = true, Group = 1, Semester = 4022},
            new Course() {Name = "مطالعات اجتماعی", Activation = true, Group = 1, Semester = 4022}, 
            new Course() {Name = "قرآن و زندگی", Activation = true, Group = 1, Semester = 4022}
        );
        _context.Students.AddRange(
            new Student() {Firstname = "شقایق", Lastname = "اقبال پور", StudentID = "9917023105", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "+9899146334104", Description = ""},
            new Student() {Firstname = "A", Lastname = "", StudentID = "1", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "B", Lastname = "", StudentID = "2", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "C", Lastname = "", StudentID = "3", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "D", Lastname = "", StudentID = "4", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "E", Lastname = "", StudentID = "5", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "F", Lastname = "", StudentID = "6", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "G", Lastname = "", StudentID = "7", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "H", Lastname = "", StudentID = "8", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "I", Lastname = "", StudentID = "9", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "J", Lastname = "", StudentID = "10", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "K", Lastname = "", StudentID = "11", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "L", Lastname = "", StudentID = "12", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "M", Lastname = "", StudentID = "13", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "N", Lastname = "", StudentID = "14", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "O", Lastname = "", StudentID = "15", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""},
            new Student() {Firstname = "P", Lastname = "", StudentID = "16", EntryYear = 1399, Email = "", Password = "", Username = "", PhoneNumber = "", Description = ""}
            );
        _context.SaveChanges();
    }
}