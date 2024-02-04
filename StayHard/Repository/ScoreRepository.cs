using Microsoft.EntityFrameworkCore;
using StayHard.Data;
using StayHard.Interfaces;
using StayHard.Models;

namespace StayHard.Repository;

public class ScoreRepository : IScoreRepository
{
    private readonly DataContext _context;

    public ScoreRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<Score> GetScores()
    {
        return _context.Scores.Include(s=>s.Question).ToList();
    }

    public Score GetScore(int id)
    {
        return _context.Scores.Where(s => s.Id == id).Include(s => s.Question).FirstOrDefault();
    }

    public bool ScoreExists(int id)
    {
        return _context.Scores.Any(s => s.Id == id);
    }

    public bool CreateScore(Score score)
    {
        _context.Add(score);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}