using MediatR;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;

namespace DataExtractCosmosDBToMongoDB.Handler
{
    public record GetCosmosdbJsonFileDownloadQuery(string connectionString, string databaseName, string containerName) : IRequest<bool>;

    public class CosmosdbJsonFileDownloadHandler : IRequestHandler<GetCosmosdbJsonFileDownloadQuery, bool>
    {
        public CosmosdbJsonFileDownloadHandler() { }

        public async Task<bool> Handle(GetCosmosdbJsonFileDownloadQuery value, CancellationToken cancellationToken)
        {
            var detailsFolder = Path.Combine(Directory.GetCurrentDirectory(), "details");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(detailsFolder))
            {
                Directory.CreateDirectory(detailsFolder);
            }


            try
            {
                using var client = new CosmosClient(value.connectionString);
                var container = client.GetContainer(value.databaseName, value.containerName);

                var query = "SELECT * FROM c";
                var iterator = container.GetItemQueryIterator<dynamic>(query);

                var allDocuments = new List<dynamic>();

                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync(cancellationToken);

                    foreach (var item in response)
                    {
                        allDocuments.Add(item);
                    }
                }

                // Serialize all documents to a single JSON array
                var json = JsonConvert.SerializeObject(allDocuments, Formatting.Indented);

                var outputFile = Path.Combine(detailsFolder, "cosmos_export.json");
                await File.WriteAllTextAsync(outputFile, json, cancellationToken);

                Console.WriteLine($"Export completed! Total documents exported: {allDocuments.Count}. File saved at: {outputFile}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
