using MongoDB.Bson.Serialization.Attributes;

namespace WebApiMongo.Models
{
    public class Produtos
    {
        public int? id { get; set; }
        public string nome { get; set; } 
        public string marca { get; set; } 
        public string tipo { get; set; } 
        public string dataAtualizacao { get; set; } 
    }
}
