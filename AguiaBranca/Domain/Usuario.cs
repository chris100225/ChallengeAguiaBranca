using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AguiaBranca.Domain
{
    /// <summary>
    /// Represents a system user (operator, manager, or leader).
    /// Stores authentication data and profile information.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// MongoDB identifier (ObjectId) for the user.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Full name of the user.
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email address used for login.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password (BCrypt).
        /// </summary>
        public string SenhaHash { get; set; } = string.Empty;

        /// <summary>
        /// User role/profile (Operador, Gestor, Lider).
        /// </summary>
        public string Perfil { get; set; } = PerfilUsuario.Operador;

        /// <summary>
        /// Indicates whether the account is active.
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Date the user was registered.
        /// </summary>
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}
