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