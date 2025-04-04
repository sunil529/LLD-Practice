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
