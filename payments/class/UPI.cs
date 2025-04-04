public class UPI : PaymentMethod
{
    public string UPIId { get; set; }
    public override bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing UPI Payment of {amount}");
        return true;
    }
}