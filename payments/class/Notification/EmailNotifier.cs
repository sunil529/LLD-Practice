public class EmailNotifier : INotificationObserver
{
    public void Notify(Transaction transaction)
    {
        Console.WriteLine($"Email: Your transaction {transaction.TransactionId} is {transaction.Status}");
    }
}