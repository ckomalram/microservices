using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Servicios.api.libreria.Core.Entities;

public class Document : IDocument
{
    public ObjectId Id { get; set; }
    public DateTime CreatedDated => Id.CreationTime;
}

public interface IDocument
{

    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    ObjectId Id { get; set; }

    DateTime CreatedDated { get; }
}