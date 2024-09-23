using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class MaterialTypeRepository : Repository<MaterialType>, IMaterialTypeRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public MaterialTypeRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MaterialType>> GetAllMaterialTypesAsync()
        {
            return await _dbContext.MaterialTypes
                                 .ToListAsync();
        }

        public async Task<MaterialType?> GetMaterialTypeByIdAsync(int id)
        {
            var materialType = await _dbContext.MaterialTypes
                                 .FirstOrDefaultAsync(mt => mt.Id == id); 
            return materialType;
        }

        public async Task<MaterialType?> GetMaterialTypeByNameAsync(string name)
        {
            return await _dbContext.MaterialTypes
                                 .FirstOrDefaultAsync(mt => mt.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
