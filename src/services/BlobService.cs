using HttpMultipartParser;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

using Exfilterate.utilites;

namespace Exfilterate.services
{
    internal static class BlobService
    {
        private static readonly string azureStorageConnectionString = Constants.AZURE_BLOB_CONNECTION_STRING;
        private static readonly string azureBlobContainerName = Constants.AZURE_BLOB_NAME;

        private static CloudBlobContainer container;
        public static async Task<CloudBlockBlob> SaveBlobAsync(FilePart file)
        {
            GetConnection();
            // Add a timestamp to the beginning of the blob name
            string blobName = DataOperationsUtilites.PrependDateToString(file.FileName);

            // Upload the file to Azure Blob Storage
            CloudBlockBlob blobReference = container.GetBlockBlobReference(blobName);
            await blobReference.UploadFromStreamAsync(file.Data);

            return blobReference;

        }

        public static void GetConnection() 
        {
            if (BlobService.container == null)
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                BlobService.container = blobClient.GetContainerReference(azureBlobContainerName);
            }


        }

        public async static Task<Stream> DownloadFileFromBlobAsync(string blobFileName)
        {
            var blob = container.GetBlockBlobReference(blobFileName);

            if (!await blob.ExistsAsync())
            {
                throw new FileNotFoundException($"File {blobFileName} not found in the blob container.");
            }

            var memoryStream = new MemoryStream();
            await blob.DownloadToStreamAsync(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }


    }
}
