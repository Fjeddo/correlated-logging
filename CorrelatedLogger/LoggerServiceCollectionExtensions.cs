using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CorrelatedLogger;

public static class LoggerServiceCollectionExtensions
{
    public static IServiceCollection AddCorrelationDecoratedLogging(this IServiceCollection services) =>
        services
            .AddScoped(typeof(ICorrelatedLoggingProvider<>), typeof(CorrelatedLoggingProvider<>))
            .AddScoped(typeof(ILogger<>), typeof(CorrelationIdDecoratedLogger<>))
            .AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();
}