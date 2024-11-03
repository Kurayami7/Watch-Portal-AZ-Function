using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzFunctionOnVS
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger; // ILogger<T> is a generic interface that allows logging messages from a class.

        public Function1(ILogger<Function1> logger) // The constructor of the Function1 class receives an ILogger<Function1> instance.
        {
            _logger = logger; // The instance is assigned to the _logger field.
        }

        [Function("Function1")] // The Function attribute is used to define the function name.
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req) // The Run method is the entry point of the function.
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
