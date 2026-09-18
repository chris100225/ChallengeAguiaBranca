using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AguiaBranca.Domain
{

    public static class EtapaProjeto
    {
        public const string Planejamento = "Planejamento";
        public const string EmExecucao = "EmExecucao";
        public const string FasePiloto = "FasePiloto";
        public const string Concluido = "Concluido";
        public const string Cancelado = "Cancelado";
    }

    public static class StatusProjeto
    {
        public const string NoPrazo = "NoPrazo";
        public const string EmRisco = "EmRisco";
        public const string Atrasado = "Atrasado";
        public const string Concluido = "Concluido";
    }

    public class Projeto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string EstrategiaId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdeiaInovacaoId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string GestorId { get; set; } = string.Empty;

        public string NomeGestor { get; set; } = string.Empty;

        public string Etapa { get; set; } = EtapaProjeto.Planejamento;

        public string Status { get; set; } = StatusProjeto.NoPrazo;

        public decimal InvestimentoPrevisto { get; set; }

        public decimal InvestimentoReal { get; set; }

        public decimal RetornoFinanceiroEsperado { get; set; }

        public decimal RetornoFinanceiroReal { get; set; }

        public decimal AumentoProdutividadePercentual { get; set; }

        public DateTime DataInicio { get; set; } = DateTime.UtcNow;

        public DateTime PrazoFinal { get; set; } = DateTime.UtcNow.AddMonths(2);

        public DateTime? DataConclusao { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }

    }
}
