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
    }

    public async Task<List<Player>> GetAllAsync() => await _players.Find(_ => true).ToListAsync();
    public async Task<Player?> GetByIdAsync(string id) => await _players.Find(p => p.Id == id).FirstOrDefaultAsync();
    public async Task CreateAsync(Player player) => await _players.InsertOneAsync(player);
    public async Task UpdateAsync(string id, Player player) => await _players.ReplaceOneAsync(p => p.Id == id, player);
    public async Task DeleteAsync(string id) => await _players.DeleteOneAsync(p => p.Id == id);
}