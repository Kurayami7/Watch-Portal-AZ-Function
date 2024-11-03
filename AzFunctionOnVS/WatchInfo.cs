using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace WatchPortalFunction
{
    public class WatchInfo
    {
        private readonly ILogger<WatchInfo> _logger;

        public WatchInfo(ILogger<WatchInfo> logger) // The constructor of the WatchInfo class receives an ILogger<WatchInfo> instance.
        {
            _logger = logger;
        }

        [Function("WatchInfo")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string? model = req?.Query["model"];

            if (model != null)
            {
                dynamic watchDetails = new
                {
                    Manufacturer = "WatchPortal",
                    Model = model,
                    Style = "Analog",
                    WaterResistance = "50m",
                    CaseMaterial = "Stainless Steel",
                    Movement = "Quartz",
                    Price = 100
                };

                return (ActionResult)new OkObjectResult($"Watch Details: {watchDetails.Manufacturer}, {watchDetails.Model}, {watchDetails.Style}, {watchDetails.WaterResistance}, {watchDetails.CaseMaterial}, {watchDetails.Movement} & {watchDetails.Price}");
            }

            return (ActionResult)new BadRequestObjectResult("Please pass a model on the query string or in the request body");
        }
    }
}
