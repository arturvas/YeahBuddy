using System.Text.RegularExpressions;
using Dapper;
using MySqlConnector;

namespace YeahBuddy.Infrastructure.Migrations;

public static partial class DatabaseMigration
{
    public static void Migrate(string connectionString)
    {
        EnsureDatabaseCreated_MySql(connectionString);
        
    }

    private static void EnsureDatabaseCreated_MySql(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
        var databaseName = connectionStringBuilder.Database;

        if (string.IsNullOrWhiteSpace(databaseName))
            throw new ArgumentException("Database name not found in connection string");

        if (!MyRegex().IsMatch(databaseName))
            throw new ArgumentException("Invalid database name format");
        
        connectionStringBuilder.Remove("Database");
        
        using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);
        dbConnection.Open();

        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);

        var records = dbConnection.Query("SELECT * FROM information_schema.schemata WHERE SCHEMA_NAME = @name", parameters);

        if (!records.Any())
            dbConnection.Execute($"CREATE DATABASE `{databaseName}`");
    }

    [GeneratedRegex(@"^[a-zA-Z0-9_]+$")]
    private static partial Regex MyRegex();
}