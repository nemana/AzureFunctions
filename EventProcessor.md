# Creating Event Triggered Azure function

in [previous post](readme.md) we have have created Product services. In this article we will be creating Product Event Processor.

Product event processor will read message from queue and store the data into database.

# Setting up database

- Create database titled productdb and create productdetails table. Connect to SQL server using Azure Data Tool or SSMS and connect to Master database
```sh
CREATE DATABASE ProductDb
Use ProductDb
--once switched to ProductDb the create Product detail Table
CREATE TABLE ProductDetail
(
    [Name] NVARCHAR(100),
    [Description] NVARCHAR(100)
)
```

# Product Event Processor Function

- Create new function of type QueueTrigger
```sh
func new
-- select QueueTrigger
-- provide name as ProductEventProcessor.
```

Once done Implement the following two steps

- Read the message from Queue
```sh
 var productDetail = JsonConvert.DeserializeObject<ProductDetails> (myQueueItem);
```

- Save the details to database
```sh
if (_configuration == null) {
      _configuration = new ConfigurationBuilder ()
          .AddEnvironmentVariables ()
          .AddJsonFile ("local.settings.json", optional : true)
          .Build ();
}

using (var productService = new ProductService (_configuration.GetConnectionString ("ProductDb"))) {
    productService.AddProduct (productDetail.Name, productDetail.Description).GetAwaiter ().GetResult ();
}

```

# local.setting.json 

This will be having sqlDB connection string. This can be Azure SQL DB or your local DB for local development scenarion

```sh
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "storage connection string",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "QueueName": "myqueue"
    },
    "ConnectionStrings": {
        "ProductDb": "database connection string"
    }
}
```

Next tutorial we will learn about Dependency Injection in Azure Functions.
