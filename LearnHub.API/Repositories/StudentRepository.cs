using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;

namespace LearnHub.API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public StudentRepository(LearnHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateStudentAsync(Student student)
        {
            try
            {
               await _dbContext.Students.AddAsync(student);
                return true;
            }catch(Exception)
            {
                return false;
            }
        }
    }
}
