using Microsoft.Extensions.Logging;

namespace CorrelatedLogger;

public interface ICorrelatedLoggingProvider<out T>
{
    ILogger<T> Log { get; }
    ICorrelationIdProvider CorrelationIdProvider { get; }
}