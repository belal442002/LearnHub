using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class DifficultyRepository : Repository<Difficulty>, IDifficultyRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public DifficultyRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Difficulty>> GetDifficultiesWithQuestionsAsync()
        {
            var difficulties = await _dbContext.Difficulties
                                             .Include(d => d.Questions)
                                                 .ThenInclude(q => q.QuestionType)
                                             .Include(d => d.Questions)
                                                 .ThenInclude(q => q.Topic)
                                              .Include(d => d.Questions)
                                                  .ThenInclude(q => q.Answers)
                                             .ToListAsync();
            return difficulties;
        }

        public async Task<Difficulty> GetDifficultyByNameWithQuestionsAsync(string name)
        {
            var difficulty = await _dbContext.Difficulties
                         .Include(d => d.Questions)
                             .ThenInclude(q => q.Topic)
                         .Include(d => d.Questions)
                             .ThenInclude(q => q.QuestionType)
                         .Include(d => d.Questions)
                              .ThenInclude(q => q.Answers)
                         .FirstOrDefaultAsync(d => d.Name == name);

            return difficulty;
        }
        public async Task<Difficulty> GetDifficultyByIdWithQuestionsAsync(int id)
        {
            var difficulty = await _dbContext.Difficulties
                         .Include(d => d.Questions)
                             .ThenInclude(q => q.Topic)
                         .Include(d => d.Questions)
                             .ThenInclude(q => q.QuestionType)
                         .Include(d => d.Questions)
                              .ThenInclude(q => q.Answers)
                         .FirstOrDefaultAsync(d => d.Id == id);

            return difficulty;
        }
    }
}
