using System.Collections.Generic;
using System.Threading.Tasks;
using CorrelatedLogger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp3
{
    public class DurableFunction3 : LogCorrelatedFunctions<DurableFunction3>
    {
        public DurableFunction3(ICorrelatedLoggingProvider<DurableFunction3> correlatedLoggingProvider) : base(correlatedLoggingProvider) { }

        [FunctionName("DurableFunction3")]
        public async Task<List<string>> RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context) =>
            await Execute(async () =>
            {
                Log.LogInformation("About to start some activities");

                var outputs = new List<string>
                {
                    // Replace "hello" with the name of your Durable Activity Function.
                    await context.CallActivityAsync<string>("DurableFunction3_Hello", Input.CreateInstance("Tokyo", context)),
                    await context.CallActivityAsync<string>("DurableFunction3_Hello", Input.CreateInstance("Seattle", context)),
                    await context.CallActivityAsync<string>("DurableFunction3_Hello", Input.CreateInstance("London", context))
                };

                // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
                return outputs;
            }, context);

        [FunctionName("DurableFunction3_Hello")]
        public string SayHello([ActivityTrigger] Input<string> input) =>
            Execute(() =>
            {
                Log.LogInformation($"Saying hello to {input.Data}.");
                return $"Hello {input.Data}!";
            }, input);
        
        [FunctionName("DurableFunction3_HttpStart")]
        public async Task<IActionResult> HttpStart([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, [DurableClient] IDurableOrchestrationClient starter) => 
            await Execute(async () =>
            {
                // Function input comes from the request content.
                var instanceId = await starter.StartNewAsync("DurableFunction3", Input.CreateInstance(req));

                Log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

                return starter.CreateCheckStatusResponse(req, instanceId);
            }, req);
    }

    
}