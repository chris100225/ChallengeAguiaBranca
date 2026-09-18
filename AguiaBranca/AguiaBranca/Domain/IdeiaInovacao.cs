using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AguiaBranca.Domain
{
    public static class StatusIdeia
    {
        public const string Pendente = "Pendente";
        public const string EmAnalise = "EmAnalise";
        public const string Aprovada = "Aprovada";
        public const string Rejeitada = "Rejeitada";
    }

    public static class PrioridadeIdeia
    {
        public const string Baixa = "Baixa";
        public const string Media = "Media";
        public const string Alta = "Alta";
        public const string Critica = "Critica";
    }

    public class IdeiaInovacao
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string ProblemaEnfrentado { get; set; } = string.Empty;

        public string ImpactoEsperado { get; set; } = string.Empty;

        public string Status { get; set; } = StatusIdeia.Pendente;

        public string Prioridade { get; set; } = PrioridadeIdeia.Media;

        [BsonRepresentation(BsonType.ObjectId)]
        public string EstrategiaId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string OperadorId { get; set; } = string.Empty;

        public string NomeOperador { get; set; } = string.Empty;

        public string? FeedbackGestor { get; set; }

        public string? AvaliadoPorGestorId { get; set; }

        public DateTime? DataAvaliacao { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }
    }
}
