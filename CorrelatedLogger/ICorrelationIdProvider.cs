using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace CorrelatedLogger;

public interface ICorrelationIdProvider
{
    TReturn ExecuteWithinContext<TReturn>(HttpRequest request, Func<TReturn> func);
    TReturn ExecuteWithinContext<TReturn>(IDurableOrchestrationContext context, Func<TReturn> func);
    TReturn ExecuteWithinContext<TReturn>(Input input, Func<TReturn> func);

    string CorrelationId { get; }
}