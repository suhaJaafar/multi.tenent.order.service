using Microsoft.EntityFrameworkCore;
using MultiTenantOrderService.Domain.Identity;
using MultiTenantOrderService.Domain.Identity.Entities;
using MultiTenantOrderService.Domain.Identity.ObjectValues;

namespace MultiTenantOrderService.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository( DbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<User>()
            .FirstOrDefaultAsync(u => u.Email.Value == email.Value, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        // Returns true if email is unique (does not exist)
        // Using AnyAsync is more efficient than GetByEmailAsync for existence checks
        // Compare the Value property of the Email owned entity, not the entire object
        return !await DbContext
            .Set<User>()
            .AnyAsync(u => u.Email.Value == email.Value, cancellationToken);
    }
}