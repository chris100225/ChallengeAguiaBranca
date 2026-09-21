using AguiaBranca.Context;
using AguiaBranca.Domain;
using MongoDB.Bson;

namespace AguiaBranca.Repositories.InMemory
{
    public class EstrategiaInMemoryRepository
    {
        private readonly InMemoryDbContext _context;

        public EstrategiaInMemoryRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public Task<List<Estrategia>> ObterTodasAsync()
        {
            var lista = _context.Estrategias.Values
                .OrderByDescending(e => e.Data)
                .ToList();
            return Task.FromResult(lista);
        }

        public Task<Estrategia?> ObterPorIdAsync(string id)
        {
            _context.Estrategias.TryGetValue(id, out var estrategia);
            return Task.FromResult(estrategia);
        }

        public Task<Estrategia?> ObterVigenteAsync()
        {
            var vigente = _context.Estrategias.Values
                .OrderByDescending(e => e.DataCriacao)
                .FirstOrDefault(e => e.Ativa);
            return Task.FromResult(vigente);
        }

        public Task CriarAsync(Estrategia estrategia)
        {
            if (string.IsNullOrWhiteSpace(estrategia.Id))
            {
                estrategia.Id = ObjectId.GenerateNewId().ToString();
            }
            estrategia.DataCriacao = DateTime.UtcNow;

            // Se for marcada como ativa, desativa as outras para manter uma única vigente
            if (estrategia.Ativa)
            {
                foreach (var item in _context.Estrategias.Values)
                {
                    item.Ativa = false;
                }
            }

            _context.Estrategias.TryAdd(estrategia.Id, estrategia);
            return Task.CompletedTask;
        }

        public Task AtualizarAsync(Estrategia estrategia)
        {
            estrategia.DataAtualizacao = DateTime.UtcNow;
            if (estrategia.Ativa)
            {
                foreach (var item in _context.Estrategias.Values)
                {
                    if (item.Id != estrategia.Id)
                    {
                        item.Ativa = false;
                    }
                }
            }

            _context.Estrategias[estrategia.Id] = estrategia;
            return Task.CompletedTask;
        }

        public Task RemoverAsync(string id)
        {
            _context.Estrategias.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
