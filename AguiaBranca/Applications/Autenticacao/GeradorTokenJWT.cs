using AguiaBranca.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AguiaBranca.Applications.Autenticacao
{
    public class GeradorTokenJWT
    {
        private readonly IConfiguration _config;

        public GeradorTokenJWT(IConfiguration config)
        {
            _config = config;
        }

        public (string Token, DateTime ExpiraEm) GerarToken(Usuario usuario)
        {
            var chave = _config["Jwt:Key"]
                ?? "MinhaChaveSuperSecretaParaAssinarTokenJwtInovacao2026!@#";
            var issuer = _config["Jwt:Issuer"] ?? "InovacaoEstrategiaApi";
            var audience = _config["Jwt:Audience"] ?? "InovacaoEstrategiaClient";
            var duracaoMinutos = int.TryParse(_config["Jwt:DuracaoMinutos"], out var m) ? m : 480;

            var keyBytes = Encoding.UTF8.GetBytes(chave);
            var tokenHandler = new JwtSecurityTokenHandler();
            var expiraEm = DateTime.UtcNow.AddMinutes(duracaoMinutos);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Perfil)
                }),
                Expires = expiraEm,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(securityToken);

            return (tokenString, expiraEm);
        }
    }
}
