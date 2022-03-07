using Microsoft.Extensions.Logging;

namespace CorrelatedLogger;

public class ExtendedLogger<T> : IExtendedLogger<T>
{
    private readonly ILogger<T> _log;
    private readonly ICorrelationIdProvider _correlationIdProvider;

    public ExtendedLogger(ILogger<T> log, ICorrelationIdProvider correlationIdProvider)
    {
        _log = log;
        _correlationIdProvider = correlationIdProvider;
    }

    public void LogInformation(string message)
    {
        if (_log.IsEnabled(LogLevel.Information))
        {
            var correlationId = _correlationIdProvider.CorrelationId;
            _log.LogInformation("{message} (correlationId={correlationId})", message, correlationId);
        }
    }
}
