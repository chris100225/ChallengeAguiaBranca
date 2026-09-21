using AguiaBranca.Domain;
using AguiaBranca.DTOs.Dashboard;
using AguiaBranca.Interfaces;

namespace AguiaBranca.Applications.Services
{
    public class DashboardService
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IEstrategiaRepository _estrategiaRepository;
        private readonly IIdeiaInovacaoRepository _ideiaRepository;

        public DashboardService(
            IProjetoRepository projetoRepository,
            IEstrategiaRepository estrategiaRepository,
            IIdeiaInovacaoRepository ideiaRepository)
        {
            _projetoRepository = projetoRepository;
            _estrategiaRepository = estrategiaRepository;
            _ideiaRepository = ideiaRepository;
        }

        public async Task<DashboardDTO> ObterResumoGeralAsync()
        {
            var projetos = await _projetoRepository.ObterTodosAsync();
            var ideias = await _ideiaRepository.ObterTodasAsync();

            var totalProjetos = projetos.Count;
            var concluidos = projetos.Count(p => p.Etapa == EtapaProjeto.Concluido || p.Status == StatusProjeto.Concluido);
            var emExecucao = projetos.Count(p => p.Etapa == EtapaProjeto.EmExecucao);
            var atrasados = projetos.Count(p => (p.DataConclusao == null && DateTime.UtcNow > p.PrazoFinal) || p.Status == StatusProjeto.Atrasado);
            var noPrazo = totalProjetos - atrasados;

            var investimentoPrevisto = projetos.Sum(p => p.InvestimentoPrevisto);
            var investimentoReal = projetos.Sum(p => p.InvestimentoReal);

            var retornoEsperado = projetos.Sum(p => p.RetornoFinanceiroEsperado);
            var retornoReal = projetos.Sum(p => p.RetornoFinanceiroReal);

            var lucroLiquido = retornoReal - investimentoReal;
            var roiGeral = investimentoReal > 0
                ? Math.Round(((retornoReal - investimentoReal) / investimentoReal) * 100, 2)
                : 0;

            var mediaProdutividade = projetos.Any()
                ? Math.Round(projetos.Average(p => p.AumentoProdutividadePercentual), 2)
                : 0;

            var ideiasAprovadas = ideias.Count(i => i.Status == StatusIdeia.Aprovada);
            var ideiasPendentes = ideias.Count(i => i.Status == StatusIdeia.Pendente);

            var etapas = projetos
                .GroupBy(p => p.Etapa)
                .Select(g => new ItemGraficoDTO { Chave = g.Key, Valor = g.Count(), Rotulo = g.Key })
                .ToList();

            var statusList = projetos
                .GroupBy(p => p.Status)
                .Select(g => new ItemGraficoDTO { Chave = g.Key, Valor = g.Count(), Rotulo = g.Key })
                .ToList();

            return new DashboardDTO
            {
                TotalProjetos = totalProjetos,
                ProjetosConcluidos = concluidos,
                ProjetosEmExecucao = emExecucao,
                ProjetosAtrasados = atrasados,
                ProjetosNoPrazo = noPrazo < 0 ? 0 : noPrazo,
                InvestimentoTotalPrevisto = investimentoPrevisto,
                InvestimentoTotalReal = investimentoReal,
                RetornoFinanceiroTotalEsperado = retornoEsperado,
                RetornoFinanceiroTotalReal = retornoReal,
                LucroLiquidoTotal = lucroLiquido,
                RoiGeralPercentual = roiGeral,
                MediaAumentoProdutividadePercentual = mediaProdutividade,
                TotalIdeiasRecebidas = ideias.Count,
                IdeiasAprovadas = ideiasAprovadas,
                IdeiasPendentes = ideiasPendentes,
                DistribuicaoPorEtapa = etapas,
                DistribuicaoPorStatus = statusList
            };
        }

        public async Task<List<DashboardRetornoPorEstrategiaDTO>> ObterRetornosPorEstrategiaAsync()
        {
            var estrategias = await _estrategiaRepository.ObterTodasAsync();
            var projetos = await _projetoRepository.ObterTodosAsync();

            var resultado = new List<DashboardRetornoPorEstrategiaDTO>();

            foreach (var est in estrategias)
            {
                var projetosDaEstrategia = projetos.Where(p => p.EstrategiaId == est.Id).ToList();

                var investimento = projetosDaEstrategia.Sum(p => p.InvestimentoReal);
                var retorno = projetosDaEstrategia.Sum(p => p.RetornoFinanceiroReal);
                var lucro = retorno - investimento;
                var roi = investimento > 0 ? Math.Round(((retorno - investimento) / investimento) * 100, 2) : 0;
                var mediaProd = projetosDaEstrategia.Any()
                    ? Math.Round(projetosDaEstrategia.Average(p => p.AumentoProdutividadePercentual), 2)
                    : 0;

                resultado.Add(new DashboardRetornoPorEstrategiaDTO
                {
                    EstrategiaId = est.Id,
                    Campanha = est.Campanha,
                    Categoria = est.Categoria,
                    Ativa = est.Ativa,
                    QuantidadeProjetos = projetosDaEstrategia.Count,
                    InvestimentoTotal = investimento,
                    RetornoTotal = retorno,
                    LucroTotal = lucro,
                    RoiPercentual = roi,
                    MediaProdutividade = mediaProd,
                    ProjetosVinculados = projetosDaEstrategia.Select(p => p.Nome).ToList()
                });
            }

            return resultado;
        }

        public async Task<MetricasGraficosDTO> ObterMetricasGraficosAsync()
        {
            var estrategias = await _estrategiaRepository.ObterTodasAsync();
            var projetos = await _projetoRepository.ObterTodosAsync();
            var ideias = await _ideiaRepository.ObterTodasAsync();

            var roiPorEstrategia = new List<ItemGraficoDTO>();
            var investVsRetorno = new List<ItemGraficoDTO>();

            foreach (var est in estrategias)
            {
                var projs = projetos.Where(p => p.EstrategiaId == est.Id).ToList();
                var invest = projs.Sum(p => p.InvestimentoReal);
                var ret = projs.Sum(p => p.RetornoFinanceiroReal);
                var roi = invest > 0 ? Math.Round(((ret - invest) / invest) * 100, 2) : 0;

                roiPorEstrategia.Add(new ItemGraficoDTO
                {
                    Chave = est.Campanha,
                    Valor = roi,
                    Rotulo = $"{est.Campanha} ({roi}%)"
                });

                investVsRetorno.Add(new ItemGraficoDTO
                {
                    Chave = $"{est.Campanha} (Investimento)",
                    Valor = invest,
                    Rotulo = "Investimento Real"
                });

                investVsRetorno.Add(new ItemGraficoDTO
                {
                    Chave = $"{est.Campanha} (Retorno)",
                    Valor = ret,
                    Rotulo = "Retorno Real"
                });
            }

            var etapas = projetos
                .GroupBy(p => p.Etapa)
                .Select(g => new ItemGraficoDTO { Chave = g.Key, Valor = g.Count(), Rotulo = g.Key })
                .ToList();

            var ideiasStatus = ideias
                .GroupBy(i => i.Status)
                .Select(g => new ItemGraficoDTO { Chave = g.Key, Valor = g.Count(), Rotulo = g.Key })
                .ToList();

            return new MetricasGraficosDTO
            {
                RoiPorEstrategia = roiPorEstrategia,
                InvestimentoVsRetornoPorEstrategia = investVsRetorno,
                ProjetosPorEtapa = etapas,
                IdeiasPorStatus = ideiasStatus
            };
        }
    }
}