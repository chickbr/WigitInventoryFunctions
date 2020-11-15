using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;

namespace WigitInventoryFunctions
{
    public static class GetInventory
    {
        [FunctionName("GetInventory")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Inventory/{id:guid}")]
            HttpRequest req, Guid id, ILogger log,
            [CosmosDB(
                databaseName: "wigits",
                collectionName: "productinventory",
                ConnectionStringSetting = "CONNECTION_STRING",
                Id = "{id}",
                PartitionKey = "{id}"
                )] dynamic document)
        {
            if (document is null)
            {
                return new NotFoundResult();
            }
            else
            {
                dynamic returndocument = new { document.id, document.stock };
                return new OkObjectResult(returndocument);
            }
            
        }
    }
}