using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Servicios.api.libreria.Core.Entities;

namespace Servicios.api.libreria.Core.ContextMongoDb;

public interface IAuthorContext
{
    IMongoCollection<Author> Autores { get; }
}

public class AuthorContext : IAuthorContext
{
    private IMongoDatabase database;
    private readonly IMongoCollection<Author> collectionname;
    public AuthorContext(IOptions<MongoSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        database = client.GetDatabase(options.Value.Database);
        collectionname = database.GetCollection<Author>(options.Value.AuthorCollectionName);
    }

    public IMongoCollection<Author> Autores => collectionname;
}
