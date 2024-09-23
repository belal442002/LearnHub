using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class ParentRepository : Repository<Parent>, IParentRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public ParentRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Parent>> GetAllParentsWithAccountAsync(bool archive = true)
        {
            return await _dbContext.Parents.Where(s => s.Active_YN == archive).Include(s => s.Account).ToListAsync();
        }

        public async Task<Parent?> GetParentByIdWithAccountAsync(int id)
        {
            return await _dbContext.Parents.Where(s => s.Active_YN == true).Include(s => s.Account).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> Archive(Parent parent)
        {
            parent.Active_YN = false;
            return await _dbContext.SaveChangesAsync() > 0;

        }

        public async Task<Parent?> GetParentByStudentIdAsync(int studentId)
        {
            var parent = await _dbContext.Students
                .Where(s => s.Id == studentId && s.Active_YN)
                .Include(s => s.Parent)
                .ThenInclude(s => s.Account)
                .Select(s => s.Parent)
                .FirstOrDefaultAsync();

            return parent;
        }

        public async Task<bool> UpdateParentAsync(int parentId, Parent parent)
        {
            var parentDomainModel = await _dbContext.Parents
                .Include(p => p.Account)
                .FirstOrDefaultAsync(p => p.Id == parentId);
            if (parentDomainModel == null)
            {
                throw new Exception("Parent not found");
            }
            parentDomainModel.Gender = parent.Gender;
            parentDomainModel.Address = parent.Address;
            parentDomainModel.Name = parent.Name;
            parentDomainModel.Account.PhoneNumber = parent.Account.PhoneNumber;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Parent?> GetParentByAccountId(String userId)
        {
            return await _dbContext.Parents.FirstOrDefaultAsync(p => p.AccountId.Equals(userId));
        }
        public async Task<List<Student>> GetStudentsByParentId(int parentId)
        {
            return await _dbContext.Students.Where(s => s.ParentId == parentId).Include(s => s.Account).ToListAsync();
        }
    }
}
