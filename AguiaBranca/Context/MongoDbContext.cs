using MongoDB.Driver;
using AguiaBranca.Domain;
using Microsoft.Extensions.Configuration;

namespace AguiaBranca.Context
{
    public class MongoDbContext
    {

        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration["DatabaseSettings:ConnectionString"]
                ?? "mongodb://localhost:27017";
            var databaseName = configuration["DatabaseSettings:DatabaseName"]
                ?? "InovacaoEmpresaDB";

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Usuario> Usuarios =>
            _database.GetCollection<Usuario>("Usuarios");

        public IMongoCollection<Estrategia> Estrategias =>
            _database.GetCollection<Estrategia>("Estrategias");

        public IMongoCollection<IdeiaInovacao> Ideias =>
            _database.GetCollection<IdeiaInovacao>("Ideias");

        public IMongoCollection<Projeto> Projetos =>
            _database.GetCollection<Projeto>("Projetos");
    }
}
