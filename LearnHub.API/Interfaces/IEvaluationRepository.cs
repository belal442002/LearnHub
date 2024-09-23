using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IEvaluationRepository : IRepository<Evaluation>
    {
        Task<ICollection<Evaluation>> GetAllEvaluationsAsync(String courseId, int? assignmentId, int? studentId, int? assignmentNumber);
        Task<Evaluation?> GetEvaluationAsync(int assignmentId, int studentId);
    }
}
