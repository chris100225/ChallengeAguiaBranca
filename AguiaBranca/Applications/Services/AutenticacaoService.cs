using AguiaBranca.Applications.Autenticacao;
using AguiaBranca.Domain;
using AguiaBranca.DTOs.Autenticacao;
using AguiaBranca.DTOs.Usuario;
using AguiaBranca.Exceptions;
using AguiaBranca.Interfaces;

namespace AguiaBranca.Applications.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly GeradorTokenJWT _geradorTokenJwt;

        public AutenticacaoService(IUsuarioRepository usuarioRepository, GeradorTokenJWT geradorTokenJwt)
        {
            _usuarioRepository = usuarioRepository;
            _geradorTokenJwt = geradorTokenJwt;
        }

        public async Task<LoginRespostaDTO> AutenticarAsync(LoginDTO dto)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);
            if (usuario == null)
            {
                throw new DomainException("Credenciais inválidas: e-mail ou senha incorretos.");
            }

            if (!usuario.Ativo)
            {
                throw new DomainException("Este usuário está inativo no sistema.");
            }

            var senhaValida = Criptografia.VerificarSenha(dto.Senha, usuario.SenhaHash);
            if (!senhaValida)
            {
                throw new DomainException("Credenciais inválidas: e-mail ou senha incorretos.");
            }

            var (token, expiraEm) = _geradorTokenJwt.GerarToken(usuario);

            return new LoginRespostaDTO
            {
                Token = token,
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil,
                ExpiraEm = expiraEm
            };
        }

        public async Task<UsuarioRespostaDTO> RegistrarUsuarioAsync(UsuarioDTO dto)
        {
            if (!PerfilUsuario.EhValido(dto.Perfil))
            {
                throw new DomainException($"Perfil '{dto.Perfil}' inválido. Perfis aceitos: {string.Join(", ", PerfilUsuario.Todos)}");
            }

            var usuarioExistente = await _usuarioRepository.ObterPorEmailAsync(dto.Email);
            if (usuarioExistente != null)
            {
                throw new DomainException("Já existe um usuário cadastrado com este e-mail.");
            }

            var usuario = new Usuario
            {
                Nome = dto.Nome.Trim(),
                Email = dto.Email.Trim().ToLower(),
                SenhaHash = Criptografia.GerarHashSenha(dto.Senha),
                Perfil = dto.Perfil,
                Ativo = true,
                DataCadastro = DateTime.UtcNow
            };

            await _usuarioRepository.CriarAsync(usuario);

            return MapearParaResposta(usuario);
        }

        public async Task<UsuarioRespostaDTO> ObterPorIdAsync(string id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            if (usuario == null)
            {
                throw new NaoEncontradoException("Usuário", id);
            }
            return MapearParaResposta(usuario);
        }

        public async Task<List<UsuarioRespostaDTO>> ObterTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();
            return usuarios.Select(MapearParaResposta).ToList();
        }

        private static UsuarioRespostaDTO MapearParaResposta(Usuario u) => new()
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            Perfil = u.Perfil,
            Ativo = u.Ativo,
            DataCadastro = u.DataCadastro
        };
    }
}
