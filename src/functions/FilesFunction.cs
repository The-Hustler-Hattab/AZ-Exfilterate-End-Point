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

        
        
        
        
        [Function("hello")]
        public async Task<HttpResponseData> Hello(
      [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
      FunctionContext context)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            response.WriteString("gello");
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }



    


    }
}
