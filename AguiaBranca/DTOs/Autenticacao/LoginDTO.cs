using System.ComponentModel.DataAnnotations;

namespace AguiaBranca.DTOs.Autenticacao
{
    public class LoginDTO
    {
       
            [Required(ErrorMessage = "O email é obrigatório.")]
            [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "A senha é obrigatória.")]
            public string Senha { get; set; } = string.Empty;
        }

        public class LoginRespostaDTO
        {
            public string Token { get; set; } = string.Empty;
            public string UsuarioId { get; set; } = string.Empty;
            public string Nome { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Perfil { get; set; } = string.Empty;
            public DateTime ExpiraEm { get; set; }
        }
    }

