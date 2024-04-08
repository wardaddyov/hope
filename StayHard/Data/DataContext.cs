using Microsoft.EntityFrameworkCore;
using StayHard.Models;

namespace StayHard.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Score> Scores { get; set; }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<ExamFile> ExamFiles { get; set; }
    
    // Joined Tables
    public DbSet<Enrolment> Enrolments { get; set; }
    public DbSet<ExamParticipant> ExamParticipants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Enrolment Setup
        modelBuilder.Entity<Enrolment>()
            .HasKey(enrolment => new { enrolment.CourseId, enrolment.StudentId });
        modelBuilder.Entity<Enrolment>()
            .HasOne(c => c.Course)
            .WithMany(enrolment => enrolment.Enrolments)
            .HasForeignKey(c => c.CourseId);
        modelBuilder.Entity<Enrolment>()
            .HasOne(s => s.Student)
            .WithMany(enrolment => enrolment.Enrolments)
            .HasForeignKey(s => s.StudentId);

        // ExamParticipant Setup
        modelBuilder.Entity<ExamParticipant>()
            .HasKey(examParticipant => new { examParticipant.ExamId, examParticipant.StudentId });
        modelBuilder.Entity<ExamParticipant>()
            .HasOne(e => e.Exam)
            .WithMany(examParticipant => examParticipant.ExamParticipants)
            .HasForeignKey(e => e.ExamId);
        modelBuilder.Entity<ExamParticipant>()
            .HasOne(s => s.Student)
            .WithMany(examParticipant => examParticipant.ExamParitcipants)
            .HasForeignKey(s => s.StudentId);
        
        // Exam File setup
        modelBuilder.Entity<Exam>()
            .HasOne(e => e.AnswerFile)
            .WithOne(e => e.Exam)
            .HasForeignKey<Exam>(e => e.AnswerFileId); 
        
        modelBuilder.Entity<Exam>()
            .HasOne(e => e.QuestionFile)
            .WithOne(e => e.Exam2)
            .HasForeignKey<Exam>(e => e.QuestionFileId);

        // Make the studentId a unique field
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.StudentID).IsUnique();
        
        // convert exam enum to int
        modelBuilder.Entity<Exam>()
            .Property(e => e.Type)
            .HasConversion<int>();
    }
}