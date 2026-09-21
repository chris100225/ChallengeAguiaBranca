using AguiaBranca.Applications.Services;
using AguiaBranca.Domain;
using AguiaBranca.DTOs.Estrategia;
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
    public class EstrategiaController : ControllerBase
    {
        private readonly EstrategiaService _estrategiaService;

        public EstrategiaController(EstrategiaService estrategiaService)
        {
            _estrategiaService = estrategiaService;
        }

        /// <summary>
        /// Consulta o histórico completo de orientações estratégicas (Acesso: Operadores, Gestores e Líderes).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<EstrategiaRespostaDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodas()
        {
            var lista = await _estrategiaService.ObterTodasAsync();
            return Ok(lista);
        }

        /// <summary>
        /// Consulta a orientação estratégica vigente atual da empresa (Acesso: Operadores, Gestores e Líderes).
        /// </summary>
        [HttpGet("vigente")]
        [ProducesResponseType(typeof(EstrategiaRespostaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterVigente()
        {
            var vigente = await _estrategiaService.ObterVigenteAsync();
            if (vigente == null)
            {
                return NotFound(new { mensagem = "Nenhuma estratégia vigente ativa foi encontrada no momento." });
            }
            return Ok(vigente);
        }

        /// <summary>
        /// Consulta os detalhes de uma orientação estratégica pelo ID (Acesso: Operadores, Gestores e Líderes).
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EstrategiaRespostaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(string id)
        {
            try
            {
                var estrategia = await _estrategiaService.ObterPorIdAsync(id);
                return Ok(estrategia);
            }
            catch (NaoEncontradoException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Cadastra uma nova orientação estratégica da empresa (Exclusivo: Liderança).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = PerfilUsuario.Lider)]
        [ProducesResponseType(typeof(EstrategiaRespostaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Criar([FromBody] EstrategiaDTO dto)
        {
            var liderId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            try
            {
                var resposta = await _estrategiaService.CriarAsync(dto, liderId);
                return CreatedAtAction(nameof(ObterPorId), new { id = resposta.Id }, resposta);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza uma orientação estratégica existente (Exclusivo: Liderança).
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = PerfilUsuario.Lider)]
        [ProducesResponseType(typeof(EstrategiaRespostaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Atualizar(string id, [FromBody] EstrategiaAtualizacaoDTO dto)
        {
            try
            {
                var resposta = await _estrategiaService.AtualizarAsync(id, dto);
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
        /// Remove uma orientação estratégica (Exclusivo: Liderança).
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = PerfilUsuario.Lider)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Remover(string id)
        {
            try
            {
                await _estrategiaService.RemoverAsync(id);
                return NoContent();
            }
            catch (NaoEncontradoException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
