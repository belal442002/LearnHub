using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public InstructorRepository(LearnHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<bool> CreateInstructorAsync(Instructor instructor)
        {
            try
            {
                await _dbContext.Instructors.AddAsync(instructor);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
