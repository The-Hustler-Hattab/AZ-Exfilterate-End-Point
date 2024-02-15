using Exfilterate.functions;
using Exfilterate.models;
using Exfilterate.services;
using Exfilterate.utilites;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace First_Demo_finc.services
{
    internal class FileExfiltrateService
    {
        private static readonly ILogger _logger = new LoggerFactory().CreateLogger<FileExfiltrateService>();

        public async Task<HttpResponseData> exfiltrateFile(HttpRequestData req)
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
