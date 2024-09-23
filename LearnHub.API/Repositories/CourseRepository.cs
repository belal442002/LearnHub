using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.CourseDto;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public CourseRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesWithIncludeAsync()
        {
            var courses = await _dbContext.Courses
                .Include(c => c.Teaches).ThenInclude(t => t.Instructor).ThenInclude(i => i.Account)
                .Include(c => c.Questions).ThenInclude(q => q.Difficulty)
                .Include(c => c.Questions).ThenInclude(q => q.QuestionType)
                .Include(c => c.Questions).ThenInclude(q => q.Topic)
                .Include(c => c.Questions).ThenInclude(q => q.Answers)
                .Include(c => c.Materials)
                .Include(c => c.Announcements)
                .Select(c => new Course
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Questions = c.Questions,
                    Materials = c.Materials,
                    Announcements = c.Announcements,
                    Teaches = c.Teaches.Where(t => t.Instructor.Active_YN == true).ToList()
                })
                .ToListAsync();

            return courses;
        }

        public async Task<Course> GetCourseByIdWithIncludeAsync(string id)
        {

            var course = await _dbContext.Courses
                .Include(c => c.Teaches).ThenInclude(t => t.Instructor).ThenInclude(i => i.Account)
                .Include(c => c.Questions).ThenInclude(q => q.Difficulty)
                .Include(c => c.Questions).ThenInclude(q => q.QuestionType)
                .Include(c => c.Questions).ThenInclude(q => q.Topic)
                .Include(c => c.Questions).ThenInclude(q => q.Answers)
                .Include(c => c.Materials)
                .Include(c => c.Announcements)
                .Select(c => new Course
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Questions = c.Questions,
                    Materials = c.Materials,
                    Announcements = c.Announcements,
                    Teaches = c.Teaches.Where(t => t.Instructor.Active_YN == true).ToList()
                })
                .FirstOrDefaultAsync(c => c.Id == id);

            return course!;
        }

        public async Task<IEnumerable<CourseWithStudentCountDto>> GetCourseWithStudentCountAsync(int? semesterId, int? year)
        {
            var studentCourses = _dbContext.StudentCourses.AsQueryable();

            if (semesterId != null)
            {
                studentCourses = studentCourses.Where(sc => sc.SemesterId == semesterId);
            }
            if (year != null)
            {
                studentCourses = studentCourses.Where(sc => sc.Year == year);
            }

            return await studentCourses
                .Include(sc => sc.Course)
                .GroupBy(sc => new { sc.CourseId, sc.Course.Name })
                .Select(g => new CourseWithStudentCountDto
                {
                    CourseId = g.Key.CourseId,
                    CourseName = g.Key.Name,
                    StudentCount = g.Count()
                }).ToListAsync();
        }

        public async Task<CourseWithStudentCountDto?> GetSpeceficCourseWithStudentCountAsync(String courseId, int? semesterId, int? year)
        {
            var studentCourses = _dbContext.StudentCourses
                .Where(c => c.CourseId.Equals(courseId)).AsQueryable();

            if (semesterId != null)
            {
                studentCourses = studentCourses.Where(sc => sc.SemesterId == semesterId);
            }
            if (year != null)
            {
                studentCourses = studentCourses.Where(sc => sc.Year == year);
            }

            return await studentCourses
                .Include(sc => sc.Course)
                .GroupBy(sc => new { sc.CourseId, sc.Course.Name })
                .Select(g => new CourseWithStudentCountDto
                {
                    CourseId = g.Key.CourseId,
                    CourseName = g.Key.Name,
                    StudentCount = g.Count()
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            var courses = await _dbContext.Courses.ToListAsync();
            return courses;
        }
    }
}
