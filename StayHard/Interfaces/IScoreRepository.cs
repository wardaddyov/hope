using StayHard.Models;

namespace StayHard.Interfaces;

public interface IScoreRepository
{
    ICollection<Score> GetScores();
    Score GetScore(int id);
    bool ScoreExists(int id);
    bool CreateScore(Score score);
    bool Save();
}