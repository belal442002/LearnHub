using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.QuestionBankDto;
using LearnHub.API.Repositories;

namespace LearnHub.API.Interfaces
{
    public interface IQuestionBankRepository : IRepository<QuestionBank>
    {
        Task<IEnumerable<QuestionBank>> GetAllQuestionsWithIncludeAsync();
        Task<QuestionBank?> GetByIdWithIncludeAsync(int id);
        Task<bool> UpdateQuestionAsync(int id, QuestionBank question);
        Task<bool> DeleteQuestionAsync(int id);
        Task<bool> RemoveQuestionAsync(int id);
        Task<IEnumerable<QuestionBank>> GetByCourseIdAsync(String courseId);
        Task<IEnumerable<QuestionBank>> GetByInstructorIdAsync(int instructorId);
        Task<IEnumerable<QuestionBank>> GetByCourseAndInstructorIdAsync(String courseId, int instructorId);

        Task<IEnumerable<QuestionCountByTopicDto>> GetQuestionCountByCoursePerTopicAsync(string courseId);
    }
}
