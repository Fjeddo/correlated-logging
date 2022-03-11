using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace CorrelatedLogger;

public interface ICorrelationIdDecoratedLogger<out T> : ILogger<T>
{
    void WithCorrelationId(string correlationId);
    Task<TReturn> WithCorrelationId<TReturn>(string getCorrelationId, Func<Task<TReturn>> func);
    ICorrelationIdDecoratedLogger<T> MakeSafe(IDurableOrchestrationContext context);
}