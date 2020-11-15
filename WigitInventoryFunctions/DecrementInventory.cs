using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;

namespace WigitInventoryFunctions
{
    public static class DecrementInventory
    {
        [FunctionName("DecrementInventory")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Inventory/{id:guid}/decrement/{amount:int}")]
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
            if(olddocument is null)
            {
                newdocument = null;
                return new NotFoundResult();
            }
            if (olddocument.stock - amount < 0)
            {
                string responseMessage = $"Decrement {id} by {amount} units failed. Stock cannot go below 0. This HTTP triggered function executed successfully.";
                newdocument = olddocument;
                return new BadRequestObjectResult(responseMessage);
            }
            else
            {
                newdocument = new { id = id, stock = olddocument.stock - amount };
                string responseMessage = $"Decrement {id} by {amount} units. This HTTP triggered function executed successfully.";
                return new OkObjectResult(responseMessage);
            }
        }
    }
}