using MongoDB.Bson.Serialization.Attributes;

namespace Servicios.api.libreria.Core.Entities;

class Author
{
    /**
        * Definiendo object id de mongo
    */
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("nombre")]
    public string Nombre { get; set; }

    [BsonElement("apellido")]
    public string Apellido { get; set; }


    [BsonElement("grado")]
    public string Grado { get; set; }






}