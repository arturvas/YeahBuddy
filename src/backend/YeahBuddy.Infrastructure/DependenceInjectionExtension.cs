using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YeahBuddy.Domain.Repositories;
using YeahBuddy.Domain.Repositories.User;
using YeahBuddy.Infrastructure.DataAccess;
using YeahBuddy.Infrastructure.Extensions;
using YeahBuddy.Infrastructure.Migrations.Versions;
using UserRepository = YeahBuddy.Infrastructure.DataAccess.Repositories.UserRepository;

namespace YeahBuddy.Infrastructure;

public static class DependenceInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext_MySqlServer(services, configuration);
        AddFluentMigrator_MySql(services, configuration);
        AddRepositories(services);
    }

    private static void AddDbContext_MySqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        var serverVersion = new MySqlServerVersion(new Version(9, 4, 0));

        services.AddDbContext<YeahBuddyDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
    }

    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddMySql8()
                .WithGlobalConnectionString(configuration.ConnectionString())
                .ScanIn(Assembly.Load("YeahBuddy.Infrastructure")).For.All();
        });
    }
}