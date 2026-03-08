﻿
using MultiTenantOrderService.Domain.Identity.Entities;
using MultiTenantOrderService.Domain.Identity.ObjectValues;

namespace MultiTenantOrderService.Domain.Identity;

public interface IUserRepository
{
    Task<Entities.User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Entities.User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);
    void Add(Entities.User user);
}