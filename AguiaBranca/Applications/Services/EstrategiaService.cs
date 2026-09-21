using AguiaBranca.Domain;
using AguiaBranca.DTOs.Estrategia;
using AguiaBranca.Exceptions;
using AguiaBranca.Interfaces;

namespace AguiaBranca.Applications.Services
{
    public class EstrategiaService
    {
        private readonly IEstrategiaRepository _estrategiaRepository;

        public EstrategiaService(IEstrategiaRepository estrategiaRepository)
        {
            _estrategiaRepository = estrategiaRepository;
        }

        public async Task<List<EstrategiaRespostaDTO>> ObterTodasAsync()
        {
            var estrategias = await _estrategiaRepository.ObterTodasAsync();
            return estrategias.Select(MapearParaResposta).ToList();
        }

        public async Task<EstrategiaRespostaDTO> ObterPorIdAsync(string id)
        {
            var estrategia = await _estrategiaRepository.ObterPorIdAsync(id);
            if (estrategia == null)
            {
                throw new NaoEncontradoException("Estratégia", id);
            }
            return MapearParaResposta(estrategia);
        }

        public async Task<EstrategiaRespostaDTO?> ObterVigenteAsync()
        {
            var estrategia = await _estrategiaRepository.ObterVigenteAsync();
            return estrategia != null ? MapearParaResposta(estrategia) : null;
        }

        public async Task<EstrategiaRespostaDTO> CriarAsync(EstrategiaDTO dto, string usuarioId)
        {
            var dataInicio = dto.DataInicio ?? DateTime.UtcNow;
            var dataFim = dto.DataFim ?? dataInicio.AddMonths(3);

            if (dataFim < dataInicio)
            {
                throw new DomainException("A data de término da estratégia não pode ser anterior à data de início.");
            }

            var estrategia = new Estrategia
            {
                Campanha = dto.Campanha.Trim(),
                Categoria = dto.Categoria.Trim(),
                Descricao = dto.Descricao.Trim(),
                Data = DateTime.UtcNow,
                DataInicio = dataInicio,
                DataFim = dataFim,
                Ativa = dto.Ativa,
                CriadoPorUsuarioId = usuarioId,
                DataCriacao = DateTime.UtcNow
            };

            await _estrategiaRepository.CriarAsync(estrategia);

            return MapearParaResposta(estrategia);
        }

        public async Task<EstrategiaRespostaDTO> AtualizarAsync(string id, EstrategiaAtualizacaoDTO dto)
        {
            var estrategia = await _estrategiaRepository.ObterPorIdAsync(id);
            if (estrategia == null)
            {
                throw new NaoEncontradoException("Estratégia", id);
            }

            var dataInicio = dto.DataInicio ?? estrategia.DataInicio;
            var dataFim = dto.DataFim ?? estrategia.DataFim;

            if (dataFim < dataInicio)
            {
                throw new DomainException("A data de término da estratégia não pode ser anterior à data de início.");
            }

            estrategia.Campanha = dto.Campanha.Trim();
            estrategia.Categoria = dto.Categoria.Trim();
            estrategia.Descricao = dto.Descricao.Trim();
            estrategia.DataInicio = dataInicio;
            estrategia.DataFim = dataFim;
            estrategia.Ativa = dto.Ativa;

            await _estrategiaRepository.AtualizarAsync(estrategia);

            return MapearParaResposta(estrategia);
        }

        public async Task RemoverAsync(string id)
        {
            var estrategia = await _estrategiaRepository.ObterPorIdAsync(id);
            if (estrategia == null)
            {
                throw new NaoEncontradoException("Estratégia", id);
            }

            await _estrategiaRepository.RemoverAsync(id);
        }

        private static EstrategiaRespostaDTO MapearParaResposta(Estrategia e) => new()
        {
            Id = e.Id,
            Campanha = e.Campanha,
            Categoria = e.Categoria,
            Descricao = e.Descricao,
            Data = e.Data,
            DataInicio = e.DataInicio,
            DataFim = e.DataFim,
            Ativa = e.Ativa,
            CriadoPorUsuarioId = e.CriadoPorUsuarioId,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao
        };
    }
}
