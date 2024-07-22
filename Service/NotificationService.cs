using BackendApp.Data;
using BackendApp.Model;
using BackendApp.Model.Enums;

namespace BackendApp.Service{
    public interface INotificationService {
        public Notification? GetNotificationById(ulong id);
        public Notification[] GetAllNotifications();
        public bool AddNotifications(Notification notification);
        public bool RemoveNotifications(ulong id);
        public UpdateResult UpdateNotifications(ulong id, Notification notificationContent);
    }

    public class NotificationService(ApiContext context) : INotificationService
    {
        private readonly ApiContext context = context;
        public bool AddNotifications(Notification notification)
        {
            if(this.GetNotificationById(notification.Id) is not null) return false;
            this.context.Notifications.Add(notification);
            this.context.SaveChanges();
            return true;
        }

        public Notification[] GetAllNotifications()
            => this.context.Notifications.ToArray();
        public Notification? GetNotificationById(ulong id)
            => this.context.Notifications.FirstOrDefault( 
                notif => notif.Id == id 
            );

        public bool RemoveNotifications(ulong id){
            Notification? notif = this.GetNotificationById(id);
            if( notif is null ) return false;

            this.context.Notifications.Remove(notif);
            this.context.SaveChanges();
            return true;
        }

        public UpdateResult UpdateNotifications(ulong id, Notification notif)
        {
            //Check if user exists
            Notification? notifInDb = this.GetNotificationById(id);
            if(notifInDb is null) return UpdateResult.NotFound;

            //Save new data
            notifInDb.Update(notif);
            this.context.SaveChanges();
            return UpdateResult.Ok;
        }
    }
}