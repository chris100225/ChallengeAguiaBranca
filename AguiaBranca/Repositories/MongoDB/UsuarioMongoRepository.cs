using AguiaBranca.Context;
using AguiaBranca.Domain;
using MongoDB.Driver;
using AguiaBranca.Interfaces;
using System.Text.RegularExpressions;
using MongoDB.Bson;

namespace AguiaBranca.Repositories.MongoDB
{
    public class UsuarioMongoRepository: IUsuarioRepository
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
    var filtro = Builders<Usuario>.Filter.Regex(
        u => u.Email,
        new BsonRegularExpression($"^{Regex.Escape(email)}$", "i")
    );

    return await _context.Usuarios
        .Find(filtro)
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
