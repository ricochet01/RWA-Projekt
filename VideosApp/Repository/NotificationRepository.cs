using System.Diagnostics.Metrics;
using VideosApp.Data;
using VideosApp.Interface;
using VideosApp.Model;

namespace VideosApp.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataContext context;

        public NotificationRepository(DataContext context)
        {
            this.context = context;
        }

        public ICollection<Notification> GetNotifications()
            => context.Notifications.OrderBy(n => n.Id).ToList();

        public Notification GetNotification(int id)
            => context.Notifications.FirstOrDefault(n => n.Id == id);

        public Notification GetNotification(string subject)
            => context.Notifications.FirstOrDefault(n => n.Subject == subject);

        public bool NotificationExists(int id)
            => context.Notifications.Any(n => n.Id == id);

        public bool CreateNotification(Notification notification)
        {
            context.Add(notification);

            return Save();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();

            return saved > 0;
        }
    }
}
