using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

using Microsoft.WindowsAzure.Storage.Blob;
using HttpMultipartParser;
using Exfilterate.services;
using Exfilterate.models;
using Exfilterate.utilites;



namespace Exfilterate.functions
{
    public class ExfiltrateFilesFunction
    {
        private readonly ILogger _logger;

        public ExfiltrateFilesFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ExfiltrateFilesFunction>();
        }

        [Function("ExfiltrateFiles")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext context)
        {

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            try
            {
                // get form-body        
                var parsedFormBody = MultipartFormDataParser.ParseAsync(req.Body);
                FilePart file = parsedFormBody.Result.Files[0];

                CloudBlockBlob blobReference = await BlobService.SaveBlobAsync(file);


                await saveItemInCosmosAsync(file, req, blobReference);
                RestOutputModel responseObject = new RestOutputModel
                {
                    Message = "File uploaded successfully.",
                    SavedPath = blobReference.Uri.ToString(),
                    StatusCode = (int)HttpStatusCode.OK
                };

                var jsonResponse = JsonSerializer.Serialize(responseObject);
                response.WriteString(jsonResponse);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.WriteString(JsonSerializer.Serialize(new { message = "An error occurred." }));
            }

            return response;
        }




        private async Task saveItemInCosmosAsync(FilePart file, HttpRequestData req,
            CloudBlockBlob blobReference)
        {

            var document = new
            {
                id = Guid.NewGuid().ToString(),
                FileName = blobReference.Name,
                FileContentType = file.ContentType,
                BlobUrl = blobReference.Uri.ToString(),
                IPAddress = GetIpFromRequestHeaders(req),
                Date = DataOperationsUtilites.GetCurrentDateAsEasternTime()
            };

            await CosmosDbService.SaveItemInCosmosAsync(document);
        }

        private string GetIpFromRequestHeaders(HttpRequestData request)
        {
            try
            {
                _logger.LogInformation("Getting IP address from request headers." + request.Headers.GetValues("X-Forwarded-For"));
                return request.Headers.GetValues("X-Forwarded-For").First();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the IP address.");
                return "";
            }
        }


    }
}
