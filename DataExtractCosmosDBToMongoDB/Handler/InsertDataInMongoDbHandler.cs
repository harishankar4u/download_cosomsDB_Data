using DataExtractCosmosDBToMongoDB.Handler;
using DataExtractCosmosDBToMongoDB.Properties;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Text;

public record InsertDataInMongoDbCommand(IFormFile file) : IRequest<bool>;

public class InsertDataInMongoDbHandler : IRequestHandler<InsertDataInMongoDbCommand, bool>
{
    private readonly IMongoCollection<BsonDocument> _mongoCollection;
    private readonly MongoClient _mongoClient;

    public InsertDataInMongoDbHandler()
    {
        var connectionString = Environment.GetEnvironmentVariable(Constant.MongodbConnectionString);
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentException("MongoDB connection string is not configured in environment variables.");

        _mongoClient = new MongoClient(connectionString);

        var databaseName = Environment.GetEnvironmentVariable(Constant.MongodbDatabaseName);
        var collectionName = Environment.GetEnvironmentVariable(Constant.MongodbCollectionName);

        if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(collectionName))
            throw new ArgumentException("MongoDB environment variables are not properly configured.");

        var database = _mongoClient.GetDatabase(databaseName);
        _mongoCollection = database.GetCollection<BsonDocument>(collectionName);
    }


    public async Task<bool> Handle(InsertDataInMongoDbCommand command, CancellationToken cancellationToken)
    {
        try
        {
            using var reader = new StreamReader(command.file.OpenReadStream(), Encoding.UTF8);
            var fileContent = await reader.ReadToEndAsync(cancellationToken);

            var products = JsonConvert.DeserializeObject<List<ProductResponse>>(fileContent);

            if (products == null || !products.Any())
                return false;

            foreach (var product in products)
            {
                // Set default SKU and productId if missing
                product.SKU ??= Guid.NewGuid().ToString();
                product.productId ??= Guid.NewGuid().ToString();

                // Convert to BsonDocument for MongoDB insertion
                var document = product.ToBsonDocument();

                // Use SKU as the _id
                document["_id"] = product.SKU;

                var filter = Builders<BsonDocument>.Filter.Eq("_id", product.SKU);
                await _mongoCollection.ReplaceOneAsync(filter, document, new ReplaceOptions { IsUpsert = true }, cancellationToken);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during MongoDB insert: {ex.Message}");
            return false;
        }
    }
}
