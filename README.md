# Creating Event Triggered Azure function

We will be creating a simple sample which takes Name and description of product using rest endpoint and store in SQL Server Table.

-   Creating Http Trigger (Rest Api) azure function endpoint which will be allow user to post product details (name and description)
-   Http Trigger endpoint will save the information in Queue.
-   Creating QueueTrigger azure function which will listen to storage queue and insert this information in database

# Architectural diagram

![N|Solid](image/ArchitecturalDiagram.png)


# Tools & Tecnology 

* [Visual Studio Code](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-function-vs-code) Development of Azure function with cross platform editor.
* [Dotnet Core](https://dotnet.microsoft.com/download) Cross platform dotnet framework
* [Azure Stoage Account]
* [Sql Server]
* [Azure Storage Queue](https://docs.microsoft.com/en-us/azure/storage/queues/storage-queues-introduction) Storage Message Queue

# Start with Creation of ProductApi

-   Create Azure function using core tool 
    * Open terminal and create directory "AFGetStrated" (mkdir AFGetStarted && cd AFGetStarted)
    * run func init command and select dotnet as option

    ![N|Solid](image/func-init.png)

    * run func new command and select HttpTrigger option. Give name as ProductApi
    
      ![N|Solid](image/httptrigger.png)
    


