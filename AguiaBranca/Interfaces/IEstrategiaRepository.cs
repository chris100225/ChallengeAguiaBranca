using AguiaBranca.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AguiaBranca.Interfaces
{
    public interface IEstrategiaRepository
    {
        Task<List<Estrategia>> ObterTodasAsync();
        Task<Estrategia?> ObterPorIdAsync(string id);
        Task<Estrategia?> ObterVigenteAsync();
        Task CriarAsync(Estrategia estrategia);
        Task AtualizarAsync(Estrategia estrategia);
        Task RemoverAsync(string id);
    }
}
