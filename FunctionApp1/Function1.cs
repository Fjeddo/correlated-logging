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
        private readonly Configuration _configuration;

        public Function1(IHttpClientFactory httpClientFactory, ICorrelationIdDecoratedLogger<Function1> log, IConfiguration configuration) : base(log)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration.Get<Configuration>();
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(Microsoft.Azure.WebJobs.Extensions.Http.AuthorizationLevel.Function, "get", Route = null)] HttpRequest req) =>
            await Execute(async () =>
            {
                Log.LogInformation("Will do a get request to Function 2");
                Log.LogInformation(new AbandonedMutexException("Info 123"), "Information with exception");

                Log.LogWarning("This is a warning test");
                Log.LogWarning(new AbandonedMutexException("Warning 123"), "Warning with exception");

                Log.LogError(new AbandonedMutexException("Error 123"), "This is an exception error test");
                Log.LogError("This is an error test");

                var httpClient = _httpClientFactory.CreateClient();

                var requestTo2 = new HttpRequestMessage(HttpMethod.Get, _configuration.Function2Url)
                {
                    Headers = {{ContextExtensions.Header, req.GetCorrelationId()}}
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
