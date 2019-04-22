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

namespace AFGetStarted
{
    public class ProductApi
    {
        private readonly IQueueService _queueService;
        private readonly IConfiguration _configuration;

        public ProductApi(IQueueService queueService, IConfiguration configuration)
        {
            _queueService = queueService;
            _configuration = configuration;
        }

        [FunctionName("ProductApi")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string name = req.Query["name"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (req.Method.ToLowerInvariant() == "post")
            {
                var productDetails = JsonConvert.DeserializeObject<ProductDetails>(requestBody);
                _queueService.SendMessageAsync(JsonConvert.SerializeObject(productDetails)).GetAwaiter().GetResult();
                return (ActionResult)(ActionResult)new OkResult();
            }
            else
            {
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                name = name ?? data?.name;

                return name != null ?
                    (ActionResult)new OkObjectResult($"Hello, {name}") :
                    new BadRequestObjectResult("Please pass a name on the query string or in the request body");
            }

        }
    }
}