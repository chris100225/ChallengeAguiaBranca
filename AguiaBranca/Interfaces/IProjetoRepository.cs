using AguiaBranca.Domain;

namespace AguiaBranca.Interfaces
{
    public interface IProjetoRepository
    {
        Task<List<Projeto>> ObterTodosAsync();
        Task<List<Projeto>> ObterPorEstrategiaIdAsync(string estrategiaId);
        Task<List<Projeto>> ObterPorGestorIdAsync(string gestorId);
        Task<Projeto?> ObterPorIdAsync(string id);
        Task CriarAsync(Projeto projeto);
        Task AtualizarAsync(Projeto projeto);
        Task RemoverAsync(string id);
    }
}
