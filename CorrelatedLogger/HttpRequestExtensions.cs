using Microsoft.AspNetCore.Http;

namespace CorrelatedLogger;

public static class HttpRequestExtensions
{
    public const string Header = "x-tended-logger-corr-id";

    public static string GetCorrelationId(this HttpRequest request) =>
        request.Headers.TryGetValue(Header, out var correlationId)
            ? correlationId.ToString()
            : Guid.NewGuid().ToString();
}