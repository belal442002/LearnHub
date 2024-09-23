using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class EvaluationRepository : Repository<Evaluation>, IEvaluationRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public EvaluationRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<Evaluation>> GetAllEvaluationsAsync(string courseId, 
            int? assignmentId, int? studentId, int? assignmentNumber)
        {
            var evaluations = _dbContext.Evaluations
                .Include(e => e.Assignment)
                .ThenInclude(e => e.AssignmentConfig)
                .Where(e => e.Assignment.CourseId.Equals(courseId))
                .AsQueryable();

            if (assignmentId != null )
            {
                evaluations = evaluations.Where(e => e.AssignmentId == assignmentId);
            }
            if (studentId != null )
            {
                evaluations = evaluations.Where(e => e.StudentId == studentId);
            }
            if (assignmentNumber != null )
            {
                evaluations = evaluations.Where(e => e.Assignment.AssignmentNumber == assignmentNumber);
            }

            return await evaluations.ToListAsync();
        }

        public async Task<Evaluation?> GetEvaluationAsync(int assignmentId, int studentId)
        {
            return await _dbContext.Evaluations
                .FirstOrDefaultAsync(e => e.AssignmentId == assignmentId && e.StudentId == studentId);
        }
    }
    
}
