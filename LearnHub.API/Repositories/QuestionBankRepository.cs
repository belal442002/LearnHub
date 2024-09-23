using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.QuestionBankDto;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class QuestionBankRepository : Repository<QuestionBank>, IQuestionBankRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public QuestionBankRepository(LearnHubDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<QuestionBank>> GetAllQuestionsWithIncludeAsync()
        {
            return await _dbContext.Questions
                .Include(q => q.Difficulty)
                .Include(q => q.QuestionType)
                .Include(q => q.Topic)
                .Include(q => q.Answers)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuestionBank>> GetByCourseAndInstructorIdAsync(String courseId, int instructorId)
        {
            return await _dbContext.Questions
                .Include(q => q.Answers)
                .Include(q => q.Difficulty)
                .Include(q => q.Topic)
                .Include(q => q.QuestionType)
                .Where(q => q.CourseId.Equals(courseId) && q.InstructorId == instructorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuestionBank>> GetByCourseIdAsync(String courseId)
        {
            return await _dbContext.Questions
                .Where(q => q.CourseId.Equals(courseId))
                .Include(q => q.Answers)
                .Include(q => q.Difficulty)
                .Include(q => q.Topic)
                .Include(q => q.QuestionType)
                .ToListAsync();
        }

        public async Task<QuestionBank?> GetByIdWithIncludeAsync(int id)
        {
            return await _dbContext.Questions
                .Include(q => q.Answers)
                .Include(q => q.Difficulty)
                .Include(q => q.Topic)
                .Include(q => q.QuestionType)
                .FirstOrDefaultAsync(q => q.QuestionId == id);
        }

        public async Task<IEnumerable<QuestionBank>> GetByInstructorIdAsync(int instructorId)
        {
            return await _dbContext.Questions
                .Include(q => q.Answers)
                .Include(q => q.Difficulty)
                .Include(q => q.Topic)
                .Include(q => q.QuestionType)
                .Where(q => q.InstructorId == instructorId).ToListAsync();
        }

        public async Task<bool> UpdateQuestionAsync(int id, QuestionBank question)
        {
            var questionDomainModel = await _dbContext.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuestionId == id);
            if(questionDomainModel == null)
            {
                return false;
            }
            questionDomainModel.QuestionText = question.QuestionText;
            questionDomainModel.DifficultyId = question.DifficultyId;
            questionDomainModel.TopicId = question.TopicId;

            var answers = question.Answers.ToList();
            var i = 0;
            foreach(var answer in questionDomainModel.Answers)
            {
                answer.AnswerText = answers[i].AnswerText;
                answer.Answer_TF = answers[i].Answer_TF;
                i++;
            }

            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<QuestionCountByTopicDto>> GetQuestionCountByCoursePerTopicAsync(string courseId)
        {
            return await _dbContext.Questions
                .Where(q => q.CourseId == courseId)
                .Include(q => q.Topic)
                .Include(q => q.Difficulty)
                .GroupBy(q => new { q.TopicId , q.DifficultyId, q.Topic.TopicName, q.Difficulty.Name})
                .Select(g => new QuestionCountByTopicDto
                {
                    TopicId = g.Key.TopicId,
                    TopicName = g.Key.TopicName,
                    DifficultyId = g.Key.DifficultyId,
                    DifficultyLevel = g.Key.Name,
                    QuestionCount = g.Count()
                }).OrderBy(q => q.TopicId)
                .ToListAsync();
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            var questionDomainModel = await _dbContext.Questions.Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuestionId == id);

            if(questionDomainModel == null)
            {
                throw new Exception("Question not found");
            }

            var answers = questionDomainModel.Answers;
            foreach(var answer in answers)
            {
                _dbContext.QBAnswers.Remove(answer);
            }
            _dbContext.Questions.Remove(questionDomainModel);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveQuestionAsync(int id)
        {
            var questionDomainModel = await _dbContext.Questions
                .FirstOrDefaultAsync(q => q.QuestionId == id);

            if (questionDomainModel == null)
            {
                throw new Exception("Question not found");
            }

            _dbContext.Questions.Remove(questionDomainModel);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
