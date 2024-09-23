using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LearnHub.API.Repositories
{
    public class StudentCourseRepository : Repository<StudentCourse>, IStudentCourseRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public StudentCourseRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistAsync(StudentCourse studentCourse)
        {
            return await _dbContext.StudentCourses
                .AnyAsync(sc => sc.Year == studentCourse.Year
                && sc.StudentId == studentCourse.StudentId 
                && sc.CourseId.Equals(studentCourse.CourseId));
        }

        public async Task<IEnumerable<StudentCourse>> GetAllStudentCoursesWithIncludeAsync(int? studentId, string? courseId, int? semesterId, int? year)
        {
            var studentCourses = _dbContext.StudentCourses
                .Include(sc => sc.Course)
                .Include(sc => sc.Student)
                .Include(sc => sc.Semester)
                .AsQueryable();
            if (studentId != null)
            {
                studentCourses = studentCourses.Where(sc => sc.StudentId == studentId);
            }
            if (!courseId.IsNullOrEmpty())
            {
                studentCourses = studentCourses.Where(sc => sc.CourseId == courseId);
            }
            if (semesterId != null)
            {
                studentCourses = studentCourses.Where(sc => sc.SemesterId == semesterId);
            }
            if (year != null)
            {
                studentCourses = studentCourses.Where(sc => sc.Year == year);
            }

            return await studentCourses.ToListAsync();
        }

        public async Task<StudentCourse?> GetByStudentAndCourseIdAsync(int studentId, string courseId)
        {
            return await _dbContext.StudentCourses
                .Where(sc => sc.StudentId == studentId && courseId.Equals(courseId))
                .OrderByDescending(sc => sc.Id)!.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<StudentCourse>> GetStudentCoursesByStudentIdAsync(int studentId, int? semesterId, int? year)
        {
            var studentCourses = _dbContext.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .Include(sc => sc.Course)
                .Include(sc => sc.Student)
                .Include(sc => sc.Semester)
                .AsQueryable();

            if (semesterId != null)
            {
                studentCourses = studentCourses.Where(sc => sc.SemesterId == semesterId);
            }
            if (year != null)
            {
                studentCourses = studentCourses.Where(sc => sc.Year == year);
            }

            return await studentCourses.ToListAsync();
        }

        public async Task<IEnumerable<StudentCourse>> GetStudentCoursesByCourseIdAsync(string courseId, int? semesterId, int? year)
        {
            var studentCourses = _dbContext.StudentCourses
                .Where(sc => sc.CourseId.Equals(courseId))
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .Include(sc => sc.Semester)
                .AsQueryable();

            if (semesterId != null)
            {
                studentCourses = studentCourses.Where(sc => sc.SemesterId == semesterId);
            }
            if (year != null)
            {
                studentCourses = studentCourses.Where(sc => sc.Year == year);
            }

            return await studentCourses.ToListAsync();
        }
    }
}
