using StayHard.Data;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Repository;

public class QuestionRepository : IQuestionRepository
{
    private readonly DataContext _context;

    public QuestionRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Question> GetQuestions()
    {
        return _context.Questions.ToList();
    }

    public Question GetQuestion(int id)
    {
        return _context.Questions.Where(q => q.Id == id).FirstOrDefault();
    }

    public bool QuestionExists(int id)
    {
        return _context.Questions.Any(q => q.Id == id);
    }

    public bool CreateQuestion(Question question)
    {
        _context.Add(question);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}