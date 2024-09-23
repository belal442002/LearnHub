using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IMaterialRepository : IRepository<Material>
    {
        Task<IEnumerable<Material>> GetAllWithIncludeAsync();
        Task<IEnumerable<Material>> GetAllByCourseIdAsync(String courseId, int? materialTypeId);
    }
}
