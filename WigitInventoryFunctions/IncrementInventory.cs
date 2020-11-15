using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;

namespace WigitInventoryFunctions
{
    public static class IncrementInventory
    {
        [FunctionName("IncrementInventory")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Inventory/{id:guid}/increment/{amount:int}")]
            HttpRequest req, Guid id, int amount, ILogger log,
            [CosmosDB(
                databaseName: "wigits",
                collectionName: "productinventory",
                ConnectionStringSetting = "CONNECTION_STRING",
                Id = "{id}",
                PartitionKey = "{id}"
                )]dynamic olddocument,
            [CosmosDB(
                databaseName: "wigits",
                collectionName: "productinventory",
                ConnectionStringSetting = "CONNECTION_STRING" )]out dynamic newdocument
            )
        {
            var currentAmount = olddocument.stock;
            newdocument = new { id = id, stock = currentAmount + amount };
            string responseMessage = $"Increment {id} by {amount} units. This HTTP triggered function executed successfully.";
            return new OkObjectResult(responseMessage);
        }
    }
}