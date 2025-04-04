public class PayPalGateway : IPaymentGateway
{
    public bool Process(Transaction transaction)
    {
        Console.WriteLine("PayPal processing transaction...");
        return transaction.PaymentMethod.ProcessPayment(transaction.Amount);
    }
}