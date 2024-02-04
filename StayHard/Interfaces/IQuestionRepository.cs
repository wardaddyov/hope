using StayHard.Models;

namespace StayHard.Interfaces;

public interface IQuestionRepository
{
    ICollection<Question> GetQuestions();
    Question GetQuestion(int id);
    bool QuestionExists(int id);
    bool CreateQuestion(Question question);
    bool Save();
}