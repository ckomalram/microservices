using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Servicios.api.libreria.Core.Entities;

[BsonCollectionAtribute("Book")]
public class BookEntity : Document
{

    [BsonElement("titulo")]
    public string Titulo { get; set; }

    [BsonElement("descripcion")]
    public string Descripcion { get; set; }


    [BsonElement("precio")]
    public int Precio { get; set; }

    [BsonElement("fechapublicacion")]
    public DateTime? FechaPublicacion { get; set; }

    [BsonElement("author")]
    public AuthorEntity Author { get; set; }

}