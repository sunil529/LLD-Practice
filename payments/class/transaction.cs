// Step 4: Define Transaction
public class Transaction
{
    public int TransactionId { get; set; }
    public User User { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending";
}