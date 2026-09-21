namespace AguiaBranca.Exceptions
{
    public class DomainException:Exception
    {
        public DomainException(string mensagem) : base(mensagem) { }
    }

    public class NaoEncontradoException : Exception
    {
        public NaoEncontradoException(string recurso, string id)
            : base($"{recurso} com identificador '{id}' não foi encontrado(a).") { }

        public NaoEncontradoException(string mensagem) : base(mensagem) { }
    }

    public class AcessoNegadoException : Exception
    {
        public AcessoNegadoException(string mensagem = "Você não possui permissão para executar esta operação.")
            : base(mensagem) { }
    }
}

