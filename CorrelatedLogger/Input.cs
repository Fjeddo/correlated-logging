namespace CorrelatedLogger;

public class Input<T> : Input
{
    public T Data { get; init; }
    public Input(T data, string correlationId) : base(correlationId) => Data = data;
    public static implicit operator T(Input<T> input) => input.Data;
}

public class Input
{
    public string? CorrelationId { get; }
    public Input(string correlationId) => CorrelationId = correlationId;

    public static Input CreateInstance(ICorrelationIdProvider correlationIdProvider) => new(correlationIdProvider.CorrelationId);
    public static Input<TData> CreateInstance<TData>(TData data, ICorrelationIdProvider correlationIdProvider) => new(data, correlationIdProvider.CorrelationId);
}