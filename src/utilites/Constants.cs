using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exfilterate.utilites
{
    internal static class Constants
    {
        /* COSMOS DB CREDS*/
        public static string COSMOS_DB_CONNECTION_STRING = Environment.GetEnvironmentVariable("COSMOS_DB_CONNECTION_STRING");
        public static string COSMOS_DB_NAME = Environment.GetEnvironmentVariable("COSMOS_DB_NAME");
        public static string COSMOS_DB_CONTAINER_NAME = Environment.GetEnvironmentVariable("COSMOS_DB_CONTAINER_NAME");

        /* BLOB CREDS*/
        public static string AZURE_BLOB_CONNECTION_STRING = Environment.GetEnvironmentVariable("AZURE_BLOB_CONNECTION_STRING");
        public static string AZURE_BLOB_NAME = Environment.GetEnvironmentVariable("AZURE_BLOB_NAME");
        
        /*JWK ENDPOINT*/
        public static string JWK_ENDPOINT = Environment.GetEnvironmentVariable("JWK_ENDPOINT");

        
    }
}
