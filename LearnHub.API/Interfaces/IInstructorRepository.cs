using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IInstructorRepository
    {
        Task<bool> CreateInstructorAsync(Instructor instructor);
    }
}
