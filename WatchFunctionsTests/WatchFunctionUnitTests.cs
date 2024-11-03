using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using WatchPortalFunction;
using Azure;
using Microsoft.Extensions.Logging;
namespace WatchFunctionsTests

{
    public class WatchFunctionUnitTests
    {
        [Fact]
        public void TestWatchFunctionSuccess()
        {
            string queryString = "G-Shock";
            var request = new DefaultHttpContext().Request; // The request object is created from the context object.

            // This statement creates a mock HTTP context and an HTTP request. The request includes a query string that includes the model parameter
            request.Query = new QueryCollection
            (
                new System.Collections.Generic.Dictionary<string, StringValues>()
                {
                            { "model", queryString}
                }
            );

            var logger = NullLogger<WatchPortalFunction.WatchInfo>.Instance; // The NullLoggerFactory class is used to create an instance of the ILogger class. This statement creates a dummy logger.
            var watchInfo = new WatchPortalFunction.WatchInfo(logger); // Create an instance of WatchInfo class with the logger.

            var response = watchInfo.Run(request); // The Run method of the WatchInfo class is called with the request and logger objects.

            Assert.IsType<OkObjectResult>(response); // Check if the response is an OkObjectResult
            var okResult = response as OkObjectResult;

            Assert.NotNull(okResult); // Check if the response is not null
            Assert.Equal(200, okResult.StatusCode); // Check if the status code is 200

            // Verify the response content
            string expectedContent = "Watch Details: WatchPortal, G-Shock, Analog, 50m, Stainless Steel, Quartz & 100";
            Assert.Equal(expectedContent, okResult.Value);
        }

        [Fact]
        public void TestWatchFunctionFailureNoQueryString()
        {
            string queryString = "G-Shock";
            var request = new DefaultHttpContext().Request;
            var logger = NullLogger<WatchInfo>.Instance;

            // This statement creates a mock HTTP context and an HTTP request. The request includes a query string that includes the model parameter
            request.Query = new QueryCollection
            (
                new System.Collections.Generic.Dictionary<string, StringValues>()
                {
                            { "not-model", queryString}
                }
            );

            var watchInfo = new WatchPortalFunction.WatchInfo(logger); // Create an instance of WatchInfo class with the logger.
            var response = watchInfo.Run(request); // The Run method of the WatchInfo class is called with the request and logger objects.

            // Check that the response is an "Bad" response
            Assert.IsAssignableFrom<BadRequestObjectResult>(response);
            var badRequestResult = response as BadRequestObjectResult;

            // Check that the contents of the response are the expected contents
            var result = (BadRequestObjectResult)response;
            Assert.Equal("Please pass a model on the query string or in the request body", result.Value);
        }

        [Fact]
        public void TestWatchFunctionFailureNoModel()
        {
            var queryStringValue = "abc";
            var request = new DefaultHttpContext().Request;
            request.Query = new QueryCollection
                (
                    new System.Collections.Generic.Dictionary<string, StringValues>()
                    {
                        { "not-model", queryStringValue }
                    }
                );

            var logger = NullLogger<WatchInfo>.Instance;

            var watchInfo = new WatchPortalFunction.WatchInfo(logger);
            var response = watchInfo.Run(request);

            // Check that the response is an "Bad" response
            Assert.IsAssignableFrom<BadRequestObjectResult>(response);
            //var result = (BadRequestObjectResult) response;

            // Check that the contents of the response are the expected contents
            var result = (BadRequestObjectResult)response;
            Assert.Equal("Please pass a model on the query string or in the request body", result.Value);
        }
    }
}