using AguiaBranca.Applications.Services;
using AguiaBranca.Domain;
using AguiaBranca.DTOs.Projeto;
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
    [Authorize]
    public class ProjetoController : ControllerBase
    {
        private readonly ProjetoService _projetoService;

        public ProjetoController(ProjetoService projetoService)
        {
            _projetoService = projetoService;
        }

        /// <summary>
        /// Consulta todos os projetos e iniciativas com status, prazos e métricas financeiras (Acesso: Gestores e Líderes).
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{PerfilUsuario.Gestor},{PerfilUsuario.Lider}")]
        [ProducesResponseType(typeof(List<ProjetoRespostaDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodos()
        {
            var lista = await _projetoService.ObterTodosAsync();
            return Ok(lista);
        }

        /// <summary>
        /// Consulta um projeto específico pelo ID (Acesso: Gestores e Líderes).
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = $"{PerfilUsuario.Gestor},{PerfilUsuario.Lider}")]
        [ProducesResponseType(typeof(ProjetoRespostaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(string id)
        {
            try
            {
                var projeto = await _projetoService.ObterPorIdAsync(id);
                return Ok(projeto);
            }
            catch (NaoEncontradoException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Consulta os projetos vinculados a uma determinada estratégia (Acesso: Gestores e Líderes).
        /// </summary>
        [HttpGet("por-estrategia/{estrategiaId}")]
        [Authorize(Roles = $"{PerfilUsuario.Gestor},{PerfilUsuario.Lider}")]
        [ProducesResponseType(typeof(List<ProjetoRespostaDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterPorEstrategia(string estrategiaId)
        {
            var lista = await _projetoService.ObterPorEstrategiaAsync(estrategiaId);
            return Ok(lista);
        }

        /// <summary>
        /// Cadastra um novo projeto/iniciativa vinculado à estratégia vigente (Exclusivo para Gestores).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = PerfilUsuario.Gestor)]
        [ProducesResponseType(typeof(ProjetoRespostaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Criar([FromBody] ProjetoDTO dto)
        {
            var gestorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            try
            {
                var resposta = await _projetoService.CriarAsync(dto, gestorId);
                return CreatedAtAction(nameof(ObterPorId), new { id = resposta.Id }, resposta);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (NaoEncontradoException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza os dados de um projeto, seu progresso (etapa, status) e resultados obtidos (Exclusivo para Gestores).
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = PerfilUsuario.Gestor)]
        [ProducesResponseType(typeof(ProjetoRespostaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Atualizar(string id, [FromBody] ProjetoAtualizacaoDTO dto)
        {
            var gestorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            try
            {
                var resposta = await _projetoService.AtualizarAsync(id, dto, gestorId);
                return Ok(resposta);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (NaoEncontradoException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Remove um projeto do sistema (Exclusivo para Gestores).
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = PerfilUsuario.Gestor)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Remover(string id)
        {
            try
            {
                await _projetoService.RemoverAsync(id);
                return NoContent();
            }
            catch (NaoEncontradoException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
