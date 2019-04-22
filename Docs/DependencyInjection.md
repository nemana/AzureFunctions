# Setting up dependency Injection in Azure Function

As we are using Dotnet core framework for Azure function V2 version, we will get all fetaure of inbuild Dependency Injection feature of AspNet core.

Lets start converting ProductApi and pass IQueueService as constructor parameter

```sh
    private readonly IQueueService _queueService;
    private readonly IConfiguration _configuration;

    public ProductApi(IQueueService queueService, IConfiguration configuration)
    {
        _queueService = queueService;
        _configuration = configuration;
    }
```

* Make Product Api service from Static class to Non static class
* Define IQueueService and IConfiguration as non static class variables

```sh
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
```

* Convert Static to Non Static keyword.
* Update the code to use the injected service in code.

Now we have updated the code for productApi and its time to inject services to container. 

Following steps need to be perform for same

- Create class AFStartup.cs 
- Inherit the class with IWebJobsStartup interface and implement configure method.
- Runtime search with all the class which implemented the IWebJobsStartup and register them with runtime.
- Decorate namespace like where AFStartup [assembly: WebJobsStartup(typeof(AFGetStarted.AFStartup))]

# [AFStartup.cs]

```sh
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
```

Now we can do constructor injection. Implemented the same thing for EventProcessor class

As next step we will be Unit Test case for Azure function using MSTest and NSubsititude.
    
[link to go to home](/README.md)



