using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IDifficultyRepository : IRepository<Difficulty>
    {
        Task<List<Difficulty>> GetDifficultiesWithQuestionsAsync();
        Task<Difficulty> GetDifficultyByNameWithQuestionsAsync(string name);
        Task<Difficulty> GetDifficultyByIdWithQuestionsAsync(int id);
    }
}
