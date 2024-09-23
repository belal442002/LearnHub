using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class QuestionTypeRepository : Repository<QuestionType>, IQuestionTypeRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public QuestionTypeRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<QuestionType>> GetQuestionTypesWithQuestionsAsync()
        {
            var questionTypes = await _dbContext.QuestionTypes.Include(q => q.Questions)
                .ThenInclude(q => q.Topic)
                .Include(q => q.Questions).ThenInclude(q => q.Difficulty).
                Include(q => q.Questions).ThenInclude(q => q.Answers).ToListAsync();
            return questionTypes;
        }

        public async Task<QuestionType> GetQuestionTypeByNameWithQuestionsAsync(string name)
        {
            var questionType = await _dbContext.QuestionTypes
                         .Include(q => q.Questions)
                             .ThenInclude(q => q.Topic)
                         .Include(q => q.Questions)
                             .ThenInclude(q => q.Difficulty)
                         .Include(q => q.Questions)
                              .ThenInclude(q => q.Answers)
                         .FirstOrDefaultAsync(q => q.Name == name);

            return questionType;
        }

        public async Task<QuestionType> GetQuestionTypeByIdWithQuestionsAsync(int id)
        {
            var questionType = await _dbContext.QuestionTypes
                         .Include(q => q.Questions)
                             .ThenInclude(q => q.Topic)
                         .Include(q => q.Questions)
                             .ThenInclude(q => q.Difficulty)
                         .Include(q => q.Questions)
                              .ThenInclude(q => q.Answers)
                         .FirstOrDefaultAsync(q => q.Id == id);

            return questionType;
        }


    }
}
