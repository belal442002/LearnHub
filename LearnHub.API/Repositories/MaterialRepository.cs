using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public MaterialRepository(LearnHubDbContext dbContext) :base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Material>> GetAllByCourseIdAsync(string courseId, int? materialTypeId)
        {
            
            var materials = _dbContext.Materials.Where(m => m.CourseId.Equals(courseId))
                .Include(m => m.Course)
                .Include(m => m.MaterialType).AsQueryable();

            if (materialTypeId != null)
            {
                materials = materials.Where(m => m.MaterialTypeId == materialTypeId);
            }
            return await materials.ToListAsync();
        }

        public async Task<IEnumerable<Material>> GetAllWithIncludeAsync()
        {
            return await _dbContext.Materials
                .Include(m => m.Course)
                .Include(m => m.MaterialType).ToListAsync();
        }
    }
}
