using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IQuestionTypeRepository : IRepository<QuestionType>
    {
        Task<List<QuestionType>> GetQuestionTypesWithQuestionsAsync();
        Task<QuestionType> GetQuestionTypeByNameWithQuestionsAsync(string name);
        Task<QuestionType> GetQuestionTypeByIdWithQuestionsAsync(int id);


    }
}
