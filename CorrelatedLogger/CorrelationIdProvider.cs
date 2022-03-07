using Microsoft.AspNetCore.Http;

namespace CorrelatedLogger;

internal class CorrelationIdProvider : ICorrelationIdProvider
{
    public string CorrelationId { get; }

    public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        CorrelationId = GetLogCorrelationId(httpContextAccessor);
    }

    private static string GetLogCorrelationId(IHttpContextAccessor httpContextAccessor)
    {
        var header = httpContextAccessor.HttpContext.Request.Headers[ICorrelationIdProvider.Header];
        return header.Count == 0 ? Guid.NewGuid().ToString() : header.ToString();
    }
}
