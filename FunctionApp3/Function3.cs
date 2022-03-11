using System.Threading.Tasks;
using CorrelatedLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp3
{
    public class Function3 : HttpTriggeredFunctions<Function3>
    {

        public Function3(ICorrelationIdDecoratedLogger<Function3> log) : base(log) {}

        [FunctionName("Function3")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req) =>
            await Execute(async () =>
            {
                Log.LogInformation("Function3 is about to return a string");
                return new OkObjectResult("Hello from Function3");
            }, req);
    }
}
