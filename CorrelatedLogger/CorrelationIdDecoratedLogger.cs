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

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
        _logger.Log(
            logLevel,
            eventId,
            AppendCorrelationId(state),
            exception,
            (_, e) => $"{formatter(state, e)} (correlationId={_correlationIdProvider.CorrelationId})".TrimStart());

    private IEnumerable<KeyValuePair<string, object>> AppendCorrelationId<TState>(TState state) => 
        (state as IEnumerable<KeyValuePair<string, object>> ?? Array.Empty<KeyValuePair<string, object>>()).Append(new KeyValuePair<string, object>("correlationId", _correlationIdProvider.CorrelationId));

    public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);

    public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope(state);
}