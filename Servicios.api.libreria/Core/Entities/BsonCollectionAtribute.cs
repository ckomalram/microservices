using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Servicios.api.libreria.Core.Entities;

//Al trabajar con documentos genericos, se crea este helper
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAtribute : Attribute
{
    public string CollectionName { get; }

    public BsonCollectionAtribute(string collectionName)
    {
        CollectionName = collectionName;
    }


}