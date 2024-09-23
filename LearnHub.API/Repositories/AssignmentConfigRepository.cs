using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;

namespace LearnHub.API.Repositories
{
    public class AssignmentConfigRepository : Repository<AssignmentConfig>, IAssignmentConfigRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public AssignmentConfigRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    
    }
}
