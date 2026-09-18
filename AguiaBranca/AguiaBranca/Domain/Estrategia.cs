using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AguiaBranca.Domain
{
    public class Estrategia
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Campanha { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public DateTime Data { get; set; } = DateTime.UtcNow;

        public DateTime DataInicio { get; set; } = DateTime.UtcNow;

        public DateTime DataFim { get; set; } = DateTime.UtcNow.AddMonths(3);

        public bool Ativa { get; set; } = true;

        public string CriadoPorUsuarioId { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }
    }
}
