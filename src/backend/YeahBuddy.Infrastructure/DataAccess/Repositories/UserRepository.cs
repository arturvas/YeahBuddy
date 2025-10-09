using Microsoft.EntityFrameworkCore;
using YeahBuddy.Domain.Entities;
using YeahBuddy.Domain.Repositories.User;

namespace YeahBuddy.Infrastructure.DataAccess.Repositories;

public class UserRepository(YeahBuddyDbContext dbContext) : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    public async Task Add(User user) => await dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email) =>
        await dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.IsActive);
}