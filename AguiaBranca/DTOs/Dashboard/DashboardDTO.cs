namespace AguiaBranca.DTOs.Dashboard
{
    public class DashboardDTO
    {
        public int TotalProjetos { get; set; }
        public int ProjetosConcluidos { get; set; }
        public int ProjetosEmExecucao { get; set; }
        public int ProjetosAtrasados { get; set; }
        public int ProjetosNoPrazo { get; set; }

        public decimal InvestimentoTotalPrevisto { get; set; }
        public decimal InvestimentoTotalReal { get; set; }

        public decimal RetornoFinanceiroTotalEsperado { get; set; }
        public decimal RetornoFinanceiroTotalReal { get; set; }

        public decimal LucroLiquidoTotal { get; set; }
        public decimal RoiGeralPercentual { get; set; }

        public decimal MediaAumentoProdutividadePercentual { get; set; }

        public int TotalIdeiasRecebidas { get; set; }
        public int IdeiasAprovadas { get; set; }
        public int IdeiasPendentes { get; set; }

        public List<ItemGraficoDTO> DistribuicaoPorEtapa { get; set; } = new();
        public List<ItemGraficoDTO> DistribuicaoPorStatus { get; set; } = new();
    }

    public class DashboardRetornoPorEstrategiaDTO
    {
        public string EstrategiaId { get; set; } = string.Empty;
        public string Campanha { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public bool Ativa { get; set; }

        public int QuantidadeProjetos { get; set; }
        public decimal InvestimentoTotal { get; set; }
        public decimal RetornoTotal { get; set; }
        public decimal LucroTotal { get; set; }
        public decimal RoiPercentual { get; set; }
        public decimal MediaProdutividade { get; set; }

        public List<string> ProjetosVinculados { get; set; } = new();
    }

    public class ItemGraficoDTO
    {
        public string Chave { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string? Rotulo { get; set; }
    }

    public class MetricasGraficosDTO
    {
        public List<ItemGraficoDTO> RoiPorEstrategia { get; set; } = new();
        public List<ItemGraficoDTO> InvestimentoVsRetornoPorEstrategia { get; set; } = new();
        public List<ItemGraficoDTO> ProjetosPorEtapa { get; set; } = new();
        public List<ItemGraficoDTO> IdeiasPorStatus { get; set; } = new();
    }
}
