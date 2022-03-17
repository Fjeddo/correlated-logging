using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CorrelatedLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class Function1 : LogCorrelatedFunctions<Function1>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITestService _testService;
        private readonly Configuration _configuration;

        public Function1(IHttpClientFactory httpClientFactory, IConfiguration configuration, ITestService testService, ICorrelatedLoggingProvider<Function1> correlatedLoggingProvider) : base(correlatedLoggingProvider)
        {
            _httpClientFactory = httpClientFactory;
            _testService = testService;
            _configuration = configuration.Get<Configuration>();
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(Microsoft.Azure.WebJobs.Extensions.Http.AuthorizationLevel.Function, "get", Route = null)] HttpRequest req) =>
            await Execute(async () =>
            {
                Log.LogTrace("Log trace test");
                Log.LogDebug("Log debug test");

                Log.LogInformation("Will do a get request to Function 2, {prop1}, {prop2}, {prop3}", DateTimeOffset.UtcNow, "test", 3);
                Log.LogInformation(new AbandonedMutexException("Info 123"), "Information with exception");

                Log.LogWarning("This is a warning test");
                Log.LogWarning(new AbandonedMutexException("Warning 123"), "Warning with exception");

                Log.LogError(new AbandonedMutexException("Error 123"), "This is an exception error test");
                Log.LogError("This is an error test");

                _testService.TestLogger("Hello world!");

                var httpClient = _httpClientFactory.CreateClient();

                var requestTo2 = new HttpRequestMessage(HttpMethod.Get, _configuration.Function2Url)
                {
                    Headers = {{ContextExtensions.Header, CorrelationIdProvider.CorrelationId}}
                };

                var response = await httpClient.SendAsync(requestTo2);

                Log.LogInformation("Did a get request to Function 2");

                return new OkObjectResult($"GET request to 2: {(int) response.StatusCode}");
            }, req);

        private class Configuration
        {
            public string Function2Url { get; init; }
        }
    }
}
