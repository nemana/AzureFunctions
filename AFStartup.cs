using AFGetStarted.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(AFGetStarted.AFStartup))]
namespace AFGetStarted
{
    public class AFStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var config = builder.Services.BuildServiceProvider().GetService<IConfiguration>();
            builder.Services.AddScoped<IProductService>(c => new ProductService(config.GetConnectionString("ProductDb")));
            builder.Services.AddSingleton<IQueueService>(c => new QueueService(config.GetSection("AzureWebJobsStorage").Value, config.GetSection("QueueName").Value));
        }
    }
}