using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using LearnHub.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LearnHub.API.UniteOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LearnHubDbContext _dbContext;
        private IDbContextTransaction _transaction;

        public IStudentRepository Student { get; private set; }
        public IInstructorRepository Instructor { get; private set; }

        public IQuestionBankRepository QuestionBank { get; private set; }

        public IQBAnswersRepository QBAnswers { get; private set; }
        public ICourseRepository Course { get; private set; }
        public IAnnouncementRepository Announcement { get; private set; }
        public ITopicRepository Topic { get; private set; }
        public IDifficultyRepository Difficulty { get; private set; }
        public IQuestionTypeRepository QuestionType { get; private set; }
        public IMaterialTypeRepository MaterialType { get; private set; }

        public IStudentCourseRepository StudentCourse { get; private set; }
        public ITeachRepository Teach { get; private set; }
        public IMaterialRepository Material { get; private set; }
        public IAssignmentRepository Assignment { get; private set; }
        public IAssignmentConfigRepository AssignmentConfig { get; private set; }
        public IAssignmentQuestionRepository AssignmentQuestion { get; private set; }
        public IEvaluationRepository Evaluation { get; private set; }
        //new
        public IParentRepository Parent { get; private set; }





        public UnitOfWork(LearnHubDbContext dbContext)
        {
            _dbContext = dbContext;
            Instructor = new InstructorRepository(dbContext);
            Student = new StudentRepository(dbContext);
            QuestionBank = new QuestionBankRepository(dbContext);
            QBAnswers = new QBAnswersRepository(dbContext);
            Course = new CourseRepository(dbContext);
            Announcement = new AnnouncementRepository(dbContext);
            Topic = new TopicRepository(dbContext);
            Difficulty = new DifficultyRepository(dbContext);
            QuestionType = new QuestionTypeRepository(dbContext);
            MaterialType = new MaterialTypeRepository(dbContext);
            StudentCourse = new StudentCourseRepository(dbContext);
            Teach = new TeachRepository(dbContext);
            Material = new MaterialRepository(dbContext);
            Assignment = new AssignmentRepository(dbContext);
            AssignmentConfig = new AssignmentConfigRepository(dbContext);
            AssignmentQuestion = new AssignmentQuestionRepository(dbContext);
            Evaluation = new EvaluationRepository(dbContext);
            Parent = new ParentRepository(dbContext);


        }
        public async Task CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task<bool> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
