using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<List<Instructor>> GetAllInstructorsWithAccountAsync(bool archive = true);
        Task<Instructor?> GetInstructorByIdWithAccountAsync(int id);
        Task<bool> UpdateInstructorAsync(int instructorId, Instructor instructor);
        Task<bool> Archive(Instructor instructor);
        Task<Instructor?> GetInstructorByAccountId(String userId);
    }
}
