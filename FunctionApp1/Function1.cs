using System.Net.Http;
using System.Threading.Tasks;
using CorrelatedLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IExtendedLogger<Function1> _log;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly Configuration _configuration;

        public Function1(IHttpClientFactory httpClientFactory, IExtendedLogger<Function1> log, ICorrelationIdProvider correlationIdProvider, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _log = log;
            _correlationIdProvider = correlationIdProvider;
            _configuration = configuration.Get<Configuration>();
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            _log.LogInformation("Will do a get request to Function 2");

            var httpClient = _httpClientFactory.CreateClient();

            var requestTo2 = new HttpRequestMessage(HttpMethod.Get, _configuration.Function2Url)
                {Headers = {{ICorrelationIdProvider.Header, _correlationIdProvider.CorrelationId}}};

            var response = await httpClient.SendAsync(requestTo2);

            _log.LogInformation("Did a get request to Function 2");

            return new OkObjectResult($"GET request to 2: {(int) response.StatusCode}");
        }

        private class Configuration
        {
            public string Function2Url { get; init; }
        }
    }

}
