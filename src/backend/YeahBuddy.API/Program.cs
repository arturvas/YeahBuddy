using YeahBuddy.API.Filters;
using YeahBuddy.API.Middleware;
using YeahBuddy.Application;
using YeahBuddy.Infrastructure;
using YeahBuddy.Infrastructure.Extensions;
using YeahBuddy.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

MigrateDatabase();

app.Run();

void MigrateDatabase()
{
    var connectionString = builder.Configuration.ConnectionString();
    
    app.Logger.LogInformation("Database connection string: {connectionString}", connectionString);
    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    
    DatabaseMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}