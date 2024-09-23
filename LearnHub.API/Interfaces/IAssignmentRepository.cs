using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<IEnumerable<Assignment>> GetAllAssignmentsByCourseIdAsync(string courseId);

        Task<bool> AddAssignmentAsync(string courseId, int assignmentConfigId);
        Task<Assignment?> GetByIdWithIncludeAsync(int assignmentId);
        Task<bool> UpdateAssignmentAsync(int assignmentId, Assignment assignment);
        Task<bool> DeleteAssignmentAsync(Assignment assignment);
    }
}
