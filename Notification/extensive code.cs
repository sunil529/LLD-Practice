using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// 1. Observer Interface
public interface INotificationSubscriber
{
    Task UpdateAsync(Notification notification);
}

// 2. Concrete Observers (Email, SMS, Push Notifications)
public class EmailNotification : INotificationSubscriber
{
    public async Task UpdateAsync(Notification notification)
    {
        await Task.Delay(100); // Simulating network delay
        Console.WriteLine($"[EMAIL] {notification.User.Name} received: {notification.Message}");
    }
}

public class SMSNotification : INotificationSubscriber
{
    public async Task UpdateAsync(Notification notification)
    {
        await Task.Delay(200);
        Console.WriteLine($"[SMS] {notification.User.Name} received: {notification.Message}");
    }
}

public class PushNotification : INotificationSubscriber
{
    public async Task UpdateAsync(Notification notification)
    {
        await Task.Delay(50);
        Console.WriteLine($"[PUSH] {notification.User.Name} received: {notification.Message}");
    }
}

// 3. Notification Publisher Interface
public interface INotificationPublisher
{
    void Attach(INotificationSubscriber subscriber);
    void Detach(INotificationSubscriber subscriber);
    Task NotifyAsync(Notification notification);
}

// 4. Concrete Notification Manager with Queue, Priority, and Batch Processing
public class NotificationManager : INotificationPublisher
{
    private List<INotificationSubscriber> subscribers = new List<INotificationSubscriber>();
    private PriorityQueue<Notification, int> notificationQueue = new();
    private readonly int retryLimit = 3;

    public void Attach(INotificationSubscriber subscriber)
    {
        subscribers.Add(subscriber);
    }

    public void Detach(INotificationSubscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }

    public void AddToQueue(Notification notification, int priority = 1)
    {
        notificationQueue.Enqueue(notification, priority);
    }

    public async Task ProcessBatchAsync(int batchSize = 5)
    {
        List<Task> batchTasks = new List<Task>();

        while (notificationQueue.Count > 0 && batchSize-- > 0)
        {
            notificationQueue.TryDequeue(out Notification notification, out int _);
            batchTasks.Add(NotifyWithRetryAsync(notification));
        }

        await Task.WhenAll(batchTasks);
    }

    private async Task NotifyWithRetryAsync(Notification notification)
    {
        for (int attempt = 1; attempt <= retryLimit; attempt++)
        {
            try
            {
                await NotifyAsync(notification);
                LogNotification(notification, true);
                return;
            }
            catch (Exception)
            {
                if (attempt == retryLimit)
                {
                    LogNotification(notification, false);
                    Console.WriteLine($"[ERROR] Failed to send notification to {notification.User.Name} after {retryLimit} attempts.");
                }
                else
                {
                    Console.WriteLine($"[RETRY] Retrying notification for {notification.User.Name} (Attempt {attempt})...");
                    await Task.Delay(attempt * 1000);
                }
            }
        }
    }

    public async Task NotifyAsync(Notification notification)
    {
        List<Task> tasks = new List<Task>();
        foreach (var subscriber in subscribers)
        {
            tasks.Add(subscriber.UpdateAsync(notification));
        }
        await Task.WhenAll(tasks);
    }

    private void LogNotification(Notification notification, bool success)
    {
        Console.WriteLine($"[LOG] Notification to {notification.User.Name}: {notification.Message} - {(success ? "SUCCESS" : "FAILED")}");
    }
}

// 5. Core Entities
public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
}

public class Notification
{
    public string Message { get; set; }
    public User User { get; set; }
}

// 6. Third-Party Integration via Adapter Pattern
public class ThirdPartyNotificationAdapter : INotificationSubscriber
{
    public async Task UpdateAsync(Notification notification)
    {
        // Simulate calling an external service like Twilio, Firebase
        await Task.Delay(300);
        Console.WriteLine($"[THIRD-PARTY] Sent notification via external service to {notification.User.Name}");
    }
}

// 7. Rate Limiting Implementation (Simple Token Bucket)
public class RateLimiter
{
    private int _tokens;
    private readonly int _maxTokens;
    private readonly int _refillRate;
    private DateTime _lastRefillTime;

    public RateLimiter(int maxTokens, int refillRate)
    {
        _maxTokens = maxTokens;
        _refillRate = refillRate;
        _tokens = maxTokens;
        _lastRefillTime = DateTime.UtcNow;
    }

    public bool Allow()
    {
        Refill();
        if (_tokens > 0)
        {
            _tokens--;
            return true;
        }
        return false;
    }

    private void Refill()
    {
        var now = DateTime.UtcNow;
        var elapsed = (now - _lastRefillTime).TotalSeconds;
        int refillAmount = (int)(elapsed * _refillRate);
        if (refillAmount > 0)
        {
            _tokens = Math.Min(_maxTokens, _tokens + refillAmount);
            _lastRefillTime = now;
        }
    }
}

// 8. Test the Notification System
class Program
{
    static async Task Main()
    {
        // Create a user
        User user = new User { UserId = 1, Name = "John Doe" };

        // Create a notification manager
        NotificationManager notificationManager = new NotificationManager();

        // Create different notification channels (subscribers)
        INotificationSubscriber emailNotifier = new EmailNotification();
        INotificationSubscriber smsNotifier = new SMSNotification();
        INotificationSubscriber pushNotifier = new PushNotification();
        INotificationSubscriber thirdPartyNotifier = new ThirdPartyNotificationAdapter();

        // Register subscribers
        notificationManager.Attach(emailNotifier);
        notificationManager.Attach(smsNotifier);
        notificationManager.Attach(pushNotifier);
        notificationManager.Attach(thirdPartyNotifier);

        // Rate Limiting
        RateLimiter rateLimiter = new RateLimiter(10, 5); // Max 10 messages, refills 5/sec

        // Create a batch of notifications
        for (int i = 0; i < 10; i++)
        {
            if (rateLimiter.Allow())
            {
                Notification newNotification = new Notification
                {
                    Message = $"Notification {i + 1}",
                    User = user
                };
                notificationManager.AddToQueue(newNotification, i % 3 == 0 ? 10 : 1); // Priority handling
            }
            else
            {
                Console.WriteLine("[RATE LIMIT] Skipped notification due to rate limiting.");
            }
        }

        // Process batch notifications
        await notificationManager.ProcessBatchAsync();
    }
}
