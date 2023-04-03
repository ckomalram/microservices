

using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Servicios.api.libreria.Core.Entities;
using Servicios.api.libreria.Core;

namespace Servicios.api.libreria.Repository;


public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
{
    private readonly IMongoCollection<TDocument> collectionname;
    private IMongoDatabase database;

    public MongoRepository(IOptions<MongoSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        database = client.GetDatabase(options.Value.Database);
        collectionname = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
    }

    // Metodo privado que retorna el nombre de la coleccion pasandole el document type.    
    private protected string GetCollectionName(Type documentType)
    {
        return ((BsonCollectionAtribute)documentType.GetCustomAttributes(typeof(BsonCollectionAtribute), true).FirstOrDefault()).CollectionName;
    }
    public IQueryable<TDocument> Getall()
    {

        return collectionname.AsQueryable();
    }

}

public interface IMongoRepository<TDocument> where TDocument : IDocument
{

    IQueryable<TDocument> Getall();
}