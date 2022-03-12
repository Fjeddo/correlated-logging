using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace CorrelatedLogger;

public abstract class LogCorrelatedFunctions<T>
{
    protected ILogger Log { get; private set; }
    protected ICorrelationIdProvider CorrelationIdProvider { get; }

    protected LogCorrelatedFunctions(ICorrelatedLoggingProvider<T> correlatedLoggingProvider)
    {
        Log = correlatedLoggingProvider.Log;
        CorrelationIdProvider = correlatedLoggingProvider.CorrelationIdProvider;
    }

    protected TReturn Execute<TReturn>(Func<TReturn> func, HttpRequest request) => CorrelationIdProvider.ExecuteWithinContext(request, func);
    protected TReturn Execute<TReturn>(Func<TReturn> func, Input input) => CorrelationIdProvider.ExecuteWithinContext(input, func);
    protected TReturn Execute<TReturn>(Func<TReturn> func, IDurableOrchestrationContext context)
    {
        Log = context.CreateReplaySafeLogger(Log);
        return CorrelationIdProvider.ExecuteWithinContext(context, func);
    }
}