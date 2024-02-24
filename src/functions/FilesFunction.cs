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
using First_Demo_finc.services;
using Newtonsoft.Json;
using System.Collections.Generic;



namespace Exfilterate.functions
{
    public class FilesFunction
    {

        [Function("ExfiltrateFiles")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext context)
        {
            FileExfiltrateService fileExfiltrateService = new FileExfiltrateService();

            return await fileExfiltrateService.exfiltrateFile(req);
        }

        
        
        
        
        [Function("getAllCosmosRecords")]
        public async Task<HttpResponseData> GetAllCosmosRecords(
      [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
      FunctionContext context)
        {
            if (! await ValidiateJWT.ValidateToken(req))
            {
                return req.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            }

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            CosmosDbService.GetConnection();
            string cosmosList= await CosmosDbService.GetAllRecordsAsync();
            response.WriteString(cosmosList);


            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        [Function("getFileFromBlob")]
        public async Task<HttpResponseData> getFileFromBlob(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            FunctionContext context)
        {
            if (! await ValidiateJWT.ValidateToken(req))
            {
                return req.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            }
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            BlobService.GetConnection();
            string blobFileName = req.Query["file"];
            try
            {
                Stream fileStream= await BlobService.DownloadFileFromBlobAsync(blobFileName);
                response.Body = fileStream;

                response.Headers.Add("Content-Type", "application/octet-stream");

                response.Headers.Add("Content-Disposition", "attachment; filename=\"" + blobFileName + "\"");
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.WriteString(System.Text.Json.JsonSerializer.Serialize(new { message = $"error occured: {ex.Message}" }));
                return response;
            }

        }






    }
}
