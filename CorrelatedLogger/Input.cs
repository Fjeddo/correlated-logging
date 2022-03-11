using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace CorrelatedLogger;

public class Input<T> : Input
{
    public T Data { get; init; }
    public Input(T data, string correlationId) : base(correlationId) => Data = data;
    public static implicit operator T(Input<T> input) => input.Data;
}

public class Input
{
    public string? CorrelationId { get; init; }
    public Input(string correlationId) => CorrelationId = correlationId;

    public static Input<T> CreateInstance<T>(T data, Input input) => new(data, input.GetCorrelationId());
    public static Input<T> CreateInstance<T>(T data, HttpRequest request) => new(data, request.GetCorrelationId());
    public static Input<T> CreateInstance<T>(T data, IDurableOrchestrationContext context) => new(data, context.GetCorrelationId());

    public static Input CreateInstance(Input input) => new(input.GetCorrelationId());
    public static Input CreateInstance(HttpRequest request) => new(request.GetCorrelationId());
    public static Input CreateInstance(IDurableOrchestrationContext context) => new(context.GetCorrelationId());
}