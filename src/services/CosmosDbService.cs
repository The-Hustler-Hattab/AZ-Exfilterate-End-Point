using Exfilterate.utilites;
using Microsoft.Azure.Cosmos;


namespace Exfilterate.services
{
    internal static class CosmosDbService
    {
        private static readonly string cosmosDbConnectionString = Constants.COSMOS_DB_CONNECTION_STRING;
        private static readonly string cosmosDatabaseName = Constants.COSMOS_DB_NAME;
        private static readonly string cosmosDbContainerName = Constants.COSMOS_DB_CONTAINER_NAME;
        static Container CosmosContainer;

        public static async Task SaveItemInCosmosAsync(Object document)
        {
            GetConnection();

            await CosmosDbService.CosmosContainer.CreateItemAsync(document);

        }

        public static void GetConnection()
        {
            if (CosmosDbService.CosmosContainer == null)
            {
                CosmosClient cosmosClient = new CosmosClient(cosmosDbConnectionString);
                Database cosmosDatabase = cosmosClient.GetDatabase(cosmosDatabaseName);
                CosmosDbService.CosmosContainer =  cosmosDatabase.GetContainer(cosmosDbContainerName);
            }


        }
    }
}
