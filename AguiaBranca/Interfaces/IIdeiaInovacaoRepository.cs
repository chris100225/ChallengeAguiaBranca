using AguiaBranca.Domain;

namespace AguiaBranca.Interfaces
{
    public interface IIdeiaInovacaoRepository
    {
        Task<List<IdeiaInovacao>> ObterTodasAsync();
        Task<List<IdeiaInovacao>> ObterPorOperadorIdAsync(string operadorId);
        Task<List<IdeiaInovacao>> ObterPorEstrategiaIdAsync(string estrategiaId);
        Task<IdeiaInovacao?> ObterPorIdAsync(string id);
        Task CriarAsync(IdeiaInovacao ideia);
        Task AtualizarAsync(IdeiaInovacao ideia);
        Task RemoverAsync(string id);
    }
}
