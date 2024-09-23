using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class InstructorRepository : Repository<Instructor>, IInstructorRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public InstructorRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Archive(Instructor instructor)
        {
            instructor.Active_YN = false;
            return await _dbContext.SaveChangesAsync() > 0;

        }

        public async Task<List<Instructor>> GetAllInstructorsWithAccountAsync(bool archive = true)
        {
            return await _dbContext.Instructors.Where(s => s.Active_YN == archive).Include(s => s.Account).ToListAsync();
        }



        public async Task<Instructor?> GetInstructorByIdWithAccountAsync(int id)
        {
            return await _dbContext.Instructors.Where(s => s.Active_YN == true).Include(s => s.Account).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> UpdateInstructorAsync(int instructorId, Instructor instructor)
        {
            var instructorDomainModel = await _dbContext.Instructors
                .Include(i => i.Account)
                .FirstOrDefaultAsync(i => i.Id == instructorId);
            if (instructorDomainModel == null)
            {
                throw new Exception("Instructor not found");
            }
            instructorDomainModel.Gender = instructor.Gender;
            instructorDomainModel.Address = instructor.Address;
            instructorDomainModel.Name = instructor.Name;
            instructorDomainModel.Account.PhoneNumber = instructor.Account.PhoneNumber;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Instructor?> GetInstructorByAccountId(String userId)
        {
            return await _dbContext.Instructors.FirstOrDefaultAsync(i => i.AccountId.Equals(userId));
        }

    }
}
