using System.ComponentModel.DataAnnotations;

namespace AguiaBranca.DTOs.Projeto
{
    public class ProjetoDTO
    {
        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição do projeto é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        // Opcional: Se não for informado, vincula à estratégia vigente ativa
        public string? EstrategiaId { get; set; }

        public string? IdeiaInovacaoId { get; set; }

        [Range(0, 1000000000, ErrorMessage = "O investimento previsto deve ser positivo.")]
        public decimal InvestimentoPrevisto { get; set; }

        [Range(0, 1000000000, ErrorMessage = "O retorno financeiro esperado deve ser positivo.")]
        public decimal RetornoFinanceiroEsperado { get; set; }

        public DateTime? DataInicio { get; set; }
        public DateTime? PrazoFinal { get; set; }
    }

    public class ProjetoAtualizacaoDTO
    {
        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição do projeto é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A etapa do projeto é obrigatória (Planejamento, EmExecucao, FasePiloto, Concluido, Cancelado).")]
        public string Etapa { get; set; } = string.Empty;

        [Required(ErrorMessage = "O status do projeto é obrigatório (NoPrazo, EmRisco, Atrasado, Concluido).")]
        public string Status { get; set; } = string.Empty;

        public decimal InvestimentoPrevisto { get; set; }
        public decimal InvestimentoReal { get; set; }
        public decimal RetornoFinanceiroEsperado { get; set; }
        public decimal RetornoFinanceiroReal { get; set; }

        [Range(0, 1000, ErrorMessage = "O percentual de aumento de produtividade deve ser positivo.")]
        public decimal AumentoProdutividadePercentual { get; set; }

        public DateTime? PrazoFinal { get; set; }
        public DateTime? DataConclusao { get; set; }
    }

    public class ProjetoRespostaDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string EstrategiaId { get; set; } = string.Empty;
        public string? CampanhaEstrategia { get; set; }
        public string? IdeiaInovacaoId { get; set; }
        public string GestorId { get; set; } = string.Empty;
        public string NomeGestor { get; set; } = string.Empty;
        public string Etapa { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal InvestimentoPrevisto { get; set; }
        public decimal InvestimentoReal { get; set; }
        public decimal RetornoFinanceiroEsperado { get; set; }
        public decimal RetornoFinanceiroReal { get; set; }
        public decimal LucroObtido => RetornoFinanceiroReal - InvestimentoReal;
        public decimal RoiPercentual => InvestimentoReal > 0
            ? Math.Round(((RetornoFinanceiroReal - InvestimentoReal) / InvestimentoReal) * 100, 2)
            : 0;
        public decimal AumentoProdutividadePercentual { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime PrazoFinal { get; set; }
        public DateTime? DataConclusao { get; set; }
        public bool EstaAtrasado => (DataConclusao == null && DateTime.UtcNow > PrazoFinal) || Status == "Atrasado";
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
