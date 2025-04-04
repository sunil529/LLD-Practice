7. Summary of Execution Flow
LogManager instance is created (Singleton).

Loggers (ConsoleLogger, FileLogger) are registered.

LogManager receives a log request (ERROR: System has thrown an exception).

Log message is stored in a concurrent queue.

Background thread dequeues and processes the log.

Log is forwarded to registered loggers.

ConsoleLogger prints to the console.

FileLogger writes to logs.txt.

End of Execution
The log message is successfully logged to both the console and file.

The process is non-blocking and efficient due to:

Thread-safe queue (ConcurrentQueue)

Background processing (asynchronous logging)

Scalability (supports multiple loggers)