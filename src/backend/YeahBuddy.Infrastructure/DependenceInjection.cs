using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YeahBuddy.Domain.Repositories.User;
using YeahBuddy.Infrastructure.DataAccess;
using YeahBuddy.Infrastructure.DataAccess.Repositories;

namespace YeahBuddy.Infrastructure;

public static class DependenceInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        AddDbContext_MySqlServer(services);
        AddRepositories(services);
    }

    private static void AddDbContext_MySqlServer(IServiceCollection services)
    {
        const string connectionString = "Server=127.0.0.1;Database=YeahBuddy;User Id=sa;Pwd={PWD};";
        var serverVersion = new MySqlServerVersion(new Version(9, 4, 0));

        services.AddDbContext<YeahBuddyDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
    }
}