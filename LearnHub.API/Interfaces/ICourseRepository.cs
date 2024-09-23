using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.CourseDto;

namespace LearnHub.API.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetAllCoursesWithIncludeAsync();
        Task<Course> GetCourseByIdWithIncludeAsync(string id);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<IEnumerable<CourseWithStudentCountDto>> GetCourseWithStudentCountAsync(int? semesterId, int? year);
        Task<CourseWithStudentCountDto?> GetSpeceficCourseWithStudentCountAsync(String courseId, int? semesterId, int? year);
    }
}
