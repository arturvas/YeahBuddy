using Microsoft.Extensions.Configuration;

namespace YeahBuddy.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("MySqlConnectionString")!;
    }
}