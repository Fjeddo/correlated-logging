using System.Net.Http;
using System.Threading.Tasks;
using CorrelatedLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionApp3
{
    public class Function3 : LogCorrelatedFunctions<Function3>
    {
        private readonly Configuration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public Function3(IConfiguration configuration, IHttpClientFactory httpClientFactory, ICorrelatedLoggingProvider<Function3> correlatedLoggingProvider) : base(correlatedLoggingProvider)
        {
            _configuration = configuration.Get<Configuration>();
            _httpClientFactory = httpClientFactory;
        }

        public class Configuration
        {
            public string OrchestrationStarterUrl { get; set; }
        }

        [FunctionName("Function3")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req) =>
            await Execute(async () =>
            {
                Log.LogInformation("Function3 is about to start an orchestration, via http invocation");

                var httpClient = _httpClientFactory.CreateClient();
                await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, _configuration.OrchestrationStarterUrl) {Headers = {{ContextExtensions.Header, CorrelationIdProvider.CorrelationId}}});

                return new OkObjectResult("Hello from Function3");
            }, req);
    }
}
