using System.Net.Http;
using System.Threading.Tasks;
using CorrelatedLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionApp2
{
    public class Function2 : HttpTriggeredFunctions<Function2>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Configuration _configuration;

        public Function2(IHttpClientFactory httpClientFactory, ICorrelationIdDecoratedLogger<Function2> log, IConfiguration configuration) : base(log)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration.Get<Configuration>();
        }

        [FunctionName("Function2")]
        public async Task<IActionResult> Run([HttpTrigger(Microsoft.Azure.WebJobs.Extensions.Http.AuthorizationLevel.Function, "get", Route = null)] HttpRequest req) =>
            await Execute(async () =>
            {
                Log.LogInformation("Will do a get request to Function 3");

                var httpClient = _httpClientFactory.CreateClient();
                var requestTo3 = new HttpRequestMessage(HttpMethod.Get, _configuration.Function3Url)
                    {Headers = {{HttpRequestExtensions.Header, req.GetCorrelationId() } }};
                var response = await httpClient.SendAsync(requestTo3);

                Log.LogInformation("Did a get request to Function 3");

                return new OkObjectResult($"GET request to 3: {(int) response.StatusCode}");
            }, req);

        private class Configuration
        {
            public string Function3Url { get; init; }
        }
    }
}
