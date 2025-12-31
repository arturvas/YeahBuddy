using System.Text.RegularExpressions;
using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace YeahBuddy.Infrastructure.Migrations;

public static partial class DatabaseMigration
{
    public static void Migrate(string connectionString, IServiceProvider serviceProvider)
    {
        EnsureDatabaseCreated_MySql(connectionString);
        MigrateDatabase(serviceProvider);

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
    
    
    private static void MigrateDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
    }
}