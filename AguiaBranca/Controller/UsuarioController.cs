using AguiaBranca.Applications.Services;
using AguiaBranca.Domain;
using AguiaBranca.DTOs.Usuario;
using AguiaBranca.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AguiaBranca.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsuarioController : ControllerBase
    {
        private readonly AutenticacaoService _autenticacaoService;

        public UsuarioController(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        /// <summary>
        /// Cadastra um novo usuário no sistema (Operador, Gestor ou Líder).
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UsuarioRespostaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] UsuarioDTO dto)
        {
            try
            {
                var resposta = await _autenticacaoService.RegistrarUsuarioAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = resposta.Id }, resposta);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos os usuários cadastrados (Acesso para Gestores e Líderes).
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{PerfilUsuario.Lider},{PerfilUsuario.Gestor}")]
        [ProducesResponseType(typeof(List<UsuarioRespostaDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodos()
        {
            var lista = await _autenticacaoService.ObterTodosAsync();
            return Ok(lista);
        }

        /// <summary>
        /// Retorna os dados do usuário autenticado atual a partir do token JWT.
        /// </summary>
        [HttpGet("meu-perfil")]
        [Authorize]
        [ProducesResponseType(typeof(UsuarioRespostaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MeuPerfil()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return Unauthorized(new { mensagem = "Token inválido ou identificador não encontrado." });
            }

            try
            {
                var usuario = await _autenticacaoService.ObterPorIdAsync(usuarioId);
                return Ok(usuario);
            }
            catch (NaoEncontradoException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Busca os dados de um usuário pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(UsuarioRespostaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(string id)
        {
            try
            {
                var usuario = await _autenticacaoService.ObterPorIdAsync(id);
                return Ok(usuario);
            }
            catch (NaoEncontradoException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
