using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IParentRepository : IRepository<Parent>
    {
        Task<List<Parent>> GetAllParentsWithAccountAsync(bool archive = true);
        Task<Parent?> GetParentByIdWithAccountAsync(int id);
        Task<bool> Archive(Parent parent);
        Task<Parent?> GetParentByStudentIdAsync(int studentId);

        Task<bool> UpdateParentAsync(int parentId, Parent parent);
        Task<Parent?> GetParentByAccountId(String userId);
        Task<List<Student>> GetStudentsByParentId(int parentId);

    }
}
