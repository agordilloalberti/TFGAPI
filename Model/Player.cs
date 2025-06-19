using MongoDB.Bson.Serialization.Attributes;

namespace TFGAPI.Models;

public class Player
{
    [BsonId]
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    public double Score { get; set; }
}