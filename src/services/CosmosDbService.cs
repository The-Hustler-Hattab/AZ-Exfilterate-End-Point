using Exfilterate.utilites;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;


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
        public async static Task<string> GetAllRecordsAsync()
        {
            var records = new List<dynamic>();


            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = CosmosContainer.GetItemQueryIterator<dynamic>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                foreach (var item in response)
                {
                    records.Add(item);
                }
            }
            // Serialize the list to JSON string
            string jsonString = JsonConvert.SerializeObject(records);
            return jsonString;
        }
    }


}

