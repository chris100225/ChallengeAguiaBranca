using AguiaBranca.Context;
using AguiaBranca.Domain;
using AguiaBranca.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AguiaBranca.Repositories.MongoDB
{
    public class EstrategiaMongoRepository : IEstrategiaRepository
    {
        private readonly MongoDbContext _context;

        public EstrategiaMongoRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Estrategia>> ObterTodasAsync()
        {
            var result = await _context.Estrategias
                .Find(_ => true)
                .SortByDescending(e => e.DataCriacao)
                .ToListAsync();
            return result;
        }

        public async Task<Estrategia?> ObterPorIdAsync(string id)
        {
            var filter = Builders<Estrategia>.Filter.Eq(e => e.Id, id);
            var estrategia = await _context.Estrategias
                .Find(filter)
                .FirstOrDefaultAsync();
            return estrategia;
        }

        public async Task<Estrategia?> ObterVigenteAsync()
        {
            var filter = Builders<Estrategia>.Filter.Eq(e => e.Ativa, true);
            var estrategia = await _context.Estrategias
                .Find(filter)
                .SortByDescending(e => e.DataCriacao)
                .FirstOrDefaultAsync();
            return estrategia;
        }

        public async Task CriarAsync(Estrategia estrategia)
        {
            if (string.IsNullOrWhiteSpace(estrategia.Id))
            {
                estrategia.Id = ObjectId.GenerateNewId().ToString();
            }
            estrategia.DataCriacao = DateTime.UtcNow;

            if (estrategia.Ativa)
            {
                var filter = Builders<Estrategia>.Filter.Ne(e => e.Id, estrategia.Id);
                var update = Builders<Estrategia>.Update.Set(e => e.Ativa, false);
                await _context.Estrategias.UpdateManyAsync(filter, update);
            }

            await _context.Estrategias.InsertOneAsync(estrategia);
        }

        public async Task AtualizarAsync(Estrategia estrategia)
        {
            estrategia.DataAtualizacao = DateTime.UtcNow;

            if (estrategia.Ativa)
            {
                var filter = Builders<Estrategia>.Filter.Ne(e => e.Id, estrategia.Id);
                var update = Builders<Estrategia>.Update.Set(e => e.Ativa, false);
                await _context.Estrategias.UpdateManyAsync(filter, update);
            }

            var replaceFilter = Builders<Estrategia>.Filter.Eq(e => e.Id, estrategia.Id);
            await _context.Estrategias.ReplaceOneAsync(replaceFilter, estrategia);
        }

        public async Task RemoverAsync(string id)
        {
            await _context.Estrategias.DeleteOneAsync(e => e.Id == id);
        }
    }
}
