namespace CorrelatedLogger;

public interface ICorrelationIdProvider
{
    public const string Header = "x-tended-logger-corr-id";
    string CorrelationId { get; }
}