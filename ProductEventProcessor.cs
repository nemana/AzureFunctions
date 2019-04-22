using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AFGetStarted
{
    public class ProductEventProcessor
    {

        private readonly IConfiguration _configuration;
        private readonly IProductService _productService;

        public ProductEventProcessor(IConfiguration configuration, IProductService productService)
        {
            _configuration = configuration;
            _productService = productService;
        }

        [FunctionName("ProductEventProcessor")]
        public void Run([QueueTrigger("%QueueName%", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var productDetail = JsonConvert.DeserializeObject<ProductDetails>(myQueueItem);
            _productService.AddProduct(productDetail.Name, productDetail.Description).GetAwaiter().GetResult();
        }
    }
}