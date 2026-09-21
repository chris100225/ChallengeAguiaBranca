using System.ComponentModel.DataAnnotations;

namespace AguiaBranca.DTOs.Estrategia
{
    public class EstrategiaDTO
    {
        [Required(ErrorMessage = "O nome da campanha é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "A campanha deve ter entre 3 e 150 caracteres.")]
        public string Campanha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public string Categoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public bool Ativa { get; set; } = true;
    }

    public class EstrategiaAtualizacaoDTO
    {
        [Required(ErrorMessage = "O nome da campanha é obrigatório.")]
        public string Campanha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public string Categoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public bool Ativa { get; set; }
    }

    public class EstrategiaRespostaDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Campanha { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Ativa { get; set; }
        public string CriadoPorUsuarioId { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
