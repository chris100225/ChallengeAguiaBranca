using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AguiaBranca.Domain
{
    /// <summary>
    /// Represents a strategic initiative within the company.
    /// Contains campaign information, category, description, timeline and status.
    /// </summary>
    /// <summary>
/// Represents a strategic initiative within the company.
/// </summary>
public class Estrategia
    {
        /// <summary>
        /// MongoDB identifier (ObjectId) for the strategy.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Name or title of the campaign associated with the strategy.
        /// </summary>
        public string Campanha { get; set; } = string.Empty;

        /// <summary>
        /// Category of the strategy (e.g., Marketing, Operations, Innovation).
        /// </summary>
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the strategic initiative.
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// The date the strategy was created (default UTC now).
        /// </summary>
        public DateTime Data { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Start date of the strategy execution period.
        /// </summary>
        public DateTime DataInicio { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// End date of the strategy execution period. Defaults to three months after start.
        /// </summary>
        public DateTime DataFim { get; set; } = DateTime.UtcNow.AddMonths(3);

        /// <summary>
        /// Indicates whether this strategy is the currently active one.
        /// </summary>
        public bool Ativa { get; set; } = true;

        /// <summary>
        /// Identifier of the user who created the strategy.
        /// </summary>
        public string CriadoPorUsuarioId { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp when the strategy record was inserted.
        /// </summary>
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp of the last update, if any.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; }
    }
}
