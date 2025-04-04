public class NotificationManager : INotificationPublisher
{
    private List<INotificationSubscriber> subscribers = new List<INotificationSubscriber>();
    
    public void Attach(INotificationSubscriber subscriber)
    {
        subscribers.Add(subscriber);
    }
    
    public void Detach(INotificationSubscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }
    
    public void Notify(Notification notification)
    {
        foreach (var subscriber in subscribers)
        {
            subscriber.Update(notification);
        }
    }
}