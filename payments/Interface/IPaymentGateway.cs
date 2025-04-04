// Step 5: Define Payment Gateway Interface & Implementations
public interface IPaymentGateway
{
    bool Process(Transaction transaction);
}