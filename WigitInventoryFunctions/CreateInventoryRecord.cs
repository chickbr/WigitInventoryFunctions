using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WigitInventoryFunctions
{
    public static class CreateInventoryRecord
    {
        [FunctionName("CreateInventoryRecord")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Inventory/{ProductId:guid}")]
            HttpRequest req, Guid ProductId, ILogger log,
            [CosmosDB(
                databaseName: "wigits",
                collectionName: "productinventory",
                ConnectionStringSetting = "CONNECTION_STRING" )]out dynamic document)
        {
            string responseMessage = $"You created an inventory record for product {ProductId}. This HTTP triggered function executed successfully.";
            Guid newid = ProductId;
            int newstock = 0;
            document = new { newid, newstock };
            return new OkObjectResult(responseMessage);
        }
    }
}