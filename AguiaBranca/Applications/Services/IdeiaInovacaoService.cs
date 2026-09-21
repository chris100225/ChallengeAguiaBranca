using AguiaBranca.Domain;
using AguiaBranca.DTOs.Ideia;
using AguiaBranca.Exceptions;
using AguiaBranca.Interfaces;

namespace AguiaBranca.Applications.Services
{
    public class IdeiaInovacaoService
    {
        private readonly IIdeiaInovacaoRepository _ideiaRepository;
        private readonly IEstrategiaRepository _estrategiaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public IdeiaInovacaoService(
            IIdeiaInovacaoRepository ideiaRepository,
            IEstrategiaRepository estrategiaRepository,
            IUsuarioRepository usuarioRepository)
        {
            _ideiaRepository = ideiaRepository;
            _estrategiaRepository = estrategiaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<IdeiaRespostaDTO>> ObterTodasAsync()
        {
            var ideias = await _ideiaRepository.ObterTodasAsync();
            var estrategias = (await _estrategiaRepository.ObterTodasAsync()).ToDictionary(e => e.Id, e => e.Campanha);

            return ideias.Select(i => MapearParaResposta(i, estrategias)).ToList();
        }

        public async Task<List<IdeiaRespostaDTO>> ObterPorOperadorAsync(string operadorId)
        {
            var ideias = await _ideiaRepository.ObterPorOperadorIdAsync(operadorId);
            var estrategias = (await _estrategiaRepository.ObterTodasAsync()).ToDictionary(e => e.Id, e => e.Campanha);

            return ideias.Select(i => MapearParaResposta(i, estrategias)).ToList();
        }

        public async Task<IdeiaRespostaDTO> ObterPorIdAsync(string id)
        {
            var ideia = await _ideiaRepository.ObterPorIdAsync(id);
            if (ideia == null)
            {
                throw new NaoEncontradoException("Ideia de Inovação", id);
            }

            var estrategia = await _estrategiaRepository.ObterPorIdAsync(ideia.EstrategiaId);
            return MapearParaResposta(ideia, estrategia?.Campanha);
        }

        public async Task<IdeiaRespostaDTO> CriarAsync(IdeiaDTO dto, string operadorId)
        {
            var operador = await _usuarioRepository.ObterPorIdAsync(operadorId);
            if (operador == null)
            {
                throw new NaoEncontradoException("Operador", operadorId);
            }

            // Identificar a estratégia: se não fornecida, usa a estratégia vigente
            string estrategiaId = dto.EstrategiaId ?? string.Empty;
            string? campanha = null;

            if (string.IsNullOrWhiteSpace(estrategiaId))
            {
                var vigente = await _estrategiaRepository.ObterVigenteAsync();
                if (vigente == null)
                {
                    throw new DomainException("Nenhuma estratégia vigente ativa foi encontrada no sistema. Solicite à liderança o cadastro da estratégia da empresa antes de submeter ideias.");
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

            var ideia = new IdeiaInovacao
            {
                Titulo = dto.Titulo.Trim(),
                Descricao = dto.Descricao.Trim(),
                ProblemaEnfrentado = dto.ProblemaEnfrentado.Trim(),
                ImpactoEsperado = dto.ImpactoEsperado.Trim(),
                Status = StatusIdeia.Pendente,
                Prioridade = PrioridadeIdeia.Media,
                EstrategiaId = estrategiaId,
                OperadorId = operadorId,
                NomeOperador = operador.Nome,
                DataCriacao = DateTime.UtcNow
            };

            await _ideiaRepository.CriarAsync(ideia);

            return MapearParaResposta(ideia, campanha);
        }

        public async Task<IdeiaRespostaDTO> AtualizarPeloOperadorAsync(string id, IdeiaAtualizacaoDTO dto, string operadorId)
        {
            var ideia = await _ideiaRepository.ObterPorIdAsync(id);
            if (ideia == null)
            {
                throw new NaoEncontradoException("Ideia de Inovação", id);
            }

            if (ideia.OperadorId != operadorId)
            {
                throw new AcessoNegadoException("Você só pode editar as suas próprias ideias registradas.");
            }

            if (ideia.Status != StatusIdeia.Pendente)
            {
                throw new DomainException($"Não é possível alterar uma ideia que já está com status '{ideia.Status}'.");
            }

            ideia.Titulo = dto.Titulo.Trim();
            ideia.Descricao = dto.Descricao.Trim();
            ideia.ProblemaEnfrentado = dto.ProblemaEnfrentado.Trim();
            ideia.ImpactoEsperado = dto.ImpactoEsperado.Trim();

            await _ideiaRepository.AtualizarAsync(ideia);

            var estrategia = await _estrategiaRepository.ObterPorIdAsync(ideia.EstrategiaId);
            return MapearParaResposta(ideia, estrategia?.Campanha);
        }

        public async Task<IdeiaRespostaDTO> AvaliarPorGestorAsync(string id, IdeiaAvaliacaoDTO dto, string gestorId)
        {
            var ideia = await _ideiaRepository.ObterPorIdAsync(id);
            if (ideia == null)
            {
                throw new NaoEncontradoException("Ideia de Inovação", id);
            }

            ideia.Status = dto.Status;
            ideia.Prioridade = dto.Prioridade;
            ideia.FeedbackGestor = dto.FeedbackGestor;
            ideia.AvaliadoPorGestorId = gestorId;
            ideia.DataAvaliacao = DateTime.UtcNow;

            await _ideiaRepository.AtualizarAsync(ideia);

            var estrategia = await _estrategiaRepository.ObterPorIdAsync(ideia.EstrategiaId);
            return MapearParaResposta(ideia, estrategia?.Campanha);
        }

        public async Task RemoverPeloOperadorAsync(string id, string operadorId)
        {
            var ideia = await _ideiaRepository.ObterPorIdAsync(id);
            if (ideia == null)
            {
                throw new NaoEncontradoException("Ideia de Inovação", id);
            }

            if (ideia.OperadorId != operadorId)
            {
                throw new AcessoNegadoException("Você só pode remover as suas próprias ideias registradas.");
            }

            if (ideia.Status != StatusIdeia.Pendente)
            {
                throw new DomainException($"Não é possível excluir uma ideia que já está em status '{ideia.Status}'.");
            }

            await _ideiaRepository.RemoverAsync(id);
        }

        private static IdeiaRespostaDTO MapearParaResposta(IdeiaInovacao i, Dictionary<string, string> campanhas)
        {
            campanhas.TryGetValue(i.EstrategiaId, out var campanha);
            return MapearParaResposta(i, campanha);
        }

        private static IdeiaRespostaDTO MapearParaResposta(IdeiaInovacao i, string? campanha) => new()
        {
            Id = i.Id,
            Titulo = i.Titulo,
            Descricao = i.Descricao,
            ProblemaEnfrentado = i.ProblemaEnfrentado,
            ImpactoEsperado = i.ImpactoEsperado,
            Status = i.Status,
            Prioridade = i.Prioridade,
            EstrategiaId = i.EstrategiaId,
            CampanhaEstrategia = campanha,
            OperadorId = i.OperadorId,
            NomeOperador = i.NomeOperador,
            FeedbackGestor = i.FeedbackGestor,
            AvaliadoPorGestorId = i.AvaliadoPorGestorId,
            DataAvaliacao = i.DataAvaliacao,
            DataCriacao = i.DataCriacao,
            DataAtualizacao = i.DataAtualizacao
        };
    }
}
