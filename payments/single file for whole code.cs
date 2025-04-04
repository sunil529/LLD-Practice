using System;
using System.Collections.Generic;
using System.Threading;

// Step 1: Define User & Payment Account
public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
}

public class PaymentAccount
{
    public int AccountId { get; set; }
    public User Owner { get; set; }
    public decimal Balance { get; set; }
}

// Step 2: Define Abstract Payment Method
public abstract class PaymentMethod
{
    public abstract bool ProcessPayment(decimal amount);
}

// Step 3: Define Concrete Payment Methods
public class CreditCard : PaymentMethod
{
    public string CardNumber { get; set; }
    public override bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing Credit Card payment of {amount}");
        return true; // Simulate success
    }
}

public class BankAccount : PaymentMethod
{
    public string AccountNumber { get; set; }
    public override bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing Bank Transfer of {amount}");
        return true; 
    }
}

public class UPI : PaymentMethod
{
    public string UPIId { get; set; }
    public override bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing UPI Payment of {amount}");
        return true;
    }
}

// Step 4: Define Transaction
public class Transaction
{
    public int TransactionId { get; set; }
    public User User { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending";
}

// Step 5: Define Payment Gateway Interface & Implementations
public interface IPaymentGateway
{
    bool Process(Transaction transaction);
}

public class StripeGateway : IPaymentGateway
{
    public bool Process(Transaction transaction)
    {
        Console.WriteLine("Stripe processing transaction...");
        return transaction.PaymentMethod.ProcessPayment(transaction.Amount);
    }
}

public class PayPalGateway : IPaymentGateway
{
    public bool Process(Transaction transaction)
    {
        Console.WriteLine("PayPal processing transaction...");
        return transaction.PaymentMethod.ProcessPayment(transaction.Amount);
    }
}

// Step 6: Observer Pattern for Notifications
public interface INotificationObserver
{
    void Notify(Transaction transaction);
}

public class EmailNotifier : INotificationObserver
{
    public void Notify(Transaction transaction)
    {
        Console.WriteLine($"Email: Your transaction {transaction.TransactionId} is {transaction.Status}");
    }
}

public class SMSNotifier : INotificationObserver
{
    public void Notify(Transaction transaction)
    {
        Console.WriteLine($"SMS: Your transaction {transaction.TransactionId} is {transaction.Status}");
    }
}

// Step 7: Payment Manager with Retry Mechanism
public class PaymentManager
{
    private List<INotificationObserver> _observers = new List<INotificationObserver>();
    private IPaymentGateway _gateway;

    public PaymentManager(IPaymentGateway gateway)
    {
        _gateway = gateway;
    }

    public void Attach(INotificationObserver observer) => _observers.Add(observer);
    public void Detach(INotificationObserver observer) => _observers.Remove(observer);

    public void ProcessTransaction(Transaction transaction)
    {
        int retryCount = 3;
        while (retryCount > 0)
        {
            bool success = _gateway.Process(transaction);
            if (success)
            {
                transaction.Status = "Success";
                break;
            }
            retryCount--;
            Thread.Sleep(1000);
        }
        if (retryCount == 0) transaction.Status = "Failed";

        foreach (var observer in _observers) observer.Notify(transaction);
    }
}

// Step 8: Running the Payment System
class Program
{
    static void Main()
    {
        User user = new User { UserId = 1, Name = "John Doe" };
        PaymentMethod creditCard = new CreditCard { CardNumber = "1234-5678-9876-5432" };
        user.PaymentMethods.Add(creditCard);

        Transaction transaction = new Transaction
        {
            TransactionId = 101,
            User = user,
            PaymentMethod = creditCard,
            Amount = 100.0m
        };

        PaymentManager paymentManager = new PaymentManager(new StripeGateway());
        paymentManager.Attach(new EmailNotifier());
        paymentManager.Attach(new SMSNotifier());

        paymentManager.ProcessTransaction(transaction);
    }
}
