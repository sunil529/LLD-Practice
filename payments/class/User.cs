// Step 1: Define User & Payment Account
public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
}