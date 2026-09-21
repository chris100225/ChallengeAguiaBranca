using AguiaBranca.Applications.Services;
using AguiaBranca.DTOs.Autenticacao;
using AguiaBranca.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AguiaBranca.Controller
{
        [ApiController]
        [Route("api/[controller]")]
        [Produces("application/json")]
        public class AutenticacaoController : ControllerBase
        {
            private readonly AutenticacaoService     _autenticacaoService;

            public AutenticacaoController(AutenticacaoService autenticacaoService)
            {
                _autenticacaoService = autenticacaoService;
            }

            /// <summary>
            /// Realiza o login no sistema para os 3 perfis (Operadores, Gestores e Líderes) e retorna o Bearer Token JWT.
            /// </summary>
            [HttpPost("login")]
            [ProducesResponseType(typeof(LoginRespostaDTO), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Login([FromBody] LoginDTO dto)
            {
                try
                {
                    var resposta = await _autenticacaoService.AutenticarAsync(dto);
                    return Ok(resposta);
                }
                catch (DomainException ex)
                {
                    return BadRequest(new { mensagem = ex.Message });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno ao processar o login.", detalhe = ex.Message });
                }
            }
        }
    }
