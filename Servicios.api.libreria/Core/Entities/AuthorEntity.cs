using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Servicios.api.libreria.Core.Entities;

public class AuthorEntity : Document
{

    [BsonElement("nombre")]
    public string Nombre { get; set; }

    [BsonElement("apellido")]
    public string Apellido { get; set; }


    [BsonElement("grado")]
    public string Grado { get; set; }

}