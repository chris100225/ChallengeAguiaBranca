using AguiaBranca.Applications.Services;
using AguiaBranca.Domain;
using AguiaBranca.DTOs.Ideia;
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
    public class IdeiaInovacaoController : ControllerBase
    {
        private readonly IdeiaInovacaoService _ideiaService;

        public IdeiaInovacaoController(IdeiaInovacaoService ideiaService)
        {
            _ideiaService = ideiaService;
        }

        /// <summary>
        /// Lista todas as ideias de inovação submetidas (Acesso para Gestores e Líderes).
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{PerfilUsuario.Gestor},{PerfilUsuario.Lider}")]
        [ProducesResponseType(typeof(List<IdeiaRespostaDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodas()
        {
            var lista = await _ideiaService.ObterTodasAsync();
            return Ok(lista);
        }

        /// <summary>
        /// Lista apenas as ideias registradas pelo operador autenticado (Acesso exclusivo para Operadores).
        /// </summary>
        [HttpGet("minhas")]
        [Authorize(Roles = PerfilUsuario.Operador)]
        [ProducesResponseType(typeof(List<IdeiaRespostaDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterMinhasIdeias()
        {
            var operadorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var lista = await _ideiaService.ObterPorOperadorAsync(operadorId);
            return Ok(lista);
        }

        /// <summary>
        /// Obtém os detalhes de uma ideia específica por ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdeiaRespostaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(string id)
        {
            try
            {
                var ideia = await _ideiaService.ObterPorIdAsync(id);
                return Ok(ideia);
            }
            catch (NaoEncontradoException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Cadastra uma nova ideia de inovação vinculada à estratégia vigente (Exclusivo para Operadores).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = PerfilUsuario.Operador)]
        [ProducesResponseType(typeof(IdeiaRespostaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] IdeiaDTO dto)
        {
            var operadorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            try
            {
                var resposta = await _ideiaService.CriarAsync(dto, operadorId);
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
        /// Atualiza uma ideia de inovação própria enquanto estiver Pendente (Exclusivo para Operadores).
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = PerfilUsuario.Operador)]
        [ProducesResponseType(typeof(IdeiaRespostaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(string id, [FromBody] IdeiaAtualizacaoDTO dto)
        {
            var operadorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            try
            {
                var resposta = await _ideiaService.AtualizarPeloOperadorAsync(id, dto, operadorId);
                return Ok(resposta);
            }
            catch (AcessoNegadoException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { mensagem = ex.Message });
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
        /// Remove uma ideia de inovação própria enquanto estiver Pendente (Exclusivo para Operadores).
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = PerfilUsuario.Operador)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remover(string id)
        {
            var operadorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            try
            {
                await _ideiaService.RemoverPeloOperadorAsync(id, operadorId);
                return NoContent();
            }
            catch (AcessoNegadoException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { mensagem = ex.Message });
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
        /// Permite aos gestores priorizar e aprovar/rejeitar ideias de inovação (Exclusivo para Gestores).
        /// </summary>
        [HttpPatch("{id}/avaliar")]
        [Authorize(Roles = PerfilUsuario.Gestor)]
        [ProducesResponseType(typeof(IdeiaRespostaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Avaliar(string id, [FromBody] IdeiaAvaliacaoDTO dto)
        {
            var gestorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            try
            {
                var resposta = await _ideiaService.AvaliarPorGestorAsync(id, dto, gestorId);
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
    }
}
