using Microsoft.Extensions.Logging;

namespace CorrelatedLogger;

public class CorrelationIdDecoratedLogger<T> : ILogger<T>
{
    private readonly ICorrelationIdProvider _correlationIdProvider;

    private readonly ILogger _logger;

    public CorrelationIdDecoratedLogger(ILoggerFactory factory, ICorrelationIdProvider correlationIdProvider)
    {
        _logger = factory.CreateLogger<T>();
        _correlationIdProvider = correlationIdProvider;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter) =>
        Log(logLevel,
            eventId,
            (state as IEnumerable<KeyValuePair<string, object>> ?? Array.Empty<KeyValuePair<string, object>>()).Append(new KeyValuePair<string, object>("correlationId", _correlationIdProvider.CorrelationId)),
            exception,
            (_, e) => $"{formatter(state, e)} (correlationId={_correlationIdProvider.CorrelationId})".TrimStart());

    private void Log(LogLevel logLevel, EventId eventId, IEnumerable<KeyValuePair<string, object>> state, Exception? exception, Func<IEnumerable<KeyValuePair<string, object>>, Exception, string> formatter) => _logger.Log(logLevel, eventId, state, exception, formatter!);

    public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);

    public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope(state);
}