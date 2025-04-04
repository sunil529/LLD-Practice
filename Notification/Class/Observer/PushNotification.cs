public class PushNotification : INotificationSubscriber
{
    public void Update(Notification notification)
    {
        Console.WriteLine($"[PUSH] {notification.User.Name} received notification: {notification.Message}");
    }
}