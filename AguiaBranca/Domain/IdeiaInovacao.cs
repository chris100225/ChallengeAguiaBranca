using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AguiaBranca.Domain
{
    /// <summary>
    /// Holds constant values for the possible status values of an innovation idea.
    /// </summary>
    public static class StatusIdeia
    {
        public const string Pendente = "Pendente";
        public const string EmAnalise = "EmAnalise";
        public const string Aprovada = "Aprovada";
        public const string Rejeitada = "Rejeitada";
    }

    /// <summary>
    /// Holds constant values for the priority levels of an innovation idea.
    /// </summary>
    public static class PrioridadeIdeia
    {
        public const string Baixa = "Baixa";
        public const string Media = "Media";
        public const string Alta = "Alta";
        public const string Critica = "Critica";
    }

    /// <summary>
    /// Represents an idea of innovation submitted by an operator.
    /// Includes details, status, priority and linkage to a strategy.
    /// </summary>
    public class IdeiaInovacao
    {
        /// <summary>
        /// MongoDB identifier (ObjectId) for the idea.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Title of the idea.
        /// </summary>
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the idea.
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// The problem the operator is facing that the idea aims to solve.
        /// </summary>
        public string ProblemaEnfrentado { get; set; } = string.Empty;

        /// <summary>
        /// Expected impact if the idea is implemented.
        /// </summary>
        public string ImpactoEsperado { get; set; } = string.Empty;

        /// <summary>
        /// Current workflow status of the idea.
        /// </summary>
        public string Status { get; set; } = StatusIdeia.Pendente;

        /// <summary>
        /// Priority level assigned to the idea.
        /// </summary>
        public string Prioridade { get; set; } = PrioridadeIdeia.Media;

        /// <summary>
        /// Identifier of the associated strategy (ObjectId).
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string EstrategiaId { get; set; } = string.Empty;

        /// <summary>
        /// Identifier of the operator who submitted the idea.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string OperadorId { get; set; } = string.Empty;

        /// <summary>
        /// Name of the operator (redundant for quick access).
        /// </summary>
        public string NomeOperador { get; set; } = string.Empty;

        /// <summary>
        /// Optional feedback provided by a manager after evaluation.
        /// </summary>
        public string? FeedbackGestor { get; set; }

        /// <summary>
        /// Identifier of the manager who evaluated the idea.
        /// </summary>
        public string? AvaliadoPorGestorId { get; set; }

        /// <summary>
        /// Date when the idea was evaluated.
        /// </summary>
        public DateTime? DataAvaliacao { get; set; }

        /// <summary>
        /// Timestamp of creation.
        /// </summary>
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp of the last update, if any.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; }
    }
}
