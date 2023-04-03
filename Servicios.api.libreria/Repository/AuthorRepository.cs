using MongoDB.Driver;
using Servicios.api.libreria.Core.ContextMongoDb;
using Servicios.api.libreria.Core.Entities;

namespace Servicios.api.libreria.Repository;

public class AuthorRepository : IAuthorRepository
{
    private readonly IAuthorContext authorContext;
    public AuthorRepository(IAuthorContext context)
    {
        authorContext = context;
    }
    public async Task<IEnumerable<Author>> GetAutores()
    {
        var filter = Builders<Author>.Filter.Empty;
        var cursor = await authorContext.Autores.FindAsync(filter);
        var authors = await cursor.ToListAsync();
        return authors;
    }
}

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAutores();
}