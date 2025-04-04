class Program
{
    static void Main()
    {
        // Get Singleton Instance
        LogManager logManager = LogManager.Instance;

        // Register Different Loggers
        logManager.RegisterLogger(new ConsoleLogger());
        logManager.RegisterLogger(new FileLogger());

        // Log Messages
        logManager.Log(LogLevel.INFO, "System started.");
        logManager.Log(LogLevel.ERROR, "An error occurred!");

        // Stop Logging Gracefully
        Console.ReadLine();
        logManager.StopProcessing();
    }
}
