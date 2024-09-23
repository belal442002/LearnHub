using LearnHub.API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace LearnHub.API.Helper
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendGradeNotification(string parentId, string message)
        {
            await _hubContext.Clients.User(parentId).SendAsync("ReceiveNotification", message);
        }

        public async Task SendQuizNotification(string studentId, string message)
        {
            await _hubContext.Clients.User(studentId).SendAsync("ReceiveNotification", message);
        }
    }
}
