using MongoDB.Driver;

namespace LouisasAstroWebsite.Data;

public class MongoDbContext
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoClient client)
    {
        _client = client;
        _database = _client.GetDatabase("GirlfriendDB");
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("users");
}