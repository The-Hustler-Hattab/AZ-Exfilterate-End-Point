using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace First_Demo_finc
{
    public class ExfiltrateFiles
    {
        private readonly ILogger _logger;

        public ExfiltrateFiles(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ExfiltrateFiles>();
        }

        [Function("ExfiltrateFiles")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            var responseObject = new { message = "Hello World" };
            var jsonResponse = JsonSerializer.Serialize(responseObject);
            response.WriteString(jsonResponse);

            return response;
        }
    }
}
