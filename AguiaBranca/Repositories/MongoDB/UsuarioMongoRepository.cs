using AguiaBranca.Context;
using AguiaBranca.Domain;
using MongoDB.Driver;

namespace AguiaBranca.Repositories.MongoDB
{
    public class UsuarioMongoRepository
    {
        private readonly MongoDbContext _context;

        public UsuarioMongoRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> ObterTodosAsync()
        {
            return await _context.Usuarios
                .Find(Builders<Usuario>.Filter.Empty)
                .SortByDescending(u => u.DataCadastro)
                .ToListAsync();
        }

        public async Task<Usuario?> ObterPorIdAsync(string id)
        {
            return await _context.Usuarios
                .Find(Builders<Usuario>.Filter.Eq(u => u.Id, id))
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .Find(Builders<Usuario>.Filter.Eq(u => u.Email.ToLower(), email.ToLower()))
                .FirstOrDefaultAsync();
        }

        public async Task CriarAsync(Usuario usuario)
        {
            usuario.DataCadastro = DateTime.UtcNow;
            await _context.Usuarios.InsertOneAsync(usuario);
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            await _context.Usuarios.ReplaceOneAsync(Builders<Usuario>.Filter.Eq(u => u.Id, usuario.Id), usuario);
        }

        public async Task RemoverAsync(string id)
        {
            await _context.Usuarios.DeleteOneAsync(Builders<Usuario>.Filter.Eq(u => u.Id, id));
        }

    }
}
