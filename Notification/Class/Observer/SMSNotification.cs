public class SMSNotification : INotificationSubscriber
{
    public void Update(Notification notification)
    {
        Console.WriteLine($"[SMS] {notification.User.Name} received notification: {notification.Message}");
    }
}