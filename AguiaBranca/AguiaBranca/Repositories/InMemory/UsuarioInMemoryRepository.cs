using AguiaBranca.Interfaces;
using MongoDB.Bson;
using AguiaBranca.Context;
using AguiaBranca.Domain;

namespace AguiaBranca.Repositories.InMemory
{
        public class UsuarioInMemoryRepository : IUsuarioRepository
        {
            private readonly InMemoryDbContext _context;

            public UsuarioInMemoryRepository(InMemoryDbContext context)
            {
                _context = context;
            }

            public Task<List<Usuario>> ObterTodosAsync()
            {
                var lista = _context.Usuarios.Values.OrderByDescending(u => u.DataCadastro).ToList();
                return Task.FromResult(lista);
            }

            public Task<Usuario?> ObterPorIdAsync(string id)
            {
                _context.Usuarios.TryGetValue(id, out var usuario);
                return Task.FromResult(usuario);
            }

            public Task<Usuario?> ObterPorEmailAsync(string email)
            {
                var usuario = _context.Usuarios.Values.FirstOrDefault(u =>
                    string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
                return Task.FromResult(usuario);
            }

            public Task CriarAsync(Usuario usuario)
            {
                if (string.IsNullOrWhiteSpace(usuario.Id))
                {
                    usuario.Id = ObjectId.GenerateNewId().ToString();
                }
                usuario.DataCadastro = DateTime.UtcNow;
                _context.Usuarios.TryAdd(usuario.Id, usuario);
                return Task.CompletedTask;
            }

            public Task AtualizarAsync(Usuario usuario)
            {
                _context.Usuarios[usuario.Id] = usuario;
                return Task.CompletedTask;
            }

            public Task RemoverAsync(string id)
            {
                _context.Usuarios.TryRemove(id, out _);
                return Task.CompletedTask;
            }
        }
}

