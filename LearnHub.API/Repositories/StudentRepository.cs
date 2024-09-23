using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public StudentRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Archive(Student student)
        {
            student.Active_YN = false;
            return await _dbContext.SaveChangesAsync() > 0;

        }

        public async Task<List<Student>> GetAllStudentsWithAccountAsync(bool archive = true)
        {
            return await _dbContext.Students.Where(s => s.Active_YN == archive).Include(s => s.Account).ToListAsync();
        }

        public async Task<bool> UpdateStudentAsync(int studentId, Student student)
        {
            var studentDomainModel = await _dbContext.Students
                .Include(s => s.Account)
                .FirstOrDefaultAsync(s => s.Id == studentId);
            if (studentDomainModel == null)
            {
                throw new Exception("Student not found");
            }
            studentDomainModel.Gender = student.Gender;
            studentDomainModel.Address = student.Address;
            studentDomainModel.Name = student.Name;
            studentDomainModel.Account.PhoneNumber = student.Account.PhoneNumber;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Student?> GetStudentByIdWithAccountAsync(int id)
        {
            return await _dbContext.Students.Where(s => s.Active_YN == true)
                .Include(s => s.Account)
                .Include(s => s.Parent)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student?> GetStudentByAccountIdAsync(String userId)
        {
            return await _dbContext.Students.FirstOrDefaultAsync(s => s.AccountId.Equals(userId));
        }
    }
}
