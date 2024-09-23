using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface ITeachRepository : IRepository<Teach>
    {
        Task<bool> ExistAsync(Teach teach);
        Task<IEnumerable<Teach>> GetAllWithIncludeAsync(int? instructorId, String? courseId, int? semesterId, int? year);
        Task<Teach?> GetByInstructorAndCourseIdAsync(int instructorId, String courseId);
        Task<IEnumerable<Teach>> GetTeachesByInstructorIdAsync(int instructorId, int? semesterId, int? year);
        Task<IEnumerable<Teach>> GetTeachesByCourseIdAsync(String courseId, int? semesterId, int? year);

    }
}
