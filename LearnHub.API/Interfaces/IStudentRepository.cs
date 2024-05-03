using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IStudentRepository
    {
        Task<bool> CreateStudentAsync(Student student);
    }
}
