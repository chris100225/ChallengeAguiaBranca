using MongoDB.Bson;
using System.Collections.Concurrent;
using AguiaBranca.Domain;

namespace AguiaBranca.Context
{
    public class InMemoryDbContext
    {

        public ConcurrentDictionary<string, Usuario> Usuarios { get; } = new();
        public ConcurrentDictionary<string, Estrategia> Estrategias { get; } = new();
        public ConcurrentDictionary<string, IdeiaInovacao> Ideias { get; } = new();
        public ConcurrentDictionary<string, Projeto> Projetos { get; } = new();

        public InMemoryDbContext()
        {
            SeedData();
        }

        private void SeedData()
        {
            // Seed de Usuários (Senhas com hash BCrypt)
            // Lider: Lider@123
            // Gestor: Gestor@123
            // Operador: Operador@123
            var idLider = ObjectId.GenerateNewId().ToString();
            var idGestor = ObjectId.GenerateNewId().ToString();
            var idOperador = ObjectId.GenerateNewId().ToString();

            var lider = new Usuario
            {
                Id = idLider,
                Nome = "Ana Líder",
                Email = "lider@empresa.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("Lider@123"),
                Perfil = PerfilUsuario.Lider,
                Ativo = true,
                DataCadastro = DateTime.UtcNow.AddMonths(-6)
            };

            var gestor = new Usuario
            {
                Id = idGestor,
                Nome = "Carlos Gestor",
                Email = "gestor@empresa.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("Gestor@123"),
                Perfil = PerfilUsuario.Gestor,
                Ativo = true,
                DataCadastro = DateTime.UtcNow.AddMonths(-5)
            };

            var operador = new Usuario
            {
                Id = idOperador,
                Nome = "Lucas Operador",
                Email = "operador@empresa.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("Operador@123"),
                Perfil = PerfilUsuario.Operador,
                Ativo = true,
                DataCadastro = DateTime.UtcNow.AddMonths(-4)
            };

            Usuarios.TryAdd(lider.Id, lider);
            Usuarios.TryAdd(gestor.Id, gestor);
            Usuarios.TryAdd(operador.Id, operador);

            // Seed de Estratégias
            var idEstrategiaHistorica = ObjectId.GenerateNewId().ToString();
            var estrategiaHistorica = new Estrategia
            {
                Id = idEstrategiaHistorica,
                Campanha = "Excelência Operacional e Redução de Desperdícios 2025",
                Categoria = "Eficiência",
                Descricao = "Otimização dos processos internos e redução de custos operacionais na linha de montagem.",
                Data = DateTime.UtcNow.AddYears(-1),
                DataInicio = DateTime.UtcNow.AddYears(-1),
                DataFim = DateTime.UtcNow.AddMonths(-2),
                Ativa = false,
                CriadoPorUsuarioId = idLider,
                DataCriacao = DateTime.UtcNow.AddYears(-1)
            };

            var idEstrategiaVigente = ObjectId.GenerateNewId().ToString();
            var estrategiaVigente = new Estrategia
            {
                Id = idEstrategiaVigente,
                Campanha = "Inovação Digital & Inteligência Operacional 2026",
                Categoria = "Inovação Tecnológica",
                Descricao = "Adoção de soluções ágeis, IoT e integração digital para impulsionar a produtividade do time de ponta.",
                Data = DateTime.UtcNow.AddMonths(-2),
                DataInicio = DateTime.UtcNow.AddMonths(-2),
                DataFim = DateTime.UtcNow.AddMonths(10),
                Ativa = true,
                CriadoPorUsuarioId = idLider,
                DataCriacao = DateTime.UtcNow.AddMonths(-2)
            };

            Estrategias.TryAdd(estrategiaHistorica.Id, estrategiaHistorica);
            Estrategias.TryAdd(estrategiaVigente.Id, estrategiaVigente);

            // Seed de Ideias de Inovação
            var idIdeia1 = ObjectId.GenerateNewId().ToString();
            var ideia1 = new IdeiaInovacao
            {
                Id = idIdeia1,
                Titulo = "Check-in Digital por Tablet nos Postos de Montagem",
                Descricao = "Substituir planilhas físicas por formulários rápidos via tablet na linha de operação.",
                ProblemaEnfrentado = "Perda de 25 minutos diários preenchendo relatórios em papel e risco de extravio.",
                ImpactoEsperado = "Economia de tempo e rastreabilidade em tempo real de não-conformidades.",
                Status = StatusIdeia.Aprovada,
                Prioridade = PrioridadeIdeia.Alta,
                EstrategiaId = idEstrategiaVigente,
                OperadorId = idOperador,
                NomeOperador = operador.Nome,
                FeedbackGestor = "Excelente iniciativa! Aprovada para implementação imediata como projeto piloto.",
                AvaliadoPorGestorId = idGestor,
                DataAvaliacao = DateTime.UtcNow.AddMonths(-1),
                DataCriacao = DateTime.UtcNow.AddMonths(-1).AddDays(-5)
            };

            var idIdeia2 = ObjectId.GenerateNewId().ToString();
            var ideia2 = new IdeiaInovacao
            {
                Id = idIdeia2,
                Titulo = "Sensores de Temperatura nas Máquinas de Solda",
                Descricao = "Instalar sensores IoT com alerta sonoro quando a máquina atingir limite térmico crítico.",
                ProblemaEnfrentado = "Superaquecimento inesperado gera paradas não programadas e queima de peças.",
                ImpactoEsperado = "Manutenção preventiva imediata antes de quebras.",
                Status = StatusIdeia.Pendente,
                Prioridade = PrioridadeIdeia.Media,
                EstrategiaId = idEstrategiaVigente,
                OperadorId = idOperador,
                NomeOperador = operador.Nome,
                DataCriacao = DateTime.UtcNow.AddDays(-3)
            };

            Ideias.TryAdd(ideia1.Id, ideia1);
            Ideias.TryAdd(ideia2.Id, ideia2);

            // Seed de Projetos
            var idProjeto1 = ObjectId.GenerateNewId().ToString();
            var projeto1 = new Projeto
            {
                Id = idProjeto1,
                Nome = "Implantação de Terminais Digitais na Linha A",
                Descricao = "Projeto originado da ideia de tablets nos postos para check-in operacional.",
                EstrategiaId = idEstrategiaVigente,
                IdeiaInovacaoId = idIdeia1,
                GestorId = idGestor,
                NomeGestor = gestor.Nome,
                Etapa = EtapaProjeto.Concluido,
                Status = StatusProjeto.Concluido,
                InvestimentoPrevisto = 15000m,
                InvestimentoReal = 12000m,
                RetornoFinanceiroEsperado = 40000m,
                RetornoFinanceiroReal = 48000m,
                AumentoProdutividadePercentual = 18.5m,
                DataInicio = DateTime.UtcNow.AddMonths(-1),
                PrazoFinal = DateTime.UtcNow.AddDays(-5),
                DataConclusao = DateTime.UtcNow.AddDays(-6),
                DataCriacao = DateTime.UtcNow.AddMonths(-1)
            };

            var idProjeto2 = ObjectId.GenerateNewId().ToString();
            var projeto2 = new Projeto
            {
                Id = idProjeto2,
                Nome = "Automação do Fluxo de Peças por RFID",
                Descricao = "Rastreamento automatizado da entrada e saída de matéria-prima no almoxarifado.",
                EstrategiaId = idEstrategiaVigente,
                GestorId = idGestor,
                NomeGestor = gestor.Nome,
                Etapa = EtapaProjeto.EmExecucao,
                Status = StatusProjeto.NoPrazo,
                InvestimentoPrevisto = 35000m,
                InvestimentoReal = 28000m,
                RetornoFinanceiroEsperado = 85000m,
                RetornoFinanceiroReal = 30000m,
                AumentoProdutividadePercentual = 12.0m,
                DataInicio = DateTime.UtcNow.AddDays(-20),
                PrazoFinal = DateTime.UtcNow.AddMonths(2),
                DataCriacao = DateTime.UtcNow.AddDays(-20)
            };

            Projetos.TryAdd(projeto1.Id, projeto1);
            Projetos.TryAdd(projeto2.Id, projeto2);
        }
    }
}

