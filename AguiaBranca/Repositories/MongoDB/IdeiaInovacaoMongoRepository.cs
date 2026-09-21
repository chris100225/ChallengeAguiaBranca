using AguiaBranca.Context;
using AguiaBranca.Domain;
using MongoDB.Driver;

namespace AguiaBranca.Repositories.MongoDB
{
    public class IdeiaInovacaoMongoRepository
    {
        private readonly MongoDbContext _context;

        public IdeiaInovacaoMongoRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<IdeiaInovacao>> ObterTodasAsync()
        {
            return await _context.Ideias
                .Find(Builders<IdeiaInovacao>.Filter.Empty)
                .SortByDescending(i => i.DataCriacao)
                .ToListAsync();
        }

        public async Task<List<IdeiaInovacao>> ObterPorOperadorIdAsync(string operadorId)
        {
            return await _context.Ideias
                .Find(Builders<IdeiaInovacao>.Filter.Eq(i => i.OperadorId, operadorId))
                .SortByDescending(i => i.DataCriacao)
                .ToListAsync();
        }

        public async Task<List<IdeiaInovacao>> ObterPorEstrategiaIdAsync(string estrategiaId)
        {
            return await _context.Ideias
                .Find(Builders<IdeiaInovacao>.Filter.Eq(i => i.EstrategiaId, estrategiaId))
                .SortByDescending(i => i.DataCriacao)
                .ToListAsync();
        }

        public async Task<IdeiaInovacao?> ObterPorIdAsync(string id)
        {
            return await _context.Ideias
                .Find(Builders<IdeiaInovacao>.Filter.Eq(i => i.Id, id))
                .FirstOrDefaultAsync();
        }

        public async Task CriarAsync(IdeiaInovacao ideia)
        {
            ideia.DataCriacao = DateTime.UtcNow;
            await _context.Ideias.InsertOneAsync(ideia);
        }

        public async Task AtualizarAsync(IdeiaInovacao ideia)
        {
            ideia.DataAtualizacao = DateTime.UtcNow;
            await _context.Ideias.ReplaceOneAsync(Builders<IdeiaInovacao>.Filter.Eq(i => i.Id, ideia.Id), ideia);
        }

        public async Task RemoverAsync(string id)
        {
            await _context.Ideias.DeleteOneAsync(Builders<IdeiaInovacao>.Filter.Eq(i => i.Id, id));
        }
    }
}
