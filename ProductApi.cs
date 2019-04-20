using System;
using System.IO;
using System.Threading.Tasks;
using AFGetStarted.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AFGetStarted {
    public static class ProductApi {
        private static IQueueService _queueService;
        private static IConfiguration _configuration;
        [FunctionName ("ProductApi")]
        public static async Task<IActionResult> Run (
            [HttpTrigger (AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log) {
            log.LogInformation ("C# HTTP trigger function processed a request.");

            if (_configuration == null) {
                _configuration = new ConfigurationBuilder ()
                    .AddEnvironmentVariables ()
                    .AddJsonFile ("local.settings.json", optional : true)
                    .Build ();
            }
            string name = req.Query["name"];
            string requestBody = await new StreamReader (req.Body).ReadToEndAsync ();
            if (req.Method.ToLowerInvariant () == "post") {
                if (_queueService == null) {
                    _queueService = new QueueService (_configuration.GetSection ("AzureWebJobsStorage").Value, _configuration.GetSection ("QueueName").Value);
                }
                var productDetails = JsonConvert.DeserializeObject<ProductDetails> (requestBody);
                _queueService.SendMessageAsync (JsonConvert.SerializeObject (productDetails)).GetAwaiter ().GetResult ();
                return (ActionResult) (ActionResult) new OkResult ();
            } else {
                dynamic data = JsonConvert.DeserializeObject (requestBody);
                name = name ?? data?.name;

                return name != null ?
                    (ActionResult) new OkObjectResult ($"Hello, {name}") :
                    new BadRequestObjectResult ("Please pass a name on the query string or in the request body");
            }

        }
    }
}