public class EmailNotification : INotificationSubscriber
{
    public void Update(Notification notification)
    {
        Console.WriteLine($"[EMAIL] {notification.User.Name} received notification: {notification.Message}");
    }
}