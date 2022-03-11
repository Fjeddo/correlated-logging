using Microsoft.AspNetCore.Http;

namespace CorrelatedLogger;

public abstract class HttpTriggeredFunctions<TFunctions>
{
    protected readonly ICorrelationIdDecoratedLogger<TFunctions> Log;
    protected HttpTriggeredFunctions(ICorrelationIdDecoratedLogger<TFunctions> log) => Log = log;
    protected async Task<TReturn> Execute<TReturn>(Func<Task<TReturn>> func, HttpRequest request) => await Log.WithCorrelationId(request.GetCorrelationId(), func);
}