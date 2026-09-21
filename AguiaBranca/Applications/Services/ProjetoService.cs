using AguiaBranca.Domain;
using AguiaBranca.DTOs.Projeto;
using AguiaBranca.Exceptions;
using AguiaBranca.Interfaces;

namespace AguiaBranca.Applications.Services
{
    public class ProjetoService
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IEstrategiaRepository _estrategiaRepository;
        private readonly IIdeiaInovacaoRepository _ideiaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ProjetoService(
            IProjetoRepository projetoRepository,
            IEstrategiaRepository estrategiaRepository,
            IIdeiaInovacaoRepository ideiaRepository,
            IUsuarioRepository usuarioRepository)
        {
            _projetoRepository = projetoRepository;
            _estrategiaRepository = estrategiaRepository;
            _ideiaRepository = ideiaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<ProjetoRespostaDTO>> ObterTodosAsync()
        {
            var projetos = await _projetoRepository.ObterTodosAsync();
            var estrategias = (await _estrategiaRepository.ObterTodasAsync()).ToDictionary(e => e.Id, e => e.Campanha);

            return projetos.Select(p => MapearParaResposta(p, estrategias)).ToList();
        }

        public async Task<ProjetoRespostaDTO> ObterPorIdAsync(string id)
        {
            var projeto = await _projetoRepository.ObterPorIdAsync(id);
            if (projeto == null)
            {
                throw new NaoEncontradoException("Projeto", id);
            }

            var estrategia = await _estrategiaRepository.ObterPorIdAsync(projeto.EstrategiaId);
            return MapearParaResposta(projeto, estrategia?.Campanha);
        }

        public async Task<List<ProjetoRespostaDTO>> ObterPorEstrategiaAsync(string estrategiaId)
        {
            var projetos = await _projetoRepository.ObterPorEstrategiaIdAsync(estrategiaId);
            var estrategia = await _estrategiaRepository.ObterPorIdAsync(estrategiaId);

            return projetos.Select(p => MapearParaResposta(p, estrategia?.Campanha)).ToList();
        }

        public async Task<ProjetoRespostaDTO> CriarAsync(ProjetoDTO dto, string gestorId)
        {
            var gestor = await _usuarioRepository.ObterPorIdAsync(gestorId);
            if (gestor == null)
            {
                throw new NaoEncontradoException("Gestor", gestorId);
            }

            // Se não fornecido EstrategiaId, vincular à estratégia vigente
            string estrategiaId = dto.EstrategiaId ?? string.Empty;
            string? campanha = null;

            if (string.IsNullOrWhiteSpace(estrategiaId))
            {
                var vigente = await _estrategiaRepository.ObterVigenteAsync();
                if (vigente == null)
                {
                    throw new DomainException("Nenhuma estratégia vigente ativa foi encontrada no sistema. O projeto deve ser vinculado a uma estratégia.");
                }
                estrategiaId = vigente.Id;
                campanha = vigente.Campanha;
            }
            else
            {
                var estrategia = await _estrategiaRepository.ObterPorIdAsync(estrategiaId);
                if (estrategia == null)
                {
                    throw new NaoEncontradoException("Estratégia", estrategiaId);
                }
                campanha = estrategia.Campanha;
            }

            // Validar Ideia de Inovação vinculada (se informada)
            if (!string.IsNullOrWhiteSpace(dto.IdeiaInovacaoId))
            {
                var ideia = await _ideiaRepository.ObterPorIdAsync(dto.IdeiaInovacaoId);
                if (ideia == null)
                {
                    throw new NaoEncontradoException("Ideia de Inovação vinculada", dto.IdeiaInovacaoId);
                }
            }

            var dataInicio = dto.DataInicio ?? DateTime.UtcNow;
            var prazoFinal = dto.PrazoFinal ?? dataInicio.AddMonths(2);

            if (prazoFinal < dataInicio)
            {
                throw new DomainException("O prazo final do projeto não pode ser anterior à data de início.");
            }

            var projeto = new Projeto
            {
                Nome = dto.Nome.Trim(),
                Descricao = dto.Descricao.Trim(),
                EstrategiaId = estrategiaId,
                IdeiaInovacaoId = dto.IdeiaInovacaoId,
                GestorId = gestorId,
                NomeGestor = gestor.Nome,
                Etapa = EtapaProjeto.Planejamento,
                Status = StatusProjeto.NoPrazo,
                InvestimentoPrevisto = dto.InvestimentoPrevisto,
                InvestimentoReal = 0,
                RetornoFinanceiroEsperado = dto.RetornoFinanceiroEsperado,
                RetornoFinanceiroReal = 0,
                AumentoProdutividadePercentual = 0,
                DataInicio = dataInicio,
                PrazoFinal = prazoFinal,
                DataCriacao = DateTime.UtcNow
            };

            await _projetoRepository.CriarAsync(projeto);

            return MapearParaResposta(projeto, campanha);
        }

        public async Task<ProjetoRespostaDTO> AtualizarAsync(string id, ProjetoAtualizacaoDTO dto, string gestorId)
        {
            var projeto = await _projetoRepository.ObterPorIdAsync(id);
            if (projeto == null)
            {
                throw new NaoEncontradoException("Projeto", id);
            }

            var prazoFinal = dto.PrazoFinal ?? projeto.PrazoFinal;
            if (prazoFinal < projeto.DataInicio)
            {
                throw new DomainException("O prazo final do projeto não pode ser anterior à data de início.");
            }

            projeto.Nome = dto.Nome.Trim();
            projeto.Descricao = dto.Descricao.Trim();
            projeto.Etapa = dto.Etapa;
            projeto.Status = dto.Status;
            projeto.InvestimentoPrevisto = dto.InvestimentoPrevisto;
            projeto.InvestimentoReal = dto.InvestimentoReal;
            projeto.RetornoFinanceiroEsperado = dto.RetornoFinanceiroEsperado;
            projeto.RetornoFinanceiroReal = dto.RetornoFinanceiroReal;
            projeto.AumentoProdutividadePercentual = dto.AumentoProdutividadePercentual;
            projeto.PrazoFinal = prazoFinal;
            projeto.DataConclusao = dto.DataConclusao;

            if (dto.Etapa == EtapaProjeto.Concluido && projeto.DataConclusao == null)
            {
                projeto.DataConclusao = DateTime.UtcNow;
            }

            await _projetoRepository.AtualizarAsync(projeto);

            var estrategia = await _estrategiaRepository.ObterPorIdAsync(projeto.EstrategiaId);
            return MapearParaResposta(projeto, estrategia?.Campanha);
        }

        public async Task RemoverAsync(string id)
        {
            var projeto = await _projetoRepository.ObterPorIdAsync(id);
            if (projeto == null)
            {
                throw new NaoEncontradoException("Projeto", id);
            }

            await _projetoRepository.RemoverAsync(id);
        }

        private static ProjetoRespostaDTO MapearParaResposta(Projeto p, Dictionary<string, string> campanhas)
        {
            campanhas.TryGetValue(p.EstrategiaId, out var campanha);
            return MapearParaResposta(p, campanha);
        }

        private static ProjetoRespostaDTO MapearParaResposta(Projeto p, string? campanha) => new()
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            EstrategiaId = p.EstrategiaId,
            CampanhaEstrategia = campanha,
            IdeiaInovacaoId = p.IdeiaInovacaoId,
            GestorId = p.GestorId,
            NomeGestor = p.NomeGestor,
            Etapa = p.Etapa,
            Status = p.Status,
            InvestimentoPrevisto = p.InvestimentoPrevisto,
            InvestimentoReal = p.InvestimentoReal,
            RetornoFinanceiroEsperado = p.RetornoFinanceiroEsperado,
            RetornoFinanceiroReal = p.RetornoFinanceiroReal,
            AumentoProdutividadePercentual = p.AumentoProdutividadePercentual,
            DataInicio = p.DataInicio,
            PrazoFinal = p.PrazoFinal,
            DataConclusao = p.DataConclusao,
            DataCriacao = p.DataCriacao,
            DataAtualizacao = p.DataAtualizacao
        };
    }
}

