

using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Servicios.api.libreria.Core.Entities;
using Servicios.api.libreria.Core;
using System.Linq.Expressions;

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

    public async Task<PaginationEntity<TDocument>> PaginationBy(Expression<Func<TDocument, bool>> filterExpression, PaginationEntity<TDocument> pagination)
    {

        var sort = Builders<TDocument>.Sort.Ascending(pagination.Sort);
        var emptyFilter = Builders<TDocument>.Filter.Empty;
        var skip = ((pagination.Page - 1) * pagination.PageSize);

        // Obtener count de toda la colección
        long totalDocument = await collectionname.CountDocumentsAsync(emptyFilter);
        var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalDocument / pagination.PageSize)));

        pagination.PagesQuantity = totalPages;

        if (pagination.SortDirection == "desc")
        {
            sort = Builders<TDocument>.Sort.Descending(pagination.Sort);
        }

        if (string.IsNullOrEmpty(pagination.Filter))
        {

            var rta = await collectionname
                            .Find(emptyFilter)
                            .Sort(sort)
                            .Skip(skip)
                            .Limit(pagination.PageSize)
                            .ToListAsync();
            pagination.Data = rta;
        }
        else
        {
            var rta = await collectionname
                .Find(filterExpression)
                .Sort(sort)
                .Skip(skip)
                .Limit(pagination.PageSize)
                .ToListAsync();
            pagination.Data = rta;
        }

        return pagination;

    }

    public async Task<PaginationEntity<TDocument>> PaginationByFilter(PaginationEntity<TDocument> pagination)
    {
        var sort = Builders<TDocument>.Sort.Ascending(pagination.Sort);
        var emptyFilter = Builders<TDocument>.Filter.Empty;
        var skip = ((pagination.Page - 1) * pagination.PageSize);

        // Obtener count de toda la colección
        var totalDocument = 0;


        if (pagination.SortDirection == "desc")
        {
            sort = Builders<TDocument>.Sort.Descending(pagination.Sort);
        }

        if (pagination.FilterValue == null)
        {

            var rta = await collectionname
                            .Find(emptyFilter)
                            .Sort(sort)
                            .Skip(skip)
                            .Limit(pagination.PageSize)
                            .ToListAsync();
            pagination.Data = rta;

            totalDocument = (await collectionname
                            .Find(emptyFilter)
                            .ToListAsync()).Count();
        }
        else
        {
            // Expresion regular con valores que coincidadn
            var customFilterValue = ".*" + pagination.FilterValue.Valor + ".*";
            var customFilter = Builders<TDocument>.Filter.Regex(pagination.FilterValue.Propiedad, new MongoDB.Bson.BsonRegularExpression(customFilterValue, "i"));

            var rta = await collectionname
                .Find(customFilter)
                .Sort(sort)
                .Skip(skip)
                .Limit(pagination.PageSize)
                .ToListAsync();
            pagination.Data = rta;

            totalDocument = (await collectionname
                .Find(customFilter)
                .ToListAsync()).Count();
        }

        // long totalDocument = await collectionname.CountDocumentsAsync(emptyFilter);
        var rounded = Math.Ceiling(totalDocument / Convert.ToDecimal(pagination.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        pagination.PagesQuantity = totalPages;
        pagination.TotalRows = totalDocument;

        return pagination;
    }
}

public interface IMongoRepository<TDocument> where TDocument : IDocument
{

    Task<IEnumerable<TDocument>> Getall();
    Task<TDocument> GetById(string Id);

    Task InsertDocument(TDocument document);

    Task UpdateDocument(TDocument document);
    Task DeleteDocument(string Id);

    Task<PaginationEntity<TDocument>> PaginationBy(
        Expression<Func<TDocument, bool>> filterExpression,
        PaginationEntity<TDocument> pagination
    );

    Task<PaginationEntity<TDocument>> PaginationByFilter(
    PaginationEntity<TDocument> pagination
    );

}
