using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TFGAPI.Models;
using TFGAPI.Settings;

namespace TFGAPI.Services;

public class PlayerService
{
    private readonly IMongoCollection<Player> _players;

    public PlayerService(IOptions<DatabaseSettings> dbSettings)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var database = client.GetDatabase(dbSettings.Value.DatabaseName);
        _players = database.GetCollection<Player>(dbSettings.Value.CollectionName);
        
        var indexOptions = new CreateIndexOptions { Unique = true };
        var indexKeys = Builders<Player>.IndexKeys.Ascending(p => p.Name);
        var indexModel = new CreateIndexModel<Player>(indexKeys, indexOptions);
        _players.Indexes.CreateOne(indexModel);
    }

    public async Task<List<Player>> GetAllAsync() =>
        await _players.Find(_ => true).ToListAsync();

    public async Task<Player?> GetByNameAsync(string name) =>
        await _players.Find(p => p.Name == name).FirstOrDefaultAsync();

    public async Task<bool> CreateAsync(Player player)
    {
        var existing = await GetByNameAsync(player.Name);
        if (existing is not null)
            return false;

        try
        {
            await _players.InsertOneAsync(player);
            return true;
        }
        catch (MongoWriteException ex) when (ex.WriteError.Code == 11000)
        {
            return false;
        }

    }

    public async Task<bool> UpdateAsync(string name, Player player)
    {
        var existing = await GetByNameAsync(name);
        if (existing is null)
            return false;

        player.Name = name;
        var result = await _players.ReplaceOneAsync(p => p.Name == name, player);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string name)
    {
        var result = await _players.DeleteOneAsync(p => p.Name == name);
        return result.DeletedCount > 0;
    }
}
