using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IMaterialTypeRepository : IRepository<MaterialType>
    {
        Task<List<MaterialType>> GetAllMaterialTypesAsync();
        Task<MaterialType?> GetMaterialTypeByIdAsync(int id);
        Task<MaterialType?> GetMaterialTypeByNameAsync(string name);
    }
}
