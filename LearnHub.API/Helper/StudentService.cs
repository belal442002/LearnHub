
using LearnHub.API.Data;

namespace LearnHub.API.Helper
{
    public class StudentService
    {
        private readonly LearnHubDbContext _dbContext;

         public StudentService(LearnHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public String GenerateStudentId()
        {
            var year = DateTime.UtcNow.Year;
            var count = _dbContext.Students.Count(s => s.DateOfJoin.Year == year);
            var yearId = year.ToString();
            var countId = $"{count + 1 : 0000}";
            var studentId = yearId + countId;
            return studentId.Replace(" ", "");
        }
    }
}
