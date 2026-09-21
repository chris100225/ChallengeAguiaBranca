using AguiaBranca.Context;
using AguiaBranca.Domain;
using MongoDB.Bson;

namespace AguiaBranca.Repositories.InMemory
{
    public class IdeiaInovacaoInMemoryRepository
    {
        private readonly InMemoryDbContext _context;

        public IdeiaInovacaoInMemoryRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public Task<List<IdeiaInovacao>> ObterTodasAsync()
        {
            var lista = _context.Ideias.Values
                .OrderByDescending(i => i.DataCriacao)
                .ToList();
            return Task.FromResult(lista);
        }

        public Task<List<IdeiaInovacao>> ObterPorOperadorIdAsync(string operadorId)
        {
            var lista = _context.Ideias.Values
                .Where(i => i.OperadorId == operadorId)
                .OrderByDescending(i => i.DataCriacao)
                .ToList();
            return Task.FromResult(lista);
        }

        public Task<List<IdeiaInovacao>> ObterPorEstrategiaIdAsync(string estrategiaId)
        {
            var lista = _context.Ideias.Values
                .Where(i => i.EstrategiaId == estrategiaId)
                .OrderByDescending(i => i.DataCriacao)
                .ToList();
            return Task.FromResult(lista);
        }

        public Task<IdeiaInovacao?> ObterPorIdAsync(string id)
        {
            _context.Ideias.TryGetValue(id, out var ideia);
            return Task.FromResult(ideia);
        }

        public Task CriarAsync(IdeiaInovacao ideia)
        {
            if (string.IsNullOrWhiteSpace(ideia.Id))
            {
                ideia.Id = ObjectId.GenerateNewId().ToString();
            }
            ideia.DataCriacao = DateTime.UtcNow;
            _context.Ideias.TryAdd(ideia.Id, ideia);
            return Task.CompletedTask;
        }

        public Task AtualizarAsync(IdeiaInovacao ideia)
        {
            ideia.DataAtualizacao = DateTime.UtcNow;
            _context.Ideias[ideia.Id] = ideia;
            return Task.CompletedTask;
        }

        public Task RemoverAsync(string id)
        {
            _context.Ideias.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
