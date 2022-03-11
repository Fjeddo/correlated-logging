using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace CorrelatedLogger;

public class CorrelationIdDecoratedLogger<T> : ICorrelationIdDecoratedLogger<T>
{
    private readonly ILoggerFactory _loggerFactory;
    
    private ILogger? _logger;
    private ILogger Logger
    {
        get { return _logger ??= _loggerFactory.CreateLogger<T>(); }
    }

    private string? _correlationId;

    public CorrelationIdDecoratedLogger(ILoggerFactory factory) => _loggerFactory = factory;

    public void WithCorrelationId(string correlationId) => _correlationId = correlationId;

    public async Task<TReturn> WithCorrelationId<TReturn>(string correlationId, Func<Task<TReturn>> func)
    {
        WithCorrelationId(correlationId);
        return await func();
    }

    public ICorrelationIdDecoratedLogger<T> MakeSafe(IDurableOrchestrationContext context)
    {
        _logger = context.CreateReplaySafeLogger(Logger);
        return this;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter) =>
        Logger.Log(logLevel, eventId, exception, $"{formatter(state, exception!)} (correlationId={{correlationId}})", _correlationId);

    public bool IsEnabled(LogLevel logLevel) => Logger.IsEnabled(logLevel);

    public IDisposable BeginScope<TState>(TState state) => Logger.BeginScope(state);
}