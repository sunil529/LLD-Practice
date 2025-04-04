public interface INotificationPublisher
{
    void Attach(INotificationSubscriber subscriber);
    void Detach(INotificationSubscriber subscriber);
    void Notify(Notificaiton notification)//Notify(Notification notification)	Sends updates to all observers	Called by the system to trigger notifications
}