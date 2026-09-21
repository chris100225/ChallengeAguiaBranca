using AguiaBranca.Context;
using AguiaBranca.Domain;
using MongoDB.Driver;

namespace AguiaBranca.Repositories.MongoDB
{
    public class ProjetoMongoRepository
    {
        private readonly MongoDbContext _context;

        public ProjetoMongoRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Projeto>> ObterTodosAsync()
        {
            return await _context.Projetos
                .Find(Builders<Projeto>.Filter.Empty)
                .SortByDescending(p => p.DataCriacao)
                .ToListAsync();
        }

        public async Task<List<Projeto>> ObterPorEstrategiaIdAsync(string estrategiaId)
        {
            return await _context.Projetos
                .Find(Builders<Projeto>.Filter.Eq(p => p.EstrategiaId, estrategiaId))
                .SortByDescending(p => p.DataCriacao)
                .ToListAsync();
        }

        public async Task<List<Projeto>> ObterPorGestorIdAsync(string gestorId)
        {
            return await _context.Projetos
                .Find(Builders<Projeto>.Filter.Eq(p => p.GestorId, gestorId))
                .SortByDescending(p => p.DataCriacao)
                .ToListAsync();
        }

        public async Task<Projeto?> ObterPorIdAsync(string id)
        {
            return await _context.Projetos
                .Find(Builders<Projeto>.Filter.Eq(p => p.Id, id))
                .FirstOrDefaultAsync();
        }

        public async Task CriarAsync(Projeto projeto)
        {
            projeto.DataCriacao = DateTime.UtcNow;
            await _context.Projetos.InsertOneAsync(projeto);
        }

        public async Task AtualizarAsync(Projeto projeto)
        {
            projeto.DataAtualizacao = DateTime.UtcNow;
            await _context.Projetos.ReplaceOneAsync(Builders<Projeto>.Filter.Eq(p => p.Id, projeto.Id), projeto);
        }

        public async Task RemoverAsync(string id)
        {
            await _context.Projetos.DeleteOneAsync(Builders<Projeto>.Filter.Eq(p => p.Id, id));
        }
    }
}
