using Microsoft.Extensions.Logging;

namespace CorrelatedLogger;

public class CorrelationIdDecoratedLogger<T> : ICorrelationIdDecoratedLogger<T>
{
    private readonly ILogger<T> _logger;
    public string? CorrelationId { get; private set; }

    public CorrelationIdDecoratedLogger(ILoggerFactory factory) => _logger = factory.CreateLogger<T>();

    public void WithCorrelationId(string correlationId) => CorrelationId = correlationId;
    public async Task<TReturn> WithCorrelationId<TReturn>(string correlationId, Func<Task<TReturn>> func)
    {
        WithCorrelationId(correlationId);
        return await func();
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter) =>
        _logger.Log(logLevel, eventId, exception, $"{formatter(state, exception!)} (correlationId={{correlationId}})", CorrelationId);

    public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);

    public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope(state);
}