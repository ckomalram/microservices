

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
    public async Task<IEnumerable<TDocument>> Getall()
    {
        var filter = Builders<TDocument>.Filter.Empty;
        var cursor = await collectionname.FindAsync(filter);
        var response = await cursor.ToListAsync();
        return response;
    }

    public async Task<TDocument> GetById(string Id)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, Id);
        var response = await collectionname.Find(filter).SingleOrDefaultAsync();
        return response;
    }

    public async Task InsertDocument(TDocument document)
    {
        await collectionname.InsertOneAsync(document);
    }

    public async Task UpdateDocument(TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        await collectionname.FindOneAndReplaceAsync(filter, document);
    }

    public async Task DeleteDocument(string Id)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, Id);
        await collectionname.FindOneAndDeleteAsync(filter);


    }
}

public interface IMongoRepository<TDocument> where TDocument : IDocument
{

    Task<IEnumerable<TDocument>> Getall();
    Task<TDocument> GetById(string Id);

    Task InsertDocument(TDocument document);

    Task UpdateDocument(TDocument document);
    Task DeleteDocument(string Id);

}
