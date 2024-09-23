using LearnHub.API.Helper;
using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IStudentCourseRepository : IRepository<StudentCourse>
    {
        Task<IEnumerable<StudentCourse>> GetAllStudentCoursesWithIncludeAsync(int? studentId, String? courseId, int? semesterId, int? year);
        Task<IEnumerable<StudentCourse>> GetStudentCoursesByStudentIdAsync(int studentId, int? semesterId, int? year);
        Task<IEnumerable<StudentCourse>> GetStudentCoursesByCourseIdAsync(String courseId, int? semesterId, int? year);
        Task<StudentCourse?> GetByStudentAndCourseIdAsync(int studentId, String courseId);
        Task<bool> ExistAsync(StudentCourse studentCourse);
    }
}
