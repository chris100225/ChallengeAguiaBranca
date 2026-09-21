using AguiaBranca.Context;
using AguiaBranca.Domain;
using MongoDB.Bson;
using AguiaBranca.Interfaces;

namespace AguiaBranca.Repositories.InMemory
{
    public class ProjetoInMemoryRepository: IProjetoRepository
    {
        private readonly InMemoryDbContext _context;

        public ProjetoInMemoryRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public Task<List<Projeto>> ObterTodosAsync()
        {
            var lista = _context.Projetos.Values
                .OrderByDescending(p => p.DataCriacao)
                .ToList();
            return Task.FromResult(lista);
        }

        public Task<List<Projeto>> ObterPorEstrategiaIdAsync(string estrategiaId)
        {
            var lista = _context.Projetos.Values
                .Where(p => p.EstrategiaId == estrategiaId)
                .OrderByDescending(p => p.DataCriacao)
                .ToList();
            return Task.FromResult(lista);
        }

        public Task<List<Projeto>> ObterPorGestorIdAsync(string gestorId)
        {
            var lista = _context.Projetos.Values
                .Where(p => p.GestorId == gestorId)
                .OrderByDescending(p => p.DataCriacao)
                .ToList();
            return Task.FromResult(lista);
        }

        public Task<Projeto?> ObterPorIdAsync(string id)
        {
            _context.Projetos.TryGetValue(id, out var projeto);
            return Task.FromResult(projeto);
        }

        public Task CriarAsync(Projeto projeto)
        {
            if (string.IsNullOrWhiteSpace(projeto.Id))
            {
                projeto.Id = ObjectId.GenerateNewId().ToString();
            }
            projeto.DataCriacao = DateTime.UtcNow;
            _context.Projetos.TryAdd(projeto.Id, projeto);
            return Task.CompletedTask;
        }

        public Task AtualizarAsync(Projeto projeto)
        {
            projeto.DataAtualizacao = DateTime.UtcNow;
            _context.Projetos[projeto.Id] = projeto;
            return Task.CompletedTask;
        }

        public Task RemoverAsync(string id)
        {
            _context.Projetos.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
