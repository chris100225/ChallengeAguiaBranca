using AguiaBranca.Applications.Services;
using AguiaBranca.Domain;
using AguiaBranca.DTOs.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AguiaBranca.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize(Roles = PerfilUsuario.Lider)] // Exclusivo para Liderança
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Retorna o resumo executivo geral da empresa: ROI global, lucro obtido, investimentos, produtividade e status de projetos (Exclusivo para Líderes).
        /// </summary>
        [HttpGet("resumo-geral")]
        [ProducesResponseType(typeof(DashboardDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObterResumoGeral()
        {
            var resumo = await _dashboardService.ObterResumoGeralAsync();
            return Ok(resumo);
        }

        /// <summary>
        /// Retorna os retornos consolidados agrupados por estratégia da empresa (Exclusivo para Líderes).
        /// </summary>
        [HttpGet("por-estrategia")]
        [ProducesResponseType(typeof(List<DashboardRetornoPorEstrategiaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObterRetornosPorEstrategia()
        {
            var retornos = await _dashboardService.ObterRetornosPorEstrategiaAsync();
            return Ok(retornos);
        }

        /// <summary>
        /// Retorna conjuntos de dados formatados em pares chave/valor para alimentação direta de gráficos no frontend (Exclusivo para Líderes).
        /// </summary>
        [HttpGet("metricas-graficos")]
        [ProducesResponseType(typeof(MetricasGraficosDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObterMetricasGraficos()
        {
            var metricas = await _dashboardService.ObterMetricasGraficosAsync();
            return Ok(metricas);
        }
    }
}
