using FluentMigrator;

namespace YeahBuddy.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TableUser, "InitialCreate")]
public class Version0000001 : VersionBase
{
    public override void Up()
    {
        CreateTable("Users")
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable();
    }
}