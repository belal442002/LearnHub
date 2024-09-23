using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LearnHub.API.Repositories
{
    public class TeachRepository : Repository<Teach>, ITeachRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public TeachRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistAsync(Teach teach)
        {
            return await _dbContext.Teaches
                .AnyAsync(t => t.InstructorId == teach.InstructorId && t.CourseId.Equals(teach.CourseId)
                && t.SemesterId == teach.SemesterId && t.Year == teach.Year);
        }

        public async Task<IEnumerable<Teach>> GetAllWithIncludeAsync(int? instructorId, String? courseId, int? semesterId, int? year)
        {
            var teach = _dbContext.Teaches
                .Include(t => t.Instructor)
                .Include(t => t.Course)
                .Include(t => t.Semester)
                .AsQueryable();

            if(instructorId != null)
            {
                teach = teach.Where(t => t.InstructorId == instructorId);
            }
            if (!courseId.IsNullOrEmpty())
            {
                teach = teach.Where(t => t.CourseId.Equals(courseId));
            }
            if (semesterId != null)
            {
                teach = teach.Where(t => t.SemesterId == semesterId);
            }
            if (year != null)
            {
                teach = teach.Where(t => t.Year == year);
            }

            return await teach.ToListAsync();
        }

        public async Task<Teach?> GetByInstructorAndCourseIdAsync(int instructorId, string courseId)
        {
            return await _dbContext.Teaches
                .Where(t => t.InstructorId == instructorId && t.CourseId.Equals(courseId))
                .OrderByDescending(t => t.Id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Teach>> GetTeachesByCourseIdAsync(string courseId, int? semesterId, int? year)
        {
            var teaches = _dbContext.Teaches.Where(t => t.CourseId.Equals(courseId)).AsQueryable();

            if(semesterId != null)
            {
                teaches = teaches.Where(t => t.SemesterId == semesterId);
            }
            if(year != null)
            {
                teaches = teaches.Where(t => t.Year == year);
            }

            return await teaches
                .Include(t => t.Course)
                .Include(t => t.Instructor)
                .Include(t => t.Semester)
                .ToListAsync();
        }

        public async Task<IEnumerable<Teach>> GetTeachesByInstructorIdAsync(int instructorId, int? semesterId, int? year)
        {
            var teaches = _dbContext.Teaches.Where(t => t.InstructorId == instructorId).AsQueryable();

            if (semesterId != null)
            {
                teaches = teaches.Where(t => t.SemesterId == semesterId);
            }
            if (year != null)
            {
                teaches = teaches.Where(t => t.Year == year);
            }

            return await teaches
                .Include(t => t.Instructor)
                .Include(t => t.Course)
                .Include(t => t.Semester)
                .ToListAsync();
        }
    }
}
