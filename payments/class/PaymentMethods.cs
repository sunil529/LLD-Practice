// Step 2: Define Abstract Payment Method
public abstract class PaymentMethod
{
    public abstract bool ProcessPayment(decimal amount);
}