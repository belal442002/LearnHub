using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface ITopicRepository : IRepository<Topic>
    {
        Task<List<Topic>> GetTopicsByCourseIdAsync(string courseId);
        Task<bool> DeleteTopicAsync(int topicId);
        Task<bool> UpdateTopicAsync(int topicId, Topic topic);

    }
}
