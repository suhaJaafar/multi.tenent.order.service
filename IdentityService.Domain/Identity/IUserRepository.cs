﻿
using IdentityService.Domain.Identity.ObjectValues;
using IdentityService.Domain.Identity.Entities;

namespace IdentityService.Domain.Identity;

public interface IUserRepository
{
    Task<Entities.User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Entities.User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);
    void Add(Entities.User user);
}