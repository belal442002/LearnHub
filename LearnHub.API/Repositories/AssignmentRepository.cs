using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public AssignmentRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddAssignmentAsync(string courseId, int assignmentConfigId)
        {
            var assignmentCounts = _dbContext.Assignments.Count(a => a.CourseId.Equals(courseId));
            assignmentCounts += 1;

            var assignmentDomainModel = new Assignment
            {
                CourseId = courseId,
                AssignmentConfigId = assignmentConfigId,
                AssignmentNumber = assignmentCounts
                
            };
            await _dbContext.Assignments.AddAsync(assignmentDomainModel);
            
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Assignment>> GetAllAssignmentsByCourseIdAsync(string courseId)
        {
            return await _dbContext.Assignments.Where(a => a.CourseId.Equals(courseId))
                .Include(a => a.AssignmentConfig)
                .ThenInclude(a => a.AssignmentConfigTopics).ThenInclude(a => a.Topic)
                .Include(a => a.AssignmentConfig)
                .ThenInclude(a => a.AssignmentConfigTopics).ThenInclude(a => a.Difficulty)
                .ToListAsync();

        }

        public async Task<bool> UpdateAssignmentAsync(int assignmentId, Assignment assignment)
        {
            var assignmentDomainModel = await _dbContext.Assignments
                .Include(a => a.AssignmentConfig)
                .ThenInclude (a => a.AssignmentConfigTopics)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignmentDomainModel == null)
            {
                return false;
            }
            assignmentDomainModel.AssignmentConfig.Title = assignment.AssignmentConfig.Title;
            assignmentDomainModel.AssignmentConfig.Type = assignment.AssignmentConfig.Type;
            assignmentDomainModel.AssignmentConfig.MaxScore = assignment.AssignmentConfig.MaxScore;
            assignmentDomainModel.AssignmentConfig.StartDate = assignment.AssignmentConfig.StartDate;
            assignmentDomainModel.AssignmentConfig.EndDate = assignment.AssignmentConfig.EndDate;


            var updatedTopicsCount = assignment.AssignmentConfig.AssignmentConfigTopics.Count;
            var oldTopicsCount = assignmentDomainModel.AssignmentConfig.AssignmentConfigTopics.Count;

            var updatedTopicsList = assignment.AssignmentConfig.AssignmentConfigTopics.ToList();

            int i = 0;
            if (updatedTopicsCount >= oldTopicsCount)
            {
                foreach (var assignmentTopic in assignmentDomainModel.AssignmentConfig.AssignmentConfigTopics)
                {
                    assignmentTopic.NumberOfQuestions = updatedTopicsList[i].NumberOfQuestions;
                    assignmentTopic.TopicId = updatedTopicsList[i].TopicId;
                    assignmentTopic.DifficultyId = updatedTopicsList[i].DifficultyId;
                    i++;
                }
                for (int j = i; j < updatedTopicsCount; j++)
                {
                    updatedTopicsList[j].AssignmentConfigId = assignmentDomainModel.AssignmentConfigId;
                    await _dbContext.AssignmentConfigTopics.AddAsync(updatedTopicsList[j]);
                }
            }
            else
            {
                foreach (var assignmentTopic in assignmentDomainModel.AssignmentConfig.AssignmentConfigTopics)
                {
                    if (i < updatedTopicsCount)
                    {
                        assignmentTopic.NumberOfQuestions = updatedTopicsList[i].NumberOfQuestions;
                        assignmentTopic.TopicId = updatedTopicsList[i].TopicId;
                        assignmentTopic.DifficultyId = updatedTopicsList[i].DifficultyId;
                        i++;

                    }
                    else
                    {
                        _dbContext.AssignmentConfigTopics.Remove(assignmentTopic);
                    }
                }
            }
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAssignmentAsync(Assignment assignmentDomainModel)
        {
            _dbContext.AssignmentConfigTopics.RemoveRange(assignmentDomainModel.AssignmentConfig.AssignmentConfigTopics);

            var assignmentConfig = assignmentDomainModel.AssignmentConfig;

            _dbContext.Assignments.Remove(assignmentDomainModel);
            _dbContext.AssignmentConfigs.Remove(assignmentConfig);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Assignment?> GetByIdWithIncludeAsync(int assignmentId)
        {
            return await _dbContext.Assignments
                .Include(a => a.AssignmentConfig).ThenInclude(a => a.AssignmentConfigTopics)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);
        }
    }
}