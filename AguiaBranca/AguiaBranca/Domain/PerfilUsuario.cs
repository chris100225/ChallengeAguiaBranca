namespace AguiaBranca.Domain
{
    public class PerfilUsuario
    {

        public const string Operador = "Operador";
        public const string Gestor = "Gestor";
        public const string Lider = "Lider";

        public static readonly string[] Todos = new[] { Operador, Gestor, Lider };

        public static bool EhValido(string perfil) =>
            perfil is Operador or Gestor or Lider;
    }
}
