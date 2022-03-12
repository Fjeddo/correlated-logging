using Microsoft.Extensions.Logging;

namespace CorrelatedLogger;

public class CorrelatedLoggingProvider<T> : ICorrelatedLoggingProvider<T>
{
    public ILogger<T> Log { get; }
    public ICorrelationIdProvider CorrelationIdProvider { get; }

    public CorrelatedLoggingProvider(ILogger<T> log, ICorrelationIdProvider correlationIdProvider)
    {
        Log = log;
        CorrelationIdProvider = correlationIdProvider;
    }
}