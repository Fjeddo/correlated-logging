using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CorrelatedLogger;

public static class LoggerServiceCollectionExtensions
{
    public static IServiceCollection AddCorrelationDecoratedLogging(this IServiceCollection services)
    {
        services.TryAdd(ServiceDescriptor.Scoped(typeof(ICorrelationIdDecoratedLogger<>), typeof(CorrelationIdDecoratedLogger<>)));

        return services;
    }
}