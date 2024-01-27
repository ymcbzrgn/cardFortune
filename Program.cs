using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using DotNetEnv;

class Program
{
    static async Task Main(string[] args)
    {
        DotNetEnv.Env.Load();
        
        string connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") ?? string.Empty;
        string databaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") ?? string.Empty;
        string collectionName = Environment.GetEnvironmentVariable("MONGODB_COLLECTION_NAME") ?? string.Empty;

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        var collection = database.GetCollection<BsonDocument>(collectionName);

        var cardFortune = new CardFortune(collection);
        await cardFortune.StartFortune();

    }
}
