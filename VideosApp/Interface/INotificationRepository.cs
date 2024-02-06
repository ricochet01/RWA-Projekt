using VideosApp.Model;

namespace VideosApp.Interface
{
    public interface INotificationRepository
    {
        ICollection<Notification> GetNotifications();
        Notification GetNotification(int id);
        Notification GetNotification(string subject);
        bool NotificationExists(int id);

        bool CreateNotification(Notification notification);
        bool Save();
    }
}
