namespace AguiaBranca.Applications.Autenticacao
{
    public class Criptografia
    {
        public static string GerarHashSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha, workFactor: 11);
        }

        public static bool VerificarSenha(string senha, string hash)
        {
            if (string.IsNullOrEmpty(senha) || string.IsNullOrEmpty(hash))
                return false;

            return BCrypt.Net.BCrypt.Verify(senha, hash);
        }
    }
}
