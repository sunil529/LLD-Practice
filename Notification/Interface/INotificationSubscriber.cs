

public interface INotificationSubscriber
{
    void update(Notification notification);// Update(Notification notification)	Processes received notifications	Called by Notify() in NotificationManager
}