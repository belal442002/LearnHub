using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IQBAnswersRepository : IRepository<QBAnswers>
    {
        Task<bool> DeleteAnswerAsync(int id);
    }
}
