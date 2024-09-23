using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.AssignmentQuestionDto;

namespace LearnHub.API.Interfaces
{
    public interface IAssignmentQuestionRepository : IRepository<AssignmentQuestion>
    {
        Task<ICollection<AssignmentQuestionsGeneratedDto>> GenerateAssignmentQuestionsAsync(int studentId, String courseId);
        Task<AssignmentQuestionsGeneratedWithDurationDto> GenerateAssignmentQuestionsWithDurationAsync(int studentId, String courseId);
        Task<Evaluation> SaveStudentAnswersAsync(StudentAnswersSubmissionDto submission);
        Task<ICollection<AssignmentQuestion>> GetAllAsync(String courseId, int? studentId, int? assignmentId, int? assignmentNumber);
    }
}
