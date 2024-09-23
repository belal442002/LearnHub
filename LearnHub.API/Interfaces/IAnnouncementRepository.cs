using LearnHub.API.Models.Domain;

namespace LearnHub.API.Interfaces
{
    public interface IAnnouncementRepository : IRepository<Announcement>
    {
        Task<IEnumerable<Announcement>> GetAllAnnouncementWithIncludeAsync(String courseId);
        Task<bool> UpdateAnnouncementAsync(int announcementId, Announcement announcement);
    }
}
