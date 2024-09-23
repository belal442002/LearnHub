using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class QBAnswersRepository : Repository<QBAnswers>, IQBAnswersRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public QBAnswersRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteAnswerAsync(int id)
        {
            var answerDomainModel = await _dbContext.QBAnswers
                .FirstOrDefaultAsync(a => a.AnswerId == id);

            if(answerDomainModel == null)
            {
                throw new Exception($"There is no answer with id {id}");
            }

            _dbContext.QBAnswers.Remove(answerDomainModel);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
