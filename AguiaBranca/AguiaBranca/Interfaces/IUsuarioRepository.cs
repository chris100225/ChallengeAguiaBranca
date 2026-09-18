using AguiaBranca.Domain;

namespace AguiaBranca.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> ObterTodosAsync();
        Task<Usuario?> ObterPorIdAsync(string id);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task CriarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task RemoverAsync(string id);
    }
}
