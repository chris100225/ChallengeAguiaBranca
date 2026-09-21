using System.ComponentModel.DataAnnotations;

namespace AguiaBranca.DTOs.Ideia
{
    public class IdeiaDTO
    {
        [Required(ErrorMessage = "O título da ideia é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 150 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição da ideia é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O problema enfrentado deve ser especificado.")]
        public string ProblemaEnfrentado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O impacto esperado deve ser informado.")]
        public string ImpactoEsperado { get; set; } = string.Empty;

        // Opcional: Se não for informado, a ideia será automaticamente vinculada à estratégia vigente ativa
        public string? EstrategiaId { get; set; }
    }

    public class IdeiaAtualizacaoDTO
    {
        [Required(ErrorMessage = "O título da ideia é obrigatório.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição da ideia é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O problema enfrentado deve ser especificado.")]
        public string ProblemaEnfrentado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O impacto esperado deve ser informado.")]
        public string ImpactoEsperado { get; set; } = string.Empty;
    }

    public class IdeiaAvaliacaoDTO
    {
        [Required(ErrorMessage = "O status da avaliação é obrigatório (Aprovada, Rejeitada, EmAnalise).")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "A prioridade é obrigatória (Baixa, Media, Alta, Critica).")]
        public string Prioridade { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "O feedback do gestor pode ter no máximo 500 caracteres.")]
        public string? FeedbackGestor { get; set; }
    }

    public class IdeiaRespostaDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string ProblemaEnfrentado { get; set; } = string.Empty;
        public string ImpactoEsperado { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Prioridade { get; set; } = string.Empty;
        public string EstrategiaId { get; set; } = string.Empty;
        public string? CampanhaEstrategia { get; set; }
        public string OperadorId { get; set; } = string.Empty;
        public string NomeOperador { get; set; } = string.Empty;
        public string? FeedbackGestor { get; set; }
        public string? AvaliadoPorGestorId { get; set; }
        public DateTime? DataAvaliacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
