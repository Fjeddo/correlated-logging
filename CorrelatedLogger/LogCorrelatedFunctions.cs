using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace CorrelatedLogger;

public abstract class LogCorrelatedFunctions<TFunctions>
{
    protected readonly ICorrelationIdDecoratedLogger<TFunctions> Log;
    
    protected LogCorrelatedFunctions(ICorrelationIdDecoratedLogger<TFunctions> log) => Log = log;

    protected async Task<TReturn> Execute<TReturn>(Func<Task<TReturn>> func, HttpRequest request) => await Log.WithCorrelationId(request.GetCorrelationId(), func);
    protected async Task<TReturn> Execute<TReturn>(Func<Task<TReturn>> func, IDurableOrchestrationContext context) => await Log.WithCorrelationId(context.GetCorrelationId(), func);
    protected async Task<TReturn> Execute<TReturn>(Func<Task<TReturn>> func, Input input) => await Log.WithCorrelationId(input.GetCorrelationId(), func);
}