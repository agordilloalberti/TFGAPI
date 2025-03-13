﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TFGAPI.Models;

public class Player
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Score { get; set; }
}