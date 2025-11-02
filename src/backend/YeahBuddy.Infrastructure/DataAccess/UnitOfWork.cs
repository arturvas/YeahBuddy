using YeahBuddy.Domain.Repositories;

namespace YeahBuddy.Infrastructure.DataAccess;

public class UnitOfWork(YeahBuddyDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync() => await dbContext.SaveChangesAsync();
}