using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<List<Student>> GetAllStudentsWithAccountAsync(bool archive = true);
        Task<Student?> GetStudentByIdWithAccountAsync(int id);
        Task<bool> UpdateStudentAsync(int studentId, Student student);
        Task<bool> Archive(Student student);
        Task<Student?> GetStudentByAccountIdAsync(String userId);


    }
}
