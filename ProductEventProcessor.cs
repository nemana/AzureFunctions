using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AFGetStarted {
    public class ProductEventProcessor {

        private static IConfiguration _configuration;

        [FunctionName ("ProductEventProcessor")]
        public static void Run ([QueueTrigger ("%QueueName%", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log) {
            log.LogInformation ($"C# Queue trigger function processed: {myQueueItem}");

            if (_configuration == null) {
                _configuration = new ConfigurationBuilder ()
                    .AddEnvironmentVariables ()
                    .AddJsonFile ("local.settings.json", optional : true)
                    .Build ();
            }

            var productDetail = JsonConvert.DeserializeObject<ProductDetails> (myQueueItem);
            using (var productService = new ProductService (_configuration.GetConnectionString ("ProductDb"))) {
                productService.AddProduct (productDetail.Name, productDetail.Description).GetAwaiter ().GetResult ();
            }
        }
    }
}