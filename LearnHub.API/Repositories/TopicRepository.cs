using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public TopicRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Topic>> GetTopicsByCourseIdAsync(string courseId)
        {
            var topics = await _dbContext.Topics
                 .Where(t => t.CourseId == courseId)
                 .Include(t => t.Course)
                 .Include(t => t.Questions)
                 .ToListAsync();

            return topics;
        }

        public async Task<bool> DeleteTopicAsync(int topicId)
        {
            var topicDomainModel = await _dbContext.Topics
                .Include(t => t.Questions).ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync();

            if(topicDomainModel == null)
            {
                throw new Exception($"there is no topic with id {topicId}");
            }

            var questions = topicDomainModel.Questions;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var question in questions)
                    {
                        _dbContext.QBAnswers.RemoveRange(question.Answers);
                        _dbContext.Questions.Remove(question);
                    }

                    _dbContext.Topics.Remove(topicDomainModel);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }

            return true;
        }

        public async Task<bool> UpdateTopicAsync(int topicId, Topic topic)
        {
            var topicDomainModel = await _dbContext.Topics
                .FirstOrDefaultAsync(t => t.Id == topicId);

            if(topicDomainModel == null)
            {
                throw new Exception($"there is no topic with id {topicId}");
            }

            topicDomainModel.TopicName = topic.TopicName;

            return await _dbContext.SaveChangesAsync() > 0;
        }


    }
}
