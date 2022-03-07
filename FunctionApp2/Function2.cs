using System.Net.Http;
using System.Threading.Tasks;
using CorrelatedLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FunctionApp2
{
    public class Function2
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IExtendedLogger<Function2> _log;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly Configuration _configuration;

        public Function2(IHttpClientFactory httpClientFactory, IExtendedLogger<Function2> log, ICorrelationIdProvider correlationIdProvider, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _log = log;
            _correlationIdProvider = correlationIdProvider;
            _configuration = configuration.Get<Configuration>();
        }

        [FunctionName("Function2")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            _log.LogInformation("Will do a get request to Function 3");

            var httpClient = _httpClientFactory.CreateClient();
            var requestTo3 = new HttpRequestMessage(HttpMethod.Get, _configuration.Function3Url) { Headers = { { ICorrelationIdProvider.Header, _correlationIdProvider.CorrelationId } } };
            var response = await httpClient.SendAsync(requestTo3);

            _log.LogInformation("Did a get request to Function 3");

            return new OkObjectResult($"GET request to 3: {(int)response.StatusCode}");
        }

        private class Configuration
        {
            public string Function3Url { get; init; }
        }
    }
}
