using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace CorrelatedLogger;

public static class ContextExtensions
{
    public const string Header = "x-tended-logger-corr-id";

    public static string GetCorrelationId(this IDurableOrchestrationContext context) => context.GetInput<Input>()?.CorrelationId ?? NewCorrelationId;
    public static string GetCorrelationId(this HttpRequest request) => request.Headers.TryGetValue(Header, out var correlationId) ? correlationId.ToString() : NewCorrelationId;
    public static string GetCorrelationId(this Input input) => input.CorrelationId ?? NewCorrelationId;

    private static string NewCorrelationId => Guid.NewGuid().ToString();
}