class Program
{
    static void Main()
    {
        // Create a user
        User user = new User { UserId = 1, Name = "John Doe" };

        // Create a notification manager (publisher)
        NotificationManager notificationManager = new NotificationManager();

        // Create different notification channels (subscribers)
        INotificationSubscriber emailNotifier = new EmailNotification();
        INotificationSubscriber smsNotifier = new SMSNotification();
        INotificationSubscriber pushNotifier = new PushNotification();

        // Register subscribers
        notificationManager.Attach(emailNotifier);
        notificationManager.Attach(smsNotifier);
        notificationManager.Attach(pushNotifier);

        // Create a new notification
        Notification newNotification = new Notification
        {
            Message = "Your order has been shipped!",
            User = user
        };

        // Notify all registered subscribers
        notificationManager.Notify(newNotification);
    }
}