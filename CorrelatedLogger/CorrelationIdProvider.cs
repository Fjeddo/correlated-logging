using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace CorrelatedLogger;

public class CorrelationIdProvider : ICorrelationIdProvider
{
    public CorrelationIdProvider() => CorrelationId = Guid.NewGuid().ToString();

    public TReturn ExecuteWithinContext<TReturn>(HttpRequest request, Func<TReturn> func) => InitAndExecute(request.GetCorrelationId(), func);
    public TReturn ExecuteWithinContext<TReturn>(IDurableOrchestrationContext context, Func<TReturn> func) => InitAndExecute(context.GetCorrelationId(), func);
    public TReturn ExecuteWithinContext<TReturn>(Input input, Func<TReturn> func) => InitAndExecute(input.GetCorrelationId(), func);

    public string CorrelationId { get; private set; }

    private TReturn InitAndExecute<TReturn>(string correlationId, Func<TReturn> func)
    {
        CorrelationId = correlationId;
        return func();
    }
}