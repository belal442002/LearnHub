using LearnHub.API.Data;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Repositories
{
    public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public AnnouncementRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Announcement>> GetAllAnnouncementWithIncludeAsync(String courseId)
        {
            return await _dbContext.Announcements
                .Where(a => a.CourseId == courseId)
                .Include(a => a.Instructor)
                .OrderBy(a => a.DateOfAnnouncement).ToListAsync();
        }

        public async Task<bool> UpdateAnnouncementAsync(int announcementId, Announcement announcement)
        {
            var announcementDomainModel = await _dbContext.Announcements.FirstOrDefaultAsync(a => a.Id == announcementId);

            if(announcementDomainModel == null)
            {
                throw new Exception("Incorerct announcement id");
            }
            announcementDomainModel.Text = announcement.Text;

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
