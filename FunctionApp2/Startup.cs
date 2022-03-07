using CorrelatedLogger;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FunctionApp2.Startup))]

namespace FunctionApp2;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services
            .AddLogging()
            .AddCorrelationDecoratedLogging()
            .AddHttpContextAccessor();
    }
}