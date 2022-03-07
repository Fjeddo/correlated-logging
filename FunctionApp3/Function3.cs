using System.Threading.Tasks;
using CorrelatedLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace FunctionApp3
{
    public class Function3
    {
        private readonly IExtendedLogger<Function3> _log;

        public Function3(IExtendedLogger<Function3> log)
        {
            _log = log;
        }

        [FunctionName("Function3")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            _log.LogInformation("Function3 is about to return a string");

            return new OkObjectResult("Hello from Function3");
        }
    }
}
