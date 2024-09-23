using LearnHub.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.UniteOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
        public IStudentRepository Student { get; }
        public IInstructorRepository Instructor { get; }
        public IQuestionBankRepository QuestionBank { get; }
        public IQBAnswersRepository QBAnswers { get; }
        public ICourseRepository Course { get; }
        public IAnnouncementRepository Announcement { get; }
        public ITopicRepository Topic { get; }
        public IDifficultyRepository Difficulty { get; }
        public IQuestionTypeRepository QuestionType { get; }
        public IMaterialTypeRepository MaterialType { get; }
        public IStudentCourseRepository StudentCourse { get; }
        public ITeachRepository Teach { get; }
        public IMaterialRepository Material { get; }
        public IAssignmentRepository Assignment { get; }
        public IAssignmentConfigRepository AssignmentConfig { get; }
        public IAssignmentQuestionRepository AssignmentQuestion { get; }
        public IEvaluationRepository Evaluation { get; }

        //new
        public IParentRepository Parent { get; }


        // Functions
        Task BeginTransactionAsync();
        Task<int> SaveChangesAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<bool> CompleteAsync();
        
    }
}
