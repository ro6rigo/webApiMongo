using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiMongo.Models
{
    public class Produto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("Nome")]
        public string Nome { get; set; } = null;
        [BsonElement("Marca")]
        public string Marca { get; set; } = null;
        [BsonElement("Tipo")]
        public string Tipo { get; set; } = null;
        [BsonElement("DataAtualizacao")]
        public string DataAtualizacao { get; set; } = null;
    }
}
