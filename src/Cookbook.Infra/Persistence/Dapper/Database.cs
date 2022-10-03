using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Cookbook.Infra.Persistence.Dapper;

public static class Database
{
    public static void InitializeDb(IConfiguration config)
    {
        var connectionString = config.GetConnectionString("ConnectionMySQL");
        var databaseName = config.GetConnectionString("DatabaseName");

        // Criando conexão com o B.D MySQL
        using var connection = new MySqlConnection(connectionString);

        // Cria variável parâmetros atraves de mapeamento para passar nas consultas SQL, utilizando "@"
        var parameters = new DynamicParameters();
        parameters.Add("database", databaseName);

        // Verifica antes de tudo se existe o database passado
        // INFORMATION_SCHEMA.SCHEMATA: é uma tabela interna do MySQL quando existe um database
        var query = connection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA where SCHEMA_NAME = @database", parameters);

        // Caso não existir o database, será criado (Executado uma query de Create)
        if (!query.Any())
        {
            connection.Execute($"CREATE DATABASE {databaseName}");
        }
    }
}
